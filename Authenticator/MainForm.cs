﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Authenticator.Resources;

namespace Authenticator {
  public partial class MainForm : ResourceForm {
    public MainForm() {
      InitializeComponent();
    }

    #region Properties

    public AuthConfig Config { get; set; }

    private DateTime? saveConfigTime;

    private bool mExplictClose;

    public delegate void SetClipboardDataDelegate(object data);

    public delegate object GetClipboardDataDelegate(Type format);

    private Rectangle listoffset;

    private bool initiallyMinimised;

    private string existingv2Config;

    private string startupConfigFile;

    private bool mInitOnce;

    #endregion

    private void MainForm_Load(object sender, EventArgs e) {
      //Text = $"Authenticator {(Environment.Is64BitProcess ? "x64" : "x32")} - {Updater.CurrentVersion}";
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
      notifyIcon.Icon = Icon;

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
          var webproxy = new WebProxy(uri.Host + ":" + uri.Port, true);
          if (string.IsNullOrEmpty(uri.UserInfo) == false) {
            var auth = uri.UserInfo.Split(':');
            webproxy.Credentials = new NetworkCredential(auth[0], (auth.Length > 1 ? auth[1] : string.Empty));
          }

          WebRequest.DefaultWebProxy = webproxy;
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
      var configFile = startupConfigFile;

      loadingPanel.Visible = true;
      passwordPanel.Visible = false;

      Task.Factory.StartNew(() => {
        try {
          // use previous config if we have one
          var config = AuthHelper.LoadConfig(configFile, password);
          return new Tuple<AuthConfig, Exception>(config, null);
        }
        catch (Exception ex) {
          return new Tuple<AuthConfig, Exception>(null, ex);
        }
      }).ContinueWith((configTask) => {
        var ex = configTask.Result.Item2;
        switch (ex) {
          case AuthInvalidNewerConfigException _:
            MessageBox.Show(this, ex.Message, AuthMain.APPLICATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            return;
          case EncryptedSecretDataException _:
            loadingPanel.Visible = false;
            passwordPanel.Visible = true;

            passwordButton.Focus();
            passwordField.Focus();

            return;
          case BadYubiKeyException _:
            loadingPanel.Visible = false;
            passwordPanel.Visible = false;
            return;
          case BadPasswordException _:
            loadingPanel.Visible = false;
            passwordPanel.Visible = true;
            passwordErrorLabel.Text = strings.InvalidPassword;
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
          if (ErrorDialog(this, strings.UnknownError + ": " + ex.Message, ex, MessageBoxButtons.RetryCancel) ==
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

        // check for a v2 config file if this is a new config
        if (config.Count == 0 && string.IsNullOrEmpty(config.Filename)) {
          existingv2Config = AuthHelper.GetLastV2Config();
        }

        Config = config;
        Config.OnConfigChanged += OnConfigChanged;

        if (config.Upgraded) {
          SaveConfig(true);
          // display warning
          ErrorDialog(this, string.Format(strings.ConfigUpgraded, AuthConfig.Currentversion));
        }

        InitializeForm();
      }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    /// <summary>
    /// Import authenticators from a file
    /// 
    /// *.xml = Authenticator v2
    /// *.txt = plain text with KeyUriFormat per line (https://code.google.com/p/google-authenticator/wiki/KeyUriFormat)
    /// *.zip = encrypted zip, containing import file
    /// *.pgp = PGP encrypted, containing import file
    /// 
    /// </summary>
    /// <param name="authenticatorFile">name import file</param>
    private void ImportAuthenticator(string authenticatorFile) {
      // call legacy import for v2 xml files
      if (String.Compare(Path.GetExtension(authenticatorFile), ".xml", StringComparison.OrdinalIgnoreCase) == 0) {
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

        // save off any new authenticators as a backup
        AuthHelper.SaveToRegistry(Config, authenticator);

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
      introLabel.Visible = (Config.Count == 0);
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
            while (Config.Where(a => a.Name == importedName).Count() != 0) {
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
            // save off any new authenticators as a backup
            AuthHelper.SaveToRegistry(Config, auth);

            // add to main list
            Config.Add(auth);
            LoadAuthenticatorList(auth);
          }

          SaveConfig(true);

          // reset UI
          SetAutoSize();
          introLabel.Visible = (Config.Count == 0);

          needPassword = false;
          retry = false;
        }
        catch (EncryptedSecretDataException) {
          needPassword = true;
          invalidPassword = false;
        }
        catch (BadYubiKeyException) {
          needPassword = true;
          invalidPassword = false;
        }
        catch (BadPasswordException) {
          needPassword = true;
          invalidPassword = true;
        }
        catch (Exception ex) {
          if (ErrorDialog(this, strings.UnknownError + ": " + ex.Message, ex, MessageBoxButtons.RetryCancel) ==
              DialogResult.Cancel) {
            return;
          }

          needPassword = false;
          invalidPassword = false;
          retry = true;
        }

        if (needPassword) {
          var form = new GetPasswordForm();
          form.InvalidPassword = invalidPassword;
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
      if (mInitOnce == false) {
        // hook into System time change event
        Microsoft.Win32.SystemEvents.TimeChanged += SystemEvents_TimeChanged;

        // save the initial form size

        // redirect mouse wheel events
        new WinApi.MessageForwarder(authenticatorList, WinApi.WM_MOUSEWHEEL);

        mInitOnce = true;
      }
    }

    private void InitializeForm() {
      // set up list
      LoadAuthenticatorList();

      // set always on top
      TopMost = Config.AlwaysOnTop;

      // size the form based on the authenticators
      SetAutoSize();

      // initialize UI
      LoadAddAuthenticatorTypes();
      LoadOptionsMenu(optionsMenu);
      LoadNotifyMenu(notifyMenu);
      loadingPanel.Visible = false;
      passwordPanel.Visible = false;
      commandPanel.Visible = true;
      introLabel.Visible = (Config.Count == 0);
      authenticatorList.Visible = (Config.Count != 0);
      addAuthenticatorButton.Visible = !Config.IsReadOnly;

      // set title
      notifyIcon.Visible = Config.UseTrayIcon;
      notifyIcon.Text = Text = AuthMain.APPLICATION_TITLE;

      // hook Steam notifications
      HookSteam();

      // save the position of the list within the form else starting as minimized breaks the size
      listoffset = new Rectangle(authenticatorList.Left, authenticatorList.Top, (Width - authenticatorList.Width),
        (Height - authenticatorList.Height));

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
        notifyIcon.Visible = true;
        notifyIcon.Text = Text;

        // if initially minimized, we need to hide
        if (WindowState == FormWindowState.Minimized) {
          // hide this and metro owner
          Form form = this;
          do {
            form.Hide();
          } while ((form = form.Owner) != null);
        }
      }
    }

    private void LoadAuthenticatorList(AuthAuthenticator added = null) {
      // set up list
      authenticatorList.Items.Clear();

      var index = 0;
      foreach (var auth in Config) {
        var ali = new AuthenticatorListitem(auth, index);
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
        message = strings.ErrorOccured + (ex != null ? ": " + ex.Message : string.Empty);
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
        AuthMain.LogException(ex);
      }
#endif

      return MessageBox.Show(form, message, AuthMain.APPLICATION_TITLE, buttons, MessageBoxIcon.Exclamation);
    }

    public static DialogResult ConfirmDialog(Form form, string message,
      MessageBoxButtons buttons = MessageBoxButtons.YesNo, MessageBoxIcon icon = MessageBoxIcon.Question,
      MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1) {
      return MessageBox.Show(form, message, AuthMain.APPLICATION_TITLE, buttons, icon, defaultButton);
    }

    private void LoadAddAuthenticatorTypes() {
      addAuthenticatorMenu.Items.Clear();

      ToolStripMenuItem subitem;
      var index = 0;
      foreach (var auth in AuthMain.RegisteredAuthenticators) {
        if (auth == null) {
          addAuthenticatorMenu.Items.Add(new ToolStripSeparator());
          continue;
        }

        subitem = new ToolStripMenuItem();
        subitem.Text = auth.Name;
        subitem.Name = "addAuthenticatorMenuItem_" + index++;
        subitem.Tag = auth;
        if (string.IsNullOrEmpty(auth.Icon) == false) {
          subitem.Image = new Bitmap(Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("Authenticator.Resources." + auth.Icon));
          subitem.ImageAlign = ContentAlignment.MiddleLeft;
          subitem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
        }

        subitem.Click += addAuthenticatorMenu_Click;
        addAuthenticatorMenu.Items.Add(subitem);
      }

      //
      addAuthenticatorMenu.Items.Add(new ToolStripSeparator());
      //
      subitem = new ToolStripMenuItem();
      subitem.Text = strings.MenuImportText;
      subitem.Name = "importTextMenuItem";
      subitem.Image = new Bitmap(Assembly.GetExecutingAssembly()
        .GetManifestResourceStream("Authenticator.Resources.TextIcon.png"));
      subitem.ImageAlign = ContentAlignment.MiddleLeft;
      subitem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
      subitem.Click += importTextMenu_Click;
      addAuthenticatorMenu.Items.Add(subitem);
    }

    #region Steam Notifications

    public void UnhookSteam() {
      if (Config == null) {
        return;
      }

      foreach (var auth in Config) {
        if (auth.AuthenticatorData != null && auth.AuthenticatorData is SteamAuthenticator &&
            ((SteamAuthenticator) auth.AuthenticatorData).Client != null) {
          var client = ((SteamAuthenticator) auth.AuthenticatorData).GetClient();
          client.ConfirmationEvent -= SteamClient_ConfirmationEvent;
          client.ConfirmationErrorEvent -= SteamClient_ConfirmationErrorEvent;
        }
      }
    }

    public void HookSteam() {
      UnhookSteam();
      if (Config == null) {
        return;
      }

      // do async as setting up clients can take time (Task.Factory.StartNew wait for UI so need to use new Thread(...))
      new Thread(() => {
        foreach (var auth in Config) {
          if (auth.AuthenticatorData != null && auth.AuthenticatorData is SteamAuthenticator) {
            var client = ((SteamAuthenticator) auth.AuthenticatorData).GetClient();
            client.ConfirmationEvent += SteamClient_ConfirmationEvent;
            client.ConfirmationErrorEvent += SteamClient_ConfirmationErrorEvent;
          }
        }
      }).Start();
    }

    private void SteamClient_ConfirmationErrorEvent(object sender, string message, SteamClient.PollerAction action,
      Exception ex) {
      var steam = sender as SteamClient;
      var auth = Config.FirstOrDefault(a => a.AuthenticatorData is SteamAuthenticator authenticator &&
                                            authenticator.Serial == steam.Authenticator.Serial);

      AuthMain.LogException(ex, true);

      if (action != SteamClient.PollerAction.SilentAutoConfirm) {
        // show the Notification window in the correct context
        Invoke(new ShowNotificationCallback(ShowNotification), auth, auth.Name, message, false, 0);
      }
    }

    public delegate void ShowNotificationCallback(AuthAuthenticator auth, string title, string message,
      bool openOnClick, int extraHeight);

    public void ShowNotification(AuthAuthenticator auth, string title, string message, bool openOnClick,
      int extraHeight) {
      var notify = new Notification(title, message, 10000);
      if (extraHeight != 0) {
        notify.Height += extraHeight;
      }

      notify.Tag = auth;
      if (openOnClick) {
        notify.OnNotificationClicked += Notify_Click;
      }

      notify.Show();
    }

    private void Notify_Click(object sender, EventArgs e) {
      var auth = ((Notification) sender).Tag as AuthAuthenticator;

      // ensure window is front
      BringToFront();
      Show();
      WindowState = FormWindowState.Normal;
      Activate();

      // show waiting
      Cursor.Current = Cursors.WaitCursor;

      // open the confirmations
      var item = authenticatorList.ContextMenuStrip.Items.Cast<ToolStripItem>()
        .FirstOrDefault(i => i.Name == "showSteamTradesMenuItem");
      authenticatorList.CurrentItem = authenticatorList.Items.Cast<AuthenticatorListitem>()
        .FirstOrDefault(i => i.Authenticator == auth);
      item?.PerformClick();
    }

    private void SteamClient_ConfirmationEvent(object sender, SteamClient.Confirmation confirmation,
      SteamClient.PollerAction action) {
      var steam = sender as SteamClient;

      var auth = Config.FirstOrDefault(a =>
        a.AuthenticatorData is SteamAuthenticator authenticator &&
        authenticator.Serial == steam.Authenticator.Serial);

      string title = null;
      string message = null;
      var openOnClick = false;
      var extraHeight = 0;

      if (action == SteamClient.PollerAction.AutoConfirm || action == SteamClient.PollerAction.SilentAutoConfirm) {
        if (steam.ConfirmTrade(confirmation.Id, confirmation.Key, true)) {
          if (action != SteamClient.PollerAction.SilentAutoConfirm) {
            title = "Confirmed";
            message = string.Format(
              "<h1>{0}</h1><table width=250 cellspacing=0 cellpadding=0 border=0><tr><td width=40><img src=\"{1}\" /></td><td width=210>{2}<br/>{3}</td></tr></table>",
              auth.Name, confirmation.Image, confirmation.Details, confirmation.Traded);
          }
        }
        else {
          title = "Confirmation Failed";
          message = string.Format(
            "<h1>{0}</h1><table width=250 cellspacing=0 cellpadding=0 border=0><tr><td width=40><img src=\"{1}\" /></td><td width=210>{2}<br/>{3}<br/>Error: {4}</td></tr></table>",
            auth.Name, confirmation.Image, confirmation.Details, confirmation.Traded, steam.Error ?? "Unknown error");
          extraHeight += 20;
        }
      }
      else if (confirmation.IsNew) // if (action == SteamClient.PollerAction.Notify)
      {
        title = "New Confirmation";
        message = string.Format(
          "<h1>{0}</h1><table width=250 cellspacing=0 cellpadding=0 border=0><tr valign=top><td width=40><img src=\"{1}\" /></td><td width=210>{2}<br/>{3}</td></tr></table>",
          auth.Name, confirmation.Image, confirmation.Details, confirmation.Traded);
        openOnClick = true;
      }

      if (title != null) {
        // show the Notification window in the correct context
        Invoke(new ShowNotificationCallback(ShowNotification), new object[] {
          auth,
          title,
          message,
          openOnClick,
          extraHeight
        });
      }
    }

    #endregion

    protected override void WndProc(ref Message m) {
      base.WndProc(ref m);

      // pick up the HotKey message from RegisterHotKey and call hook callback
      if (m.Msg == WinApi.WM_USER + 1) {
        // show the main form
        BringToFront();
        Show();
        WindowState = FormWindowState.Normal;
        Activate();
      }
    }

    void hotkeyTimer_Tick(object sender, EventArgs e) {
      var data = hotkeyTimer.Tag as HotKeyLauncher;

      // check we don't wait forever
      if (data.Started.AddSeconds(5) < DateTime.Now) {
        hotkeyTimer.Enabled = false;
        return;
      }

      // wait until the modifiers are released
      if ((ModifierKeys & Keys.Alt) != 0
          || (ModifierKeys & Keys.Control) != 0
          || (ModifierKeys & Keys.Shift) != 0) {
        return;
      }

      // cancel the timer
      hotkeyTimer.Enabled = false;

      // invoke the handler method in the correct context
      data.Form.Invoke((MethodInvoker) delegate { HandleHotkey(data.Authenticator); });
    }

    private void HandleHotkey(AuthAuthenticator auth) {
      // get the code
      string code;
      try {
        code = auth.CurrentCode;
      }
      catch (EncryptedSecretDataException) {
        // if the authenticator is current protected we display the password window, get the code, and reprotect it
        // with a bit of window jiggling to make sure we get focus and then put it back

        // save the current window
        var fgwindow = WinApi.GetForegroundWindow();
        var screen = Screen.FromHandle(fgwindow);
        var activewindow = IntPtr.Zero;
        if (Visible) {
          activewindow = WinApi.SetActiveWindow(Handle);
          BringToFront();
        }

        var item = authenticatorList.Items.Cast<AuthenticatorListitem>().FirstOrDefault(i => i.Authenticator == auth);
        code = authenticatorList.GetItemCode(item, screen);

        // restore active window
        if (activewindow != IntPtr.Zero) {
          WinApi.SetActiveWindow(activewindow);
        }

        WinApi.SetForegroundWindow(fgwindow);
      }

      if (code != null) {
        // default to sending the code to the current window
        var keysend = new KeyboardSender(auth.HotKey.Window);
        string command = null;
        if (auth.HotKey.Action == HotKey.HotKeyActions.Notify) {
          if (auth.CopyOnCode) {
            auth.CopyCodeToClipboard(this, code);
          }

          if (code.Length > 5) {
            code = code.Insert(code.Length / 2, " ");
          }

          notifyIcon.ShowBalloonTip(10000, auth.Name, code, ToolTipIcon.Info);
        }

        if (auth.HotKey.Action == HotKey.HotKeyActions.Copy) {
          command = "{COPY}";
        }
        else if (auth.HotKey.Action == HotKey.HotKeyActions.Advanced) {
          command = auth.HotKey.Advanced;
        }
        else if (auth.HotKey.Action == HotKey.HotKeyActions.Inject) {
          command = "{CODE}";
        }

        if (command != null) {
          keysend.SendKeys(this, command, code);
        }
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

        var item = authenticatorList.Items.Cast<AuthenticatorListitem>().FirstOrDefault(i => i.Authenticator == auth);
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

    public void SetClipboardData(object data) {
      var clipRetry = false;
      do {
        try {
          Clipboard.Clear();
          Clipboard.SetDataObject(data, true, 4, 250);
        }
        catch (ExternalException) {
          // only show an error the first time
          clipRetry = (MessageBox.Show(this, strings.ClipboardInUse,
            AuthMain.APPLICATION_NAME,
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
        }
      } while (clipRetry);
    }

    public object GetClipboardData(Type format) {
      bool clipRetry;
      do {
        try {
          var clipdata = Clipboard.GetDataObject();
          return (clipdata != null ? clipdata.GetData(format) : null);
        }
        catch (ExternalException) {
          // only show an error the first time
          clipRetry = (MessageBox.Show(this, strings.ClipboardInUse,
            AuthMain.APPLICATION_NAME,
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
        }
      } while (clipRetry);

      return null;
    }

    private void SetAutoSize() {
      if (Config.AutoSize) {
        if (Config.Count != 0) {
          Width = Math.Max(420,
            authenticatorList.Margin.Horizontal + authenticatorList.GetMaxItemWidth() +
            (Width - authenticatorList.Width));
        }
        else {
          Width = 420;
        }

        // Issue#175; take the smallest of full height or 62% screen height
        var height = Height - authenticatorList.Height;
        height += (Config.Count * authenticatorList.ItemHeight);
        height += commandPanel.Height;
        Height = Math.Min(Screen.GetWorkingArea(this).Height * 62 / 100, height);

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
        notifyIcon.Visible = true;
        notifyIcon.Text = Text;

        // if initially minizied, we need to hide
        if (WindowState == FormWindowState.Minimized) {
          // hide this and metro owner
          Form form = this;
          do {
            form.Hide();
          } while ((form = form.Owner) != null);
        }
      }

      // prompt to import v2
      if (string.IsNullOrEmpty(existingv2Config) == false) {
        var importResult = MessageBox.Show(this,
          string.Format(strings.LoadPreviousAuthenticator, existingv2Config),
          AuthMain.APPLICATION_TITLE,
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question);
        if (importResult == DialogResult.Yes) {
          ImportAuthenticatorFromV2(existingv2Config);
        }

        existingv2Config = null;
      }
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
      // keep in the tray when closing Form 
      if (Config != null && Config.UseTrayIcon && Visible && mExplictClose == false) {
        e.Cancel = true;
        notifyIcon.Visible = true;
        // hide this and metro owner
        Form form = this;
        do {
          form.Hide();
        } while ((form = form.Owner) != null);

        return;
      }

      // remove the Steam hook
      UnhookSteam();

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
          } while (authenticatorList.Items.Cast<AuthenticatorListitem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          // create the Battle.net authenticator
          var form = new AddBattleNetAuthenticator();
          form.Authenticator = authenticator;
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Trion) {
          // create the Trion authenticator
          var existing = 0;
          string name;
          do {
            name = "Trion" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListitem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          var form = new AddTrionAuthenticator();
          form.Authenticator = authenticator;
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Steam) {
          // create the authenticator
          var existing = 0;
          string name;
          do {
            name = "Steam" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListitem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          var form = new AddSteamAuthenticator();
          form.Authenticator = authenticator;
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Google) {
          // create the Google authenticator
          // add the new authenticator
          var existing = 0;
          string name;
          do {
            name = "Google" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListitem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          var form = new AddGoogleAuthenticator();
          form.Authenticator = authenticator;
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.GuildWars) {
          // create the GW2 authenticator
          var existing = 0;
          string name;
          do {
            name = "GuildWars" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListitem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          var form = new AddGuildWarsAuthenticator();
          form.Authenticator = authenticator;
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Microsoft) {
          // create the Microsoft authenticator
          var existing = 0;
          string name;
          do {
            name = "Microsoft" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListitem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          var form = new AddMicrosoftAuthenticator();
          form.Authenticator = authenticator;
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.RFC6238_TIME) {
          // create the Google authenticator
          // add the new authenticator
          var existing = 0;
          string name;
          do {
            name = "Authenticator" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListitem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;
          authenticator.Skin = "AppIcon.png";

          var form = new AddAuthenticator();
          form.Authenticator = authenticator;
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.OktaVerify) {
          // create the Okta Verify authenticator
          var existing = 0;
          string name;
          do {
            name = "Okta" + (existing != 0 ? " (" + existing + ")" : string.Empty);
            existing++;
          } while (authenticatorList.Items.Cast<AuthenticatorListitem>().Count(a => a.Authenticator.Name == name) != 0);

          authenticator.Name = name;
          authenticator.AutoRefresh = false;

          var form = new AddOktaVerifyAuthenticator();
          form.Authenticator = authenticator;
          added = (form.ShowDialog(this) == DialogResult.OK);
        }
        else {
          throw new NotImplementedException(strings.AuthenticatorNotImplemented + ": " +
                                            registeredauth.AuthenticatorType);
        }

        if (added) {
          // save off any new authenticators as a backup
          AuthHelper.SaveToRegistry(Config, authenticator);

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
          introLabel.Visible = (Config.Count == 0);
        }
      }
    }

    void importTextMenu_Click(object sender, EventArgs e) {
      var ofd = new OpenFileDialog();
      ofd.AddExtension = true;
      ofd.CheckFileExists = true;
      ofd.CheckPathExists = true;
      //
      var lastv2File = AuthHelper.GetLastV2Config();
      if (string.IsNullOrEmpty(lastv2File) == false) {
        ofd.InitialDirectory = Path.GetDirectoryName(lastv2File);
        ofd.FileName = Path.GetFileName(lastv2File);
      }

      ofd.Filter = "Authenticator Files (*.config)|*.config|WinAuth Files (*.xml)|*.xml|Text Files (*.txt)|*.txt|Zip Files (*.zip)|*.zip|PGP Files (*.pgp)|*.pgp|All Files (*.*)|*.*";
      ofd.RestoreDirectory = true;
      ofd.Title = AuthMain.APPLICATION_TITLE;
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

    private void addAuthenticatorButton_Click(object sender, EventArgs e) {
      addAuthenticatorMenu.Show(addAuthenticatorButton, addAuthenticatorButton.Width, 0);
    }

    private void optionsButton_Click(object sender, EventArgs e) {
      optionsMenu.Show(optionsButton, optionsButton.Width - optionsMenu.Width, optionsButton.Height - 1);
    }

    private void notifyIcon_DoubleClick(object sender, EventArgs e) {
      BringToFront();
      Show();
      WindowState = FormWindowState.Normal;
      Activate();
    }

    private void authenticatorList_ItemRemoved(object source, AuthenticatorListItemRemovedEventArgs args) {
      foreach (var auth in Config) {
        if (auth == args.Item.Authenticator) {
          Config.Remove(auth);
          break;
        }
      }

      // update UI
      SetAutoSize();

      // if no authenticators, show intro text and remove any encryption
      if (Config.Count == 0) {
        introLabel.Visible = true;
        authenticatorList.Visible = false;
        Config.PasswordType = Authenticator.PasswordTypes.None;
        Config.Password = null;
      }

      // save the current config
      SaveConfig();
    }

    private void authenticatorList_Reordered(object source, AuthenticatorListReorderedEventArgs args) {
      // set the new order of items in Config from that of the list
      var count = authenticatorList.Items.Count;
      for (var i = 0; i < count; i++) {
        var item = (AuthenticatorListitem) authenticatorList.Items[i];
        Config.Where(a => a == item.Authenticator).FirstOrDefault().Index = i;
      }

      // resort the config list
      Config.Sort();
      // update the notify menu
      LoadNotifyMenu(notifyMenu);

      // update UI
      SetAutoSize();

      // save the current config
      SaveConfig();
    }

    private void authenticatorList_DoubleClick(object source, AuthenticatorListDoubleClickEventArgs args) {
      RunAction(args.Authenticator, AuthConfig.NotifyActions.CopyToClipboard);
    }

    private void MainForm_MouseDown(object sender, MouseEventArgs e) {
      EndRenaming();
    }

    private void commandPanel_MouseDown(object sender, MouseEventArgs e) {
      EndRenaming();
    }

    private void MainForm_Resize(object sender, EventArgs e) {
      SuspendLayout();
      if (listoffset.Bottom != 0) {
        authenticatorList.Height = Height - listoffset.Height;
        authenticatorList.Width = Width - listoffset.Width;
      }

      ResumeLayout(true);
    }

    private void MainForm_ResizeEnd(object sender, EventArgs e) {
      if (Config != null && Config.AutoSize == false) {
        Config.Width = Width;
        Config.Height = Height;
      }
    }

    private void passwordButton_Click(object sender, EventArgs e) {
      if (passwordField.Text.Trim().Length == 0) {
        passwordErrorLabel.Text = strings.EnterPassword;
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

    void SystemEvents_TimeChanged(object sender, EventArgs e) {
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

    #region Options menu

    private void LoadOptionsMenu(ContextMenuStrip menu) {
      ToolStripMenuItem menuitem;

      menu.Items.Clear();

      if (Config == null || Config.IsReadOnly == false) {
        menuitem = new ToolStripMenuItem(strings.MenuChangeProtection + "...");
        menuitem.Name = "changePasswordOptionsMenuItem";
        menuitem.Click += changePasswordOptionsMenuItem_Click;
        menu.Items.Add(menuitem);
        menu.Items.Add(new ToolStripSeparator() {Name = "changePasswordOptionsSeparatorItem"});
      }

      if (Config != null && Config.IsPortable == false) {
        menuitem = new ToolStripMenuItem(strings.MenuStartWithWindows);
        menuitem.Name = "startWithWindowsOptionsMenuItem";
        menuitem.Click += startWithWindowsOptionsMenuItem_Click;
        menu.Items.Add(menuitem);
      }

      menuitem = new ToolStripMenuItem(strings.MenuAlwaysOnTop);
      menuitem.Name = "alwaysOnTopOptionsMenuItem";
      menuitem.Click += alwaysOnTopOptionsMenuItem_Click;
      menu.Items.Add(menuitem);

      menuitem = new ToolStripMenuItem(strings.MenuUseSystemTrayIcon);
      menuitem.Name = "useSystemTrayIconOptionsMenuItem";
      menuitem.Click += useSystemTrayIconOptionsMenuItem_Click;
      menu.Items.Add(menuitem);

      menuitem = new ToolStripMenuItem(strings.MenuAutoSize);
      menuitem.Name = "autoSizeOptionsMenuItem";
      menuitem.Click += autoSizeOptionsMenuItem_Click;
      menu.Items.Add(menuitem);

      menu.Items.Add(new ToolStripSeparator());

      menuitem = new ToolStripMenuItem(strings.MenuExport);
      menuitem.Name = "exportOptionsMenuItem";
      menuitem.Click += exportOptionsMenuItem_Click;
      menu.Items.Add(menuitem);

      menu.Items.Add(new ToolStripSeparator());

      menuitem = new ToolStripMenuItem(strings.MenuAbout + "...");
      menuitem.Name = "aboutOptionsMenuItem";
      menuitem.Click += aboutOptionMenuItem_Click;
      menu.Items.Add(menuitem);

      menuitem = new ToolStripMenuItem("Check for updates");
      menuitem.Name = "checkUpdatesMenuItem";
      menuitem.Click += (s, e) => Updater.CheckForUpdates(false);
      menu.Items.Add(menuitem);

      menu.Items.Add(new ToolStripSeparator());

      menuitem = new ToolStripMenuItem(strings.MenuExit);
      menuitem.Name = "exitOptionsMenuItem";
      menuitem.ShortcutKeys = Keys.F4 | Keys.Alt;
      menuitem.Click += exitOptionMenuItem_Click;
      menu.Items.Add(menuitem);
    }

    private void LoadNotifyMenu(ContextMenuStrip menu) {
      ToolStripMenuItem menuitem;
      ToolStripMenuItem subitem;

      menu.Items.Clear();

      menuitem = new ToolStripMenuItem(strings.MenuOpen);
      menuitem.Name = "openOptionsMenuItem";
      menuitem.Click += openOptionsMenuItem_Click;
      menu.Items.Add(menuitem);
      menu.Items.Add(new ToolStripSeparator() {Name = "openOptionsSeparatorItem"});

      if (Config != null && Config.Count != 0) {
        // because of window size, we only show first 30.
        // @todo change to MRU
        var index = 1;
        foreach (var auth in Config.Take(30)) {
          menuitem = new ToolStripMenuItem(index + ". " + auth.Name);
          menuitem.Name = "authenticatorOptionsMenuItem_" + index;
          menuitem.Tag = auth;
          menuitem.Click += authenticatorOptionsMenuItem_Click;
          menu.Items.Add(menuitem);
          index++;
        }

        var separator = new ToolStripSeparator();
        separator.Name = "authenticatorOptionsSeparatorItem";
        menu.Items.Add(separator);

        menuitem = new ToolStripMenuItem(strings.DefaultAction);
        menuitem.Name = "defaultActionOptionsMenuItem";
        menu.Items.Add(menuitem);
        subitem = new ToolStripMenuItem(strings.DefaultActionNotification);
        subitem.Name = "defaultActionNotificationOptionsMenuItem";
        subitem.Click += defaultActionNotificationOptionsMenuItem_Click;
        menuitem.DropDownItems.Add(subitem);
        subitem = new ToolStripMenuItem(strings.DefaultActionCopyToClipboard);
        subitem.Name = "defaultActionCopyToClipboardOptionsMenuItem";
        subitem.Click += defaultActionCopyToClipboardOptionsMenuItem_Click;
        menuitem.DropDownItems.Add(subitem);
        menu.Items.Add(menuitem);

        separator = new ToolStripSeparator();
        separator.Name = "authenticatorActionOptionsSeparatorItem";
        menu.Items.Add(separator);
      }

      //if (this.Config != null)
      //{
      //	menuitem = new ToolStripMenuItem(strings.MenuUseSystemTrayIcon);
      //	menuitem.Name = "useSystemTrayIconOptionsMenuItem";
      //	menuitem.Click += useSystemTrayIconOptionsMenuItem_Click;
      //	menu.Items.Add(menuitem);

      //	menu.Items.Add(new ToolStripSeparator());
      //}

      menuitem = new ToolStripMenuItem(strings.MenuAbout + "...");
      menuitem.Name = "aboutOptionsMenuItem";
      menuitem.Click += aboutOptionMenuItem_Click;
      menu.Items.Add(menuitem);

      menu.Items.Add(new ToolStripSeparator());

      menuitem = new ToolStripMenuItem(strings.MenuExit);
      menuitem.Name = "exitOptionsMenuItem";
      menuitem.ShortcutKeys = Keys.F4 | Keys.Alt;
      menuitem.Click += exitOptionMenuItem_Click;
      menu.Items.Add(menuitem);
    }

    private void optionsMenu_Opening(object sender, CancelEventArgs e) {
      OpeningOptionsMenu(optionsMenu, e);
    }

    private void notifyMenu_Opening(object sender, CancelEventArgs e) {
      OpeningNotifyMenu(notifyMenu, e);
    }

    private void OpeningOptionsMenu(ContextMenuStrip menu, CancelEventArgs e) {
      if (Config == null) {
        return;
      }

      if (menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "changePasswordOptionsMenuItem") is ToolStripMenuItem menuItem) {
        menuItem.Enabled = (Config != null && Config.Count != 0);
      }

      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "openOptionsMenuItem") as
          ToolStripMenuItem;
      if (menuItem != null) {
        menuItem.Visible = (Config.UseTrayIcon && Visible == false);
      }

      var item = menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "openOptionsSeparatorItem");
      if (item != null) {
        item.Visible = (Config.UseTrayIcon && Visible == false);
      }

      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "startWithWindowsOptionsMenuItem") as ToolStripMenuItem;
      if (menuItem != null) {
        menuItem.Checked = Config.StartWithWindows;
      }

      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "alwaysOnTopOptionsMenuItem") as
          ToolStripMenuItem;
      if (menuItem != null) {
        menuItem.Checked = Config.AlwaysOnTop;
      }

      menuItem = menu.Items.Cast<ToolStripItem>()
        .FirstOrDefault(t => t.Name == "useSystemTrayIconOptionsMenuItem") as ToolStripMenuItem;
      if (menuItem != null) {
        menuItem.Checked = Config.UseTrayIcon;
      }

      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "autoSizeOptionsMenuItem") as
          ToolStripMenuItem;
      if (menuItem != null) {
        menuItem.Checked = Config.AutoSize;
      }

      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "autoSizeOptionsMenuItem") as
          ToolStripMenuItem;
      if (menuItem != null) {
        menuItem.Checked = Config.AutoSize;
      }
    }

    private void OpeningNotifyMenu(ContextMenuStrip menu, CancelEventArgs e) {
      if (Config == null) {
        return;
      }

      if (menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "changePasswordOptionsMenuItem") is ToolStripMenuItem menuItem) {
        menuItem.Enabled = (Config != null && Config.Count != 0);
      }

      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "openOptionsMenuItem") as
          ToolStripMenuItem;
      if (menuItem != null) {
        menuItem.Visible = (Config.UseTrayIcon && Visible == false);
      }

      var item = menu.Items.Cast<ToolStripItem>().FirstOrDefault(t => t.Name == "openOptionsSeparatorItem");
      if (item != null) {
        item.Visible = (Config.UseTrayIcon && Visible == false);
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
          var passwordForm = new GetPasswordForm();
          passwordForm.InvalidPassword = invalidPassword;
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

      var form = new ChangePasswordForm();
      form.PasswordType = Config.PasswordType;
      form.HasPassword = ((Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0);
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

    private void openOptionsMenuItem_Click(object sender, EventArgs e) {
      // show the main form
      BringToFront();
      Show();
      WindowState = FormWindowState.Normal;
      Activate();
    }

    private void authenticatorOptionsMenuItem_Click(object sender, EventArgs e) {
      var menuitem = (ToolStripMenuItem) sender;
      var auth = menuitem.Tag as AuthAuthenticator;
      var item = authenticatorList.Items.Cast<AuthenticatorListitem>().FirstOrDefault(i => i.Authenticator == auth);
      if (item != null) {
        RunAction(auth, Config.NotifyAction);

        //string code = authenticatorList.GetItemCode(item);
        //if (code != null)
        //{
        //	if (auth.CopyOnCode)
        //	{
        //		auth.CopyCodeToClipboard(this, code);
        //	}
        //	if (code.Length > 5)
        //	{
        //		code = code.Insert(code.Length / 2, " ");
        //	}
        //	notifyIcon.ShowBalloonTip(10000, auth.Name, code, ToolTipIcon.Info);
        //}
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
          var checkform = new GetPasswordForm();
          checkform.InvalidPassword = invalidPassword;
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
      var form = new AboutForm();
      form.Config = Config;
      form.ShowDialog(this);
    }

    private void exitOptionMenuItem_Click(object sender, EventArgs e) {
      mExplictClose = true;
      Close();
    }

    #endregion

    #region Custom Events

    void OnConfigChanged(object source, ConfigChangedEventArgs args) {
      if (args.PropertyName == "AlwaysOnTop") {
        TopMost = Config.AlwaysOnTop;
      }
      else if (args.PropertyName == "UseTrayIcon") {
        var useTrayIcon = Config.UseTrayIcon;
        if (useTrayIcon == false && Visible == false) {
          BringToFront();
          Show();
          WindowState = FormWindowState.Normal;
          Activate();
        }

        notifyIcon.Visible = useTrayIcon;
      }
      else if (args.PropertyName == "AutoSize" ||
               (args.PropertyName == "Authenticator" && args.AuthenticatorChangedEventArgs.Property == "Name")) {
        SetAutoSize();
        Invalidate();
      }
      else if (args.PropertyName == "StartWithWindows") {
        if (Config.IsPortable == false) {
          AuthHelper.SetStartWithWindows(Config.StartWithWindows);
        }
      }

      // batch up saves so they can be done out of line
      SaveConfig();
    }

    #endregion

    class HotKeyLauncher {
      public MainForm Form { get; }

      public AuthAuthenticator Authenticator { get; }

      public DateTime Started { get; }

      public HotKeyLauncher(MainForm form, AuthAuthenticator auth) {
        Started = DateTime.Now;
        Form = form;
        Authenticator = auth;
      }
    }
  }
}