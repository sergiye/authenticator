﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Authenticator {
  [Serializable]
  public class AuthConfig : IList<AuthAuthenticator>, ICloneable {
    public static decimal CurrentVersion = decimal.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString(2),
      System.Globalization.CultureInfo.InvariantCulture);

    public enum NotifyActions {
      Notification = 0,
      CopyToClipboard = 1,
    }

    public static event Action<ConfigChangedEventArgs> OnConfigChanged;

    public decimal Version { get; private set; }

    public string Password { protected get; set; }

    public bool Upgraded { get; set; }

    private Authenticator.PasswordTypes passwordType = Authenticator.PasswordTypes.None;

    public Authenticator.PasswordTypes PasswordType {
      get => passwordType;
      set {
        passwordType = value;

        if ((passwordType & Authenticator.PasswordTypes.Explicit) == 0) {
          Password = null;
        }
      }
    }

    private List<AuthAuthenticator> authenticators = new List<AuthAuthenticator>();

    private AuthAuthenticator authenticator;

    #region static settings

    private static bool alwaysOnTop;
    private static bool useTrayIcon;
    private static NotifyActions notifyAction;
    private static bool startWithWindows;
    private static bool autoSize;
    private static Point position = Point.Empty;
    private static int width;
    private static int height;
    private static string theme;

    #endregion

    private bool readOnly;

    [XmlRoot(ElementName = "settings")]
    public class Setting {
      [XmlAttribute(AttributeName = "key")] public string Key;

      [XmlAttribute(AttributeName = "value")]
      public string Value;
    }

    #region System Settings

    public string Filename { get; set; }

    public static bool AlwaysOnTop {
      get => alwaysOnTop;
      set {
        alwaysOnTop = value;
        if (OnConfigChanged != null) {
          OnConfigChanged(new ConfigChangedEventArgs("AlwaysOnTop"));
        }
      }
    }

    public static bool UseTrayIcon {
      get => useTrayIcon;
      set {
        useTrayIcon = value;
        if (OnConfigChanged != null) {
          OnConfigChanged(new ConfigChangedEventArgs("UseTrayIcon"));
        }
      }
    }

    public static NotifyActions NotifyAction {
      get => notifyAction;
      set {
        notifyAction = value;
        if (OnConfigChanged != null) {
          OnConfigChanged(new ConfigChangedEventArgs("NotifyAction"));
        }
      }
    }

    public static bool StartWithWindows {
      get => startWithWindows;
      set {
        startWithWindows = value;
        if (OnConfigChanged != null) {
          OnConfigChanged(new ConfigChangedEventArgs("StartWithWindows"));
        }
      }
    }

    public static bool AutoSize {
      get => autoSize;
      set {
        autoSize = value;
        if (OnConfigChanged != null) {
          OnConfigChanged(new ConfigChangedEventArgs("AutoSize"));
        }
      }
    }

    public static Point Position {
      get => position;
      set {
        if (position != value) {
          position = value;
          if (OnConfigChanged != null) {
            OnConfigChanged(new ConfigChangedEventArgs("Position"));
          }
        }
      }
    }

    public static int Width {
      get => width;
      set {
        if (width != value) {
          width = value;
          if (OnConfigChanged != null) {
            OnConfigChanged(new ConfigChangedEventArgs("Width"));
          }
        }
      }
    }

    public static int Height {
      get => height;
      set {
        if (height != value) {
          height = value;
          if (OnConfigChanged != null) {
            OnConfigChanged(new ConfigChangedEventArgs("Height"));
          }
        }
      }
    }

    public static string Theme {
      get => theme;
      set {
        if (theme != value) {
          theme = value;
          if (OnConfigChanged != null) {
            OnConfigChanged(new ConfigChangedEventArgs("Theme"));
          }
        }
      }
    }

    public string PgpKey { get; private set; }

    public bool IsPortable =>
      !string.IsNullOrEmpty(Filename)
      && Path.GetDirectoryName(Filename) == Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    public bool IsPassword(string password) {
      return String.CompareOrdinal(password, Password) == 0;
    }

    #endregion

    #region IList

    private void SetIndexes() {
      var count = Count;
      for (var i = 0; i < count; i++) {
        this[i].Index = i;
      }
    }

    public void Add(AuthAuthenticator auth) {
      if (auth != null) {
        auth.OnAuthAuthenticatorChanged += OnAuthAuthenticatorChanged;
        authenticators.Add(auth);
      }
      SetIndexes();
    }

    public void Clear() {
      foreach (var auth in this) {
        auth.Index = 0;
        auth.OnAuthAuthenticatorChanged -= OnAuthAuthenticatorChanged;
      }

      authenticators.Clear();
    }

    public bool Contains(AuthAuthenticator auth) {
      return authenticators.Contains(auth);
    }

    public void CopyTo(int index, AuthAuthenticator[] array, int arrayIndex, int count) {
      authenticators.CopyTo(index, array, arrayIndex, count);
    }

    public void CopyTo(AuthAuthenticator[] array, int index) {
      authenticators.CopyTo(array, index);
    }

    public int Count => authenticators.Count;

    public int IndexOf(AuthAuthenticator auth) {
      return authenticators.IndexOf(auth);
    }

    public void Insert(int index, AuthAuthenticator auth) {
      if (auth != null) {
        auth.OnAuthAuthenticatorChanged += OnAuthAuthenticatorChanged;
        authenticators.Insert(index, auth);
      }
      SetIndexes();
    }

    public bool IsReadOnly {
      get => readOnly;
      set => readOnly = value;
    }

    public bool Remove(AuthAuthenticator auth) {
      var result = false;
      if (auth != null) {
        auth.OnAuthAuthenticatorChanged -= OnAuthAuthenticatorChanged;
        result = authenticators.Remove(auth);
      }
      SetIndexes();
      return result;
    }

    public void RemoveAt(int index) {
      authenticators[index].OnAuthAuthenticatorChanged -= OnAuthAuthenticatorChanged;
      authenticators.RemoveAt(index);
      SetIndexes();
    }

    public IEnumerator<AuthAuthenticator> GetEnumerator() {
      return authenticators.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public AuthAuthenticator this[int index] {
      get => authenticators[index];
      set => throw new NotImplementedException();
    }

    public void Sort() {
      authenticators.Sort((a, b) => a.Index.CompareTo(b.Index));
    }

    #endregion

    #region Authenticator Settings

    public AuthAuthenticator CurrentAuthenticator {
      get => authenticator;
      set => authenticator = value;
    }

    #endregion

    static AuthConfig() {
      AutoSize = true;
      Theme = "Auto";
      NotifyAction = NotifyActions.Notification;
    }

    public AuthConfig() {
      Version = CurrentVersion;
    }

    public void OnAuthAuthenticatorChanged(AuthAuthenticator sender, AuthAuthenticatorChangedEventArgs e) {
      if (OnConfigChanged != null) {
        OnConfigChanged(new ConfigChangedEventArgs("Authenticator", sender, e));
      }
    }

    #region ICloneable

    public object Clone() {
      var clone = (AuthConfig) MemberwiseClone();
      // close the internal authenticator so the data is kept separate
      //clone.OnConfigChanged = null;
      clone.authenticators = new List<AuthAuthenticator>();
      foreach (var wa in authenticators) {
        clone.authenticators.Add(wa.Clone() as AuthAuthenticator);
      }

      clone.CurrentAuthenticator = (CurrentAuthenticator != null
        ? clone.authenticators[authenticators.IndexOf(CurrentAuthenticator)]
        : null);
      return clone;
    }

    public bool ReadXml(XmlReader reader, string password = null) {
      var changed = false;

      reader.Read();
      while (reader.EOF == false && reader.IsEmptyElement) {
        reader.Read();
      }

      reader.MoveToContent();
      while (reader.EOF == false) {
        if (reader.IsStartElement()) {
          switch (reader.Name) {
            // v2 file
            case "Authenticator":
              changed = ReadXmlInternal(reader, password);
              break;

            default:
              reader.Skip();
              break;
          }
        }
        else {
          reader.Read();
          break;
        }
      }

      return changed;
    }

    protected bool ReadXmlInternal(XmlReader reader, string password = null) {
      var changed = false;

      if (decimal.TryParse(reader.GetAttribute("version"), System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var version)) {
        Version = version;

        if (version > CurrentVersion) {
          // ensure we don't overwrite a newer config
          throw new AuthInvalidNewerConfigException($"Your authenticators were saved with newer a version of Authenticator and so cannot be loaded. Please use Authenticator {version} or later.");
        }
      }

      var encrypted = reader.GetAttribute("encrypted");
      PasswordType = Authenticator.DecodePasswordTypes(encrypted);
      if (PasswordType != Authenticator.PasswordTypes.None) {
        // read the encrypted text from the node
        var data = reader.ReadElementContentAsString();
        // decrypt
        data = Authenticator.DecryptSequence(data, PasswordType, password);

        using (var ms = new MemoryStream(Authenticator.StringToByteArray(data))) {
          reader = XmlReader.Create(ms);
          changed = ReadXml(reader, password);
        }

        PasswordType = Authenticator.DecodePasswordTypes(encrypted);
        Password = password;

        return changed;
      }

      reader.MoveToContent();
      if (reader.IsEmptyElement) {
        reader.Read();
        return false;
      }

      var defaultAutoRefresh = true;
      var defaultCopyOnCode = false;

      reader.Read();
      while (reader.EOF == false) {
        if (reader.IsStartElement()) {
          switch (reader.Name) {
            case "config":
              changed = ReadXmlInternal(reader, password) || changed;
              break;

            // 3.2 has new layout 
            case "data": {
              encrypted = reader.GetAttribute("encrypted");
              PasswordType = Authenticator.DecodePasswordTypes(encrypted);
              if (PasswordType != Authenticator.PasswordTypes.None) {
                HashAlgorithm hasher;
                var hash = reader.GetAttribute("sha1");
                if (string.IsNullOrEmpty(hash) == false) {
                  hasher = Authenticator.SafeHasher("SHA1");
                }
                else {
                  // old version has md5
                  hash = reader.GetAttribute("md5");
                  hasher = Authenticator.SafeHasher("MD5");
                }

                // read the encrypted text from the node
                var data = reader.ReadElementContentAsString();

                hasher.ComputeHash(Authenticator.StringToByteArray(data));
                hasher.Dispose();

                // decrypt
                data = Authenticator.DecryptSequence(data, PasswordType, password);
                var plain = Authenticator.StringToByteArray(data);

                using (var ms = new MemoryStream(plain)) {
                  var datareader = XmlReader.Create(ms);
                  changed = ReadXmlInternal(datareader, password) || changed;
                }

                PasswordType = Authenticator.DecodePasswordTypes(encrypted);
                Password = password;
              }
            }
              break;

            case "alwaysontop":
              alwaysOnTop = reader.ReadElementContentAsBoolean();
              break;

            case "usetrayicon":
              useTrayIcon = reader.ReadElementContentAsBoolean();
              break;

            case "notifyaction":
              var s = reader.ReadElementContentAsString();
              if (string.IsNullOrEmpty(s) == false) {
                try {
                  notifyAction = (NotifyActions) Enum.Parse(typeof(NotifyActions), s, true);
                }
                catch (Exception) {
                  // ignored
                }
              }

              break;

            case "startwithwindows":
              startWithWindows = reader.ReadElementContentAsBoolean();
              break;

            case "autosize":
              autoSize = reader.ReadElementContentAsBoolean();
              break;

            case "left":
              position.X = reader.ReadElementContentAsInt();
              break;

            case "top":
              position.Y = reader.ReadElementContentAsInt();
              break;

            case "width":
              width = reader.ReadElementContentAsInt();
              break;

            case "height":
              height = reader.ReadElementContentAsInt();
              break;

            case "theme":
              theme = reader.ReadElementContentAsString();
              break;

            case "pgpkey":
              PgpKey = reader.ReadElementContentAsString();
              break;

            // previous setting used as defaults for new
            case "autorefresh":
              defaultAutoRefresh = reader.ReadElementContentAsBoolean();
              break;
            case "copyoncode":
              defaultCopyOnCode = reader.ReadElementContentAsBoolean();
              break;
            case AuthAuthenticator.ConfigSectionName:
              var wa = new AuthAuthenticator();
              changed = wa.ReadXml(reader, password) || changed;
              Add(wa);
              if (CurrentAuthenticator == null) {
                CurrentAuthenticator = wa;
              }

              break;

            // for old 2.x configs
            case "authenticator":
              var authOld = new AuthAuthenticator {
                AuthenticatorData = Authenticator.ReadXmlv2(reader, password)
              };
              switch (authOld.AuthenticatorData) {
                case BattleNetAuthenticator _:
                  authOld.Name = "Battle.net";
                  break;
                case GuildWarsAuthenticator _:
                  authOld.Name = "GuildWars 2";
                  break;
                default:
                  authOld.Name = "Authenticator";
                  break;
              }

              Add(authOld);
              CurrentAuthenticator = authOld;
              authOld.AutoRefresh = defaultAutoRefresh;
              authOld.CopyOnCode = defaultCopyOnCode;
              break;

            default:
              reader.Skip();
              break;
          }
        }
        else {
          reader.Read();
          break;
        }
      }

      return changed;
    }

    public void WriteXmlString(XmlWriter writer, bool includeFilename = false) {
      writer.WriteStartDocument(true);
      //
      if (includeFilename && string.IsNullOrEmpty(Filename) == false) {
        writer.WriteComment(Filename);
      }

      //
      writer.WriteStartElement("Authenticator");
      writer.WriteAttributeString("version",
        Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
      //
      writer.WriteStartElement("alwaysontop");
      writer.WriteValue(AlwaysOnTop);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("usetrayicon");
      writer.WriteValue(UseTrayIcon);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("notifyaction");
      writer.WriteValue(Enum.GetName(typeof(NotifyActions), NotifyAction));
      writer.WriteEndElement();
      //
      writer.WriteStartElement("startwithwindows");
      writer.WriteValue(StartWithWindows);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("autosize");
      writer.WriteValue(AutoSize);
      writer.WriteEndElement();
      //
      if (Position.IsEmpty == false) {
        writer.WriteStartElement("left");
        writer.WriteValue(Position.X);
        writer.WriteEndElement();
        writer.WriteStartElement("top");
        writer.WriteValue(Position.Y);
        writer.WriteEndElement();
      }

      //
      writer.WriteStartElement("width");
      writer.WriteValue(Width);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("height");
      writer.WriteValue(Height);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("theme");
      writer.WriteValue(Theme);
      writer.WriteEndElement();
      //
      if (string.IsNullOrEmpty(PgpKey) == false) {
        writer.WriteStartElement("pgpkey");
        writer.WriteCData(PgpKey);
        writer.WriteEndElement();
      }

      if (PasswordType != Authenticator.PasswordTypes.None) {
        writer.WriteStartElement("data");

        var encryptedTypes = new StringBuilder();
        if ((PasswordType & Authenticator.PasswordTypes.Explicit) != 0) {
          encryptedTypes.Append("y");
        }

        if ((PasswordType & Authenticator.PasswordTypes.User) != 0) {
          encryptedTypes.Append("u");
        }

        if ((PasswordType & Authenticator.PasswordTypes.Machine) != 0) {
          encryptedTypes.Append("m");
        }

        writer.WriteAttributeString("encrypted", encryptedTypes.ToString());

        byte[] data;
        using (var ms = new MemoryStream()) {
          var settings = new XmlWriterSettings {
            Indent = true,
            Encoding = Encoding.UTF8
          };
          using (var encryptedwriter = XmlWriter.Create(ms, settings)) {
            encryptedwriter.WriteStartElement("config");
            foreach (var wa in this) {
              wa.WriteXmlString(encryptedwriter);
            }

            encryptedwriter.WriteEndElement();
          }

          data = ms.ToArray();
        }

        using (var hasher = Authenticator.SafeHasher("SHA1")) {
          var encdata = Authenticator.EncryptSequence(Authenticator.ByteArrayToString(data), PasswordType, Password);
          var enchash =
            Authenticator.ByteArrayToString(hasher.ComputeHash(Authenticator.StringToByteArray(encdata)));
          writer.WriteAttributeString("sha1", enchash);
          writer.WriteString(encdata);
        }

        writer.WriteEndElement();
      }
      else {
        foreach (var wa in this) {
          wa.WriteXmlString(writer);
        }
      }

      // close Authenticator
      writer.WriteEndElement();

      // end document
      writer.WriteEndDocument();
    }

    #endregion
  }

  public class ConfigChangedEventArgs : EventArgs {
    public string PropertyName { get; }

    public AuthAuthenticator Authenticator { get; }
    public AuthAuthenticatorChangedEventArgs AuthenticatorChangedEventArgs { get; }

    public ConfigChangedEventArgs(string propertyName, AuthAuthenticator authenticator = null,
      AuthAuthenticatorChangedEventArgs acargs = null) {
      PropertyName = propertyName;
      Authenticator = authenticator;
      AuthenticatorChangedEventArgs = acargs;
    }
  }

  public class AuthInvalidNewerConfigException : ApplicationException {
    public AuthInvalidNewerConfigException(string msg)
      : base(msg) {
    }
  }
}