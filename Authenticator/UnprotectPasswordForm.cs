using System;
using Authenticator.Resources;

namespace Authenticator {
  public partial class UnprotectPasswordForm : ResourceForm {
    public UnprotectPasswordForm() : base() {
      InitializeComponent();
    }

    public AuthAuthenticator Authenticator { get; set; }

    private void UnprotectPasswordForm_Load(object sender, EventArgs e) {
      // window text is "{0} Password" 
      Text = string.Format(Text, Authenticator.Name);

      // force this window to the front and topmost
      // see: http://stackoverflow.com/questions/278237/keep-window-on-top-and-steal-focus-in-winforms
      var oldtopmost = TopMost;
      TopMost = true;
      TopMost = oldtopmost;
      Activate();
    }

    private void okButton_Click(object sender, EventArgs e) {
      // it isn't empty
      var password = passwordField.Text;
      if (password.Length == 0) {
        invalidPasswordLabel.Text = strings.EnterPassword;
        invalidPasswordLabel.Visible = true;
        invalidPasswordTimer.Enabled = true;
        DialogResult = System.Windows.Forms.DialogResult.None;
        return;
      }

      // try to unprotect
      try {
        if (Authenticator.AuthenticatorData.Unprotect(password)) {
          Authenticator.MarkChanged();
        }
      }
      catch (BadYubiKeyException) {
        invalidPasswordLabel.Text = "Please insert your YubiKey";
        invalidPasswordLabel.Visible = true;
        invalidPasswordTimer.Enabled = true;
        DialogResult = System.Windows.Forms.DialogResult.None;
        return;
      }
      catch (BadPasswordException) {
        invalidPasswordLabel.Text = strings.InvalidPassword;
        invalidPasswordLabel.Visible = true;
        invalidPasswordTimer.Enabled = true;
        DialogResult = System.Windows.Forms.DialogResult.None;
        return;
      }
    }

    private void invalidPasswordTimer_Tick(object sender, EventArgs e) {
      invalidPasswordTimer.Enabled = false;
      invalidPasswordLabel.Visible = false;
    }
  }
}