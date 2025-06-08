using System;
using System.Drawing;
using System.Windows.Forms;
using sergiye.Common;

namespace Authenticator {
  public partial class SetPasswordForm : Form {
    public SetPasswordForm() {
      InitializeComponent();
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
      Theme.Current.Apply(this);
    }

    public string Password { get; protected set; }

    private void showCheckbox_CheckedChanged(object sender, EventArgs e) {
      if (showCheckbox.Checked) {
        passwordField.UseSystemPasswordChar = false;
        passwordField.PasswordChar = (char) 0;
        verifyField.UseSystemPasswordChar = false;
        verifyField.PasswordChar = (char) 0;
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
        errorLabel.Visible = true;
        errorTimer.Enabled = true;
        DialogResult = DialogResult.None;
        return;
      }

      Password = password;
    }

    private void errorTimer_Tick(object sender, EventArgs e) {
      errorTimer.Enabled = false;
      errorLabel.Visible = false;
    }
  }
}