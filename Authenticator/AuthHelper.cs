using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using Authenticator.Resources;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Authenticator {
  class AuthHelper {
    private const string AUTH2_REGKEY = @"Software\Authenticator";

    private const string AUTHREGKEY_LASTFILE = @"File{0}";

    private const string AUTHREGKEY_BACKUP = @"Software\Authenticator\Backup";

    private const string AUTHREGKEY_CONFIGBACKUP = @"Software\Authenticator\Backup\Config";

    private const string RUNKEY = @"Software\Microsoft\Windows\CurrentVersion\Run";

    public const string DEFAULT_AUTHENTICATOR_FILE_NAME = "Authenticator.config";

    public const string AUTH_PGP_PUBLICKEY =
      @"-----BEGIN PGP PUBLIC KEY BLOCK-----
			Version: BCPG C# v1.7.4114.6375
			
			mQEMBFA8sxQBCAC5EWjbGHDgyo4e9rcwse1mWbCOyeTwGZH2malJreF2v81KwBZa
			eCAPX6cP6EWJPlMOgkJpBQOgh+AezkYEidrW4+NXCGv+Z03U1YBc7e/nYnABZrJx
			XsqWVyM3d3iLSpKsMfk2OAIAIvoCvzcdx0ljm2IXGKRHGnc0nU7hSFXh5S/sJErN
			Cgrll6lD2CPNIPuUiMSWptgO1RAjerk0rwLh1DSChicPMJZfxJWn7JD1VVQLmAon
			EJ4x0MUIbff7ZmEna4O2rF9mrCjwfANkcz8N6WFp3PrfhxArXkvOBPYF9iEigFRS
			QVt6XAF6sjGhSYxZRaRj0tE4PyajE/HfNk0DAAkBAbQbV2luQXV0aCA8d2luYXV0
			aEBnbWFpbC5jb20+iQE0BBABAgASBQJQPRWEApsPAhYBAhUCAgsDABYJEJ3DDyNp
			qwwqApsPAhYBAhUCAgsDqb8IAKJRlRu5ne2BuHrMlKW/BI6I/hpkGQ8BzmO7LatM
			YYj//XKkbQ2BSvbDNI1al5HSo1iseokIZqD07iMwsp9GvLXSOVCROK9HYJ4dHsdP
			l68KgNDWu8ZDhPRGerf4+pn1jRfXW4NdFT8W1TX3RArpdVSd5Q2tV2tZrANErBYa
			UTDodsNKwikcgk89a2NI+Lh17lFGCFdAdZ07gRwu6cOm4SqP2TjWjDreXqlE9fHd
			0dwmYeS1QlGYK3ETNS1KvVTNaKdht231jGwlxy09Rxtx1EBLqFNsc+BW5rjYEPN2
			EAlelUJsVidUjZNB1ySm9uW8xurSEXWPZxWITl+LYmgwtn0=
			=dvwu
			-----END PGP PUBLIC KEY BLOCK-----";


    public static AuthConfig LoadConfig(string configFile, string password = null) {
      var config = new AuthConfig();
      if (string.IsNullOrEmpty(password) == false) {
        config.Password = password;
      }

      if (string.IsNullOrEmpty(configFile)) {
        // check for file in current directory
        configFile = Path.Combine(Environment.CurrentDirectory, DEFAULT_AUTHENTICATOR_FILE_NAME);
        if (File.Exists(configFile) == false) {
          configFile = null;
        }
      }

      if (string.IsNullOrEmpty(configFile)) {
        // check for file in exe directory
        configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), DEFAULT_AUTHENTICATOR_FILE_NAME);
        if (File.Exists(configFile) == false) {
          configFile = null;
        }
      }

      if (string.IsNullOrEmpty(configFile)) {
        // do we have a file specific in the registry?
        var configDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
          AuthMain.APPLICATION_NAME);
        // check for default authenticator
        configFile = Path.Combine(configDirectory, DEFAULT_AUTHENTICATOR_FILE_NAME);
        // if no config file, just return a blank config
        if (File.Exists(configFile) == false) {
          return config;
        }
      }

      // if no config file when one was specified; report an error
      if (File.Exists(configFile) == false) {
        //MessageBox.Show(form,
        // strings.CannotFindConfigurationFile + ": " + configFile,
        //  form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        // return config;
        throw new ApplicationException(strings.CannotFindConfigurationFile + ": " + configFile);
      }

      // check if readonly
      var fi = new FileInfo(configFile);
      if (fi.Exists && fi.IsReadOnly) {
        config.IsReadOnly = true;
      }

      var changed = false;
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

        using (var fs = new FileStream(configFile, FileMode.Open, FileAccess.Read)) {
          var reader = XmlReader.Create(fs);
          changed = config.ReadXml(reader, password);
        }

        config.Filename = configFile;

        if (config.Version < AuthConfig.Currentversion) {
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
        throw;
      }

      SaveToRegistry(config);

      return config;
    }

    public static string GetLastV2Config() {
      // check for a v2 last file entry
      try {
        using (var key = Registry.CurrentUser.OpenSubKey(AUTH2_REGKEY, false)) {
          string lastfile;
          if (key != null
              && (lastfile =
                key.GetValue(string.Format(CultureInfo.InvariantCulture, AUTHREGKEY_LASTFILE, 1), null) as string) !=
              null
              && File.Exists(lastfile)) {
            return lastfile;
          }
        }
      }
      catch (System.Security.SecurityException) {
      }

      return null;
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
            AuthMain.APPLICATION_NAME);
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

    private static void SaveToRegistry(AuthConfig config) {
      config.WriteSetting(AUTHREGKEY_CONFIGBACKUP, null);
    }

    public static void SaveToRegistry(AuthConfig config, AuthAuthenticator wa) {
      if (config == null || wa == null || wa.AuthenticatorData == null) {
        return;
      }

      using (var sha = Authenticator.SafeHasher("SHA256")) {
        // get a hash based on the authenticator key
        var authkey = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(wa.AuthenticatorData.SecretData)));

        // save the PGP encrypted key
        using (var sw = new EncodedStringWriter(Encoding.UTF8)) {
          var xmlsettings = new XmlWriterSettings();
          xmlsettings.Indent = true;
          using (var xw = XmlWriter.Create(sw, xmlsettings)) {
            xw.WriteStartElement("Authenticator");
            xw.WriteAttributeString("version",
              System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
            wa.WriteXmlString(xw);
            xw.WriteEndElement();
          }

          var pgpkey = string.IsNullOrEmpty(config.PgpKey) == false ? config.PgpKey : AUTH_PGP_PUBLICKEY;
          config.WriteSetting(AUTHREGKEY_BACKUP + "\\" + authkey, PgpEncrypt(sw.ToString(), pgpkey));
        }
      }
    }

    public static string ReadBackupFromRegistry(AuthConfig config) {
      var buffer = new StringBuilder();
      foreach (var name in config.ReadSettingKeys(AUTHREGKEY_BACKUP)) {
        var val = ReadRegistryValue(name);
        if (val != null) {
          buffer.Append(name + "=" + Convert.ToString(val)).Append(Environment.NewLine);
        }
      }

      return buffer.ToString();
    }

    public static void SetStartWithWindows(bool enabled) {
      if (enabled) {
        // get path of exe and minimize flag
        WriteRegistryValue(RUNKEY + "\\" + AuthMain.APPLICATION_NAME, Application.ExecutablePath + " -min");
      }
      else {
        DeleteRegistryKey(RUNKEY + "\\" + AuthMain.APPLICATION_NAME);
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
                  MainForm.ErrorDialog(parent, strings.InvalidPassword, ex.InnerException, MessageBoxButtons.OK);
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
            MainForm.ErrorDialog(parent, strings.InvalidPassword, ex.InnerException, MessageBoxButtons.OK);

            pgpKey = null;
            password = null;
            retry = true;
            continue;
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
            else if (string.Compare(issuer, "Steam", true) == 0) {
              auth = new SteamAuthenticator();
              ((SteamAuthenticator) auth).SecretKey = Base32.GetInstance().Decode(secret);
              ((SteamAuthenticator) auth).Serial = string.Empty;
              ((SteamAuthenticator) auth).DeviceId = query["deviceid"] ?? string.Empty;
              ((SteamAuthenticator) auth).SteamData = query["data"] ?? string.Empty;
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

            var period = 0;
            int.TryParse(query["period"], out period);
            if (period != 0) {
              auth.Period = period;
            }

            var digits = 0;
            int.TryParse(query["digits"], out digits);
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
        throw new ImportException(string.Format(strings.ImportInvalidUri, linenumber), ex);
      }
      catch (Exception ex) {
        throw new ImportException(string.Format(strings.ImportError, linenumber, ex.Message), ex);
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

    #region HttpUtility

    public static string HtmlEncode(string text) {
      if (string.IsNullOrEmpty(text)) {
        return text;
      }

      var sb = new StringBuilder(text.Length);

      var len = text.Length;
      for (var i = 0; i < len; i++) {
        switch (text[i]) {
          case '<':
            sb.Append("&lt;");
            break;
          case '>':
            sb.Append("&gt;");
            break;
          case '"':
            sb.Append("&quot;");
            break;
          case '&':
            sb.Append("&amp;");
            break;
          default:
            if (text[i] > 159) {
              // decimal numeric entity
              sb.Append("&#");
              sb.Append(((int) text[i]).ToString(CultureInfo.InvariantCulture));
              sb.Append(";");
            }
            else {
              sb.Append(text[i]);
            }

            break;
        }
      }

      return sb.ToString();
    }

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

    public static object ReadRegistryValue(string keyname, object defaultValue = null) {
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
        using (var key = basekey.OpenSubKey(subkey)) {
          return (key != null ? key.GetValue(valuekey, defaultValue) : defaultValue);
        }
      }
      catch (System.Security.SecurityException) {
        return defaultValue;
      }
    }

    public static string[] ReadRegistryKeys(string keyname) {
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

      var subkey = string.Join("\\", keyparts.ToArray());

      try {
        using (var key = basekey.OpenSubKey(subkey)) {
          if (key == null) {
            return new string[0];
          }

          // get all value names
          var keys = key.GetValueNames().ToList();
          for (var i = 0; i < keys.Count; i++) {
            keys[i] = keyname + "\\" + keys[i];
          }

          // read any subkeys
          if (key.SubKeyCount != 0) {
            foreach (var subkeyname in key.GetSubKeyNames()) {
              keys.AddRange(ReadRegistryKeys(keyname + "\\" + subkeyname));
            }
          }

          return keys.ToArray();
        }
      }
      catch (System.Security.SecurityException) {
        return new string[0];
      }
    }

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
      catch (System.Security.SecurityException) {
        return;
      }
    }

    public static void DeleteRegistryKey(string keyname) {
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
          if (key != null) {
            if (key.GetValueNames().Contains(valuekey)) {
              key.DeleteValue(valuekey, false);
            }

            if (key.GetSubKeyNames().Contains(valuekey)) {
              key.DeleteSubKeyTree(valuekey, false);
            }

            // if the parent now has no values, we can remove it too
            if (key.SubKeyCount == 0 && key.ValueCount == 0) {
              basekey.DeleteSubKey(subkey, false);
            }
          }
        }
      }
      catch (System.Security.SecurityException) {
        return;
      }
    }

    #endregion

    #region PGP functions

    public static void PgpGenerateKey(int bits, string identifier, string password, out string privateKey,
      out string publicKey) {
      // generate a new RSA keypair 
      var gen = new RsaKeyPairGenerator();
      gen.Init(new RsaKeyGenerationParameters(BigInteger.ValueOf(0x101), new Org.BouncyCastle.Security.SecureRandom(),
        bits, 80));
      var pair = gen.GenerateKeyPair();

      // create PGP subpacket
      var hashedGen = new PgpSignatureSubpacketGenerator();
      hashedGen.SetKeyFlags(true,
        PgpKeyFlags.CanCertify | PgpKeyFlags.CanSign | PgpKeyFlags.CanEncryptCommunications |
        PgpKeyFlags.CanEncryptStorage);
      hashedGen.SetPreferredCompressionAlgorithms(false, new int[] {(int) CompressionAlgorithmTag.Zip});
      hashedGen.SetPreferredHashAlgorithms(false, new int[] {(int) HashAlgorithmTag.Sha1});
      hashedGen.SetPreferredSymmetricAlgorithms(false, new int[] {(int) SymmetricKeyAlgorithmTag.Cast5});
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
        new Org.BouncyCastle.Security.SecureRandom());

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
            new Org.BouncyCastle.Security.SecureRandom());
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
  }

  class EncodedStringWriter : StringWriter {
    private readonly Encoding encoding;

    public EncodedStringWriter(Encoding encoding) {
      this.encoding = encoding;
    }

    public override Encoding Encoding => encoding;
  }
}