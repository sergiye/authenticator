using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using sergiye.Common;

namespace Authenticator {
  public partial class MainForm : Form {
    
    private readonly NotifyIcon notifyIcon;
    private readonly ContextMenuStrip notifyMenu;

    #region Properties

    public readonly AuthConfig Config;

    private ToolStripMenuItem themeMenuItem;
    private readonly Timer passwordTimer;
    private readonly StartupManager startupManager = new StartupManager();

    private DateTime? saveConfigTime;
    private bool mExplicitClose;
    private readonly bool startMinimized;
    private readonly string startupConfigFile;

    #endregion

    public MainForm() {
      InitializeComponent();

      //Text = $"Authenticator {(Environment.Is64BitProcess ? "x64" : "x32")} - {Updater.CurrentVersion}";
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
      authenticatorList.ItemHeight = 50;
      authenticatorList.SelectionMode = SelectionMode.One;

      MinimumSize = new Size(200, mainMenu.Height + Height - ClientRectangle.Height + authenticatorList.ItemHeight);

      //will display prompt only if update available & when main form displayed
      Updater.Subscribe(
        (message, isError) => { MessageBox.Show(message, Updater.ApplicationName, MessageBoxButtons.OK, isError ? MessageBoxIcon.Warning : MessageBoxIcon.Information); },
        (message) => { return MessageBox.Show(message, Updater.ApplicationName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK; },
        () => { exitOptionMenuItem_Click(null, EventArgs.Empty); }
      );

      // initialize UI
      notifyMenu = new ContextMenuStrip(components);
      
      notifyIcon = new NotifyIcon(components);
      notifyIcon.DoubleClick += ShowHideMenuItem_Click;
      notifyIcon.Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
      notifyIcon.ContextMenuStrip = notifyMenu;
      notifyIcon.Text = Text = Updater.ApplicationTitle;

      passwordTimer = new Timer(components);
      passwordTimer.Tick += (s, e) => {
        if (passwordErrorLabel.Tag == null || (DateTime)passwordErrorLabel.Tag > DateTime.Now)
          return;
        passwordTimer.Enabled = false;
        passwordErrorLabel.Tag = null;
        passwordErrorLabel.Text = string.Empty;
      };
      passwordTimer.Interval = 500;

      // hook into System time change event
      Microsoft.Win32.SystemEvents.TimeChanged += SystemEvents_TimeChanged;
      // redirect mouse wheel events
      new MessageForwarder(authenticatorList, WinApiHelper.WM_MOUSEWHEEL);

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
              startMinimized = true;
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

      Config = LoadConfig(password);

      LoadMainMenu();
      InitializeForm();

      AuthConfig.OnConfigChanged += OnConfigChanged;

      if (Config.Upgraded) {
        SaveConfig(true);
        // display warning
        ErrorDialog(this, string.Format("Authenticator has upgraded your authenticators to version {0}.\nDo NOT run an older version of Authenticator as this could overwrite them.\nNow is a good time to make a backup. Click the Options icon and choose Export.", AuthConfig.CurrentVersion));
      }

      notifyIcon.Visible = AuthConfig.UseTrayIcon;

      InitializeTheme();
    }

    #region themes

    private void OnThemeCurrentChanged() {
      authenticatorList.SelectedBackColor = Theme.Current.SelectedBackgroundColor;
      authenticatorList.SelectedForeColor = Theme.Current.SelectedForegroundColor;
      authenticatorList.CodeColor = Theme.Current.HyperlinkColor;
      authenticatorList.SelectedCodeColor = Theme.Current.SelectedForegroundColor;
      authenticatorList.PieColor = Theme.Current.WarnColor;
      authenticatorList.LineColor = Theme.Current.LineColor;
      authenticatorList.Invalidate();

      AuthConfig.Theme = Theme.IsAutoThemeEnabled ? "auto" : Theme.Current.Id;
    }

    private void InitializeTheme() {

      mainMenu.Renderer = new ThemedToolStripRenderer();
      notifyMenu.Renderer = new ThemedToolStripRenderer();
      authenticatorList.ContextMenuStrip.Renderer = new ThemedToolStripRenderer();

      var currentItem = CustomTheme.FillThemesMenu((title, theme, onClick) => {
        if (theme == null && onClick == null) {
          themeMenuItem.DropDownItems.Add(title);
          return null;
        }
        var item = new ToolStripRadioButtonMenuItem(title, null, onClick);
        themeMenuItem.DropDownItems.Add(item);
        return item;
      }, OnThemeCurrentChanged, AuthConfig.Theme, "Authenticator.themes");
      currentItem?.PerformClick();
      Theme.Current.Apply(this);
    }

    #endregion

    #region Private Methods

    private AuthConfig LoadConfig(string password) {
      loadingPanel.Visible = true;
      passwordPanel.Visible = false;
      mainMenu.Visible = false;
      AuthConfig config = null;
      try {
        config = AuthHelper.LoadConfig(startupConfigFile, password);
      }
      catch (AuthInvalidNewerConfigException ex) {
        MessageBox.Show(this, ex.Message, Updater.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        System.Diagnostics.Process.GetCurrentProcess().Kill();
        return null;
      }
      catch (BadPasswordException) {
        loadingPanel.Visible = false;
        passwordPanel.Visible = true;
        mainMenu.Visible = false;

        passwordErrorLabel.Text = "Invalid password";
        passwordErrorLabel.Tag = DateTime.Now.AddSeconds(3);
        // oddity with MetroFrame controls in have to set focus away and back to field to make it stick
        Invoke((MethodInvoker)delegate {
          passwordButton.Focus();
          passwordField.Focus();
        });
        passwordTimer.Enabled = true;
        return null;
      }
      catch (Exception ex) {
        if (ex is AuthInvalidNewerConfigException || ex is EncryptedSecretDataException) {
          loadingPanel.Visible = false;
          passwordPanel.Visible = true;
          mainMenu.Visible = false;
          passwordButton.Focus();
          passwordField.Focus();
          return null;
        }

        ErrorDialog(this, "An unknown error occurred: " + ex.Message, ex, MessageBoxButtons.OK);
        Close();
        return null;
      }

      if (config == null) {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
        return null;
      }

      return config;
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
          if (ErrorDialog(this, "An unknown error occurred: " + ex.Message, ex, MessageBoxButtons.RetryCancel) ==
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

    private void InitializeForm() {

      // set up list
      LoadAuthenticatorList();

      // size the form based on the authenticators
      SetAutoSize();

      notifyMenu.Opening += (s, e) => OpeningNotifyMenu(notifyMenu, e);
      LoadNotifyMenu(notifyMenu.Items);

      loadingPanel.Visible = false;
      passwordPanel.Visible = false;
      mainMenu.Visible = true;

      authenticatorList.Focus();

      // set positions
      if (AuthConfig.Position.IsEmpty == false) {
        // check we aren't out of bounds in case of multi-monitor change
        var v = SystemInformation.VirtualScreen;
        if ((AuthConfig.Position.X + Width) >= v.Left && AuthConfig.Position.X < v.Width &&
            AuthConfig.Position.Y > v.Top) {
          try {
            StartPosition = FormStartPosition.Manual;
            Left = AuthConfig.Position.X;
            Top = AuthConfig.Position.Y;
          }
          catch (Exception) {
            // ignored
          }
        }

        // check we aren't below the taskbar
        var lowestY = Screen.GetWorkingArea(this).Bottom;
        var bottom = Top + Height;
        if (bottom > lowestY) {
          Top -= (bottom - lowestY);
          if (Top < 0) {
            Height += Top;
            Top = 0;
          }
        }
      }
      else if (AuthConfig.AutoSize) {
        CenterToScreen();
      }

      // if we passed "-min" flag
      if (startMinimized || AuthConfig.StartMinimized) {
        WindowState = FormWindowState.Minimized;
        ShowInTaskbar = true;
      }

      if (AuthConfig.UseTrayIcon) {
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
            auth.AuthenticatorData is not HotpAuthenticator) {
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

      return MessageBox.Show(form, message, Updater.ApplicationName, buttons, MessageBoxIcon.Exclamation);
    }

    public static DialogResult ConfirmDialog(Form form, string message,
      MessageBoxButtons buttons = MessageBoxButtons.YesNo, MessageBoxIcon icon = MessageBoxIcon.Question,
      MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1) {
      return MessageBox.Show(form, message, Updater.ApplicationName, buttons, icon, defaultButton);
    }

    protected override void WndProc(ref Message m) {
      base.WndProc(ref m);
      if (m.Msg == WinApiHelper.WM_SHOWME) {
        Visible = true;
        BringToFront();
        WindowState = FormWindowState.Normal;
        Activate();
      }
    }

    private void RunAction(AuthAuthenticator auth, AuthConfig.NotifyActions action) {
      // get the code
      string code;
      try {
        code = auth.CurrentCode;
      }
      catch (EncryptedSecretDataException) {
        // if the authenticator is current protected we display the password window, get the code, and re-protect it
        // with a bit of window jiggling to make sure we get focus and then put it back

        // save the current window
        var foregroundWindow = WinApiHelper.GetForegroundWindow();
        var screen = Screen.FromHandle(foregroundWindow);
        var activeWindow = IntPtr.Zero;
        if (Visible) {
          activeWindow = WinApiHelper.SetActiveWindow(Handle);
          BringToFront();
        }

        var item = authenticatorList.Items.Cast<AuthenticatorListBox.ListItem>().FirstOrDefault(i => i.Authenticator == auth);
        code = authenticatorList.GetItemCode(item, screen);

        // restore active window
        if (activeWindow != IntPtr.Zero) {
          WinApiHelper.SetActiveWindow(activeWindow);
        }

        WinApiHelper.SetForegroundWindow(foregroundWindow);
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
      if (AuthConfig.AutoSize) {
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
        if (AuthConfig.Width != 0) {
          Width = AuthConfig.Width;
        }

        if (AuthConfig.Height != 0) {
          Height = AuthConfig.Height;
        }
      }
      introLabel.Visible = Config.Count == 0;
    }

    private void EndRenaming() {
      // set focus to form, so that the edit field will disappear if it is visible
      if (authenticatorList.IsRenaming) {
        authenticatorList.EndRenaming();
      }
    }

    private void MainForm_Shown(object sender, EventArgs e) {
      // if we use tray icon make sure it is set
      if (AuthConfig.UseTrayIcon) {
        // if initially minimized, we need to hide
        if (WindowState == FormWindowState.Minimized) {
          Hide();
        }
      }
      else {
        Activate();
      }
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
      // keep in the tray when closing Form 
      if (AuthConfig.UseTrayIcon && Visible && mExplicitClose == false) {
        e.Cancel = true;
        Hide();
        return;
      }

      // ensure the notify icon is closed
      notifyIcon.Visible = false;

      // save size if we are not auto-resize
      if (Config != null) {
        if (!AuthConfig.AutoSize && (AuthConfig.Width != Width || AuthConfig.Height != Height)) {
          AuthConfig.Width = Width;
          AuthConfig.Height = Height;
        }
        AuthConfig.Position = new Point(Left, Top);
      }

      // perform save if we have one pending
      if (saveConfigTime != null) {
        SaveConfig(true);
      }
    }

    void addAuthenticatorMenu_Click(object sender, EventArgs e) {
      if (sender is not ToolStripItem menuItem || menuItem.Tag is not RegisteredAuthenticator registeredAuth) return;
      // add the new authenticator
      var authenticator = new AuthAuthenticator();
      bool added;

      if (registeredAuth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.BattleNet) {
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
      else if (registeredAuth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Trion) {
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
      else if (registeredAuth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.GuildWars) {
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
      else if (registeredAuth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Microsoft) {
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
      else if (registeredAuth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.RFC6238_TIME_COUNTER) {
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
      else if (registeredAuth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.OktaVerify) {
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
                                          registeredAuth.AuthenticatorType);
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

    void importTextMenu_Click(object sender, EventArgs e) {
      var ofd = new OpenFileDialog {
        AddExtension = true,
        CheckFileExists = true,
        CheckPathExists = true
      };

      ofd.Filter = "Authenticator Files (*.config)|*.config|Text Files (*.txt)|*.txt|Zip Files (*.zip)|*.zip|PGP Files (*.pgp)|*.pgp|All Files (*.*)|*.*";
      ofd.RestoreDirectory = true;
      ofd.Title = Updater.ApplicationTitle;
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
      if (Config != null && AuthConfig.AutoSize == false) {
        AuthConfig.Width = Width;
        AuthConfig.Height = Height;
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
      if (!Config.IsReadOnly) {
        //Add section
        var addMenuItem = AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems, "Add");
        AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems);
        var index = 0;
        foreach (var auth in AuthHelper.RegisteredAuthenticators) {
          if (auth == null) {
            addMenuItem.DropDownItems.Add(new ToolStripSeparator());
            continue;
          }
          var shortcut = index == 0 ? Keys.Control | Keys.A : Keys.None;
          var icon = string.IsNullOrEmpty(auth.Icon) ? null : AuthHelper.GetIconBitmap(auth.Icon);
          var subItem = AuthHelper.AddMenuItem(addMenuItem.DropDownItems, auth.Name, "addAuthenticatorMenuItem_" + index++, addAuthenticatorMenu_Click, shortcut, auth, icon);
        }
      }

      AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems, "Export", "exportOptionsMenuItem", exportOptionsMenuItem_Click, Keys.Control | Keys.E);
      if (!Config.IsReadOnly) {
        AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems, "Import", onClick: importTextMenu_Click, shortcut: Keys.Control | Keys.I);
        AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems);
        AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems, "Change Protection", "changePasswordOptionsMenuItem", changePasswordOptionsMenuItem_Click);
      }

      AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems);
      AuthHelper.AddMenuItem(fileToolStripMenuItem.DropDownItems, "Exit", "exitOptionsMenuItem", exitOptionMenuItem_Click, Keys.Control | Keys.W);
      
      //Options section
      optionsToolStripMenuItem.DropDownOpening += OpeningOptionsMenu;

      //if (Config.IsPortable == false) {
      AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Run On User Login", "startWithWindowsOptionsMenuItem", (s, e) => startupManager.Startup = !startupManager.Startup, isChecked: startupManager.Startup, checkOnClick: true);
      AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Start Minimized", "startMinimizedOptionsMenuItem", (s, e) => AuthConfig.StartMinimized = !AuthConfig.StartMinimized, isChecked: AuthConfig.StartMinimized, checkOnClick: true);
      AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Use System Tray Icon", "useSystemTrayIconOptionsMenuItem", (s, e) => {
        AuthConfig.UseTrayIcon = !AuthConfig.UseTrayIcon;
      }, isChecked: AuthConfig.UseTrayIcon, checkOnClick: true);

      AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Auto-Update App", "autoUpdateOptionsMenuItem", (s, e) => {
        Updater.AutoUpdate = !Updater.AutoUpdate;
        SaveConfig();
      }, isChecked: Updater.AutoUpdate, checkOnClick: true);
      //}

      AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems);
      AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Auto-Size", "autoSizeOptionsMenuItem", (s, e) => {
        AuthConfig.AutoSize = !AuthConfig.AutoSize;
      }, Keys.Control | Keys.S, isChecked: AuthConfig.AutoSize, checkOnClick: true);

      TopMost = AuthConfig.AlwaysOnTop;
      AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Always On Top", "alwaysOnTopOptionsMenuItem", (s, e) => {
        TopMost = AuthConfig.AlwaysOnTop = !AuthConfig.AlwaysOnTop;
        SaveConfig();
      }, Keys.Control | Keys.T, isChecked: AuthConfig.AlwaysOnTop, checkOnClick: true);
      
      themeMenuItem = AuthHelper.AddMenuItem(optionsToolStripMenuItem.DropDownItems, "Theme", "themeMenuItem");

      //Help section
      AuthHelper.AddMenuItem(helpToolStripMenuItem.DropDownItems, "Site", "siteMenuItem", (s, e) => Updater.VisitAppSite(), Keys.Control | Keys.F1);
      AuthHelper.AddMenuItem(helpToolStripMenuItem.DropDownItems, "Check For Updates", "checkUpdatesMenuItem", (s, e) => Updater.CheckForUpdates(Updater.CheckUpdatesMode.AllMessages), Keys.Control | Keys.U);
      AuthHelper.AddMenuItem(helpToolStripMenuItem.DropDownItems, "About", "aboutOptionsMenuItem", aboutOptionMenuItem_Click, Keys.F1);
    }

    private void LoadNotifyMenu(ToolStripItemCollection menuItems) {
      menuItems.Clear();

      AuthHelper.AddMenuItem(menuItems,"Show/Hide", "openOptionsMenuItem", ShowHideMenuItem_Click);

      if (Config != null && Config.Count != 0) {
        var menuItem = AuthHelper.AddMenuItem(menuItems, "Click Action", "defaultActionOptionsMenuItem");
        AuthHelper.AddMenuItem(menuItem.DropDownItems, "Show Notification", "defaultActionNotificationOptionsMenuItem", defaultActionNotificationOptionsMenuItem_Click);
        AuthHelper.AddMenuItem(menuItem.DropDownItems, "Copy To Clipboard", "defaultActionCopyToClipboardOptionsMenuItem", defaultActionCopyToClipboardOptionsMenuItem_Click);
        AuthHelper.AddMenuItem(menuItems);

        // because of window size, we only show first 30.
        // @todo change to MRU
        var index = 1;
        foreach (var auth in Config.Take(30)) {
          AuthHelper.AddMenuItem(menuItems, index + ". " + auth.Name, onClick: authenticatorOptionsMenuItem_Click, tag: auth);
          index++;
        }
      }

      AuthHelper.AddMenuItem(menuItems);
      AuthHelper.AddMenuItem(menuItems, "Exit", "exitOptionsMenuItem", exitOptionMenuItem_Click);
    }

    private void OpeningOptionsMenu(object sender, EventArgs e) {
      
      if (Config == null)
        return;

      var menuItems = optionsToolStripMenuItem.DropDownItems;
      if (menuItems.Find("changePasswordOptionsMenuItem", false).FirstOrDefault() is ToolStripMenuItem changeProtection)
        changeProtection.Enabled = Config.Count != 0;
    }

    private void OpeningNotifyMenu(ToolStrip menu, CancelEventArgs e) {
      if (Config == null) {
        return;
      }

      if (menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "changePasswordOptionsMenuItem") is ToolStripMenuItem menuItem) {
        menuItem.Enabled = Config.Count != 0;
      }

      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "defaultActionOptionsMenuItem") as
          ToolStripMenuItem;
      if (menuItem != null) {
        var subItem = menuItem.DropDownItems.Cast<ToolStripItem>()
            .FirstOrDefault(t => t.Name == "defaultActionNotificationOptionsMenuItem") as ToolStripMenuItem;
        subItem.Checked = AuthConfig.NotifyAction == AuthConfig.NotifyActions.Notification;

        subItem = menuItem.DropDownItems.Cast<ToolStripItem>()
          .FirstOrDefault(t => t.Name == "defaultActionCopyToClipboardOptionsMenuItem") as ToolStripMenuItem;
        subItem.Checked = AuthConfig.NotifyAction == AuthConfig.NotifyActions.CopyToClipboard;
      }

      //menuItem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "useSystemTrayIconOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
      //if (menuItem != null)
      //{
      //	menuItem.Checked = this.Config.UseTrayIcon;
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
        HasPassword = (Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0
      };
      if (form.ShowDialog(this) == DialogResult.OK) {
        bool retry;
        var retryPasswordType = Config.PasswordType;
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

            Config.PasswordType = retryPasswordType;
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
      if (sender is not ToolStripMenuItem menuItem || menuItem.Tag is not AuthAuthenticator auth) return;
      var item = authenticatorList.Items.Cast<AuthenticatorListBox.ListItem>().FirstOrDefault(i => i.Authenticator == auth);
      if (item != null) {
        RunAction(auth, AuthConfig.NotifyAction);
      }
    }

    private void defaultActionNotificationOptionsMenuItem_Click(object sender, EventArgs e) {
      AuthConfig.NotifyAction = AuthConfig.NotifyActions.Notification;
    }

    private void defaultActionCopyToClipboardOptionsMenuItem_Click(object sender, EventArgs e) {
      AuthConfig.NotifyAction = AuthConfig.NotifyActions.CopyToClipboard;
    }

    private void exportOptionsMenuItem_Click(object sender, EventArgs e) {
      // confirm current password
      if ((Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0) {
        var invalidPassword = false;
        while (true) {
          using(var checkForm = new GetPasswordForm {InvalidPassword = invalidPassword}) {
            if (checkForm.ShowDialog(this) == DialogResult.Cancel)
              return;
            if (Config.IsPassword(checkForm.Password))
              break;
          }
          invalidPassword = true;
        }
      }

      using (var exportForm = new ExportForm()) {
        if (exportForm.ShowDialog(this) == DialogResult.OK) {
          AuthHelper.ExportAuthenticators(this, Config, exportForm.ExportFile, exportForm.Password,
            exportForm.PgpKey);
        }
      }
    }

    private void aboutOptionMenuItem_Click(object sender, EventArgs e) {
      Updater.ShowAbout();
    }

    private void exitOptionMenuItem_Click(object sender, EventArgs e) {
      if (InvokeRequired) {
        Invoke(new EventHandler(exitOptionMenuItem_Click), sender, e);
        return;
      }
      mExplicitClose = true;
      Close();
    }

    #endregion

    #region Custom Events

    private void OnConfigChanged(ConfigChangedEventArgs args) {
      switch (args.PropertyName) {
        case "UseTrayIcon": {
          if (!AuthConfig.UseTrayIcon && !Visible) {
            BringToFront();
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
          }
          notifyIcon.Visible = AuthConfig.UseTrayIcon;
          break;
        }
        case "AutoSize":
        case "Authenticator" when args.AuthenticatorChangedEventArgs.Property == "Name":
          SetAutoSize();
          Invalidate();
          break;
      }
      // batch up saves so they can be done out of line
      SaveConfig();
    }

    #endregion
  }
}