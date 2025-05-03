using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sergiye.Common;

namespace Authenticator {
  public partial class MainForm : Form {
    
    private NotifyIcon notifyIcon;
    private ContextMenuStrip notifyMenu;

    public MainForm() {
      InitializeComponent();
    }

    #region Properties

    public AuthConfig Config { get; set; }

    private DateTime? saveConfigTime;

    private bool mExplicitClose;

    private bool initiallyMinimised;

    private string startupConfigFile;

    private bool mInitOnce;

    #endregion

    private void MainForm_Load(object sender, EventArgs e) {
      //Text = $"Authenticator {(Environment.Is64BitProcess ? "x64" : "x32")} - {Updater.CurrentVersion}";
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
      authenticatorList.ItemHeight = 50;
      MinimumSize = new Size(200, mainMenu.Height + Height - ClientRectangle.Height + authenticatorList.ItemHeight);

      //will display prompt only if update available & when main form displayed
      var timer = new Timer();
      timer.Interval = 3000;
      timer.Tick += async (s, eArgs) => {
        timer.Enabled = false;
        timer.Enabled = ! await Updater.CheckForUpdatesAsync(true).ConfigureAwait(false);
      };
      timer.Enabled = true; 

      // get any command arguments
      string password = null;
      string proxy = null;
      var args = Environment.GetCommandLineArgs();
      for (var i = 1; i < args.Length; i++) {
        var arg = args[i];
        if (arg[0] == '-') {
          switch (arg) {
            case "-min":
            case "--minimize":
              // set initial state as minimized
              initiallyMinimised = true;
              break;
            case "-p":
            case "--password":
              // set explicit password to use
              i++;
              password = args[i];
              break;
            case "--proxy":
              // set proxy [user[:pass]@]ip[:host]
              i++;
              proxy = args[i];
              break;
          }
        }
        else {
          startupConfigFile = arg;
        }
      }

      // set the default web proxy
      if (string.IsNullOrEmpty(proxy) == false) {
        try {
          var uri = new Uri(proxy.IndexOf("://", StringComparison.Ordinal) == -1 ? "http://" + proxy : proxy);
          var webProxy = new WebProxy(uri.Host + ":" + uri.Port, true);
          if (string.IsNullOrEmpty(uri.UserInfo) == false) {
            var auth = uri.UserInfo.Split(':');
            webProxy.Credentials = new NetworkCredential(auth[0], (auth.Length > 1 ? auth[1] : string.Empty));
          }
          WebRequest.DefaultWebProxy = webProxy;
        }
        catch (UriFormatException) {
          ErrorDialog(this,
            "Invalid proxy value (" + proxy + ")" + Environment.NewLine + Environment.NewLine +
            "Use --proxy [user[:password]@]ip[:port], e.g. 127.0.0.1:8080 or me:mypass@10.0.0.1:8080");
          Close();
        }
      }

      InitializeOnce();

      LoadConfig(password);
    }

    #region Private Methods

    private void LoadConfig(string password) {
      loadingPanel.Visible = true;
      passwordPanel.Visible = false;

      Task.Factory.StartNew(() => {
        try {
          // use previous config if we have one
          var config = AuthHelper.LoadConfig(startupConfigFile, password);
          return new Tuple<AuthConfig, Exception>(config, null);
        }
        catch (Exception ex) {
          return new Tuple<AuthConfig, Exception>(null, ex);
        }
      }).ContinueWith((configTask) => {
        var ex = configTask.Result.Item2;
        switch (ex) {
          case AuthInvalidNewerConfigException _:
            MessageBox.Show(this, ex.Message, AuthHelper.APPLICATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            return;
          case EncryptedSecretDataException _:
            loadingPanel.Visible = false;
            passwordPanel.Visible = true;

            passwordButton.Focus();
            passwordField.Focus();

            return;
          case BadPasswordException _:
            loadingPanel.Visible = false;
            passwordPanel.Visible = true;
            passwordErrorLabel.Text = "Invalid password";
            passwordErrorLabel.Tag = DateTime.Now.AddSeconds(3);
            // oddity with MetroFrame controls in have to set focus away and back to field to make it stick
            Invoke((MethodInvoker) delegate {
              passwordButton.Focus();
              passwordField.Focus();
            });
            passwordTimer.Enabled = true;
            return;
        }

        if (ex != null) {
          if (ErrorDialog(this, "An unknown error occured: " + ex.Message, ex, MessageBoxButtons.RetryCancel) ==
              DialogResult.Cancel) {
            Close();
            return;
          }

          LoadConfig(password);
          return;
        }

        var config = configTask.Result.Item1;
        if (config == null) {
          System.Diagnostics.Process.GetCurrentProcess().Kill();
          return;
        }

        Config = config;
        Config.OnConfigChanged += OnConfigChanged;

        if (config.Upgraded) {
          SaveConfig(true);
          // display warning
          ErrorDialog(this, string.Format("Authenticator has upgraded your authenticators to version {0}.\nDo NOT run an older version of Authenticator as this could overwrite them.\nNow is a good time to make a backup. Click the Options icon and choose Export.", AuthConfig.CurrentVersion));
        }

        InitializeForm();
      }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private void ImportAuthenticator(string authenticatorFile) {
      // call legacy import for v2 xml files
      if (String.Compare(Path.GetExtension(authenticatorFile), ".config", StringComparison.OrdinalIgnoreCase) == 0) {
        ImportAuthenticatorFromV2(authenticatorFile);
        return;
      }

      List<AuthAuthenticator> authenticators = null;
      bool retry;
      do {
        retry = false;
        try {
          authenticators = AuthHelper.ImportAuthenticators(this, authenticatorFile);
        }
        catch (ImportException ex) {
          if (ErrorDialog(this, ex.Message, ex.InnerException, MessageBoxButtons.RetryCancel) == DialogResult.Cancel) {
            return;
          }

          retry = true;
        }
      } while (retry);

      if (authenticators == null) {
        return;
      }

      // save all the new authenticators
      foreach (var authenticator in authenticators) {
        //sync
        authenticator.Sync();

        // make sure there isn't a name clash
        var rename = 0;
        var importedName = authenticator.Name;
        while (Config.Where(a => a.Name == importedName).Count() != 0) {
          importedName = authenticator.Name + " " + (++rename);
        }

        authenticator.Name = importedName;

        // first time we prompt for protection and set out main settings from imported config
        if (Config.Count == 0) {
          var form = new ChangePasswordForm {
            PasswordType = Authenticator.PasswordTypes.Explicit
          };
          if (form.ShowDialog(this) == DialogResult.OK) {
            Config.PasswordType = form.PasswordType;
            if ((Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0 &&
                string.IsNullOrEmpty(form.Password) == false) {
              Config.Password = form.Password;
            }
          }
        }

        // add to main list
        Config.Add(authenticator);
      }

      SaveConfig(true);
      LoadAuthenticatorList();

      // reset UI
      SetAutoSize();
   }

    private void ImportAuthenticatorFromV2(string authenticatorFile) {
      var retry = false;
      string password = null;
      var invalidPassword = false;
      do {
        bool needPassword;
        try {
          var config = AuthHelper.LoadConfig(authenticatorFile, password);
          if (config.Count == 0) {
            return;
          }

          // get the actual authenticator and ensure it is synced
          var imported = new List<AuthAuthenticator>();
          foreach (var importedAuthenticator in config) {
            importedAuthenticator.Sync();

            // make sure there isn't a name clash
            var rename = 0;
            var importedName = importedAuthenticator.Name;
            while (Config.Any(a => a.Name == importedName)) {
              importedName = importedAuthenticator.Name + " (" + (++rename) + ")";
            }

            importedAuthenticator.Name = importedName;
            imported.Add(importedAuthenticator);
          }

          // first time we prompt for protection and set out main settings from imported config
          if (Config.Count == 0) {
            Config.StartWithWindows = config.StartWithWindows;
            Config.UseTrayIcon = config.UseTrayIcon;
            Config.AlwaysOnTop = config.AlwaysOnTop;

            var form = new ChangePasswordForm {
              PasswordType = Authenticator.PasswordTypes.Explicit
            };
            if (form.ShowDialog(this) == DialogResult.OK) {
              Config.PasswordType = form.PasswordType;
              if ((Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0 &&
                  string.IsNullOrEmpty(form.Password) == false) {
                Config.Password = form.Password;
              }
            }
          }

          foreach (var auth in imported) {
            // add to main list
            Config.Add(auth);
            LoadAuthenticatorList(auth);
          }

          SaveConfig(true);

          // reset UI
          SetAutoSize();

          needPassword = false;
          retry = false;
        }
        catch (EncryptedSecretDataException) {
          needPassword = true;
          invalidPassword = false;
        }
        catch (BadPasswordException) {
          needPassword = true;
          invalidPassword = true;
        }
        catch (Exception ex) {
          if (ErrorDialog(this, "An unknown error occured: " + ex.Message, ex, MessageBoxButtons.RetryCancel) ==
              DialogResult.Cancel) {
            return;
          }

          needPassword = false;
          invalidPassword = false;
          retry = true;
        }

        if (needPassword) {
          var form = new GetPasswordForm {
            InvalidPassword = invalidPassword
          };
          var result = form.ShowDialog(this);
          if (result == DialogResult.Cancel) {
            return;
          }

          password = form.Password;
          retry = true;
        }
      } while (retry);
    }

    private void InitializeOnce() {
      if (mInitOnce) return;
      // hook into System time change event
      Microsoft.Win32.SystemEvents.TimeChanged += SystemEvents_TimeChanged;

      // redirect mouse wheel events
      new WinApi.MessageForwarder(authenticatorList, WinApi.WM_MOUSEWHEEL);

      mInitOnce = true;
    }

    private void InitializeForm() {
      // set up list
      LoadAuthenticatorList();

      // set always on top
      TopMost = Config.AlwaysOnTop;

      // size the form based on the authenticators
      SetAutoSize();

      // initialize UI
      LoadMainMenu();
      
      notifyMenu = new ContextMenuStrip(components);
      notifyMenu.Opening += (s, e) => OpeningNotifyMenu(notifyMenu, e);
      LoadNotifyMenu(notifyMenu.Items);

      notifyIcon = new NotifyIcon(components);
      notifyIcon.Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
      notifyIcon.DoubleClick += ShowHideMenuItem_Click;
      notifyIcon.ContextMenuStrip = notifyMenu;
      notifyIcon.Visible = Config.UseTrayIcon;

      // set title
      notifyIcon.Text = Text = AuthHelper.APPLICATION_TITLE;

      loadingPanel.Visible = false;
      passwordPanel.Visible = false;
      addToolStripMenuItem.Enabled = !Config.IsReadOnly;
      authenticatorList.Focus();

      // set positions
      if (Config.Position.IsEmpty == false) {
        // check we aren't out of bounds in case of multi-monitor change
        var v = SystemInformation.VirtualScreen;
        if ((Config.Position.X + Width) >= v.Left && Config.Position.X < v.Width &&
            Config.Position.Y > v.Top) {
          try {
            StartPosition = FormStartPosition.Manual;
            Left = Config.Position.X;
            Top = Config.Position.Y;
          }
          catch (Exception) {
            // ignored
          }
        }

        // check we aren't below the taskbar
        var lowesty = Screen.GetWorkingArea(this).Bottom;
        var bottom = Top + Height;
        if (bottom > lowesty) {
          Top -= (bottom - lowesty);
          if (Top < 0) {
            Height += Top;
            Top = 0;
          }
        }
      }
      else if (Config.AutoSize) {
        CenterToScreen();
      }

      // if we passed "-min" flag
      if (initiallyMinimised) {
        WindowState = FormWindowState.Minimized;
        ShowInTaskbar = true;
      }

      if (Config.UseTrayIcon) {
        // if initially minimized, we need to hide
        if (WindowState == FormWindowState.Minimized) {
          Hide();
        }
      }
    }

    private void LoadAuthenticatorList(AuthAuthenticator added = null) {
      // set up list
      authenticatorList.Items.Clear();

      var index = 0;
      foreach (var auth in Config) {
        var ali = new AuthenticatorListBox.ListItem(auth, index);
        if (added != null && added == auth && auth.AutoRefresh == false &&
            !(auth.AuthenticatorData is HotpAuthenticator)) {
          ali.LastUpdate = DateTime.Now;
          ali.DisplayUntil = DateTime.Now.AddSeconds(10);
        }

        authenticatorList.Items.Add(ali);
        index++;
      }

      authenticatorList.Visible = (authenticatorList.Items.Count != 0);
    }

    private void SaveConfig(bool immediate = false) {
      if (immediate || (saveConfigTime != null && saveConfigTime <= DateTime.Now)) {
        saveConfigTime = null;
        lock (Config) {
          AuthHelper.SaveConfig(Config);
        }
      }
      else {
        // save it in a few seconds so we can batch up saves
        saveConfigTime = DateTime.Now.AddSeconds(1);
      }
    }

    public static DialogResult ErrorDialog(Form form, string message = null, Exception ex = null,
      MessageBoxButtons buttons = MessageBoxButtons.OK) {
      if (message == null) {
        message = $"An error has occurred{(ex != null ? ": " + ex.Message : string.Empty)}";
      }

      if (ex != null && string.IsNullOrEmpty(ex.Message) == false) {
        message += Environment.NewLine + Environment.NewLine + ex.Message;
      }
#if DEBUG
      var capture = new StringBuilder();
      var e = ex;
      while (e != null) {
        capture.Append(new System.Diagnostics.StackTrace(e)).Append(Environment.NewLine);
        e = e.InnerException;
      }

      message += Environment.NewLine + Environment.NewLine + capture;

      if (ex != null) {
        AuthHelper.ShowException(ex);
      }
#endif

      return MessageBox.Show(form, message, AuthHelper.APPLICATION_TITLE, buttons, MessageBoxIcon.Exclamation);
    }

    public static DialogResult ConfirmDialog(Form form, string message,
      MessageBoxButtons buttons = MessageBoxButtons.YesNo, MessageBoxIcon icon = MessageBoxIcon.Question,
      MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1) {
      return MessageBox.Show(form, message, AuthHelper.APPLICATION_TITLE, buttons, icon, defaultButton);
    }

    protected override void WndProc(ref Message m) {
      base.WndProc(ref m);

      switch (m.Msg) {
        // pick up the HotKey message from RegisterHotKey and call hook callback
        case WinApi.WM_USER + 1:
          // show the main form
          BringToFront();
          Show();
          WindowState = FormWindowState.Normal;
          Activate();
          break;
      }
    }

    private void RunAction(AuthAuthenticator auth, AuthConfig.NotifyActions action) {
      // get the code
      string code;
      try {
        code = auth.CurrentCode;
      }
      catch (EncryptedSecretDataException) {
        // if the authenticator is current protected we display the password window, get the code, and reprotect it
        // with a bit of window jiggling to make sure we get focus and then put it back

        // save the current window
        var foregroundWindow = WinApi.GetForegroundWindow();
        var screen = Screen.FromHandle(foregroundWindow);
        var activeWindow = IntPtr.Zero;
        if (Visible) {
          activeWindow = WinApi.SetActiveWindow(Handle);
          BringToFront();
        }

        var item = authenticatorList.Items.Cast<AuthenticatorListBox.ListItem>().FirstOrDefault(i => i.Authenticator == auth);
        code = authenticatorList.GetItemCode(item, screen);

        // restore active window
        if (activeWindow != IntPtr.Zero) {
          WinApi.SetActiveWindow(activeWindow);
        }

        WinApi.SetForegroundWindow(foregroundWindow);
      }

      if (code != null) {
        if (action == AuthConfig.NotifyActions.CopyToClipboard) {
          Clipboard.SetText(code);
        }
        else // if (this.Config.NotifyAction == AuthConfig.NotifyActions.Notification)
        {
          if (code.Length > 5) {
            code = code.Insert(code.Length / 2, " ");
          }

          notifyIcon.ShowBalloonTip(10000, auth.Name, code, ToolTipIcon.Info);
        }
      }
    }

    private void SetAutoSize() {
      if (Config.AutoSize) {
        if (Config.Count != 0) {
          Width = Math.Max(this.MinimumSize.Width,
            authenticatorList.Margin.Horizontal + authenticatorList.GetMaxItemWidth() +
            (Width - authenticatorList.Width));
        }
        else {
          Width = 420;
        }

        // take the smallest of full height or 62% screen height
        var maxHeight = Screen.GetWorkingArea(this).Height * 50 / 100; //use only 50% of total screen height 
        var fixedHeight = mainMenu.Height + Height - ClientRectangle.Height; 
        
        Height = fixedHeight + authenticatorList.ItemHeight * Math.Min(Config.Count, (maxHeight - fixedHeight) / authenticatorList.ItemHeight);

        FormBorderStyle = FormBorderStyle.FixedDialog;
      }
      else {
        FormBorderStyle = FormBorderStyle.Sizable;
        if (Config.Width != 0) {
          Width = Config.Width;
        }

        if (Config.Height != 0) {
          Height = Config.Height;
        }
      }
      introLabel.Visible = (Config.Count == 0);
    }

    private void EndRenaming() {
      // set focus to form, so that the edit field will disappear if it is visble
      if (authenticatorList.IsRenaming) {
        authenticatorList.EndRenaming();
      }
    }

    private void MainForm_Shown(object sender, EventArgs e) {
      // if we use tray icon make sure it is set
      if (Config != null && Config.UseTrayIcon) {
        // if initially minizied, we need to hide
        if (WindowState == FormWindowState.Minimized) {
          Hide();
        }
      }
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
      // keep in the tray when closing Form 
      if (Config != null && Config.UseTrayIcon && Visible && mExplicitClose == false) {
        e.Cancel = true;
        Hide();
        return;
      }

      // ensure the notify icon is closed
      notifyIcon.Visible = false;

      // save size if we are not autoresize
      if (Config != null && Config.AutoSize == false &&
          (Config.Width != Width || Config.Height != Height)) {
        Config.Width = Width;
        Config.Height = Height;
      }

      if (Config != null /* && this.Config.Position.IsEmpty == false */) {
        Config.Position = new Point(Left, Top);
      }

      // perform save if we have one pending
      if (saveConfigTime != null) {
        SaveConfig(true);
      }
    }

    void addAuthenticatorMenu_Click(object sender, EventArgs e) {
      var menuitem = (ToolStripItem) sender;
      var registeredauth = menuitem.Tag as RegisteredAuthenticator;
      if (registeredauth != null) {
        // add the new authenticator
        var authenticator = new AuthAuthenticator();
        bool added;

        if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.BattleNet) {
          var existing = 0;
          string name;
          do {
            name = "Battle.net" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListBox.ListItem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          // create the Battle.net authenticator
          var form = new AddBattleNetAuthenticator {
            Authenticator = authenticator
          };
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Trion) {
          // create the Trion authenticator
          var existing = 0;
          string name;
          do {
            name = "Trion" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListBox.ListItem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          var form = new AddTrionAuthenticator {
            Authenticator = authenticator
          };
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.GuildWars) {
          // create the GW2 authenticator
          var existing = 0;
          string name;
          do {
            name = "GuildWars" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListBox.ListItem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          var form = new AddGuildWarsAuthenticator {
            Authenticator = authenticator
          };
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Microsoft) {
          // create the Microsoft authenticator
          var existing = 0;
          string name;
          do {
            name = "Microsoft" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListBox.ListItem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          var form = new AddMicrosoftAuthenticator {
            Authenticator = authenticator
          };
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.RFC6238_TIME_COUNTER) {
          // create the Google authenticator
          // add the new authenticator
          var existing = 0;
          string name;
          do {
            name = "Authenticator" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListBox.ListItem>().Count(a => a.Authenticator.Name == name) != 0);
          authenticator.Name = name;
          authenticator.AutoRefresh = false;
          authenticator.Skin = "AppIcon.png";
          added = new AddAuthenticator(authenticator).ShowDialog(this) == DialogResult.OK;
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.OktaVerify) {
          // create the Okta Verify authenticator
          var existing = 0;
          string name;
          do {
            name = "Okta" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListBox.ListItem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          var form = new AddOktaVerifyAuthenticator {
            Authenticator = authenticator
          };
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else {
          throw new NotImplementedException("Authenticator not implemented: " +
                                            registeredauth.AuthenticatorType);
        }

        if (added) {
          // first time we prompt for protection
          if (Config.Count == 0) {
            var form = new ChangePasswordForm {
              PasswordType = Authenticator.PasswordTypes.Explicit
            };
            if (form.ShowDialog(this) == DialogResult.OK) {
              Config.PasswordType = form.PasswordType;
              if ((Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0 &&
                  string.IsNullOrEmpty(form.Password) == false) {
                Config.Password = form.Password;
              }
            }
          }

          Config.Add(authenticator);
          SaveConfig(true);
          LoadAuthenticatorList(authenticator);

          // reset UI
          SetAutoSize();
        }
      }
    }

    void importTextMenu_Click(object sender, EventArgs e) {
      var ofd = new OpenFileDialog {
        AddExtension = true,
        CheckFileExists = true,
        CheckPathExists = true
      };

      ofd.Filter = "Authenticator Files (*.config)|*.config|Text Files (*.txt)|*.txt|Zip Files (*.zip)|*.zip|PGP Files (*.pgp)|*.pgp|All Files (*.*)|*.*";
      ofd.RestoreDirectory = true;
      ofd.Title = AuthHelper.APPLICATION_TITLE;
      if (ofd.ShowDialog(this) == DialogResult.OK) {
        ImportAuthenticator(ofd.FileName);
      }
    }

    private void mainTimer_Tick(object sender, EventArgs e) {
      authenticatorList.Tick(sender, e);

      // if a save is due
      if (saveConfigTime != null && saveConfigTime.Value <= DateTime.Now) {
        SaveConfig();
      }
    }

    private void authenticatorList_ItemRemoved(object source, AuthenticatorListBox.ListItem args) {
      foreach (var auth in Config) {
        if (auth == args.Authenticator) {
          Config.Remove(auth);
          break;
        }
      }

      // update UI
      SetAutoSize();
      authenticatorList.Refresh();

      // if no authenticators, show intro text and remove any encryption
      if (Config.Count == 0) {
        authenticatorList.Visible = false;
        Config.PasswordType = Authenticator.PasswordTypes.None;
        Config.Password = null;
      }

      // save the current config
      SaveConfig();
    }

    private void authenticatorList_Reordered(object source, EventArgs args) {
      // set the new order of items in Config from that of the list
      var count = authenticatorList.Items.Count;
      for (var i = 0; i < count; i++) {
        var item = (AuthenticatorListBox.ListItem) authenticatorList.Items[i];
        Config.FirstOrDefault(a => a == item.Authenticator).Index = i;
      }

      // resort the config list
      Config.Sort();
      // update the notify menu
      LoadNotifyMenu(notifyMenu.Items);

      // update UI
      SetAutoSize();

      // save the current config
      SaveConfig();
    }

    private void authenticatorList_DoubleClick(object source, AuthAuthenticator args) {
      RunAction(args, AuthConfig.NotifyActions.CopyToClipboard);
    }

    private void MainForm_MouseDown(object sender, MouseEventArgs e) {
      EndRenaming();
    }

    private void MainForm_ResizeEnd(object sender, EventArgs e) {
      if (Config != null && Config.AutoSize == false) {
        Config.Width = Width;
        Config.Height = Height;
      }
    }

    private void passwordButton_Click(object sender, EventArgs e) {
      if (passwordField.Text.Trim().Length == 0) {
        passwordErrorLabel.Text = "Please enter a password";
        passwordErrorLabel.Tag = DateTime.Now.AddSeconds(3);
        passwordTimer.Enabled = true;
        return;
      }

      LoadConfig(passwordField.Text);
      passwordField.Text = string.Empty;
    }

    private void passwordTimer_Tick(object sender, EventArgs e) {
      if (passwordErrorLabel.Tag != null && (DateTime) passwordErrorLabel.Tag <= DateTime.Now) {
        passwordTimer.Enabled = false;
        passwordErrorLabel.Tag = null;
        passwordErrorLabel.Text = string.Empty;
      }
    }

    private void passwordField_KeyPress(object sender, KeyPressEventArgs e) {
      if (e.KeyChar == (char) Keys.Return) {
        e.Handled = true;
        passwordButton_Click(sender, null);
      }
    }

    private void SystemEvents_TimeChanged(object sender, EventArgs e) {
      var cursor = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      foreach (var auth in Config) {
        if (auth.AuthenticatorData != null && auth.AuthenticatorData.RequiresPassword == false) {
          try {
            auth.Sync();
          }
          catch (Exception) {
            // ignored
          }
        }
      }

      Cursor.Current = cursor;
    }

    #endregion

    #region Menu

    private void LoadMainMenu() {
      mainMenu.Visible = true;

#if DEBUG
      //test
      // AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems, "Test", "testMenuItem",
        // (s, e) => { new ShortcutEditor().ShowDialog(); });
#endif

      //File section
      AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems, "Export", "exportOptionsMenuItem", exportOptionsMenuItem_Click, Keys.Control | Keys.E);
      var importMenuItem = AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems, "Import", onClick: importTextMenu_Click, shortcut: Keys.Control | Keys.I);
      importMenuItem.Enabled = !Config.IsReadOnly;

      AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems);
      AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems, "Exit", "exitOptionsMenuItem", exitOptionMenuItem_Click, Keys.Control | Keys.W);
      
      //Add section
      addToolStripMenuItem.DropDownItems.Clear();
      var index = 0;
      foreach (var auth in AuthHelper.RegisteredAuthenticators) {
        if (auth == null) {
          addToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
          continue;
        }
        var shortcut = Keys.None;
        if (index == 0)
          shortcut = Keys.Control | Keys.A;
        var icon = string.IsNullOrEmpty(auth.Icon) ? null : AuthHelper.GetIconBitmap(auth.Icon);
        var subItem = AuthHelper.AddMenuItem(addToolStripMenuItem.DropDownItems, auth.Name, "addAuthenticatorMenuItem_" + index++, addAuthenticatorMenu_Click, shortcut, auth, icon);
      }

      //Options section
      optionsToolStripMenuItem.DropDownOpening += OpeningOptionsMenu;

      if (Config != null && Config.IsPortable == false) {
        AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Start With Windows", "startWithWindowsOptionsMenuItem", startWithWindowsOptionsMenuItem_Click);
      }

      AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Always on Top", "alwaysOnTopOptionsMenuItem", alwaysOnTopOptionsMenuItem_Click, Keys.Control | Keys.T);
      AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Use System Tray Icon", "useSystemTrayIconOptionsMenuItem", useSystemTrayIconOptionsMenuItem_Click);
      AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Auto Size", "autoSizeOptionsMenuItem", autoSizeOptionsMenuItem_Click, Keys.Control | Keys.S);
      AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems);

      if (Config == null || Config.IsReadOnly == false) {
        AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Change Protection", "changePasswordOptionsMenuItem", changePasswordOptionsMenuItem_Click);
      }

      //Help section
      AuthHelper.AddMenuItem(helpToolStripMenuItem.DropDownItems, "Check for updates", "checkUpdatesMenuItem", (s, e) => Updater.CheckForUpdates(false));
      AuthHelper.AddMenuItem(helpToolStripMenuItem.DropDownItems, "About", "aboutOptionsMenuItem", aboutOptionMenuItem_Click, Keys.F1);
    }

    private void LoadNotifyMenu(ToolStripItemCollection menuItems) {
      menuItems.Clear();

      AuthHelper.AddMenuItem(menuItems,"Show/Hide", "openOptionsMenuItem", ShowHideMenuItem_Click);
      AuthHelper.AddMenuItem(menuItems);

      if (Config != null && Config.Count != 0) {
        // because of window size, we only show first 30.
        // @todo change to MRU
        var index = 1;
        foreach (var auth in Config.Take(30)) {
          AuthHelper.AddMenuItem(menuItems, index + ". " + auth.Name, onClick: authenticatorOptionsMenuItem_Click, tag: auth);
          index++;
        }

        AuthHelper.AddMenuItem(menuItems);

        var menuItem = AuthHelper.AddMenuItem(menuItems, "Action", "defaultActionOptionsMenuItem");
        var subItem = new ToolStripMenuItem("Show Notification") {
          Name = "defaultActionNotificationOptionsMenuItem"
        };
        subItem.Click += defaultActionNotificationOptionsMenuItem_Click;
        menuItem.DropDownItems.Add(subItem);
        subItem = new ToolStripMenuItem("Copy To Clipboard") {
          Name = "defaultActionCopyToClipboardOptionsMenuItem"
        };
        subItem.Click += defaultActionCopyToClipboardOptionsMenuItem_Click;
        menuItem.DropDownItems.Add(subItem);
        menuItems.Add(menuItem);

        AuthHelper.AddMenuItem(menuItems);
      }

      AuthHelper.AddMenuItem(menuItems, "Check for updates", "checkUpdatesMenuItem", (s, e) => Updater.CheckForUpdates(false));
      AuthHelper.AddMenuItem(menuItems, "About", "aboutOptionsMenuItem", aboutOptionMenuItem_Click);
      AuthHelper.AddMenuItem(menuItems);
      AuthHelper.AddMenuItem(menuItems, "Exit", "exitOptionsMenuItem", exitOptionMenuItem_Click);
    }

    private void OpeningOptionsMenu(object sender, EventArgs e) {
      
      var menuItems = optionsToolStripMenuItem.DropDownItems;
      
      if (Config == null)
        return;

      if (menuItems.Find("changePasswordOptionsMenuItem", false).FirstOrDefault() is ToolStripMenuItem changeProtection)
        changeProtection.Enabled = Config != null && Config.Count != 0;

      if (menuItems.Find("startWithWindowsOptionsMenuItem", false).FirstOrDefault() is ToolStripMenuItem startWithWin)
        startWithWin.Checked = Config.StartWithWindows;
      if (menuItems.Find("alwaysOnTopOptionsMenuItem", false).FirstOrDefault() is ToolStripMenuItem alwaysOnTop)
        alwaysOnTop.Checked = Config.AlwaysOnTop;
      if (menuItems.Find("useSystemTrayIconOptionsMenuItem", false).FirstOrDefault() is ToolStripMenuItem useTray)
        useTray.Checked = Config.UseTrayIcon;
      if (menuItems.Find("autoSizeOptionsMenuItem", false).FirstOrDefault() is ToolStripMenuItem autoSize)
        autoSize.Checked = Config.AutoSize;
    }

    private void OpeningNotifyMenu(ToolStrip menu, CancelEventArgs e) {
      if (Config == null) {
        return;
      }

      if (menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "changePasswordOptionsMenuItem") is ToolStripMenuItem menuItem) {
        menuItem.Enabled = Config != null && Config.Count != 0;
      }

      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "defaultActionOptionsMenuItem") as
          ToolStripMenuItem;
      if (menuItem != null) {
        var subItem = menuItem.DropDownItems.Cast<ToolStripItem>()
            .FirstOrDefault(t => t.Name == "defaultActionNotificationOptionsMenuItem") as ToolStripMenuItem;
        subItem.Checked = (Config.NotifyAction == AuthConfig.NotifyActions.Notification);

        subItem = menuItem.DropDownItems.Cast<ToolStripItem>()
          .FirstOrDefault(t => t.Name == "defaultActionCopyToClipboardOptionsMenuItem") as ToolStripMenuItem;
        subItem.Checked = (Config.NotifyAction == AuthConfig.NotifyActions.CopyToClipboard);
      }

      //menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "useSystemTrayIconOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
      //if (menuitem != null)
      //{
      //	menuitem.Checked = this.Config.UseTrayIcon;
      //}
    }

    private void changePasswordOptionsMenuItem_Click(object sender, EventArgs e) {
      // confirm current password
      if ((Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0) {
        var invalidPassword = false;
        while (true) {
          var passwordForm = new GetPasswordForm {
            InvalidPassword = invalidPassword
          };
          var result = passwordForm.ShowDialog(this);
          if (result == DialogResult.Cancel) {
            return;
          }

          if (Config.IsPassword(passwordForm.Password)) {
            break;
          }

          invalidPassword = true;
        }
      }

      var form = new ChangePasswordForm {
        PasswordType = Config.PasswordType,
        HasPassword = ((Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0)
      };
      if (form.ShowDialog(this) == DialogResult.OK) {
        bool retry;
        var retrypasswordtype = Config.PasswordType;
        do {
          retry = false;

          Config.PasswordType = form.PasswordType;
          if ((Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0 &&
              string.IsNullOrEmpty(form.Password) == false) {
            Config.Password = form.Password;
          }

          try {
            SaveConfig(true);
          }
          catch (InvalidEncryptionException) {
            var result = ConfirmDialog(this, "Decryption test failed. Retry?");
            if (result == DialogResult.Yes) {
              retry = true;
              continue;
            }

            Config.PasswordType = retrypasswordtype;
          }
        } while (retry);
      }
    }

    private void ShowHideMenuItem_Click(object sender, EventArgs e) {
      Visible = !Visible;
      if (Visible) {
        // show the main form
        BringToFront();
        WindowState = FormWindowState.Normal;
        Activate();
      }
    }

    private void authenticatorOptionsMenuItem_Click(object sender, EventArgs e) {
      var menuitem = (ToolStripMenuItem) sender;
      var auth = menuitem.Tag as AuthAuthenticator;
      var item = authenticatorList.Items.Cast<AuthenticatorListBox.ListItem>().FirstOrDefault(i => i.Authenticator == auth);
      if (item != null) {
        RunAction(auth, Config.NotifyAction);
      }
    }

    private void startWithWindowsOptionsMenuItem_Click(object sender, EventArgs e) {
      Config.StartWithWindows = !Config.StartWithWindows;
    }

    private void alwaysOnTopOptionsMenuItem_Click(object sender, EventArgs e) {
      Config.AlwaysOnTop = !Config.AlwaysOnTop;
    }

    private void useSystemTrayIconOptionsMenuItem_Click(object sender, EventArgs e) {
      Config.UseTrayIcon = !Config.UseTrayIcon;
    }

    private void defaultActionNotificationOptionsMenuItem_Click(object sender, EventArgs e) {
      Config.NotifyAction = AuthConfig.NotifyActions.Notification;
    }

    private void defaultActionCopyToClipboardOptionsMenuItem_Click(object sender, EventArgs e) {
      Config.NotifyAction = AuthConfig.NotifyActions.CopyToClipboard;
    }

    private void autoSizeOptionsMenuItem_Click(object sender, EventArgs e) {
      Config.AutoSize = !Config.AutoSize;
    }

    private void exportOptionsMenuItem_Click(object sender, EventArgs e) {
      // confirm current password
      if ((Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0) {
        var invalidPassword = false;
        while (true) {
          var checkform = new GetPasswordForm {
            InvalidPassword = invalidPassword
          };
          var result = checkform.ShowDialog(this);
          if (result == DialogResult.Cancel) {
            return;
          }

          if (Config.IsPassword(checkform.Password)) {
            break;
          }

          invalidPassword = true;
        }
      }

      var exportform = new ExportForm();
      if (exportform.ShowDialog(this) == DialogResult.OK) {
        AuthHelper.ExportAuthenticators(this, Config, exportform.ExportFile, exportform.Password,
          exportform.PgpKey);
      }
    }

    private void aboutOptionMenuItem_Click(object sender, EventArgs e) {
      var form = new AboutForm {
        Config = Config
      };
      form.ShowDialog(this);
    }

    private void exitOptionMenuItem_Click(object sender, EventArgs e) {
      mExplicitClose = true;
      Close();
    }

    #endregion

    #region Custom Events

    private void OnConfigChanged(object source, ConfigChangedEventArgs args) {
      switch (args.PropertyName) {
        case "AlwaysOnTop":
          TopMost = Config.AlwaysOnTop;
          break;
        case "UseTrayIcon": {
          var useTrayIcon = Config.UseTrayIcon;
          if (useTrayIcon == false && Visible == false) {
            BringToFront();
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
          }
          notifyIcon.Visible = useTrayIcon;
          break;
        }
        case "AutoSize":
        case "Authenticator" when args.AuthenticatorChangedEventArgs.Property == "Name":
          SetAutoSize();
          Invalidate();
          break;
        case "StartWithWindows": {
          if (Config.IsPortable == false) {
            AuthHelper.SetStartWithWindows(Config.StartWithWindows);
          }
          break;
        }
      }
      // batch up saves so they can be done out of line
      SaveConfig();
    }

    #endregion
  }
}