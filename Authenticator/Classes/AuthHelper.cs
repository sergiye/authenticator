using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Authenticator {
  class AuthHelper {
    private const string RUNKEY = @"Software\Microsoft\Windows\CurrentVersion\Run";

    public const string DEFAULT_AUTHENTICATOR_FILE_NAME = "Authenticator.config";

    public const string APPLICATION_NAME = "Authenticator";
    public const string APPLICATION_TITLE = "Authenticator";

    #region Config

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
      {"Skyvia", "SkyviaIcon.png"},
      {"Xero", "XeroIcon.png"},
      {"Zoho", "ZohoIcon.png"},
      {"ngrok", "ngrokIcon.png"},
      {"ngrok (blue)", "ngrokBlueIcon.png"},
      {"ngrok (black)", "ngrokBlackIcon.png"},
      {"ngrok (coral)", "ngrokCoralIcon.png"},


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
        Name = "Time-Based / Google", 
        AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.RFC6238_TIME_COUNTER,
        Icon = "AppIcon.png"
      },
      
      null,
      new RegisteredAuthenticator {
        Name = "Microsoft", 
        AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.Microsoft,
        Icon = "MicrosoftAuthenticatorIcon.png"
      },
      new RegisteredAuthenticator {
        Name = "Okta Verify", 
        AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.OktaVerify,
        Icon = "OktaVerifyAuthenticatorIcon.png"
      },
      
      null,
      new RegisteredAuthenticator {
        Name = "Battle.Net", 
        AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.BattleNet,
        Icon = "BattleNetAuthenticatorIcon.png"
      },
      new RegisteredAuthenticator {
        Name = "Guild Wars 2", 
        AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.GuildWars,
        Icon = "GuildWarsAuthenticatorIcon.png"
      },
      new RegisteredAuthenticator {
        Name = "Glyph / Trion", 
        AuthenticatorType = RegisteredAuthenticator.AuthenticatorTypes.Trion,
        Icon = "GlyphIcon.png"
      },
    };
    
    #endregion

    public static AuthConfig LoadConfig(string configFile, string password = null) {
      var config = new AuthConfig();
      if (string.IsNullOrEmpty(password) == false) {
        config.Password = password;
      }

      if (string.IsNullOrEmpty(configFile)) {
        // check for file in current directory
        configFile = Path.Combine(Environment.CurrentDirectory, DEFAULT_AUTHENTICATOR_FILE_NAME);
        if (!File.Exists(configFile)) {
          configFile = null;
        }
      }

      if (string.IsNullOrEmpty(configFile)) {
        // check for file in exe directory
        configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), DEFAULT_AUTHENTICATOR_FILE_NAME);
        if (!File.Exists(configFile)) {
          configFile = null;
        }
      }

      if (string.IsNullOrEmpty(configFile)) {
        // do we have a file specific in the registry?
        var configDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
          APPLICATION_NAME);
        // check for default authenticator
        configFile = Path.Combine(configDirectory, DEFAULT_AUTHENTICATOR_FILE_NAME);
        // if no config file, just return a blank config
        if (!File.Exists(configFile)) {
          return config;
        }
      }

      // if no config file when one was specified; report an error
      if (!File.Exists(configFile)) {
        //MessageBox.Show(form,
        // strings.CannotFindConfigurationFile + ": " + configFile,
        //  form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        // return config;
        throw new ApplicationException("Unable to find your configuration file: " + configFile);
      }

      // check if readonly
      var fi = new FileInfo(configFile);
      if (fi.Exists && fi.IsReadOnly) {
        config.IsReadOnly = true;
      }

      try {
        var data = File.ReadAllBytes(configFile);
        if (data.Length == 0 || data[0] == 0) {
          // switch to backup
          if (File.Exists(configFile + ".bak")) {
            data = File.ReadAllBytes(configFile + ".bak");
            if (data.Length != 0 && data[0] != 0) {
              File.WriteAllBytes(configFile, data);
            }
          }
        }

        bool changed;
        using (var fs = new FileStream(configFile, FileMode.Open, FileAccess.Read)) {
          var reader = XmlReader.Create(fs);
          changed = config.ReadXml(reader, password);
        }

        config.Filename = configFile;

        if (config.Version < AuthConfig.CurrentVersion) {
          // set new created values
          foreach (var wa in config) {
            wa.Created = fi.CreationTime;
          }

          config.Upgraded = true;
        }

        if (changed && config.IsReadOnly == false) {
          SaveConfig(config);
        }
      }
      catch (EncryptedSecretDataException) {
        // we require a password
        throw;
      }
      catch (BadPasswordException) {
        // we require a password
        throw;
      }
      catch (Exception) {
        //generic
        throw;
      }

      return config;
    }

    public static void SaveConfig(AuthConfig config) {
      // create the xml
      var settings = new XmlWriterSettings();
      settings.Indent = true;
      settings.Encoding = Encoding.UTF8;

      // Issue 41 (http://code.google.com/p/winauth/issues/detail?id=41): saving may crash leaving file corrupt, so write into memory stream first before an atomic file write
      using (var ms = new MemoryStream()) {
        // save config into memory
        using (var writer = XmlWriter.Create(ms, settings)) {
          config.WriteXmlString(writer);
        }

        // if no config file yet, use default
        if (string.IsNullOrEmpty(config.Filename)) {
          var configDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            APPLICATION_NAME);
          Directory.CreateDirectory(configDirectory);
          config.Filename = Path.Combine(configDirectory, DEFAULT_AUTHENTICATOR_FILE_NAME);
        }

        var fi = new FileInfo(config.Filename);
        if (!fi.Exists || !fi.IsReadOnly) {
          // write memory stream to file
          try {
            var data = ms.ToArray();

            // getting instance of zerod files, so do some sanity checks
            if (data.Length == 0 || data[0] == 0) {
              throw new ApplicationException("Zero data when saving config");
            }

            var tempfile = config.Filename + ".tmp";

            File.WriteAllBytes(tempfile, data);

            // read it back
            var verify = File.ReadAllBytes(tempfile);
            if (verify.Length != data.Length || verify.SequenceEqual(data) == false) {
              throw new ApplicationException("Save config doesn't compare with memory: " +
                                             Convert.ToBase64String(data));
            }

            // move it to old file
            File.Delete(config.Filename + ".bak");
            if (File.Exists(config.Filename)) {
              File.Move(config.Filename, config.Filename + ".bak");
            }

            File.Move(tempfile, config.Filename);
          }
          catch (UnauthorizedAccessException) {
            // fail silently if read only
            if (fi.IsReadOnly) {
              config.IsReadOnly = true;
              return;
            }

            throw;
          }
        }
      }
    }

    public static void SetStartWithWindows(bool enabled) {
      if (enabled) {
        // get path of exe and minimize flag
        WriteRegistryValue(RUNKEY + "\\" + APPLICATION_NAME, Application.ExecutablePath + " -min");
      }
      else {
        DeleteRegistryKey(RUNKEY + "\\" + APPLICATION_NAME);
      }
    }

    public static List<AuthAuthenticator> ImportAuthenticators(Form parent, string file) {
      var authenticators = new List<AuthAuthenticator>();

      string password = null;
      string pgpKey = null;

      var lines = new StringBuilder();
      bool retry;
      do {
        retry = false;
        lines.Length = 0;

        // open the zip file
        if (string.Compare(Path.GetExtension(file), ".zip", true) == 0) {
          using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read)) {
            ZipFile zip = null;
            try {
              zip = new ZipFile(fs);
              if (string.IsNullOrEmpty(password) == false) {
                zip.Password = password;
              }

              var buffer = new byte[4096];
              foreach (ZipEntry entry in zip) {
                if (entry.IsFile == false || string.Compare(Path.GetExtension(entry.Name), ".txt", true) != 0) {
                  continue;
                }

                // read file out
                var zs = zip.GetInputStream(entry);
                using (var ms = new MemoryStream()) {
                  StreamUtils.Copy(zs, ms, buffer);

                  // get as string and append
                  ms.Seek(0, SeekOrigin.Begin);
                  using (var sr = new StreamReader(ms)) {
                    lines.Append(sr.ReadToEnd()).Append(Environment.NewLine);
                  }
                }
              }
            }
            catch (ZipException ex) {
              if (ex.Message.IndexOf("password") != -1) {
                // already have a password
                if (string.IsNullOrEmpty(password) == false) {
                  MainForm.ErrorDialog(parent, "Invalid password", ex.InnerException);
                }

                // need password
                var form = new GetPasswordForm();
                if (form.ShowDialog(parent) == DialogResult.Cancel) {
                  return null;
                }

                password = form.Password;
                retry = true;
                continue;
              }

              throw;
            }
            finally {
              if (zip != null) {
                zip.IsStreamOwner = true;
                zip.Close();
              }
            }
          }
        }
        else if (string.Compare(Path.GetExtension(file), ".pgp", true) == 0) {
          var encoded = File.ReadAllText(file);
          if (string.IsNullOrEmpty(pgpKey)) {
            // need password
            var form = new GetPgpKeyForm();
            if (form.ShowDialog(parent) == DialogResult.Cancel) {
              return null;
            }

            pgpKey = form.PgpKey;
            password = form.Password;
            retry = true;
            continue;
          }

          try {
            var line = PgpDecrypt(encoded, pgpKey, password);
            lines.Append(line);
          }
          catch (Exception ex) {
            MainForm.ErrorDialog(parent, "Invalid password", ex.InnerException);

            pgpKey = null;
            password = null;
            retry = true;
          }
        }
        else // read a plain text file
        {
          lines.Append(File.ReadAllText(file));
        }
      } while (retry);

      var linenumber = 0;
      try {
        using (var sr = new StringReader(lines.ToString())) {
          string line;
          while ((line = sr.ReadLine()) != null) {
            linenumber++;

            // ignore blank lines or comments
            line = line.Trim();
            if (line.Length == 0 || line.IndexOf("#") == 0) {
              continue;
            }

            // bug if there is a hash before ?
            var hash = line.IndexOf("#");
            var qm = line.IndexOf("?");
            if (hash != -1 && hash < qm) {
              line = line.Substring(0, hash) + "%23" + line.Substring(hash + 1);
            }

            // parse and validate URI
            var uri = new Uri(line);

            // we only support "otpauth"
            if (uri.Scheme != "otpauth") {
              throw new ApplicationException("Import only supports otpauth://");
            }

            // we only support totp (not hotp)
            if (uri.Host != "totp" && uri.Host != "hotp") {
              throw new ApplicationException("Import only supports otpauth://totp/ or otpauth://hotp/");
            }

            // get the label and optional issuer
            var issuer = string.Empty;
            var label = (string.IsNullOrEmpty(uri.LocalPath) == false
              ? uri.LocalPath.Substring(1)
              : string.Empty); // skip past initial /
            var p = label.IndexOf(":");
            if (p != -1) {
              issuer = label.Substring(0, p);
              label = label.Substring(p + 1);
            }

            // + aren't decoded
            label = label.Replace("+", " ");

            var query = HttpUtility.ParseQueryString(uri.Query);
            var secret = query["secret"];
            if (string.IsNullOrEmpty(secret)) {
              throw new ApplicationException("Authenticator does not contain secret");
            }

            var counter = query["counter"];
            if (uri.Host == "hotp" && string.IsNullOrEmpty(counter)) {
              throw new ApplicationException("HOTP authenticator should have a counter");
            }

            var importedAuthenticator = new AuthAuthenticator();
            importedAuthenticator.AutoRefresh = false;
            //
            Authenticator auth;
            if (string.Compare(issuer, "BattleNet", true) == 0) {
              var serial = query["serial"];
              if (string.IsNullOrEmpty(serial)) {
                throw new ApplicationException("Battle.net Authenticator does not have a serial");
              }

              serial = serial.ToUpper();
              if (Regex.IsMatch(serial, @"^[A-Z]{2}-?[\d]{4}-?[\d]{4}-?[\d]{4}$") == false) {
                throw new ApplicationException("Invalid serial for Battle.net Authenticator");
              }

              auth = new BattleNetAuthenticator();
              //char[] decoded = Base32.getInstance().Decode(secret).Select(c => Convert.ToChar(c)).ToArray(); // this is hex string values
              //string hex = new string(decoded);
              //((BattleNetAuthenticator)auth).SecretKey = Authenticator.StringToByteArray(hex);

              ((BattleNetAuthenticator) auth).SecretKey = Base32.GetInstance().Decode(secret);

              ((BattleNetAuthenticator) auth).Serial = serial;

              issuer = string.Empty;
            }
            else if (uri.Host == "hotp") {
              auth = new HotpAuthenticator();
              ((HotpAuthenticator) auth).SecretKey = Base32.GetInstance().Decode(secret);
              ((HotpAuthenticator) auth).Counter = int.Parse(counter);

              if (string.IsNullOrEmpty(issuer) == false) {
                auth.Issuer = issuer;
              }
            }
            else // if (string.Compare(issuer, "Google", true) == 0)
            {
              auth = new GoogleAuthenticator();
              ((GoogleAuthenticator) auth).Enroll(secret);

              if (string.Compare(issuer, "Google", true) == 0) {
                issuer = string.Empty;
              }
              else if (string.IsNullOrEmpty(issuer) == false) {
                auth.Issuer = issuer;
              }
            }

            int.TryParse(query["period"], out var period);
            if (period != 0) {
              auth.Period = period;
            }

            int.TryParse(query["digits"], out var digits);
            if (digits != 0) {
              auth.CodeDigits = digits;
            }

            if (Enum.TryParse<Authenticator.HmacTypes>(query["algorithm"], true, out var hmactype)) {
              auth.HmacType = hmactype;
            }

            //
            if (label.Length != 0) {
              importedAuthenticator.Name = (issuer.Length != 0 ? issuer + " (" + label + ")" : label);
            }
            else if (issuer.Length != 0) {
              importedAuthenticator.Name = issuer;
            }
            else {
              importedAuthenticator.Name = "Imported";
            }

            //
            importedAuthenticator.AuthenticatorData = auth;

            // set the icon
            var icon = query["icon"];
            if (string.IsNullOrEmpty(icon) == false) {
              if (icon.StartsWith("base64:")) {
                var b64 = Convert.ToBase64String(Base32.GetInstance().Decode(icon.Substring(7)));
                importedAuthenticator.Skin = "base64:" + b64;
              }
              else {
                importedAuthenticator.Skin = icon + "Icon.png";
              }
            }

            // sync
            importedAuthenticator.Sync();

            authenticators.Add(importedAuthenticator);
          }
        }

        return authenticators;
      }
      catch (UriFormatException ex) {
        throw new ImportException($"Invalid authenticator at line {linenumber}", ex);
      }
      catch (Exception ex) {
        throw new ImportException($"Error importing at line {linenumber}:{ex.Message}", ex);
      }
    }

    public static void ExportAuthenticators(Form form, IList<AuthAuthenticator> authenticators, string file,
      string password, string pgpKey) {
      // create file in memory
      using (var ms = new MemoryStream()) {
        using (var sw = new StreamWriter(ms)) {
          var unprotected = new List<AuthAuthenticator>();
          foreach (var auth in authenticators) {
            // unprotect if necessary
            if (auth.AuthenticatorData.RequiresPassword) {
              // request the password
              var getPassForm = new UnprotectPasswordForm();
              getPassForm.Authenticator = auth;
              var result = getPassForm.ShowDialog(form);
              if (result == DialogResult.OK) {
                unprotected.Add(auth);
              }
              else {
                continue;
              }
            }

            var line = auth.ToUrl();
            sw.WriteLine(line);
          }

          // reprotect
          foreach (var auth in unprotected) {
            auth.AuthenticatorData.Protect();
          }

          // reset and write stream out to disk or as zip
          sw.Flush();
          ms.Seek(0, SeekOrigin.Begin);

          // reset and write stream out to disk or as zip
          if (string.Compare(Path.GetExtension(file), ".zip", true) == 0) {
            using (var zip = new ZipOutputStream(new FileStream(file, FileMode.Create, FileAccess.Write))) {
              if (string.IsNullOrEmpty(password) == false) {
                zip.Password = password;
              }

              zip.IsStreamOwner = true;

              var entry = new ZipEntry(ZipEntry.CleanName(Path.GetFileNameWithoutExtension(file) + ".txt"));
              entry.DateTime = DateTime.Now;
              zip.UseZip64 = UseZip64.Off;

              zip.PutNextEntry(entry);

              var buffer = new byte[4096];
              StreamUtils.Copy(ms, zip, buffer);

              zip.CloseEntry();
            }
          }
          else if (string.IsNullOrEmpty(pgpKey) == false) {
            using (var sr = new StreamReader(ms)) {
              var plain = sr.ReadToEnd();
              var encoded = PgpEncrypt(plain, pgpKey);

              File.WriteAllText(file, encoded);
            }
          }
          else {
            using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write)) {
              var buffer = new byte[4096];
              StreamUtils.Copy(ms, fs, buffer);
            }
          }
        }
      }
    }

    public static ToolStripMenuItem AddMenuItem(ToolStripItemCollection menuItems, string text = null, string name = null, EventHandler onClick = null, Keys shortcut = Keys.None, object tag = null, Image icon = null) {
      if (text == null) {
        menuItems.Add(new ToolStripSeparator { Name = name });
        return null;
      }
      var menuItem = new ToolStripMenuItem(text) {
        Name = name,
        Tag = tag,
      };
      menuItem.Click += onClick;
      if (shortcut != Keys.None) {
        menuItem.ShortcutKeys = shortcut;
        menuItem.ShowShortcutKeys = true;
      }

      if (icon != null) {
        menuItem.Image = icon;
        menuItem.ImageAlign = ContentAlignment.MiddleLeft;
        menuItem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
      }
      menuItems.Add(menuItem);
      return menuItem;
    }

    #region HttpUtility

    public static NameValueCollection ParseQueryString(string qs) {
      var pairs = new NameValueCollection();

      // ignore blanks and remove initial "?"
      if (string.IsNullOrEmpty(qs)) {
        return pairs;
      }

      if (qs.StartsWith("?")) {
        qs = qs.Substring(1);
      }

      // get each a=b&... key-value pair
      foreach (var p in qs.Split('&')) {
        var keypair = p.Split('=');
        var key = keypair[0];
        var v = (keypair.Length >= 2 ? keypair[1] : null);
        if (string.IsNullOrEmpty(v) == false) {
          // decode (without using System.Web)
          string newv;
          while ((newv = Uri.UnescapeDataString(v)) != v) {
            v = newv;
          }
        }

        pairs.Add(key, v);
      }

      return pairs;
    }

    #endregion

    #region Registry Function

    public static void WriteRegistryValue(string keyname, object value) {
      RegistryKey basekey;
      var keyparts = keyname.Split('\\').ToList();
      switch (keyparts[0]) {
        case "HKEY_CLASSES_ROOT":
          basekey = Registry.ClassesRoot;
          keyparts.RemoveAt(0);
          break;
        case "HKEY_CURRENT_CONFIG":
          basekey = Registry.CurrentConfig;
          keyparts.RemoveAt(0);
          break;
        case "HKEY_CURRENT_USER":
          basekey = Registry.CurrentUser;
          keyparts.RemoveAt(0);
          break;
        case "HKEY_LOCAL_MACHINE":
          basekey = Registry.LocalMachine;
          keyparts.RemoveAt(0);
          break;
        case "HKEY_PERFORMANCE_DATA":
          basekey = Registry.PerformanceData;
          keyparts.RemoveAt(0);
          break;
        default:
          basekey = Registry.CurrentUser;
          break;
      }

      var subkey = string.Join("\\", keyparts.Take(keyparts.Count - 1).ToArray());
      var valuekey = keyparts[keyparts.Count - 1];

      try {
        using (var key = basekey.CreateSubKey(subkey)) {
          key.SetValue(valuekey, value);
        }
      }
      catch (SecurityException) {
      }
    }

    public static void DeleteRegistryKey(string keyname) {
      RegistryKey baseKey;
      var parts = keyname.Split('\\').ToList();
      switch (parts[0]) {
        case "HKEY_CLASSES_ROOT":
          baseKey = Registry.ClassesRoot;
          parts.RemoveAt(0);
          break;
        case "HKEY_CURRENT_CONFIG":
          baseKey = Registry.CurrentConfig;
          parts.RemoveAt(0);
          break;
        case "HKEY_CURRENT_USER":
          baseKey = Registry.CurrentUser;
          parts.RemoveAt(0);
          break;
        case "HKEY_LOCAL_MACHINE":
          baseKey = Registry.LocalMachine;
          parts.RemoveAt(0);
          break;
        case "HKEY_PERFORMANCE_DATA":
          baseKey = Registry.PerformanceData;
          parts.RemoveAt(0);
          break;
        default:
          baseKey = Registry.CurrentUser;
          break;
      }

      var subKey = string.Join("\\", parts.Take(parts.Count - 1).ToArray());
      var valueKey = parts[parts.Count - 1];

      try {
        using (var key = baseKey.CreateSubKey(subKey)) {
          if (key == null) return;
          if (key.GetValueNames().Contains(valueKey)) key.DeleteValue(valueKey, false);
          if (key.GetSubKeyNames().Contains(valueKey)) key.DeleteSubKeyTree(valueKey, false);
          // if the parent now has no values, we can remove it too
          if (key.SubKeyCount == 0 && key.ValueCount == 0) baseKey.DeleteSubKey(subKey, false);
        }
      }
      catch (SecurityException) {
      }
    }

    #endregion

    #region PGP functions

    public static void PgpGenerateKey(int bits, string identifier, string password, out string privateKey,
      out string publicKey) {
      // generate a new RSA keypair 
      var gen = new RsaKeyPairGenerator();
      gen.Init(new RsaKeyGenerationParameters(BigInteger.ValueOf(0x101), new SecureRandom(),
        bits, 80));
      var pair = gen.GenerateKeyPair();

      // create PGP subpacket
      var hashedGen = new PgpSignatureSubpacketGenerator();
      hashedGen.SetKeyFlags(true,
        PgpKeyFlags.CanCertify | PgpKeyFlags.CanSign | PgpKeyFlags.CanEncryptCommunications |
        PgpKeyFlags.CanEncryptStorage);
      hashedGen.SetPreferredCompressionAlgorithms(false, new[] {(int) CompressionAlgorithmTag.Zip});
      hashedGen.SetPreferredHashAlgorithms(false, new[] {(int) HashAlgorithmTag.Sha1});
      hashedGen.SetPreferredSymmetricAlgorithms(false, new[] {(int) SymmetricKeyAlgorithmTag.Cast5});
      var sv = hashedGen.Generate();
      var unhashedGen = new PgpSignatureSubpacketGenerator();

      // create the PGP key
      var secretKey = new PgpSecretKey(
        PgpSignature.DefaultCertification,
        PublicKeyAlgorithmTag.RsaGeneral,
        pair.Public,
        pair.Private,
        DateTime.Now,
        identifier,
        SymmetricKeyAlgorithmTag.Cast5,
        (password != null ? password.ToCharArray() : null),
        hashedGen.Generate(),
        unhashedGen.Generate(),
        new SecureRandom());

      // extract the keys
      using (var ms = new MemoryStream()) {
        using (var ars = new ArmoredOutputStream(ms)) {
          secretKey.Encode(ars);
        }

        privateKey = Encoding.ASCII.GetString(ms.ToArray());
      }

      using (var ms = new MemoryStream()) {
        using (var ars = new ArmoredOutputStream(ms)) {
          secretKey.PublicKey.Encode(ars);
        }

        publicKey = Encoding.ASCII.GetString(ms.ToArray());
      }
    }

    public static string PgpEncrypt(string plain, string armoredPublicKey) {
      // encode data
      var data = Encoding.UTF8.GetBytes(plain);

      // create the Authenticator public key
      PgpPublicKey publicKey = null;
      using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(armoredPublicKey))) {
        using (var dis = PgpUtilities.GetDecoderStream(ms)) {
          var bundle = new PgpPublicKeyRingBundle(dis);
          foreach (PgpPublicKeyRing keyring in bundle.GetKeyRings()) {
            foreach (PgpPublicKey key in keyring.GetPublicKeys()) {
              if (key.IsEncryptionKey && key.IsRevoked() == false) {
                publicKey = key;
                break;
              }
            }
          }
        }
      }

      // encrypt the data using PGP
      using (var encryptedStream = new MemoryStream()) {
        using (var armored = new ArmoredOutputStream(encryptedStream)) {
          var pedg = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, true,
            new SecureRandom());
          pedg.AddMethod(publicKey);
          using (var pedgStream = pedg.Open(armored, new byte[4096])) {
            var pcdg = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
            using (var pcdgStream = pcdg.Open(pedgStream)) {
              var pldg = new PgpLiteralDataGenerator();
              using (var encrypter =
                     pldg.Open(pcdgStream, PgpLiteralData.Binary, "", (long) data.Length, DateTime.Now)) {
                encrypter.Write(data, 0, data.Length);
              }
            }
          }
        }

        return Encoding.ASCII.GetString(encryptedStream.ToArray());
      }
    }

    public static string PgpDecrypt(string armoredCipher, string armoredPrivateKey, string keyPassword) {
      // decode the private key
      var privateKeys = new Dictionary<long, PgpPrivateKey>();
      using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(armoredPrivateKey))) {
        using (var dis = PgpUtilities.GetDecoderStream(ms)) {
          var bundle = new PgpSecretKeyRingBundle(dis);
          foreach (PgpSecretKeyRing keyring in bundle.GetKeyRings()) {
            foreach (PgpSecretKey key in keyring.GetSecretKeys()) {
              privateKeys.Add(key.KeyId, key.ExtractPrivateKey(keyPassword != null ? keyPassword.ToCharArray() : null));
            }
          }
        }
      }

      // decrypt armored block using our private key
      var cipher = Encoding.ASCII.GetBytes(armoredCipher);
      using (var decryptedStream = new MemoryStream()) {
        using (var inputStream = new MemoryStream(cipher)) {
          using (var ais = new ArmoredInputStream(inputStream)) {
            var message = new PgpObjectFactory(ais).NextPgpObject();
            if (message is PgpEncryptedDataList) {
              foreach (PgpPublicKeyEncryptedData pked in ((PgpEncryptedDataList) message).GetEncryptedDataObjects()) {
                message = new PgpObjectFactory(pked.GetDataStream(privateKeys[pked.KeyId])).NextPgpObject();
              }
            }

            if (message is PgpCompressedData) {
              message = new PgpObjectFactory(((PgpCompressedData) message).GetDataStream()).NextPgpObject();
            }

            if (message is PgpLiteralData) {
              var buffer = new byte[4096];
              using (var stream = ((PgpLiteralData) message).GetInputStream()) {
                int read;
                while ((read = stream.Read(buffer, 0, 4096)) > 0) {
                  decryptedStream.Write(buffer, 0, read);
                }
              }
            }

            return Encoding.UTF8.GetString(decryptedStream.ToArray());
          }
        }
      }
    }

    #endregion

    public static Bitmap GetIconBitmap(string iconFile, int width = 48, int height = 48) {
      using (var iconStream = Assembly.GetExecutingAssembly()
               .GetManifestResourceStream("Authenticator.Resources." + iconFile)) {
        if (iconStream == null) return null;
        // if (iconFile.EndsWith(".png"))
          return new Bitmap(iconStream);
        // if (iconFile.EndsWith(".svg")) {
        //   var svgDoc = SvgDocument.Open<SvgDocument>(iconStream);
        //   return new Bitmap(svgDoc.Draw(width, height));
        //   // return new Bitmap(svgDoc.Draw(ICON_WIDTH, ICON_HEIGHT));
        // }
        // return null;
      }
    }

    public static void ShowException(Exception ex) {
      try {
        new ExceptionForm(ex).ShowDialog();
        // if (new ExceptionForm(ex).ShowDialog() == DialogResult.Cancel) {
        //   Process.GetCurrentProcess().Kill();
        // }
      }
      catch (Exception) {
        // ignored
      }
    }
  }
}