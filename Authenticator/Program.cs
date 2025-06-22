using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using sergiye.Common;

namespace Authenticator {
  static class Program {
    [STAThread]
    static void Main() {

      if (!OperatingSystemHelper.IsCompatible(false, out var errorMessage, out var fixAction)) {
        if (fixAction != null) {
          if (MessageBox.Show(errorMessage, Updater.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
            fixAction?.Invoke();
          }
        }
        else {
          MessageBox.Show(errorMessage, Updater.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        Environment.Exit(0);
      }

      if (WinApiHelper.CheckRunningInstances(true, true)) {
        MessageBox.Show($"{Updater.ApplicationName} is already running.", Updater.ApplicationName,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      AppDomain.CurrentDomain.UnhandledException +=
  (s, e) => AuthHelper.ShowException(e.ExceptionObject as Exception);
      Application.ThreadException += (s, e) => AuthHelper.ShowException(e.Exception);
      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

      try {
        StartProgram();
      }
      catch (Exception ex) {
        AuthHelper.ShowException(ex);
        throw;
      }
    }

    private static void StartProgram() {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }
  }
}