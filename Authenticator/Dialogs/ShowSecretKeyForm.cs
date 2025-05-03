using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using sergiye.Common;
using ZXing;

namespace Authenticator {
  public partial class ShowSecretKeyForm : Form {
    public AuthAuthenticator CurrentAuthenticator { get; set; }

    public ShowSecretKeyForm() {
      InitializeComponent();
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
    }

    private void ShowSecretKeyForm_Load(object sender, EventArgs e) {
      secretKeyField.SecretMode = true;

      var key = Base32.GetInstance().Encode(CurrentAuthenticator.AuthenticatorData.SecretKey);
      secretKeyField.Text = Regex.Replace(key, ".{3}", "$0 ").Trim();

      var type = CurrentAuthenticator.AuthenticatorData is HotpAuthenticator ? "hotp" : "totp";
      var counter = (CurrentAuthenticator.AuthenticatorData is HotpAuthenticator
        ? ((HotpAuthenticator) CurrentAuthenticator.AuthenticatorData).Counter
        : 0);
      var issuer = CurrentAuthenticator.AuthenticatorData.Issuer;

      //string url = "otpauth://" + type + "/" + AuthHelper.HtmlEncode(CurrentAuthenticator.Name)
      //	+ "?secret=" + key
      //	+ "&digits=" + CurrentAuthenticator.AuthenticatorData.CodeDigits
      //	+ (counter != 0 ? "&counter=" + counter : string.Empty)
      //	+ (string.IsNullOrEmpty(issuer) == false ? "&issuer=" + AuthHelper.HtmlEncode(issuer) : string.Empty);
      var url = CurrentAuthenticator.ToUrl(true);

      var writer = new BarcodeWriter();
      writer.Format = BarcodeFormat.QR_CODE;
      writer.Options = new ZXing.Common.EncodingOptions {Width = qrImage.Width, Height = qrImage.Height};
      qrImage.Image = writer.Write(url);
    }

    private void allowCopyCheckBox_CheckedChanged(object sender, EventArgs e) {
      secretKeyField.SecretMode = !allowCopyCheckBox.Checked;

      var key = Base32.GetInstance().Encode(CurrentAuthenticator.AuthenticatorData.SecretKey);
      if (secretKeyField.SecretMode) {
        secretKeyField.Text = Regex.Replace(key, ".{3}", "$0 ").Trim();
      }
      else {
        secretKeyField.Text = key;
      }
    }
  }
}