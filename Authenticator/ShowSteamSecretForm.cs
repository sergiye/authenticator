using System;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace Authenticator {
  public partial class ShowSteamSecretForm : ResourceForm {
    public AuthAuthenticator CurrentAuthenticator { get; set; }

    public ShowSteamSecretForm() {
      InitializeComponent();
    }

    private void btnOK_Click(object sender, EventArgs e) {
      Close();
    }

    private void ShowSteamSecretForm_Load(object sender, EventArgs e) {
      revocationcodeField.SecretMode = true;
      deviceidField.SecretMode = true;
      steamdataField.SecretMode = false;

      var authenticator = CurrentAuthenticator.AuthenticatorData as SteamAuthenticator;

      deviceidField.Text = authenticator.DeviceId;
      if (string.IsNullOrEmpty(authenticator.SteamData) == false && authenticator.SteamData[0] == '{') {
        revocationcodeField.Text =
          JObject.Parse(authenticator.SteamData).SelectToken("revocation_code").Value<string>();
        if (authenticator.SteamData.IndexOf("shared_secret") != -1) {
          steamdataField.SecretMode = true;
          steamdataField.Text = authenticator.SteamData;
          steamdataField.ForeColor = SystemColors.ControlText;
        }
      }
      else {
        revocationcodeField.Text = authenticator.SteamData;
      }
    }

    private void allowCopyCheckBox_CheckedChanged(object sender, EventArgs e) {
      revocationcodeField.SecretMode = !allowCopyCheckBox.Checked;
      deviceidField.SecretMode = !allowCopyCheckBox.Checked;
      steamdataField.SecretMode = !allowCopyCheckBox.Checked;
    }
  }
}