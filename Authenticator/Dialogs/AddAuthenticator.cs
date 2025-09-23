using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using sergiye.Common;
using ZXing;
using ZXing.Common;

namespace Authenticator {
  public partial class AddAuthenticator : Form {
    private const string HOTP = "hotp";
    private const string TOTP = "totp";
    private const string GoogleMigration = "otpauth-migration://offline?data=";

    public AddAuthenticator(AuthAuthenticator authenticator) {
      InitializeComponent();
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
      Authenticator = authenticator;
      Theme.Current.Apply(this);
    }

    protected override void OnLoad(EventArgs e) {
      base.OnLoad(e);
      //getFromScreenButton_Click(this, EventArgs.Empty);
    }

    public AuthAuthenticator Authenticator { get; set; }

    #region Form Events

    private void AddAuthenticator_Load(object sender, EventArgs e) {
      nameField.Text = Authenticator.Name;
      hashField.Items.Clear();
      hashField.Items.AddRange(Enum.GetNames(typeof(Authenticator.HmacTypes)));
      hashField.SelectedIndex = 0;
      intervalField.Text = global::Authenticator.Authenticator.DEFAULT_PERIOD.ToString();
      digitsField.Text = global::Authenticator.Authenticator.DEFAULT_CODE_DIGITS.ToString();
    }

    private void timer_Tick(object sender, EventArgs e) {
      if (Authenticator.AuthenticatorData != null && !(Authenticator.AuthenticatorData is HotpAuthenticator) &&
          codeProgress.Visible) {
        var time = (int) (global::Authenticator.Authenticator.CurrentTime / 1000L) % Authenticator.AuthenticatorData.Period;
        codeProgress.Value = time + 1;
        if (time == 0) {
          codeField.Text = Authenticator.AuthenticatorData.CurrentCode;
        }
      }
    }

    private void cancelButton_Click(object sender, EventArgs e) {
      if (Authenticator.AuthenticatorData != null) {
        var result = MainForm.ConfirmDialog(Owner,
          "WARNING: Your authenticator has not been saved." + Environment.NewLine + Environment.NewLine
          + "If you have added this authenticator to your online account, you will not be able to login in the future, and you need to click YES to save it." +
          Environment.NewLine + Environment.NewLine
          + "Do you want to save this authenticator?", MessageBoxButtons.YesNoCancel);
        if (result == DialogResult.Yes) {
          DialogResult = DialogResult.OK;
        }
        else if (result == DialogResult.Cancel) {
          DialogResult = DialogResult.None;
        }
      }
    }

    private void okButton_Click(object sender, EventArgs e) {

      var first = Authenticator.AuthenticatorData == null;
      if (VerifyAuthenticator(false) == false) {
        DialogResult = DialogResult.None;
        return;
      }

      if (first) {
        DialogResult = DialogResult.None;
        return;
      }

      Authenticator.Name = nameField.Text;

      // if this is a htop we reduce the counter because we are going to immediate get the code and increment
      if (Authenticator.AuthenticatorData is HotpAuthenticator authenticator) {
        authenticator.Counter--;
      }
    }

    private void verifyButton_Click(object sender, EventArgs e) {
      VerifyAuthenticator(true);
    }

    private void basementRadio_CheckedChanged(object sender, EventArgs e) {
      if (counterBasedRadio.Checked) {
        //timeBasedRadio.Checked = !counterBasedRadio.Checked;
        counterField.Visible = true;
        step3Label.Text = "3. Enter the initial counter value if known. Click the Verify button that will show the last code that was used.";
      }
      else {
        counterField.Visible = false;
        step3Label.Text = "3. Click the Verify button to check the first code.";
        //counterBasedRadio.Checked = !timeBasedRadio.Checked;
      }
    }

    private void secretCodeField_Leave(object sender, EventArgs e) {
      VerifyAuthenticator(true);
    }
    
    private void getFromScreenButton_Click(object sender, EventArgs e) {
      try {
        this.Visible = false;
        //using (var bitmap = (Bitmap)SnippingTool.GetMultipleScreenImage()) {
        using (var bitmap = (Bitmap)SnippingTool.SnipMultiple()) {
          if (bitmap == null) return;
          //todo: use gaussian filter to remove noise
          //image = new GaussianBlur(2).ProcessImage(image);
          var reader = new BarcodeReader(null, null, ls => new GlobalHistogramBinarizer(ls)) {
            AutoRotate = false,
            Options = new DecodingOptions {
              //PossibleFormats = new BarcodeFormat[] { BarcodeFormat.QR_CODE },
              TryHarder = true,
              TryInverted = false,
            }
          };
          var result = reader.Decode(bitmap);
          if (result != null && !string.IsNullOrEmpty(result.Text)) {
            secretCodeField.Text = result.Text;
            VerifyAuthenticator(true);
          }
          else {
            MainForm.ErrorDialog(Owner, "Unable to decode QR code from captured image");
          }
        }
      }
      catch (Exception ex) {
        MainForm.ErrorDialog(Owner, "Error decoding QR code from captured image", ex);
      }
      finally {
        this.Visible = true;
      }
    }

    #endregion

    #region Private methods

    private bool VerifyAuthenticator(bool updateNameField) {

      var privateKey = secretCodeField.Text.Trim();

      if (privateKey.Length == 0) {
        //MainForm.ErrorDialog(Owner, "Please enter the Secret Code");
        return false;
      }

      if (string.IsNullOrEmpty(privateKey))
        return false;

      if (string.IsNullOrEmpty(digitsField.Text) || !int.TryParse(digitsField.Text, out var digits) ||
          digits <= 0) {
        return false;
      }

      Enum.TryParse((string) hashField.SelectedItem, out Authenticator.HmacTypes hmac);

      var authType = timeBasedRadio.Checked ? TOTP : HOTP;

      if (string.IsNullOrEmpty(intervalField.Text) || !int.TryParse(intervalField.Text, out var period) ||
          period <= 0) {
        return false;
      }

      long counter = 0;
      string issuer = null;
      string serial = null;

      //google migration support
      if (privateKey.StartsWith(GoogleMigration)) {
        var szData = HttpUtility.UrlDecode(privateKey.Substring(GoogleMigration.Length));
        var arrByte = Convert.FromBase64String(szData);
        Payload item = Payload.Parser.ParseFrom(arrByte);
        var arr = item.OtpParameters;

        for (var i = 0; i < arr.Count; ++i) {
          serial = Base32.GetInstance().Encode(arr[i].Secret.ToByteArray());
          authType = arr[i].Type.ToString().ToUpper();
          if (arr[i].HasIssuer)
            issuer = arr[i].Issuer;
          if (arr[i].HasDigits) {
            switch (arr[i].Digits) {
              case Payload.Types.DigitCount.Six:
                digits = 6;
                break;
              case Payload.Types.DigitCount.Eight:
                digits = 8;
                break;
            }
          }
          var label = arr[i].HasIssuer ? $"{arr[i].Issuer} ({arr[i].Name})" : arr[i].Name;
          secretCodeField.Text = privateKey = $"otpauth://{authType}/{label}?secret={serial}";
          break; //todo: process only the first one
        }
      }

      // if this is a URL, pull it down
      Match match;
      if (Regex.IsMatch(privateKey, "https?://.*", RegexOptions.IgnoreCase) && Uri.TryCreate(privateKey, UriKind.Absolute, out var uri)) {
        try {
          var request = (HttpWebRequest) WebRequest.Create(uri);
          request.AllowAutoRedirect = true;
          request.Timeout = 10000;
          request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
          using (var response = (HttpWebResponse) request.GetResponse()) {
            if (response.StatusCode == HttpStatusCode.OK &&
                response.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) {
              using (var stream = response.GetResponseStream()) {
                if (stream == null) {
                  MainForm.ErrorDialog(Owner, "Cannot load QR code image");
                  return false;
                }
                using (var bitmap = (Bitmap) Image.FromStream(stream)) {
                  IBarcodeReader reader = new BarcodeReader();
                  var result = reader.Decode(bitmap);
                  if (result != null && !string.IsNullOrEmpty(result.Text)) {
                    privateKey = HttpUtility.UrlDecode(result.Text);
                  }
                }
              }
            }
          }
        }
        catch (Exception ex) {
          MainForm.ErrorDialog(Owner, "Cannot load QR code image from " + privateKey, ex);
          return false;
        }
      }
      
      if ((match = Regex.Match(privateKey, @"data:image/([^;]+);base64,(.*)", RegexOptions.IgnoreCase)).Success) {
        var imageData = Convert.FromBase64String(match.Groups[2].Value);
        using (var ms = new MemoryStream(imageData)) {
          using (var bitmap = (Bitmap) Image.FromStream(ms)) {
            IBarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);
            if (result != null) {
              privateKey = HttpUtility.UrlDecode(result.Text);
            }
          }
        }
      }
      
      if (File.Exists(privateKey)) {
        // assume this is the image file
        using (var bitmap = (Bitmap) Image.FromFile(privateKey)) {
          IBarcodeReader reader = new BarcodeReader();
          var result = reader.Decode(bitmap);
          if (result != null) {
            privateKey = result.Text;
          }
        }
      }

      // check for otpauth://, e.g. "otpauth://totp/dc3bf64c-2fd4-40fe-a8cf-83315945f08b@blockchain.info?secret=IHZJDKAEEC774BMUK3GX6SA"
      match = Regex.Match(privateKey, @"otpauth://([^/]+)/([^?]+)\?(.*)", RegexOptions.IgnoreCase);
      if (match.Success) {
        authType = match.Groups[1].Value.ToLower();
        var label = match.Groups[2].Value;
        var p = label.IndexOf(":", StringComparison.Ordinal);
        if (p != -1) {
          // issuer = label.Substring(0, p);
          label = label.Substring(p + 1);
        }
        
        switch (authType) {
          case HOTP:
            counterBasedRadio.Checked = true;
            break;
          case TOTP:
            timeBasedRadio.Checked = true;
            counterField.Text = string.Empty;
            break;
        }
        
        var qs = AuthHelper.ParseQueryString(match.Groups[3].Value);
        privateKey = qs["secret"] ?? privateKey;
        if (int.TryParse(qs["digits"], out var queryDigits) && queryDigits != 0) {
          digits = queryDigits;
          digitsField.Text = digits.ToString();
        }

        if (qs["counter"] != null && long.TryParse(qs["counter"], out counter)) {
          counterField.Text = counter.ToString();
        }

        if (!string.IsNullOrEmpty(qs["issuer"])) {
          issuer = qs["issuer"];
          label = issuer + (string.IsNullOrEmpty(label) == false ? " (" + label + ")" : string.Empty);
        }
        if (!string.IsNullOrEmpty(label) && updateNameField) {
          Authenticator.Name = nameField.Text = HttpUtility.UrlDecode(label);
        }
        
        serial = qs["serial"];
        
        var periods = qs["period"];
        if (!string.IsNullOrEmpty(periods) && int.TryParse(periods, out period)){
          intervalField.Text = period.ToString();
        }

        if (qs["algorithm"] != null && Enum.TryParse(qs["algorithm"], true, out hmac)) {
          hashField.SelectedItem = hmac.ToString();
        }
      }

      // just get the hex chars
      privateKey = Regex.Replace(privateKey, @"[^0-9a-z]", "", RegexOptions.IgnoreCase);
      if (privateKey.Length == 0) {
        MainForm.ErrorDialog(Owner, "The secret code is not valid");
        return false;
      }

      try {
        Authenticator auth;
        switch (authType) {
          case TOTP: {
            auth = new GoogleAuthenticator();
            ((GoogleAuthenticator) auth).Enroll(privateKey);

            timer.Enabled = true;
            codeProgress.Visible = true;
            timeBasedRadio.Checked = true;
            break;
          }
          case HOTP: {
            auth = new HotpAuthenticator();
            if (counterField.Text.Trim().Length != 0) {
              long.TryParse(counterField.Text.Trim(), out counter);
            }

            ((HotpAuthenticator) auth).Enroll(privateKey, counter); // start with the next code
            timer.Enabled = false;
            codeProgress.Visible = false;
            counterBasedRadio.Checked = true;
            break;
          }
          default:
            MainForm.ErrorDialog(Owner, "Only TOTP or HOTP authenticators are supported");
            return false;
        }

        auth.HmacType = hmac;
        auth.CodeDigits = digits;
        auth.Period = period;
        Authenticator.AuthenticatorData = auth;

        if (!string.IsNullOrEmpty(issuer))
          Authenticator.Skin = AuthHelper.DetectIconByIssuer(issuer);

        if (digits > 5) {
          codeField.SpaceOut = digits / 2;
        }
        else {
          codeField.SpaceOut = 0;
        }

        //string key = Base32.getInstance().Encode(this.Authenticator.AuthenticatorData.SecretKey);
        codeField.Text = auth.CurrentCode;

        codeProgress.Maximum = period;
      }
      catch (Exception ex) {
        MainForm.ErrorDialog(Owner, "Unable to create the authenticator. The secret code is probably invalid.", ex);
        return false;
      }

      //secretCodeField.Text = HttpUtility.UrlDecode(result.Text);
      return true;
    }

    #endregion

    private void btnBrowse_Click(object sender, EventArgs e) {
      var dlg = new OpenFileDialog {
        AddExtension = true,
        CheckFileExists = true,
        DefaultExt = "png",
        //InitialDirectory = Directory.GetCurrentDirectory(),
        FileName = string.Empty,
        Filter = "PNG Image Files (*.png)|*.png|GIF Image Files (*.gif)|*.gif|All Files (*.*)|*.*",
        RestoreDirectory = true,
        ShowReadOnly = false,
        Title = "Load from Image"
      };
      if (dlg.ShowDialog(this) != DialogResult.OK)
        return;
      secretCodeField.Text = dlg.FileName;
      VerifyAuthenticator(true);
    }
  }
}