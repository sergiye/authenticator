using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Authenticator {
  public partial class ShowSteamTradesForm : ResourceForm {
    [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData);

    class PollerActionItem {
      public string Text;
      public SteamClient.PollerAction Value;

      public override string ToString() {
        return Text;
      }
    }

    public ShowSteamTradesForm() {
      InitializeComponent();
    }

    public AuthAuthenticator Authenticator { get; set; }

    private bool autoWarned;

    public SteamAuthenticator AuthenticatorData {
      get { return Authenticator != null ? Authenticator.AuthenticatorData as SteamAuthenticator : null; }
    }

    private List<SteamClient.Confirmation> mTrades;

    private int mBrowserHeight;

    private bool mLoaded;

    private CancellationTokenSource cancelComfirmAll;

    private CancellationTokenSource cancelCancelAll;

    private Dictionary<string, TabPage> mTabPages = new Dictionary<string, TabPage>();

    #region Form Events

    private void ShowSteamTradesForm_Load(object sender, EventArgs e) {
      MinimumSize = Size;

      mBrowserHeight = browserContainer.Height;
      browserContainer.Height = 0;
      tradesContainer.Height += mBrowserHeight;

      pollAction.Items.Clear();

      var items = new BindingList<object>();
      items.Add(new PollerActionItem {Text = "Show Notification", Value = SteamClient.PollerAction.Notify});
      items.Add(new PollerActionItem {Text = "Auto-Confirm", Value = SteamClient.PollerAction.AutoConfirm});
      items.Add(new PollerActionItem
        {Text = "Auto-Confirm (silently)", Value = SteamClient.PollerAction.SilentAutoConfirm});

      pollAction.DataSource = items;
      pollAction.DisplayMember = "Text";
      //this.pollAction.ValueMember = "Value";
      pollAction.SelectedIndex = 0;

      mTabPages.Clear();
      for (var i = 0; i < tabs.TabPages.Count; i++) {
        mTabPages.Add(tabs.TabPages[i].Name, tabs.TabPages[i]);
      }

      Init();

      mLoaded = true;
    }

    private void ShowSteamTradesForm_Shown(object sender, EventArgs e) {
      usernameField.Focus();
    }

    private void ShowSteamTradesForm_FormClosing(object sender, FormClosingEventArgs e) {
      // update poller
      SetPolling();

      DialogResult = DialogResult.OK;
    }
    
    private void cancelButton_Click(object sender, EventArgs e) {
      DialogResult = DialogResult.Cancel;
    }

    private void tabControl1_DrawItem(object sender, DrawItemEventArgs e) {
      var page = tabs.TabPages[e.Index];
      e.Graphics.FillRectangle(new SolidBrush(page.BackColor), e.Bounds);

      var paddedBounds = e.Bounds;
      var yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
      paddedBounds.Offset(1, yOffset);
      TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, page.ForeColor);

      captchaGroup.BackColor = page.BackColor;
    }

    private void captchaButton_Click(object sender, EventArgs e) {
      if (captchacodeField.Text.Trim().Length == 0) {
        MainForm.ErrorDialog(this, "Please enter the characters in the image", null, MessageBoxButtons.OK);
        return;
      }

      Process(usernameField.Text.Trim(), passwordField.Text.Trim(), AuthenticatorData.GetClient().CaptchaId,
        captchacodeField.Text.Trim());
    }

    private void loginButton_Click(object sender, EventArgs e) {
      if (usernameField.Text.Trim().Length == 0 || passwordField.Text.Trim().Length == 0) {
        MainForm.ErrorDialog(this, "Please enter your username and password", null, MessageBoxButtons.OK);
        return;
      }

      Process(usernameField.Text.Trim(), passwordField.Text.Trim());
    }

    private void closeButton_Click(object sender, EventArgs e) {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void ShowSteamTradesForm_KeyPress(object sender, KeyPressEventArgs e) {
      if (e.KeyChar == 13) {
        switch (tabs.SelectedTab.Name) {
          case "loginTab":
            e.Handled = true;

            if (AuthenticatorData.GetClient().RequiresCaptcha) {
              captchaButton_Click(sender, new EventArgs());
            }
            else {
              loginButton_Click(sender, new EventArgs());
            }

            break;
          case "tradesTab":
            e.Handled = true;
            //authcodeButton_Click(sender, new EventArgs());
            break;
          default:
            e.Handled = false;
            break;
        }

        return;
      }

      e.Handled = false;
    }

    private void Trade_Click(object sender, EventArgs e) {
      // get the Trade object
      var panel = sender as Label;
      var trade = mTrades.Where(t => t.Id == panel.Tag as string).FirstOrDefault();
      if (trade == null) {
        return;
      }

      // inject browser
      TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel browser;
      if (mBrowserHeight != 0) {
        tradesContainer.Height -= mBrowserHeight;
        browserContainer.Height = mBrowserHeight;
        mBrowserHeight = 0;

        browser = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
        browser.Dock = DockStyle.Fill;
        browser.Location = new Point(0, 0);
        browser.Size = new Size(browserContainer.Width, browserContainer.Height);
        browserContainer.Controls.Add(browser);
      }
      else {
        browser = browserContainer.Controls[0] as TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel;
      }

      // pull details segment and merge with normal html
      var cursor = Cursor.Current;
      try {
        Cursor.Current = Cursors.WaitCursor;
        Application.DoEvents();

        browser.Text = AuthenticatorData.GetClient().GetConfirmationDetails(trade);
      }
      catch (Exception ex) {
        browser.Text = "<html><head></head><body><p>" + ex.Message + "</p>" +
                       ex.StackTrace.Replace(Environment.NewLine, "<br>") + "</body></html>";
      }
      finally {
        Cursor.Current = cursor;
      }
    }

    private async void tradeAccept_Click(object sender, EventArgs e) {
      var cursor = Cursor.Current;
      try {
        Cursor.Current = Cursors.WaitCursor;
        Application.DoEvents();

        var button = sender as Button;
        var tradeId = button.Tag as string;
        await AcceptTrade(tradeId);
      }
      finally {
        Cursor.Current = cursor;
      }
    }

    private async void tradeReject_Click(object sender, EventArgs e) {
      var cursor = Cursor.Current;
      try {
        Cursor.Current = Cursors.WaitCursor;
        Application.DoEvents();

        var button = sender as Button;
        var tradeId = button.Tag as string;
        await RejectTrade(tradeId);
      }
      finally {
        Cursor.Current = cursor;
      }
    }

    private void refreshButton_Click(object sender, EventArgs e) {
      Process();
    }

    private void logoutButton_Click(object sender, EventArgs e) {
      var steam = AuthenticatorData.GetClient();
      steam.Logout();

      if (String.IsNullOrEmpty(AuthenticatorData.SessionData) == false) {
        AuthenticatorData.SessionData = null;
        //AuthenticatorData.PermSession = false;
        Authenticator.MarkChanged();
      }

      Init();
    }

    private async void confirmAllButton_Click(object sender, EventArgs e) {
      if (cancelComfirmAll != null) {
        confirmAllButton.Text = "Stopping";
        cancelComfirmAll.Cancel();
        return;
      }

      if (MainForm.ConfirmDialog(this, "This will CONFIRM all your current trade confirmations." + Environment.NewLine +
                                       Environment.NewLine +
                                       "Are you sure you want to continue?",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) {
        return;
      }

      cancelComfirmAll = new CancellationTokenSource();

      var cursor = Cursor.Current;
      try {
        Cursor.Current = Cursors.WaitCursor;
        Application.DoEvents();

        confirmAllButton.Tag = confirmAllButton.Text;
        confirmAllButton.Text = "Stop";
        cancelAllButton.Enabled = false;
        closeButton.Enabled = false;

        var rand = new Random();
        var tradeIds = mTrades.Select(t => t.Id).Reverse().ToArray();
        for (var i = tradeIds.Length - 1; i >= 0; i--) {
          if (cancelComfirmAll.IsCancellationRequested) {
            break;
          }

          var start = DateTime.Now;

          var result = await AcceptTrade(tradeIds[i]);
          if (result == false || cancelComfirmAll.IsCancellationRequested) {
            break;
          }

          if (i != 0) {
            var duration = (int) DateTime.Now.Subtract(start).TotalMilliseconds;
            var delay = SteamClient.CONFIRMATION_EVENT_DELAY +
                        rand.Next(SteamClient.CONFIRMATION_EVENT_DELAY /
                                  2); // delay is 100%-150% of CONFIRMATION_EVENT_DELAY
            if (delay > duration) {
              await Task.Run(() => { Thread.Sleep(delay - duration); }, cancelComfirmAll.Token);
            }
          }
        }
      }
      finally {
        cancelComfirmAll = null;

        confirmAllButton.Text = (string) confirmAllButton.Tag;

        cancelAllButton.Enabled = true;
        closeButton.Enabled = true;

        Authenticator.MarkChanged();

        Cursor.Current = cursor;
      }
    }

    private async void cancelAllButton_Click(object sender, EventArgs e) {
      if (cancelCancelAll != null) {
        cancelAllButton.Text = "Stopping";
        cancelCancelAll.Cancel();
        return;
      }

      if (MainForm.ConfirmDialog(this, "This will CANCEL all your current trade confirmations." + Environment.NewLine +
                                       Environment.NewLine +
                                       "Are you sure you want to continue?",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) {
        return;
      }

      cancelCancelAll = new CancellationTokenSource();

      var cursor = Cursor.Current;
      try {
        Cursor.Current = Cursors.WaitCursor;
        Application.DoEvents();

        cancelAllButton.Tag = cancelAllButton.Text;
        cancelAllButton.Text = "Stop";
        confirmAllButton.Enabled = false;
        closeButton.Enabled = false;

        var rand = new Random();
        var tradeIds = mTrades.Select(t => t.Id).Reverse().ToArray();
        for (var i = tradeIds.Length - 1; i >= 0; i--) {
          if (cancelCancelAll.IsCancellationRequested) {
            break;
          }

          var start = DateTime.Now;

          var result = await RejectTrade(tradeIds[i]);
          if (result == false || cancelCancelAll.IsCancellationRequested) {
            break;
          }

          if (i != 0) {
            var duration = (int) DateTime.Now.Subtract(start).TotalMilliseconds;
            var delay = SteamClient.CONFIRMATION_EVENT_DELAY +
                        rand.Next(SteamClient.CONFIRMATION_EVENT_DELAY /
                                  2); // delay is 100%-150% of CONFIRMATION_EVENT_DELAY
            if (delay > duration) {
              await Task.Run(() => { Thread.Sleep(delay - duration); }, cancelCancelAll.Token);
            }
          }
        }
      }
      finally {
        cancelCancelAll = null;

        cancelAllButton.Text = (string) cancelAllButton.Tag;

        confirmAllButton.Enabled = true;
        closeButton.Enabled = true;

        Authenticator.MarkChanged();

        Cursor.Current = cursor;
      }
    }

    private void pollAction_SelectedIndexChanged(object sender, EventArgs e) {
      // display autoconfirm warning
      if (mLoaded
          && pollAction.SelectedValue != null
          && pollAction.SelectedValue is SteamClient.PollerAction
          && ((SteamClient.PollerAction) pollAction.SelectedValue == SteamClient.PollerAction.AutoConfirm ||
              (SteamClient.PollerAction) pollAction.SelectedValue == SteamClient.PollerAction.SilentAutoConfirm)
          && autoWarned == false) {
        if (MainForm.ConfirmDialog(this,
              "WARNING: Using auto-confirm will automatically confirm all new Confirmations on your "
              + "account. Do not use this unless you want to ignore trade confirmations." + Environment.NewLine +
              Environment.NewLine
              + "This WILL remove items from your inventory." + Environment.NewLine + Environment.NewLine
              + "Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
              MessageBoxDefaultButton.Button2) != DialogResult.Yes) {
          pollAction.SelectedIndex = 0;
        }
        else {
          autoWarned = true;
        }
      }
    }

    #endregion

    #region Private methods

    private void Init() {
      if (AuthenticatorData.GetClient().IsLoggedIn()) {
        Process();
      }
      else {
        ShowTab("loginTab");

        closeButton.Visible = false;
        cancelButton.Visible = true;
        refreshButton.Visible = false;
        logoutButton.Visible = false;
        pollPanel.Visible = false;
        confirmAllButton.Visible = false;
        cancelAllButton.Visible = false;
      }
    }

    private void Process(string username = null, string password = null, string captchaId = null,
      string captchaText = null) {
      do {
        var cursor = Cursor.Current;
        try {
          //Application.DoEvents();
          Cursor.Current = Cursors.WaitCursor;
          Application.DoEvents();

          var steam = AuthenticatorData.GetClient();

          if (steam.IsLoggedIn() == false) {
            if (steam.Login(username, password, captchaId, captchaText) == false) {
              if (steam.Error == "Incorrect Login") {
                MainForm.ErrorDialog(this, "Invalid username or password", null, MessageBoxButtons.OK);
                return;
              }

              if (steam.Requires2Fa) {
                MainForm.ErrorDialog(this,
                  "Invalid authenticator code: are you sure this is the current authenticator for your account?", null,
                  MessageBoxButtons.OK);
                return;
              }

              if (steam.RequiresCaptcha) {
                MainForm.ErrorDialog(this,
                  (string.IsNullOrEmpty(steam.Error) == false ? steam.Error : "Please enter the captcha"), null,
                  MessageBoxButtons.OK);

                using (var web = new WebClient()) {
                  var data = web.DownloadData(steam.CaptchaUrl);

                  using (var ms = new MemoryStream(data)) {
                    captchaBox.Image = Image.FromStream(ms);
                  }
                }

                loginButton.Enabled = false;
                captchaGroup.Visible = true;
                captchacodeField.Text = "";
                captchacodeField.Focus();

                return;
              }

              loginButton.Enabled = true;
              captchaGroup.Visible = false;

              if (string.IsNullOrEmpty(steam.Error) == false) {
                MainForm.ErrorDialog(this, steam.Error, null, MessageBoxButtons.OK);
                return;
              }

              if (tabs.TabPages.ContainsKey("authTab")) {
                tabs.TabPages.RemoveByKey("authTab");
              }

              ShowTab("loginTab");
              usernameField.Focus();

              return;
            }

            AuthenticatorData.SessionData = (rememberBox.Checked ? steam.Session.ToString() : null);
            //AuthenticatorData.PermSession = (rememberBox.Checked == true && rememberPermBox.Checked == true);
            Authenticator.MarkChanged();
          }

          try {
            mTrades = steam.GetConfirmations();

            // save after get new trades
            if (string.IsNullOrEmpty(AuthenticatorData.SessionData) == false) {
              Authenticator.MarkChanged();
            }
          }
          catch (SteamClient.UnauthorisedSteamRequestException) {
            // Family view is probably on
            MainForm.ErrorDialog(this,
              "You are not allowed to view confirmations. Have you enabled 'community-generated content' in Family View?",
              null, MessageBoxButtons.OK);
            return;
          }
          catch (SteamClient.InvalidSteamRequestException) {
            // likely a bad session so try a refresh first
            try {
              steam.Refresh();
              mTrades = steam.GetConfirmations();
            }
            catch (Exception) {
              // reset and show normal login
              steam.Clear();
              Init();
              return;
            }
          }

          Cursor.Current = cursor;

          var tab = ShowTab("tradesTab");

          tab.SuspendLayout();
          tradesContainer.Controls.Remove(tradePanelMaster);
          foreach (var control in tradesContainer.Controls.Cast<Control>().ToArray()) {
            if (control is Panel) {
              tradesContainer.Controls.Remove(control);
            }
          }

          for (var row = 0; row < mTrades.Count; row++) {
            var trade = mTrades[row];

            // clone the panel
            var tradePanel = Clone(tradePanelMaster, "_" + trade.Id) as Panel;
            tradePanel.SuspendLayout();

            using (var wc = new WebClient()) {
              byte[] imageData = null;

              try {
                imageData = wc.DownloadData(trade.Image);
              }
              catch (WebException ex) {
                // ignore error 404 for missing images
                if (((HttpWebResponse) ex.Response).StatusCode != HttpStatusCode.NotFound) {
                  throw;
                }
              }

              if (imageData != null && imageData.Length != 0) {
                using (var ms = new MemoryStream(imageData)) {
                  var imageBox = FindControl<PictureBox>(tradePanel, "tradeImage");
                  imageBox.Image = Image.FromStream(ms);
                }
              }
            }

            var label = FindControl<Label>(tradePanel, "tradeLabel");
            label.Text = trade.Details + Environment.NewLine + trade.Traded + Environment.NewLine + trade.When;
            label.Tag = trade.Id;
            label.Click += Trade_Click;

            var tradeAcceptButton = FindControl<Button>(tradePanel, "tradeAccept");
            tradeAcceptButton.Tag = trade.Id;
            tradeAcceptButton.Click += tradeAccept_Click;

            var tradeRejectButton = FindControl<Button>(tradePanel, "tradeReject");
            tradeRejectButton.Tag = trade.Id;
            tradeRejectButton.Click += tradeReject_Click;

            tradePanel.Top = tradePanel.Height * row;

            tradePanel.ResumeLayout();

            tradesContainer.Controls.Add(tradePanel);
          }

          tradesEmptyLabel.Visible = (mTrades.Count == 0);

          confirmAllButton.Visible = (mTrades.Count != 0);
          cancelAllButton.Visible = (mTrades.Count != 0);

          tab.ResumeLayout();

          closeButton.Location = cancelButton.Location;
          closeButton.Visible = true;
          cancelButton.Visible = false;
          refreshButton.Visible = true;
          if (string.IsNullOrEmpty(AuthenticatorData.SessionData) == false) {
            logoutButton.Visible = true;

            if (steam.Session.Confirmations != null) {
              pollCheckbox.Checked = true;
              pollNumeric.Value = Convert.ToDecimal(steam.Session.Confirmations.Duration);
              var selected = 0;
              for (var i = 0; i < pollAction.Items.Count; i++) {
                var item = pollAction.Items[i] as PollerActionItem;
                if (item != null && item.Value == steam.Session.Confirmations.Action) {
                  selected = i;

                  if (steam.Session.Confirmations.Action == SteamClient.PollerAction.AutoConfirm ||
                      steam.Session.Confirmations.Action == SteamClient.PollerAction.SilentAutoConfirm) {
                    autoWarned = true;
                  }

                  break;
                }
              }

              pollAction.SelectedIndex = selected;
            }
            else {
              pollCheckbox.Checked = false;
              pollAction.SelectedIndex = 0;
            }

            pollPanel.Visible = true;
            confirmAllButton.Visible = true;
            cancelAllButton.Visible = true;
          }

          break;
        }
        catch (SteamClient.InvalidSteamRequestException iere) {
          Cursor.Current = cursor;
          if (MainForm.ErrorDialog(this, "An error occurred while loading trades", iere,
                MessageBoxButtons.RetryCancel) != DialogResult.Retry) {
            break;
          }
        }
        finally {
          Cursor.Current = cursor;
        }
      } while (true);
    }

    private async Task<bool> AcceptTrade(string tradeId) {
      try {
        var trade = mTrades.Where(t => t.Id == tradeId).FirstOrDefault();
        if (trade == null) {
          throw new ApplicationException("Invalid trade");
        }

        var result = await Task.Factory.StartNew<bool>(
          (t) => {
            return AuthenticatorData.GetClient().ConfirmTrade(((SteamClient.Confirmation) t).Id,
              ((SteamClient.Confirmation) t).Key, true);
          }, trade);
        if (result == false) {
          throw new ApplicationException("Trade cannot be confirmed");
        }

        mTrades.Remove(trade);

        var button = FindControl<Button>(tabs.SelectedTab, "tradeAccept_" + trade.Id);
        button.Visible = false;
        button = FindControl<Button>(tabs.SelectedTab, "tradeReject_" + trade.Id);
        button.Visible = false;
        var status = FindControl<Label>(tabs.SelectedTab, "tradeStatus_" + trade.Id);
        status.Text = "Confirmed";
        status.Visible = true;

        return true;
      }
      catch (InvalidTradesResponseException ex) {
        MainForm.ErrorDialog(this, "Error trying to accept trade", ex, MessageBoxButtons.OK);
        return false;
      }
      catch (ApplicationException ex) {
        MainForm.ErrorDialog(this, ex.Message);
        return false;
      }
    }

    private async Task<bool> RejectTrade(string tradeId) {
      try {
        var trade = mTrades.Where(t => t.Id == tradeId).FirstOrDefault();
        if (trade == null) {
          throw new ApplicationException("Invalid trade");
        }

        var result = await Task.Factory.StartNew<bool>(
          (t) => {
            return AuthenticatorData.GetClient().ConfirmTrade(((SteamClient.Confirmation) t).Id,
              ((SteamClient.Confirmation) t).Key, false);
          }, trade);
        if (result == false) {
          throw new ApplicationException("Trade cannot be cancelled");
        }

        mTrades.Remove(trade);

        var button = FindControl<Button>(tabs.SelectedTab, "tradeAccept_" + trade.Id);
        button.Visible = false;
        button = FindControl<Button>(tabs.SelectedTab, "tradeReject_" + trade.Id);
        button.Visible = false;
        var status = FindControl<Label>(tabs.SelectedTab, "tradeStatus_" + trade.Id);
        status.Text = "Cancelled";
        status.Visible = true;

        return true;
      }
      catch (InvalidTradesResponseException ex) {
        MainForm.ErrorDialog(this, "Error trying to reject trade", ex, MessageBoxButtons.OK);
        return false;
      }
      catch (ApplicationException ex) {
        MainForm.ErrorDialog(this, ex.Message);
        return false;
      }
    }

    private T FindControl<T>(Control control, string name) where T : Control {
      if (control.Name.StartsWith(name) && control is T) {
        return (T) control;
      }

      foreach (Control child in control.Controls) {
        var found = FindControl<T>(child, name);
        if (found != null) {
          return found;
        }
      }

      return null;
    }

    private Control Clone(Control control, string suffix) {
      var type = control.GetType();
      var clone = Activator.CreateInstance(type) as Control;
      clone.Name = control.Name + (string.IsNullOrEmpty(suffix) == false ? suffix : string.Empty);

      clone.SuspendLayout();
      if (clone is ISupportInitialize) {
        ((ISupportInitialize) (clone)).BeginInit();
      }

      // copy public properties
      foreach (var pi in type.GetProperties(System.Reflection.BindingFlags.FlattenHierarchy |
                                            System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.Public)) {
        if (pi.CanWrite == false || pi.CanRead == false) {
          continue;
        }

        if (pi.Name == "Controls" || pi.Name == "Name" || pi.Name == "WindowTarget") {
          continue;
        }

        var value = pi.GetValue(control, (object[]) null);
        if (value != null && value.GetType().IsValueType == false) {
          if (value is ICloneable) {
            value = ((ICloneable) value).Clone();
          }
          else if (value is ISerializable) {
            var newvalue = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value));
            if (newvalue.GetType() == value.GetType()) {
              value = newvalue;
            }
          }
        }

        pi.SetValue(clone, value, (object[]) null);
      }

      // copy child controls
      if (control.Controls != null) {
        foreach (Control child in control.Controls) {
          clone.Controls.Add(Clone(child, suffix));
        }
      }

      clone.ResumeLayout();
      if (clone is ISupportInitialize) {
        ((ISupportInitialize) (clone)).EndInit();
      }

      return clone;
    }

    private TabPage ShowTab(string name, bool only = true) {
      if (only) {
        tabs.TabPages.Clear();
      }

      if (tabs.TabPages.ContainsKey(name) == false) {
        tabs.TabPages.Add(mTabPages[name]);
      }

      tabs.SelectedTab = tabs.TabPages[name];
      if (name == "loginTab") {
        // oddity with MetroFrame controls in have to set focus away and back to field to make it stick
        Invoke((MethodInvoker) delegate {
          passwordField.Focus();
          usernameField.Focus();
        });
      }

      return tabs.SelectedTab;
    }

    private void SetPolling() {
      // ignore setup changes
      if (mLoaded == false || pollAction.SelectedValue == null) {
        return;
      }

      var steam = AuthenticatorData.GetClient();
      var timeInMins = (pollCheckbox.Checked && steam.IsLoggedIn() ? (int) pollNumeric.Value : 0);

      var p = new SteamClient.ConfirmationPoller {
        Duration = (pollCheckbox.Checked && steam.IsLoggedIn() ? (int) pollNumeric.Value : 0),
        Action = ((PollerActionItem) pollAction.SelectedValue).Value
      };
      if (p.Duration != 0) {
        if (steam.Session.Confirmations == null || steam.Session.Confirmations.Duration != p.Duration ||
            steam.Session.Confirmations.Action != p.Action) {
          steam.PollConfirmations(p);
          AuthenticatorData.SessionData = steam.Session.ToString();
          Authenticator.MarkChanged();
        }
      }
      else {
        if (steam.Session.Confirmations != null) {
          steam.PollConfirmations(null);
          AuthenticatorData.SessionData = steam.Session.ToString();
          Authenticator.MarkChanged();
        }
      }
    }

    #endregion
  }
}