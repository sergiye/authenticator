using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using Authenticator.Resources;
using ZXing;

namespace Authenticator {
  public partial class AddGoogleAuthenticator : ResourceForm {
    public AddGoogleAuthenticator() {
      InitializeComponent();
    }

    public AuthAuthenticator Authenticator { get; set; }

    private bool syncErrorWarned;

    #region Form Events

    private void AddGoogleAuthenticator_Load(object sender, EventArgs e) {
      nameField.Text = Authenticator.Name;
      codeField.SecretMode = true;
    }

    private void timer_Tick(object sender, EventArgs e) {
      if (Authenticator.AuthenticatorData != null && codeProgress.Visible) {
        var time = (int)(Authenticator.AuthenticatorData.ServerTime / 1000L) % 30;
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
          + "If you have added this authenticator to your online account, you will not be able to login in the future, and you need to click YES to save it." + Environment.NewLine + Environment.NewLine
          + "Do you want to save this authenticator?", MessageBoxButtons.YesNoCancel);
        if (result == DialogResult.Yes) {
          DialogResult = DialogResult.OK;
          return;
        }
        else if (result == DialogResult.Cancel) {
          DialogResult = DialogResult.None;
          return;
        }
      }
    }

    private void okButton_Click(object sender, EventArgs e) {
      var privatekey = secretCodeField.Text.Trim();
      if (privatekey.Length == 0) {
        MainForm.ErrorDialog(Owner, "Please enter the Secret Code");
        DialogResult = DialogResult.None;
        return;
      }
      var first = !codeProgress.Visible;
      if (VerifyAuthenticator(privatekey) == false) {
        DialogResult = DialogResult.None;
        return;
      }
      if (first) {
        DialogResult = DialogResult.None;
        return;
      }
    }

    private void verifyButton_Click(object sender, EventArgs e) {
      var privatekey = secretCodeField.Text.Trim();
      if (privatekey.Length == 0) {
        MainForm.ErrorDialog(Owner, "Please enter the Secret Code");
        return;
      }
      VerifyAuthenticator(privatekey);
    }

    private void iconRadioButton_CheckedChanged(object sender, EventArgs e) {
      if (((RadioButton)sender).Checked) {
        Authenticator.Skin = (string)((RadioButton)sender).Tag;
      }
    }

    private void icon1_Click(object sender, EventArgs e) {
      icon1RadioButton.Checked = true;
    }

    private void icon2_Click(object sender, EventArgs e) {
      icon2RadioButton.Checked = true;
    }

    private void icon3_Click(object sender, EventArgs e) {
      icon3RadioButton.Checked = true;
    }

    #endregion

    #region Private methods

    private bool IsValidFile(string filename) {
      try {
        // check path is valid
        new FileInfo(filename);
        return File.Exists(filename);
      }
      catch (Exception) { }

      return false;
    }

    private bool VerifyAuthenticator(string privatekey) {
      if (string.IsNullOrEmpty(privatekey)) {
        return false;
      }

      Authenticator.Name = nameField.Text;

      var authtype = "totp";

      // if this is a URL, pull it down
      Match match;
      if (Regex.IsMatch(privatekey, "https?://.*") && Uri.TryCreate(privatekey, UriKind.Absolute, out var uri)) {
        try {
          var request = (HttpWebRequest)WebRequest.Create(uri);
          request.AllowAutoRedirect = true;
          request.Timeout = 20000;
          request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
          using (var response = (HttpWebResponse)request.GetResponse()) {
            if (response.StatusCode == HttpStatusCode.OK && response.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) {
              using (var bitmap = (Bitmap)Image.FromStream(response.GetResponseStream())) {
                IBarcodeReader reader = new BarcodeReader();
                var result = reader.Decode(bitmap);
                if (result != null) {
                  privatekey = HttpUtility.UrlDecode(result.Text);
                }
              }
            }
          }
        }
        catch (Exception ex) {
          MainForm.ErrorDialog(Owner, "Cannot load QR code image from " + privatekey, ex);
          return false;
        }
      }
      else if ((match = Regex.Match(privatekey, @"data:image/([^;]+);base64,(.*)", RegexOptions.IgnoreCase)).Success) {
        var imagedata = Convert.FromBase64String(match.Groups[2].Value);
        using (var ms = new MemoryStream(imagedata)) {
          using (var bitmap = (Bitmap)Image.FromStream(ms)) {
            IBarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);
            if (result != null) {
              privatekey = HttpUtility.UrlDecode(result.Text);
            }
          }
        }
      }
      else if (IsValidFile(privatekey)) {
        // assume this is the image file
        using (var bitmap = (Bitmap)Image.FromFile(privatekey)) {
          IBarcodeReader reader = new BarcodeReader();
          var result = reader.Decode(bitmap);
          if (result != null) {
            privatekey = result.Text;
          }
        }
      }

      // check for otpauth://, e.g. "otpauth://totp/dc3bf64c-2fd4-40fe-a8cf-83315945f08b@blockchain.info?secret=IHZJDKAEEC774BMUK3GX6SA"
      match = Regex.Match(privatekey, @"otpauth://([^/]+)/([^?]+)\?(.*)", RegexOptions.IgnoreCase);
      if (match.Success) {
        authtype = match.Groups[1].Value; // @todo we only handle totp (not hotp)
        if (string.Compare(authtype, "totp", true) != 0) {
          MainForm.ErrorDialog(Owner, "Only time-based (TOTP) authenticators are supported when adding a Google Authenticator. Use the general \"Add Authenticator\" for counter-based (HOTP) authenticators.");
          return false;
        }

        var label = match.Groups[2].Value;
        if (string.IsNullOrEmpty(label) == false) {
          Authenticator.Name = nameField.Text = label;
        }

        var qs = AuthHelper.ParseQueryString(match.Groups[3].Value);
        privatekey = qs["secret"] ?? privatekey;
      }

      // just get the hex chars
      privatekey = Regex.Replace(privatekey, @"[^0-9a-z]", "", RegexOptions.IgnoreCase);
      if (privatekey.Length == 0) {
        MainForm.ErrorDialog(Owner, "The secret code is not valid");
        return false;
      }

      try {
        var auth = new GoogleAuthenticator();
        auth.Enroll(privatekey);
        Authenticator.AuthenticatorData = auth;

        codeProgress.Visible = true;

        var key = Base32.GetInstance().Encode(Authenticator.AuthenticatorData.SecretKey);
        secretCodeField.Text = Regex.Replace(key, ".{3}", "$0 ").Trim();
        codeField.Text = auth.CurrentCode;

        if (auth.ServerTimeDiff == 0L && syncErrorWarned == false) {
          syncErrorWarned = true;
          MessageBox.Show(this, string.Format(strings.AuthenticatorSyncError, "Google"), AuthMain.APPLICATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
