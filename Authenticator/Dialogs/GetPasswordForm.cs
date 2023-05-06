using System;
using System.Drawing;
using System.Windows.Forms;

namespace Authenticator {
  public partial class GetPasswordForm : Form {
    public GetPasswordForm() {
      InitializeComponent();
    }

    public string Password { get; private set; }

    public bool InvalidPassword { get; set; }

    private void GetPasswordForm_Load(object sender, EventArgs e) {
      // force this window to the front and topmost
      // see: http://stackoverflow.com/questions/278237/keep-window-on-top-and-steal-focus-in-winforms
      var topMost = TopMost;
      TopMost = true;
      TopMost = topMost;
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
      Activate();

      if (InvalidPassword) {
        invalidPasswordLabel.Text = "Invalid password";
        invalidPasswordLabel.Visible = true;
        invalidPasswordTimer.Enabled = true;
      }
    }

    private void okButton_Click(object sender, EventArgs e) {
      // it isn't empty
      var password = passwordField.Text;
      if (password.Length == 0) {
        invalidPasswordLabel.Text = "Please enter a password";
        invalidPasswordLabel.Visible = true;
        invalidPasswordTimer.Enabled = true;
        DialogResult = System.Windows.Forms.DialogResult.None;
        return;
      }

      Password = password;
    }

    private void invalidPasswordTimer_Tick(object sender, EventArgs e) {
      invalidPasswordTimer.Enabled = false;
      invalidPasswordLabel.Visible = false;
    }
  }
}