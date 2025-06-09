using sergiye.Common;
using System;
using System.Windows.Forms;

namespace Authenticator {
  public partial class ShowTrionSecretForm : Form {
    public AuthAuthenticator CurrentAuthenticator { get; set; }

    public ShowTrionSecretForm() {
      InitializeComponent();
      Theme.Current.Apply(this);
    }

    private void ShowTrionSecretForm_Load(object sender, EventArgs e) {
      var authenticator = CurrentAuthenticator.AuthenticatorData as TrionAuthenticator;
      serialNumberField.Text = authenticator.Serial;
      deviceIdField.Text = authenticator.DeviceId;
    }
  }
}