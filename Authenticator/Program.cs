﻿using System;
using System.Diagnostics;
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
          if (process.ProcessName != Updater.ApplicationName) continue;
          process.Refresh();

          var hwnd = process.MainWindowHandle;
          if (hwnd == (IntPtr) 0) {
            hwnd = WinApi.FindWindow(null, Updater.ApplicationTitle);
          }

          // send it the open message
          WinApi.SendMessage(hwnd, WinApi.WM_USER + 1, 0, (IntPtr) 0);
          return;
        }

        // fallback
        MessageBox.Show($"{Updater.ApplicationName} is already running.", Updater.ApplicationName,
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