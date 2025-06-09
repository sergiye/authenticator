using System;
using System.Drawing;
using System.Windows.Forms;
using sergiye.Common;
using ZXing;

namespace Authenticator {
  public partial class ShowSecretKeyForm : Form {
    public AuthAuthenticator CurrentAuthenticator { get; set; }

    public ShowSecretKeyForm() {
      InitializeComponent();
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
      Theme.Current.Apply(this);
    }

    private void ShowSecretKeyForm_Load(object sender, EventArgs e) {

      secretKeyField.Text = Base32.GetInstance().Encode(CurrentAuthenticator.AuthenticatorData.SecretKey);

      //var type = CurrentAuthenticator.AuthenticatorData is HotpAuthenticator ? "hotp" : "totp";
      //var counter = CurrentAuthenticator.AuthenticatorData is HotpAuthenticator authenticator
      //  ? authenticator.Counter
      //  : 0;
      //var issuer = CurrentAuthenticator.AuthenticatorData.Issuer;
      //string url = "otpauth://" + type + "/" + AuthHelper.HtmlEncode(CurrentAuthenticator.Name)
      //	+ "?secret=" + key
      //	+ "&digits=" + CurrentAuthenticator.AuthenticatorData.CodeDigits
      //	+ (counter != 0 ? "&counter=" + counter : string.Empty)
      //	+ (string.IsNullOrEmpty(issuer) == false ? "&issuer=" + AuthHelper.HtmlEncode(issuer) : string.Empty);

      var url = CurrentAuthenticator.ToUrl(true);
      var writer = new BarcodeWriter {
        Format = BarcodeFormat.QR_CODE,
        Options = new ZXing.Common.EncodingOptions { Width = qrImage.Width, Height = qrImage.Height }
      };
      qrImage.Image = writer.Write(url);
    }
  }
}