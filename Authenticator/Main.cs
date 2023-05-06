using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace Authenticator {
  static class AuthMain {
    public const string APPLICATION_NAME = "Authenticator";
    public const string APPLICATION_TITLE = "Authenticator";

    public static Dictionary<string, string> AuthenticatorIcons = new Dictionary<string, string> {
      {"Default", "AppIcon.png"},

      {"+Social", "FacebookIcon.png"},
      {"Facebook", "FacebookIcon.png"},
      {"Instagram", "InstagramIcon.png"},
      {"Twitter", "TwitterIcon.png"},
      {"LinkedIn", "LinkedinIcon.png"},
      {"Flickr", "FlickrIcon.png"},
      {"Tumblr", "TumblrIcon.png"},
      {"Tumblr (Flat)", "Tumblr2Icon.png"},
      {"Wordpress", "WordpressIcon.png"},
      {"Wordpress (B&W)", "WordpressWhiteIcon.png"},
      {"Okta", "OktaVerifyAuthenticatorIcon.png"},
      {"Yahoo", "YahooIcon.png"},
      {"eBay", "eBayIcon.png"},

      {"+Software", "MicrosoftAuthenticatorIcon.png"},
      {"Microsoft", "MicrosoftAuthenticatorIcon.png"},
      {"Windows 8", "Windows8Icon.png"},
      {"Windows 7", "Windows7Icon.png"},
      {"Windows Phone", "WindowsPhoneIcon.png"},

      {"s1", string.Empty},
      {"Android", "AndroidIcon.png"},
      {"s2", string.Empty},
      {"Apple", "AppleIcon.png"},
      {"Apple (Black)", "AppleWhiteIcon.png"},
      {"Apple (Color)", "AppleColorIcon.png"},
      {"Mac", "MacIcon.png"},

      {"s3", string.Empty},
      {"Amazon", "AmazonIcon.png"},
      {"Amazon AWS", "AmazonAWSIcon.png"},

      {"s4", string.Empty},
      {"PayPal", "PayPalIcon.png"},

      {"s5", string.Empty},
      {"Git", "GitIcon.png"},
      {"GitHub", "GitHubIcon.png"},
      {"GitHub (White)", "GitHub2Icon.png"},
      {"GitLab", "GitLabIcon.png"},
      {"GitLab (Fox)", "GitLabFox2Icon.png"},
      {"BitBucket", "BitBucketIcon.png"},
      {"DigitalOcean", "DigitalOceanIcon.png"},
      {"Dreamhost", "DreamhostIcon.png"},
      {"DropBox", "DropboxIcon.png"},
      {"DropBox (White)", "DropboxWhiteIcon.png"},
      {"Evernote", "EvernoteIcon.png"},
      {"IFTTT", "IFTTTIcon.png"},
      {"Itch.io", "ItchIcon.png"},
      {"KickStarter", "KickStarterIcon.png"},
      {"LastPass", "LastPassIcon.png"},
      {"Name.com", "NameIcon.png"},
      {"Teamviewer", "TeamviewerIcon.png"},
      {"Xero", "XeroIcon.png"},
      {"Zoho", "ZohoIcon.png"},


      {"+Google", "GoogleIcon.png"},
      {"Authenticator", "GoogleAuthenticatorIcon.png"},
      {"Google", "GoogleIcon.png"},
      {"Chrome", "ChromeIcon.png"},
      {"Google (Blue)", "Google2Icon.png"},
      {"GMail", "GMailIcon.png"},


      {"+Crypto", "BitcoinIcon.png"},
      {"Bitcoin", "BitcoinIcon.png"},
      {"Bitcoin Gold", "BitcoinGoldIcon.png"},
      {"Bitcoin Euro", "BitcoinEuroIcon.png"},
      {"Litecoin", "LitecoinIcon.png"},
      {"Dogecoin", "DogeIcon.png"},


      {"+Games", "BattleNetAuthenticatorIcon.png"},
      {"Steam", "SteamAuthenticatorIcon.png"},
      {"Steam (Circle)", "SteamIcon.png"},
      
      {"s6", string.Empty},
      {"Battle.Net", "BattleNetAuthenticatorIcon.png"},
      {"World of Warcraft", "WarcraftIcon.png"},
      {"Diablo III", "DiabloIcon.png"},

      {"s7", string.Empty},
      {"EA", "EAIcon.png"},
      {"EA (White)", "EA2Icon.png"},
      {"EA (Black)", "EA3Icon.png"},
      
      {"s8", string.Empty},
      {"Origin", "OriginIcon.png"},
      
      {"s9", string.Empty},
      {"ArenaNet", "ArenaNetIcon.png"},
      {"Guild Wars 2", "GuildWarsAuthenticatorIcon.png"},
      
      {"s10", string.Empty},
      {"Trion", "TrionAuthenticatorIcon.png"},
      {"Glyph", "GlyphIcon.png"},
      {"ArcheAge", "ArcheAgeIcon.png"},
      {"Rift", "RiftIcon.png"},
      {"Defiance", "DefianceIcon.png"},
      
      {"s11", string.Empty},
      {"WildStar", "WildstarIcon.png"},
      
      {"s12", string.Empty},
      {"Firefall", "FirefallIcon.png"},
      
      {"s13", string.Empty},
      {"RuneScape", "RuneScapeIcon.png"},
      
      {"s14", string.Empty},
      {"SWTOR", "Swtor.png"},
      {"SWTOR (Empire)", "SwtorEmpire.png"},
      {"SWTOR (Republic)", "SwtorRepublic.png"},
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
        Name = "Okta Verify", AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.OktaVerify,
        Icon = "OktaVerifyAuthenticatorIcon.png"
      }
    };

    [STAThread]
    static void Main() {
      try {
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
        MessageBox.Show($"{APPLICATION_NAME} is already running.", APPLICATION_TITLE,
          MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    static void OnThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
      LogException(e.Exception);
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
          report.Error = ex;
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
        // ignored
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

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      form = new MainForm();
      Application.Run(form);
    }
  }
}