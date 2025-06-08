using System;
using System.Drawing;
using System.Windows.Forms;
using sergiye.Common;

namespace Authenticator {
  public partial class UnprotectPasswordForm : Form {
    public UnprotectPasswordForm() {
      InitializeComponent();
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
      Theme.Current.Apply(this);
    }

    public AuthAuthenticator Authenticator { get; set; }

    private void UnprotectPasswordForm_Load(object sender, EventArgs e) {
      Text = $"{Authenticator.Name} Password";

      // force this window to the front and topmost
      // see: http://stackoverflow.com/questions/278237/keep-window-on-top-and-steal-focus-in-winforms
      var topmost = TopMost;
      TopMost = true;
      TopMost = topmost;
      Activate();
    }

    private void okButton_Click(object sender, EventArgs e) {
      // it isn't empty
      var password = passwordField.Text;
      if (password.Length == 0) {
        invalidPasswordLabel.Text = "Please enter a password";
        invalidPasswordLabel.Visible = true;
        invalidPasswordTimer.Enabled = true;
        DialogResult = DialogResult.None;
        return;
      }

      // try to unprotect
      try {
        if (Authenticator.AuthenticatorData.Unprotect(password)) {
          Authenticator.MarkChanged();
        }
      }
      catch (BadPasswordException) {
        invalidPasswordLabel.Text = "Invalid password";
        invalidPasswordLabel.Visible = true;
        invalidPasswordTimer.Enabled = true;
        DialogResult = DialogResult.None;
      }
    }

    private void invalidPasswordTimer_Tick(object sender, EventArgs e) {
      invalidPasswordTimer.Enabled = false;
      invalidPasswordLabel.Visible = false;
    }
  }
}