using System;
using Authenticator.Resources;

namespace Authenticator {
  public partial class SetPasswordForm : ResourceForm {
    public SetPasswordForm() {
      InitializeComponent();
    }

    public string Password { get; protected set; }

    private void showCheckbox_CheckedChanged(object sender, EventArgs e) {
      if (showCheckbox.Checked) {
        passwordField.UseSystemPasswordChar = false;
        passwordField.PasswordChar = (char)0;
        verifyField.UseSystemPasswordChar = false;
        verifyField.PasswordChar = (char)0;
      }
      else {
        passwordField.UseSystemPasswordChar = true;
        passwordField.PasswordChar = '*';
        verifyField.UseSystemPasswordChar = true;
        verifyField.PasswordChar = '*';
      }
    }

    private void okButton_Click(object sender, EventArgs e) {
      var password = passwordField.Text.Trim();
      var verify = verifyField.Text.Trim();
      if (password != verify) {
        //MainForm.ErrorDialog(this, "Your passwords do not match.");
        errorLabel.Text = strings.PasswordsDontMatch;
        errorLabel.Visible = true;
        errorTimer.Enabled = true;
        DialogResult = System.Windows.Forms.DialogResult.None;
        return;
      }

      Password = password;
    }

    private void errorTimer_Tick(object sender, EventArgs e) {
      errorTimer.Enabled = false;
      errorLabel.Text = string.Empty;
      errorLabel.Visible = false;
    }
  }
}
