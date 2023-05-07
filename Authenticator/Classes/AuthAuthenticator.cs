using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;

namespace Authenticator {
  public class AuthAuthenticator : ICloneable {
    public event AuthAuthenticatorChangedHandler OnAuthAuthenticatorChanged;

    public Guid Id { get; set; }

    public int Index { get; set; }

    public Authenticator AuthenticatorData { get; set; }

    public DateTime Created { get; set; }

    private string name;
    private string skin;
    private bool autoRefresh;
    private bool copyOnCode;

    public AuthAuthenticator() {
      Id = Guid.NewGuid();
      Created = DateTime.Now;
      autoRefresh = true;
    }

    public object Clone() {
      var clone = MemberwiseClone() as AuthAuthenticator;

      clone.Id = Guid.NewGuid();
      clone.OnAuthAuthenticatorChanged = null;
      clone.AuthenticatorData =
        (AuthenticatorData != null ? AuthenticatorData.Clone() as Authenticator : null);

      return clone;
    }

    public void MarkChanged() {
      OnAuthAuthenticatorChanged?.Invoke(this, new AuthAuthenticatorChangedEventArgs());
    }

    public string Name {
      get => name;
      set {
        name = value;
        OnAuthAuthenticatorChanged?.Invoke(this, new AuthAuthenticatorChangedEventArgs("Name"));
      }
    }

    public string Skin {
      get => skin;
      set {
        skin = value;
        OnAuthAuthenticatorChanged?.Invoke(this, new AuthAuthenticatorChangedEventArgs("Skin"));
      }
    }

    public bool AutoRefresh {
      get => !(AuthenticatorData is HotpAuthenticator) && autoRefresh;
      set {
        // HTOP must always be false
        autoRefresh = !(AuthenticatorData is HotpAuthenticator) && value;
        OnAuthAuthenticatorChanged?.Invoke(this, new AuthAuthenticatorChangedEventArgs("AutoRefresh"));
      }
    }

    public bool CopyOnCode {
      get => copyOnCode;
      set {
        copyOnCode = value;
        OnAuthAuthenticatorChanged?.Invoke(this, new AuthAuthenticatorChangedEventArgs("CopyOnCode"));
      }
    }

    public Bitmap Icon {
      get {

        if (string.IsNullOrEmpty(Skin)) {
          return GenerateDrawText(Name.Substring(0, 2).ToUpper(), Color.Azure, Color.CornflowerBlue);
        }

        if (Skin.StartsWith("base64:")) {
          var bytes = Convert.FromBase64String(Skin.Substring(7));
          if (bytes.Length > 0)
            using (var stream = new MemoryStream(bytes, 0, bytes.Length))
              return new Bitmap(stream);
        }
        else {
          using (var stream = Assembly.GetExecutingAssembly()
                   .GetManifestResourceStream("Authenticator.Resources." + Skin)) {
            if (stream != null) 
              return new Bitmap(stream);
          }
        }
        
        if (AuthenticatorData == null)
          return null;

        return new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Authenticator.Resources." + AuthenticatorData.GetType().Name + "Icon.png"));
      }
      set {
        if (value == null) {
          Skin = null;
          return;
        }

        using (var ms = new MemoryStream()) {
          value.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
          Skin = "base64:" + Convert.ToBase64String(ms.ToArray());
        }
      }
    }

    private static Bitmap GenerateDrawText(string text, Color textColor, Color backColor,
      int imgSize = 48, int fontSize = 12, string fontName = "Arial", FontStyle fontStyle = FontStyle.Bold) {

      var img = new Bitmap(imgSize, imgSize);
      using (var font = new Font(fontName, fontSize, fontStyle)) {
        using (var drawing = Graphics.FromImage(img)) {
          drawing.Clear(Color.Transparent);
          using (Brush backBrush = new SolidBrush(backColor))
            drawing.FillEllipse(backBrush, 0, 0, imgSize, imgSize);
          using (Brush textBrush = new SolidBrush(textColor)) {
            var textSize = drawing.MeasureString(text, font);
            drawing.DrawString(text, font, textBrush, (imgSize - textSize.Width) / 2 + 1,
              (imgSize - textSize.Height) / 2);
            drawing.Save();
          }
        }
      }
      return img;
    }

    public string CurrentCode {
      get {
        if (AuthenticatorData == null) {
          return null;
        }

        var code = AuthenticatorData.CurrentCode;

        if (AuthenticatorData is HotpAuthenticator) {
          if (OnAuthAuthenticatorChanged != null) {
            OnAuthAuthenticatorChanged(this,
              new AuthAuthenticatorChangedEventArgs("HOTP", AuthenticatorData));
          }
        }

        return code;
      }
    }

    public void Sync() {
      if (AuthenticatorData != null) {
        try {
          AuthenticatorData.Sync();
        }
        catch (EncryptedSecretDataException) {
          // reset lastsync to force sync on next decryption
        }
      }
    }

    public void CopyCodeToClipboard(Form form, string code = null, bool showError = false) {
      if (code == null) {
        code = CurrentCode;
      }

      var clipRetry = false;
      do {
        var failed = false;
        // check if the clipboard is locked
        var hWnd = WinApi.GetOpenClipboardWindow();
        if (hWnd != IntPtr.Zero) {
          var len = WinApi.GetWindowTextLength(hWnd);
          if (len == 0) {
            AuthHelper.ShowException(new ApplicationException("Clipboard in use by another process"));
          }
          else {
            var sb = new StringBuilder(len + 1);
            WinApi.GetWindowText(hWnd, sb, sb.Capacity);
            AuthHelper.ShowException(new ApplicationException("Clipboard in use by '" + sb + "'"));
          }

          failed = true;
        }
        else {
          // Issue#170: can still get error copying even though it works, so just increase retries and ignore error
          try {
            Clipboard.Clear();

            // add delay for clip error
            System.Threading.Thread.Sleep(100);

            Clipboard.SetDataObject(code, true, 4, 250);
          }
          catch (ExternalException) {
          }
        }

        if (failed && showError) {
          // only show an error the first time
          clipRetry = (MessageBox.Show(form,
            "Unable to copy to the clipboard. Another application is probably using it.\nTry again?",
            AuthHelper.APPLICATION_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2) == DialogResult.Yes);
        }
      } while (clipRetry);
    }

    public bool ReadXml(XmlReader reader, string password) {
      var changed = false;

      if (Guid.TryParse(reader.GetAttribute("id"), out var id)) {
        Id = id;
      }

      var authenticatorType = reader.GetAttribute("type");
      if (string.IsNullOrEmpty(authenticatorType) == false) {
        var type = Assembly.GetExecutingAssembly().GetType(authenticatorType, false, true);
        AuthenticatorData = Activator.CreateInstance(type) as Authenticator;
      }

      //string encrypted = reader.GetAttribute("encrypted");
      //if (string.IsNullOrEmpty(encrypted) == false)
      //{
      //	// read the encrypted text from the node
      //	string data = reader.ReadElementContentAsString();
      //	// decrypt
      //	Authenticator.PasswordTypes passwordType;
      //	data = Authenticator.DecryptSequence(data, encrypted, password, out passwordType);

      //	using (MemoryStream ms = new MemoryStream(Authenticator.StringToByteArray(data)))
      //	{
      //		reader = XmlReader.Create(ms);
      //		ReadXml(reader, password);
      //	}
      //	this.PasswordType = passwordType;
      //	this.Password = password;

      //	return;
      //}

      reader.MoveToContent();

      if (reader.IsEmptyElement) {
        reader.Read();
        return changed;
      }

      reader.Read();
      while (reader.EOF == false) {
        if (reader.IsStartElement()) {
          switch (reader.Name) {
            case "name":
              Name = reader.ReadElementContentAsString();
              break;

            case "created":
              var t = reader.ReadElementContentAsLong();
              t += Convert.ToInt64(new TimeSpan(new DateTime(1970, 1, 1).Ticks).TotalMilliseconds);
              t *= TimeSpan.TicksPerMillisecond;
              Created = new DateTime(t).ToLocalTime();
              break;

            case "autorefresh":
              autoRefresh = reader.ReadElementContentAsBoolean();
              break;

            case "copyoncode":
              copyOnCode = reader.ReadElementContentAsBoolean();
              break;

            case "skin":
              skin = reader.ReadElementContentAsString();
              break;

            case "authenticatordata":
              try {
                // we don't pass the password as they are locked till clicked
                changed = AuthenticatorData.ReadXml(reader) || changed;
              }
              catch (EncryptedSecretDataException) {
                // no action needed
              }
              catch (BadPasswordException) {
                // no action needed
              }

              break;

            // v2
            case "authenticator":
              AuthenticatorData = Authenticator.ReadXmlv2(reader, password);
              break;
            // v2
            case "servertimediff":
              AuthenticatorData.ServerTimeDiff = reader.ReadElementContentAsLong();
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

    public void WriteXmlString(XmlWriter writer) {
      writer.WriteStartElement(typeof(AuthAuthenticator).Name);
      writer.WriteAttributeString("id", Id.ToString());
      if (AuthenticatorData != null) {
        writer.WriteAttributeString("type", AuthenticatorData.GetType().FullName);
      }

      //if (this.PasswordType != Authenticator.PasswordTypes.None)
      //{
      //	string data;

      //	using (MemoryStream ms = new MemoryStream())
      //	{
      //		XmlWriterSettings settings = new XmlWriterSettings();
      //		settings.Indent = true;
      //		settings.Encoding = Encoding.UTF8;
      //		using (XmlWriter encryptedwriter = XmlWriter.Create(ms, settings))
      //		{
      //			Authenticator.PasswordTypes savedpasswordType = PasswordType;
      //			PasswordType = Authenticator.PasswordTypes.None;
      //			WriteXmlString(encryptedwriter);
      //			PasswordType = savedpasswordType;
      //		}
      //		//data = Encoding.UTF8.GetString(ms.ToArray());
      //		data = Authenticator.ByteArrayToString(ms.ToArray());
      //	}

      //	string encryptedTypes;
      //	data = Authenticator.EncryptSequence(data, PasswordType, Password, out encryptedTypes);
      //	writer.WriteAttributeString("encrypted", encryptedTypes);
      //	writer.WriteString(data);
      //	writer.WriteEndElement();

      //	return;
      //}

      writer.WriteStartElement("name");
      writer.WriteValue(Name ?? string.Empty);
      writer.WriteEndElement();

      writer.WriteStartElement("created");
      writer.WriteValue(Convert.ToInt64((Created.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds));
      writer.WriteEndElement();

      writer.WriteStartElement("autorefresh");
      writer.WriteValue(AutoRefresh);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("copyoncode");
      writer.WriteValue(CopyOnCode);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("skin");
      writer.WriteValue(Skin ?? string.Empty);
      writer.WriteEndElement();

      // save the authenticator to the config file
      AuthenticatorData?.WriteToWriter(writer);

      writer.WriteEndElement();
    }

    public virtual string ToUrl(bool compat = false) {
      var type = "totp";
      var extraparams = string.Empty;

      Match match;
      var issuer = AuthenticatorData.Issuer;
      var label = Name;
      if (string.IsNullOrEmpty(issuer) &&
          (match = Regex.Match(label, @"^([^\(]+)\s+\((.*?)\)(.*)")).Success) {
        issuer = match.Groups[1].Value;
        label = match.Groups[2].Value + match.Groups[3].Value;
      }

      if (string.IsNullOrEmpty(issuer) == false &&
          (match = Regex.Match(label, @"^" + issuer + @"\s+\((.*?)\)(.*)")).Success) {
        label = match.Groups[1].Value + match.Groups[2].Value;
      }

      if (string.IsNullOrEmpty(issuer) == false) {
        extraparams += "&issuer=" + HttpUtility.UrlEncode(issuer);
      }

      if (AuthenticatorData.HmacType != Authenticator.DEFAULT_HMAC_TYPE) {
        extraparams += "&algorithm=" + AuthenticatorData.HmacType;
      }

      if (AuthenticatorData is BattleNetAuthenticator) {
        extraparams += "&serial=" +
                       HttpUtility.UrlEncode(((BattleNetAuthenticator) AuthenticatorData).Serial.Replace("-", ""));
      }
      else if (AuthenticatorData is HotpAuthenticator) {
        type = "hotp";
        extraparams += "&counter=" + ((HotpAuthenticator) AuthenticatorData).Counter;
      }

      var secret = HttpUtility.UrlEncode(Base32.GetInstance().Encode(AuthenticatorData.SecretKey));

      // add the skin
      if (string.IsNullOrEmpty(Skin) == false && compat == false) {
        if (Skin.StartsWith("base64:")) {
          var bytes = Convert.FromBase64String(Skin.Substring(7));
          var icon32 = Base32.GetInstance().Encode(bytes);
          extraparams += "&icon=" + HttpUtility.UrlEncode("base64:" + icon32);
        }
        else {
          extraparams += "&icon=" + HttpUtility.UrlEncode(Skin.Replace("Icon.png", ""));
        }
      }

      if (AuthenticatorData.Period != Authenticator.DEFAULT_PERIOD) {
        extraparams += "&period=" + AuthenticatorData.Period;
      }

      var url = string.Format("otpauth://" + type + "/{0}?secret={1}&digits={2}{3}",
        (string.IsNullOrEmpty(issuer) == false
          ? HttpUtility.UrlPathEncode(issuer) + ":" + HttpUtility.UrlPathEncode(label)
          : HttpUtility.UrlPathEncode(label)),
        secret,
        AuthenticatorData.CodeDigits,
        extraparams);

      return url;
    }
  }

  public delegate void AuthAuthenticatorChangedHandler(AuthAuthenticator source,
    AuthAuthenticatorChangedEventArgs args);

  public class AuthAuthenticatorChangedEventArgs : EventArgs {
    public string Property { get; }
    public Authenticator Authenticator { get; }

    public AuthAuthenticatorChangedEventArgs(string property = null, Authenticator authenticator = null) {
      Property = property;
      Authenticator = authenticator;
    }
  }
}