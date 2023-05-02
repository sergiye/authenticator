using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Authenticator {
  public partial class ShowRestoreCodeForm : ResourceForm {
    public AuthAuthenticator CurrentAuthenticator { get; set; }

    public ShowRestoreCodeForm() {
      InitializeComponent();
    }

    private void btnOK_Click(object sender, EventArgs e) {
      Close();
    }

    private void ShowRestoreCodeForm_Load(object sender, EventArgs e) {
      var authenticator = CurrentAuthenticator.AuthenticatorData as BattleNetAuthenticator;

      serialNumberField.SecretMode = true;
      restoreCodeField.SecretMode = true;

      serialNumberField.Text = authenticator.Serial;
      restoreCodeField.Text = authenticator.RestoreCode;

      // if needed start a background thread to verify the restore code
      if (authenticator.RestoreCodeVerified == false) {
        var verify = new BackgroundWorker();
        verify.DoWork += VerifyRestoreCode;
        verify.RunWorkerCompleted += VerifyRestoreCodeCompleted;
        verify.RunWorkerAsync(CurrentAuthenticator.AuthenticatorData);
      }
    }

    void VerifyRestoreCodeCompleted(object sender, RunWorkerCompletedEventArgs e) {
      var message = e.Result as string;
      if (string.IsNullOrEmpty(message) == false) {
        MessageBox.Show(this, message, AuthMain.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    void VerifyRestoreCode(object sender, DoWorkEventArgs e) {
      var auth = e.Argument as BattleNetAuthenticator;

      // check if this authenticator is too old to be restored
      try {
        var testrestore = new BattleNetAuthenticator();
        testrestore.Restore(auth.Serial, auth.RestoreCode);
        auth.RestoreCodeVerified = true;
        e.Result = null;
      }
      catch (InvalidRestoreCodeException) {
        e.Result =
          "This authenticator was created before the restore capability existed and so the restore code will not work.\n\n"
          + "You will need to remove this authenticator from your Battle.net account and create a new one.";
      }
      catch (InvalidRestoreResponseException) {
        // ignore the validation if servers are down
      }
      catch (Exception ex2) {
        e.Result = "Oops. An error (" + ex2.Message + ") occured while validating your restore code.";
      }
    }

    private void allowCopyCheckBox_CheckedChanged(object sender, EventArgs e) {
      serialNumberField.SecretMode = !allowCopyCheckBox.Checked;
      restoreCodeField.SecretMode = !allowCopyCheckBox.Checked;
    }
  }
}