using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using ZXing;

namespace Authenticator {
  public partial class AddAuthenticator : Form {
    private const string HOTP = "hotp";
    private const string TOTP = "totp";

    public AddAuthenticator() {
      InitializeComponent();
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
    }

    public AuthAuthenticator Authenticator { get; set; }

    private bool syncErrorWarned;

    #region Form Events

    private void AddAuthenticator_Load(object sender, EventArgs e) {
      nameField.Text = Authenticator.Name;
      codeField.SecretMode = true;
      hashField.Items.Clear();
      hashField.Items.AddRange(Enum.GetNames(typeof(Authenticator.HmacTypes)));
      hashField.SelectedIndex = 0;
      intervalField.Text = global::Authenticator.Authenticator.DEFAULT_PERIOD.ToString();
      digitsField.Text = global::Authenticator.Authenticator.DEFAULT_CODE_DIGITS.ToString();
    }

    private void timer_Tick(object sender, EventArgs e) {
      if (Authenticator.AuthenticatorData != null && !(Authenticator.AuthenticatorData is HotpAuthenticator) &&
          codeProgress.Visible) {
        var time = (int) (Authenticator.AuthenticatorData.ServerTime / 1000L) % Authenticator.AuthenticatorData.Period;
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
      var privateKey = secretCodeField.Text.Trim();
      if (privateKey.Length == 0) {
        MainForm.ErrorDialog(Owner, "Please enter the Secret Code");
        DialogResult = DialogResult.None;
        return;
      }

      var first = Authenticator.AuthenticatorData == null;
      if (VerifyAuthenticator(privateKey) == false) {
        DialogResult = DialogResult.None;
        return;
      }

      if (first) {
        DialogResult = DialogResult.None;
        return;
      }

      // if this is a htop we reduce the counter because we are going to immediate get the code and increment
      if (Authenticator.AuthenticatorData is HotpAuthenticator authenticator) {
        authenticator.Counter--;
      }
    }

    private void verifyButton_Click(object sender, EventArgs e) {
      var privateKey = secretCodeField.Text.Trim();
      if (privateKey.Length == 0) {
        MainForm.ErrorDialog(Owner, "Please enter the Secret Code");
        return;
      }

      VerifyAuthenticator(privateKey);
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
      if (Regex.IsMatch(secretCodeField.Text, "https?://.*", RegexOptions.IgnoreCase) &&
          Uri.TryCreate(secretCodeField.Text, UriKind.Absolute, out var uri)) {
        try {
          var request = (HttpWebRequest) WebRequest.Create(uri);
          request.AllowAutoRedirect = true;
          request.Timeout = 20000;
          request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
          using (var response = (HttpWebResponse) request.GetResponse()) {
            if (response.StatusCode == HttpStatusCode.OK &&
                response.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) {
              using (var bitmap = (Bitmap) Image.FromStream(response.GetResponseStream())) {
                IBarcodeReader reader = new BarcodeReader();
                var result = reader.Decode(bitmap);
                if (result != null && string.IsNullOrEmpty(result.Text) == false) {
                  secretCodeField.Text = HttpUtility.UrlDecode(result.Text);
                }
              }
            }
          }
        }
        catch (Exception ex) {
          MainForm.ErrorDialog(Owner, "Cannot load QR code image from " + secretCodeField.Text, ex);
          return;
        }
      }

      var match = Regex.Match(secretCodeField.Text, @"otpauth://([^/]+)/([^?]+)\?(.*)", RegexOptions.IgnoreCase);
      if (!match.Success) return;
      var authType = match.Groups[1].Value.ToLower();
      var label = match.Groups[2].Value;

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
      if (qs["counter"] != null) {
        if (long.TryParse(qs["counter"], out var counter)) {
          counterField.Text = counter.ToString();
        }
      }

      var issuer = qs["issuer"];
      if (string.IsNullOrEmpty(issuer) == false) {
        label = issuer + (string.IsNullOrEmpty(label) == false ? " (" + label + ")" : string.Empty);
      }
      nameField.Text = label;

      if (int.TryParse(qs["period"], out var period) && period > 0) {
        intervalField.Text = period.ToString();
      }
      if (int.TryParse(qs["digits"], out var digits) && digits > 0) {
        digitsField.Text = digits.ToString();
      }
      if (Enum.TryParse<Authenticator.HmacTypes>(qs["algorithm"], true, out var hmac)) {
        hashField.SelectedItem = hmac.ToString();
      }
    }
    
    private void getFromScreenButton_Click(object sender, EventArgs e) {
      try {
        using (var bitmap = (Bitmap)SnippingTool.SnipMultiple()) {
          //todo: use gaussian filter to remove noise
          // var gFilter = new GaussianBlur(2);
          // image = gFilter.ProcessImage(image);
          // var reader = new BarcodeReader(null, null, ls => new GlobalHistogramBinarizer(ls)) {
          //   AutoRotate = false,
          //   TryInverted = false,
          //   Options = new DecodingOptions {
          //     PossibleFormats = new List<BarcodeFormat> {BarcodeFormat.QR_CODE},
          //     TryHarder = true
          //   }
          // };
          if (bitmap == null) return;
          var reader = new BarcodeReader();
          var result = reader.Decode(bitmap);
          if (result != null && string.IsNullOrEmpty(result.Text) == false) {
            if (VerifyAuthenticator(result.Text))
              secretCodeField.Text = HttpUtility.UrlDecode(result.Text);
          }
          else {
            MainForm.ErrorDialog(Owner, "Unable to decode QR code from captured image");
          }
        }
      }
      catch (Exception ex) {
        MainForm.ErrorDialog(Owner, "Error decoding QR code from captured image", ex);
      }
    }

    #endregion

    #region Private methods

    private bool IsValidFile(string filename) {
      try {
        // check path is valid
        new FileInfo(filename);
        return File.Exists(filename);
      }
      catch (Exception) {
      }

      return false;
    }

    private bool VerifyAuthenticator(string privateKey) {
      if (string.IsNullOrEmpty(privateKey)) {
        return false;
      }

      Authenticator.Name = nameField.Text;

      if (string.IsNullOrEmpty(digitsField.Text) || int.TryParse(digitsField.Text, out var digits) == false ||
          digits <= 0) {
        return false;
      }

      Enum.TryParse((string) hashField.SelectedItem, out Authenticator.HmacTypes hmac);

      var authType = timeBasedRadio.Checked ? TOTP : HOTP;

      if (string.IsNullOrEmpty(intervalField.Text) || int.TryParse(intervalField.Text, out var period) == false ||
          period <= 0) {
        return false;
      }

      long counter = 0;

      // if this is a URL, pull it down
      Match match;
      if (Regex.IsMatch(privateKey, "https?://.*") && Uri.TryCreate(privateKey, UriKind.Absolute, out var uri)) {
        try {
          var request = (HttpWebRequest) WebRequest.Create(uri);
          request.AllowAutoRedirect = true;
          request.Timeout = 20000;
          request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
          using (var response = (HttpWebResponse) request.GetResponse()) {
            if (response.StatusCode == HttpStatusCode.OK &&
                response.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) {
              using (var bitmap = (Bitmap) Image.FromStream(response.GetResponseStream())) {
                IBarcodeReader reader = new BarcodeReader();
                var result = reader.Decode(bitmap);
                if (result != null) {
                  privateKey = HttpUtility.UrlDecode(result.Text);
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
      else if ((match = Regex.Match(privateKey, @"data:image/([^;]+);base64,(.*)", RegexOptions.IgnoreCase)).Success) {
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
      else if (IsValidFile(privateKey)) {
        // assume this is the image file
        using (var bitmap = (Bitmap) Image.FromFile(privateKey)) {
          IBarcodeReader reader = new BarcodeReader();
          var result = reader.Decode(bitmap);
          if (result != null) {
            privateKey = result.Text;
          }
        }
      }

      string issuer = null;
      string serial = null;

      // check for otpauth://, e.g. "otpauth://totp/dc3bf64c-2fd4-40fe-a8cf-83315945f08b@blockchain.info?secret=IHZJDKAEEC774BMUK3GX6SA"
      match = Regex.Match(privateKey, @"otpauth://([^/]+)/([^?]+)\?(.*)", RegexOptions.IgnoreCase);
      if (match.Success) {
        authType = match.Groups[1].Value.ToLower();
        var label = match.Groups[2].Value;
        var p = label.IndexOf(":");
        if (p != -1) {
          issuer = label.Substring(0, p);
          label = label.Substring(p + 1);
        }

        var qs = AuthHelper.ParseQueryString(match.Groups[3].Value);
        privateKey = qs["secret"] ?? privateKey;
        if (int.TryParse(qs["digits"], out var queryDigits) && queryDigits != 0) {
          digits = queryDigits;
        }

        if (qs["counter"] != null) {
          long.TryParse(qs["counter"], out counter);
        }

        issuer = qs["issuer"];
        if (string.IsNullOrEmpty(issuer) == false) {
          label = issuer + (string.IsNullOrEmpty(label) == false ? " (" + label + ")" : string.Empty);
        }

        serial = qs["serial"];
        if (string.IsNullOrEmpty(label) == false) {
          Authenticator.Name = nameField.Text = label;
        }

        var periods = qs["period"];
        if (string.IsNullOrEmpty(periods) == false) {
          int.TryParse(periods, out period);
        }

        if (qs["algorithm"] != null) {
          if (Enum.TryParse(qs["algorithm"], true, out hmac)) {
            hashField.SelectedItem = hmac.ToString();
          }
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
        if (authType == TOTP) {
          if (string.Compare(issuer, "BattleNet", true) == 0) {
            if (string.IsNullOrEmpty(serial)) {
              throw new ApplicationException("Battle.net Authenticator does not have a serial");
            }

            serial = serial.ToUpper();
            if (Regex.IsMatch(serial, @"^[A-Z]{2}-?[\d]{4}-?[\d]{4}-?[\d]{4}$") == false) {
              throw new ApplicationException("Invalid serial for Battle.net Authenticator");
            }

            auth = new BattleNetAuthenticator();
            ((BattleNetAuthenticator) auth).SecretKey = Base32.GetInstance().Decode(privateKey);
            ((BattleNetAuthenticator) auth).Serial = serial;

            issuer = string.Empty;
          }
          else {
            auth = new GoogleAuthenticator();
            ((GoogleAuthenticator) auth).Enroll(privateKey);
          }

          timer.Enabled = true;
          codeProgress.Visible = true;
          timeBasedRadio.Checked = true;
        }
        else if (authType == HOTP) {
          auth = new HotpAuthenticator();
          if (counterField.Text.Trim().Length != 0) {
            long.TryParse(counterField.Text.Trim(), out counter);
          }

          ((HotpAuthenticator) auth).Enroll(privateKey, counter); // start with the next code
          timer.Enabled = false;
          codeProgress.Visible = false;
          counterBasedRadio.Checked = true;
        }
        else {
          MainForm.ErrorDialog(Owner, "Only TOTP or HOTP authenticators are supported");
          return false;
        }

        auth.HmacType = hmac;
        auth.CodeDigits = digits;
        auth.Period = period;
        Authenticator.AuthenticatorData = auth;

        if (!string.IsNullOrEmpty(issuer)) {
          var detectedIssuer =
            AuthMain.AuthenticatorIcons.FirstOrDefault(i => i.Key.Equals(issuer, StringComparison.OrdinalIgnoreCase));
          Authenticator.Skin = detectedIssuer.Value;
        }

        if (digits > 5) {
          codeField.SpaceOut = digits / 2;
        }
        else {
          codeField.SpaceOut = 0;
        }

        //string key = Base32.getInstance().Encode(this.Authenticator.AuthenticatorData.SecretKey);
        codeField.Text = auth.CurrentCode;

        codeProgress.Maximum = period;

        if (!(auth is HotpAuthenticator) && auth.ServerTimeDiff == 0L && syncErrorWarned == false) {
          syncErrorWarned = true;
          MessageBox.Show(this,
            "Warning: unable to connect to Google to set time correctly.\nYour code may not be correct",
            AuthMain.APPLICATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      }
      catch (Exception irre) {
        MainForm.ErrorDialog(Owner, "Unable to create the authenticator. The secret code is probably invalid.", irre);
        return false;
      }

      return true;
    }

    #endregion
  }
}