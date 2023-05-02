﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Resources;
using System.Windows.Forms;
using Authenticator.Resources;

namespace Authenticator {
  static class AuthMain {
    public const string APPLICATION_NAME = "Authenticator";
    public const string APPLICATION_TITLE = "Authenticator";

    public static List<Tuple<string, string>> AuthenticatorIcons = new List<Tuple<string, string>> {
      {new Tuple<string, string>(APPLICATION_NAME, "AppIcon.png")},

      {new Tuple<string, string>("+Google", "GoogleIcon.png")},
      {new Tuple<string, string>("Authenticator", "GoogleAuthenticatorIcon.png")},
      {new Tuple<string, string>("Google", "GoogleIcon.png")},
      {new Tuple<string, string>("Chrome", "ChromeIcon.png")},
      {new Tuple<string, string>("Google (Blue)", "Google2Icon.png")},
      {new Tuple<string, string>("GMail", "GMailIcon.png")},

      {new Tuple<string, string>("+Games", "BattleNetAuthenticatorIcon.png")},
      {new Tuple<string, string>("Battle.Net", "BattleNetAuthenticatorIcon.png")},
      {new Tuple<string, string>("World of Warcraft", "WarcraftIcon.png")},
      {new Tuple<string, string>("Diablo III", "DiabloIcon.png")},
      {new Tuple<string, string>("s8", string.Empty)},
      {new Tuple<string, string>("Steam", "SteamAuthenticatorIcon.png")},
      {new Tuple<string, string>("Steam (Circle)", "SteamIcon.png")},
      {new Tuple<string, string>("s1", string.Empty)},
      {new Tuple<string, string>("EA", "EAIcon.png")},
      {new Tuple<string, string>("EA (White)", "EA2Icon.png")},
      {new Tuple<string, string>("EA (Black)", "EA3Icon.png")},
      {new Tuple<string, string>("s2", string.Empty)},
      {new Tuple<string, string>("Origin", "OriginIcon.png")},
      {new Tuple<string, string>("s3", string.Empty)},
      {new Tuple<string, string>("ArenaNet", "ArenaNetIcon.png")},
      {new Tuple<string, string>("Guild Wars 2", "GuildWarsAuthenticatorIcon.png")},
      {new Tuple<string, string>("s4", string.Empty)},
      {new Tuple<string, string>("Trion", "TrionAuthenticatorIcon.png")},
      {new Tuple<string, string>("Glyph", "GlyphIcon.png")},
      {new Tuple<string, string>("ArcheAge", "ArcheAgeIcon.png")},
      {new Tuple<string, string>("Rift", "RiftIcon.png")},
      {new Tuple<string, string>("Defiance", "DefianceIcon.png")},
      {new Tuple<string, string>("s5", string.Empty)},
      {new Tuple<string, string>("WildStar", "WildstarIcon.png")},
      {new Tuple<string, string>("s6", string.Empty)},
      {new Tuple<string, string>("Firefall", "FirefallIcon.png")},
      {new Tuple<string, string>("s7", string.Empty)},
      {new Tuple<string, string>("RuneScape", "RuneScapeIcon.png")},
      {new Tuple<string, string>("s9", string.Empty)},
      {new Tuple<string, string>("SWTOR", "Swtor.png")},
      {new Tuple<string, string>("SWTOR (Empire)", "SwtorEmpire.png")},
      {new Tuple<string, string>("SWTOR (Republic)", "SwtorRepublic.png")},

      {new Tuple<string, string>("+Software", "MicrosoftAuthenticatorIcon.png")},
      {new Tuple<string, string>("Microsoft", "MicrosoftAuthenticatorIcon.png")},
      {new Tuple<string, string>("Windows 8", "Windows8Icon.png")},
      {new Tuple<string, string>("Windows 7", "Windows7Icon.png")},
      {new Tuple<string, string>("Windows Phone", "WindowsPhoneIcon.png")},
      {new Tuple<string, string>("s3", string.Empty)},
      {new Tuple<string, string>("Android", "AndroidIcon.png")},
      {new Tuple<string, string>("s4", string.Empty)},
      {new Tuple<string, string>("Apple", "AppleIcon.png")},
      {new Tuple<string, string>("Apple (Black)", "AppleWhiteIcon.png")},
      {new Tuple<string, string>("Apple (Color)", "AppleColorIcon.png")},
      {new Tuple<string, string>("Mac", "MacIcon.png")},
      {new Tuple<string, string>("s5", string.Empty)},
      {new Tuple<string, string>("BitBucket", "BitBucketIcon.png")},
      {new Tuple<string, string>("DigitalOcean", "DigitalOceanIcon.png")},
      {new Tuple<string, string>("Dreamhost", "DreamhostIcon.png")},
      {new Tuple<string, string>("DropBox", "DropboxIcon.png")},
      {new Tuple<string, string>("DropBox (White)", "DropboxWhiteIcon.png")},
      {new Tuple<string, string>("Evernote", "EvernoteIcon.png")},
      {new Tuple<string, string>("Git", "GitIcon.png")},
      {new Tuple<string, string>("GitHub", "GitHubIcon.png")},
      {new Tuple<string, string>("GitHub (White)", "GitHub2Icon.png")},
      {new Tuple<string, string>("GitLab", "GitLabIcon.png")},
      {new Tuple<string, string>("GitLab (Fox)", "GitLabFox2Icon.png")},
      {new Tuple<string, string>("IFTTT", "IFTTTIcon.png")},
      {new Tuple<string, string>("Itch.io", "ItchIcon.png")},
      {new Tuple<string, string>("KickStarter", "KickStarterIcon.png")},
      {new Tuple<string, string>("LastPass", "LastPassIcon.png")},
      {new Tuple<string, string>("Name.com", "NameIcon.png")},
      {new Tuple<string, string>("Teamviewer", "TeamviewerIcon.png")},
      {new Tuple<string, string>("s7", string.Empty)},
      {new Tuple<string, string>("Amazon", "AmazonIcon.png")},
      {new Tuple<string, string>("Amazon AWS", "AmazonAWSIcon.png")},
      {new Tuple<string, string>("s8", string.Empty)},
      {new Tuple<string, string>("PayPal", "PayPalIcon.png")},

      {new Tuple<string, string>("+Crypto", "BitcoinIcon.png")},
      {new Tuple<string, string>("Bitcoin", "BitcoinIcon.png")},
      {new Tuple<string, string>("Bitcoin Gold", "BitcoinGoldIcon.png")},
      {new Tuple<string, string>("Bitcoin Euro", "BitcoinEuroIcon.png")},
      {new Tuple<string, string>("Litecoin", "LitecoinIcon.png")},
      {new Tuple<string, string>("Dogecoin", "DogeIcon.png")},

      {new Tuple<string, string>("+Social", "FacebookIcon.png")},
      {new Tuple<string, string>("eBay", "eBayIcon.png")},
      {new Tuple<string, string>("Facebook", "FacebookIcon.png")},
      {new Tuple<string, string>("Flickr", "FlickrIcon.png")},
      {new Tuple<string, string>("Instagram", "InstagramIcon.png")},
      {new Tuple<string, string>("LinkedIn", "LinkedinIcon.png")},
      {new Tuple<string, string>("Tumblr", "TumblrIcon.png")},
      {new Tuple<string, string>("Tumblr (Flat)", "Tumblr2Icon.png")},
      {new Tuple<string, string>("Twitter", "TwitterIcon.png")},
      {new Tuple<string, string>("Wordpress", "WordpressIcon.png")},
      {new Tuple<string, string>("Wordpress (B&W)", "WordpressWhiteIcon.png")},
      {new Tuple<string, string>("Yahoo", "YahooIcon.png")},
      {new Tuple<string, string>("Okta", "OktaVerifyAuthenticatorIcon.png")}
    };

    public static List<RegisteredAuthenticator> RegisteredAuthenticators = new List<RegisteredAuthenticator> {
      new RegisteredAuthenticator {
        Name = "Authenticator", AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.RFC6238_TIME,
        Icon = "AppIcon.png"
      },
      null,
      new RegisteredAuthenticator {
        Name = "Google", AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.Google, Icon = "GoogleIcon.png"
      },
      new RegisteredAuthenticator {
        Name = "Microsoft", AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.Microsoft,
        Icon = "MicrosoftAuthenticatorIcon.png"
      },
      new RegisteredAuthenticator {
        Name = "Battle.Net", AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.BattleNet,
        Icon = "BattleNetAuthenticatorIcon.png"
      },
      new RegisteredAuthenticator {
        Name = "Guild Wars 2", AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.GuildWars,
        Icon = "GuildWarsAuthenticatorIcon.png"
      },
      new RegisteredAuthenticator {
        Name = "Glyph / Trion", AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.Trion,
        Icon = "GlyphIcon.png"
      },
      new RegisteredAuthenticator {
        Name = "Steam", AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.Steam,
        Icon = "SteamAuthenticatorIcon.png"
      },
      new RegisteredAuthenticator {
        Name = "Okta Verify", AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.OktaVerify,
        Icon = "OktaVerifyAuthenticatorIcon.png"
      }
    };

    public static ResourceManager StringResources =
      new ResourceManager(typeof(strings).FullName, typeof(strings).Assembly);

    [STAThread]
    static void Main() {
      try {
        //
        //string dir = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AuthMain.APPLICATION_NAME);
        //if (Directory.Exists(dir) == false)
        //{
        //	dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //}

        using (var instance = new SingleGlobalInstance(2000)) {
          if (!Debugger.IsAttached) {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            Application.ThreadException += OnThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            try {
              main();
            }
            catch (Exception ex) {
              LogException(ex);
              throw;
            }
          }
          else {
            main();
          }
        }
      }
      catch (TimeoutException) {
        // find the window or notify window
        foreach (var process in Process.GetProcesses()) {
          if (process.ProcessName == APPLICATION_NAME) {
            process.Refresh();

            var hwnd = process.MainWindowHandle;
            if (hwnd == (IntPtr) 0) {
              hwnd = WinApi.FindWindow(null, APPLICATION_TITLE);
            }

            // send it the open message
            WinApi.SendMessage(hwnd, WinApi.WM_USER + 1, 0, (IntPtr) 0);
            return;
          }
        }

        // fallback
        MessageBox.Show(string.Format(strings.AlreadyRunning, APPLICATION_NAME), APPLICATION_TITLE,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    static void OnThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
      LogException(e.Exception as Exception);
    }

    static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e) {
      LogException(e.ExceptionObject as Exception);
    }

    public static void LogException(Exception ex, bool silently = false) {
      // add catch for unknown application exceptions to try and get closer to bug
      //StringBuilder capture = new StringBuilder(DateTime.Now.ToString("u") + " ");
      //try
      //{
      //	Exception e = ex;
      //	capture.Append(e.Message).Append(Environment.NewLine);
      //	while (e != null)
      //	{
      //		capture.Append(new StackTrace(e, true).ToString()).Append(Environment.NewLine);
      //		e = e.InnerException;
      //	}
      //	//
      //	LogMessage(capture.ToString());
      //}
      //catch (Exception) { }

      try {
        //Logger.Error(ex);

        if (silently == false) {
          var report = new ExceptionForm();
          report.ErrorException = ex;
          report.TopMost = true;
          if (form != null && form.Config != null) {
            report.Config = form.Config;
          }

          if (report.ShowDialog() == DialogResult.Cancel) {
            Process.GetCurrentProcess().Kill();
          }
        }
      }
      catch (Exception) {
      }
    }

    private static MainForm form;

    private static void main() {
      // Fix #226: set to use TLS1.2
      try {
        ServicePointManager.SecurityProtocol =
          SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      }
      catch (Exception) {
        // not 4.5 installed - we could prompt, but not for now
      }

      strings.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      form = new MainForm();
      Application.Run(form);
    }
  }
}