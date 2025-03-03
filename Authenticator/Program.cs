using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace Authenticator {
  static class Program {
    [STAThread]
    static void Main() {

      if (!VersionCompatibility.IsCompatible()) {
        MessageBox.Show("The application is not compatible with your region.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        Environment.Exit(0);
      }

      if (Debugger.IsAttached) {
        StartProgram();
        return;
      }

      try {
        using (var instance = new SingleGlobalInstance(2000)) {
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
      }
      catch (TimeoutException) {
        // find the window or notify window
        foreach (var process in Process.GetProcesses()) {
          if (process.ProcessName != AuthHelper.APPLICATION_NAME) continue;
          process.Refresh();

          var hwnd = process.MainWindowHandle;
          if (hwnd == (IntPtr) 0) {
            hwnd = WinApi.FindWindow(null, AuthHelper.APPLICATION_TITLE);
          }

          // send it the open message
          WinApi.SendMessage(hwnd, WinApi.WM_USER + 1, 0, (IntPtr) 0);
          return;
        }

        // fallback
        MessageBox.Show($"{AuthHelper.APPLICATION_NAME} is already running.", AuthHelper.APPLICATION_TITLE,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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