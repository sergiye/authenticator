using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Authenticator {
  
  /// <summary>
  /// Class to manage sending of keys to other applications.
  /// </summary>
  public class KeyboardSender {
    /// <summary>
    /// Hold the handle to the destination window
    /// </summary>
    private IntPtr m_hWnd;

    /// <summary>
    /// Create a new sender and get the window handle
    /// </summary>
    /// <param name="processName"></param>
    /// <param name="windowTitle"></param>
    public KeyboardSender(string window) {
      m_hWnd = FindWindow(window);
    }

    /// <summary>
    /// Send the keys to the expected window or current window if null
    /// </summary>
    /// <param name="keys">stirng to send</param>
    public void SendKeys(MainForm form, string keys, string code) {
      if (m_hWnd != IntPtr.Zero && m_hWnd != WinApi.GetForegroundWindow()) {
        WinApi.SetForegroundWindow(m_hWnd);
        System.Threading.Thread.Sleep(50);
      }

      // replace any {CODE} items
      keys = Regex.Replace(keys, @"\{\s*CODE\s*\}", code, RegexOptions.Singleline);

      // clear events and stop input
      Application.DoEvents();
      var blocked = WinApi.BlockInput(true);
      try {
        // for now just split into parts and run each
        foreach (Match match in Regex.Matches(keys, @"\{.*?\}|[^\{]*", RegexOptions.Singleline)) {
          // split into either {CMD d w} or just plain text
          if (match.Success) {
            var single = match.Value;
            if (single.Length == 0) {
              continue;
            }

            if (single[0] == '{') {
              // send command {COMMAND delay repeat}
              var cmdMatch = Regex.Match(single.Trim(), @"\{([^\s]+)\s*(\d*)\s*(\d*)\}");
              if (cmdMatch.Success) {
                // extract the command and any optional delay and repeat
                var cmd = cmdMatch.Groups[1].Value.ToUpper();
                var delay = 0;
                if (cmdMatch.Groups[2].Success && cmdMatch.Groups[2].Value.Length != 0) {
                  int.TryParse(cmdMatch.Groups[2].Value, out delay);
                }

                var repeat = 1;
                if (cmdMatch.Groups[3].Success && cmdMatch.Groups[3].Value.Length != 0) {
                  int.TryParse(cmdMatch.Groups[3].Value, out repeat);
                }

                // run the command
                switch (cmd) {
                  case "BS":
                    SendKey('\x08', delay, repeat);
                    break;
                  case "TAB":
                    SendKey('\t', delay, repeat);
                    break;
                  case "ENTER":
                    SendKey('\n', delay, repeat);
                    break;
                  case "WAIT":
                    for (; repeat > 0; repeat--) {
                      System.Threading.Thread.Sleep(delay);
                    }

                    break;
                  case "COPY":
                    form.Invoke(new MainForm.SetClipboardDataDelegate(form.SetClipboardData), new object[] {code});
                    break;
                  case "PASTE":
                    var clipboard = form.Invoke(new MainForm.GetClipboardDataDelegate(form.GetClipboardData),
                      new object[] {typeof(string)}) as string;
                    if (string.IsNullOrEmpty(clipboard) == false) {
                      foreach (var key in clipboard) {
                        SendKey(key);
                      }
                    }

                    break;
                  case "EXIT":
                    Application.Exit();
                    break;
                  default:
                    break;
                }
              }
            }
            else {
              SendKey(single);
            }
          }
        }
      }
      finally {
        // resume input
        if (blocked) {
          WinApi.BlockInput(false);
        }

        Application.DoEvents();
      }
    }

    /// <summary>
    /// Send a single key to the current window
    /// </summary>
    /// <param name="key">key to send</param>
    private void SendKey(char key) {
      SendKey(key, 0, 1);
    }

    /// <summary>
    /// Send a key string to the current window
    /// </summary>
    /// <param name="key">key string to send</param>
    private void SendKey(string key) {
      SendKey(key, 0, 1);
    }

    /// <summary>
    /// Send a key string to the current window a number of times with a delay after each key
    /// </summary>
    /// <param name="key">key string to send</param>
    /// <param name="delay">delay in millisecs after each keypress</param>
    /// <param name="repeat">number of times</param>
    private void SendKey(string key, int delay, int repeat) {
      for (; repeat > 0; repeat--) {
        // Issue#100: change to use InputSimulator as SendKeys does not work for internation keyboards
        InputSimulator.SimulateTextEntry(key);
        System.Threading.Thread.Sleep(delay != 0 ? delay : 50);
      }
    }

    /// <summary>
    /// Send a single key to the current window a number of times with a delay after each key
    /// </summary>
    /// <param name="key">key to send</param>
    /// <param name="delay">delay in millisecs after each keypress</param>
    /// <param name="repeat">number of times</param>
    private void SendKey(char key, int delay, int repeat) {
      SendKey(key.ToString(), delay, repeat);
    }

    /// <summary>
    /// Find a window for the give process and/or title
    /// </summary>
    /// <param name="processName">name of process</param>
    /// <param name="window">text of window title or process name</param>
    /// <returns>Window handle if we can match the process and/or title</returns>
    private static IntPtr FindWindow(string window) {
      // default to return current window
      if (string.IsNullOrEmpty(window)) {
        return WinApi.GetForegroundWindow();
      }

      // build regex
      Regex reg;
      var match = Regex.Match(window, @"/(.*)/([a-z]*)", RegexOptions.IgnoreCase);
      if (match.Success) {
        var regoptions = RegexOptions.None;
        if (match.Groups[2].Value.Contains("i")) {
          regoptions |= RegexOptions.IgnoreCase;
        }

        reg = new Regex(match.Groups[1].Value, regoptions);
      }
      else {
        reg = new Regex(Regex.Escape(window), RegexOptions.IgnoreCase);
      }


      // find process matches
      var matchingProcesses = new List<Process>();
      foreach (var process in Process.GetProcesses()) {
        if (reg.IsMatch(process.ProcessName) || reg.IsMatch(process.MainWindowTitle)) {
          matchingProcesses.Add(process);
        }
      }

      // return first match or zero
      return (matchingProcesses.Count != 0 ? matchingProcesses[0].MainWindowHandle : IntPtr.Zero);

/*
			// if we have a title match it in the window titles
			if (string.IsNullOrEmpty(windowTitle) == false)
			{
				if (useregex == true)
				{
					Regex regex = new Regex(windowTitle, RegexOptions.Singleline);
					foreach (Process process in processes)
					{
						if (regex.IsMatch(process.MainWindowTitle) == true)
						{
							return process.MainWindowHandle;
						}
					}
				}
				else
				{
					string lowerWindowTitle = (windowTitle != null ? windowTitle.ToLower() : null);
					foreach (Process process in processes)
					{
						if (process.MainWindowTitle.ToLower().IndexOf(lowerWindowTitle) != -1)
						{
							return process.MainWindowHandle;
						}
					}
				}
			}
			else if (string.IsNullOrEmpty(processName) == false)
			{
				foreach (Process process in processes)
				{
					if (string.Compare(process.ProcessName, processName, true) == 0)
					{
						return process.MainWindowHandle;
					}
				}
			}
			else if (string.IsNullOrEmpty(processName) == true)
			{
				return WinApi.GetForegroundWindow();
			}
			else if (processes.Length != 0)
			{
				return processes[0].MainWindowHandle;
			}

			// didn't find anything
			return IntPtr.Zero;
*/
    }
  }
}