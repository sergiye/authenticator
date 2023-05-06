using System;
using System.Windows.Forms;

namespace Authenticator {
  public partial class ShowTrionSecretForm : Form {
    public AuthAuthenticator CurrentAuthenticator { get; set; }

    public ShowTrionSecretForm() {
      InitializeComponent();
    }

    private void ShowTrionSecretForm_Load(object sender, EventArgs e) {
      serialNumberField.SecretMode = true;
      deviceIdField.SecretMode = true;

      var authenticator = CurrentAuthenticator.AuthenticatorData as TrionAuthenticator;
      serialNumberField.Text = authenticator.Serial;
      deviceIdField.Text = authenticator.DeviceId;
    }

    private void allowCopyCheckBox_CheckedChanged(object sender, EventArgs e) {
      serialNumberField.SecretMode = !allowCopyCheckBox.Checked;
      deviceIdField.SecretMode = !allowCopyCheckBox.Checked;
    }
  }
}