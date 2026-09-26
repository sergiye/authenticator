using sergiye.Common;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using ZXing;

namespace Authenticator {
  internal partial class AddMicrosoftAuthenticator : Form {
    public AddMicrosoftAuthenticator() {
      InitializeComponent();
      BackColor = SystemColors.Window;
      StartPosition = FormStartPosition.CenterScreen;
      Theme.Current.Apply(this);
    }

    public AuthAuthenticator Authenticator { get; set; }

    #region Form Events

    private void AddMicrosoftAuthenticator_Load(object sender, EventArgs e) {
      nameField.Text = Authenticator.Name;
      Authenticator.Skin = (string) icon1RadioButton.Tag;
    }

    private void newAuthenticatorTimer_Tick(object sender, EventArgs e) {
      if (Authenticator.AuthenticatorData != null && newAuthenticatorProgress.Visible) {
        var time = (int) (global::Authenticator.Authenticator.CurrentTime / 1000L) % 30;
        newAuthenticatorProgress.Value = time + 1;
        if (time == 0) {
          codeField.Text = Authenticator.AuthenticatorData.CurrentCode;
        }
      }
    }

    private void verifyAuthenticatorButton_Click(object sender, EventArgs e) {
      var privatekey = secretCodeField.Text.Trim();
      if (string.IsNullOrEmpty(privatekey)) {
        MainForm.ErrorDialog(this, "Please enter the Secret Code");
        return;
      }

      VerifyAuthenticator(privatekey);
    }

    private void cancelButton_Click(object sender, EventArgs e) {
      if (Authenticator.AuthenticatorData != null) {
        var result = MainForm.ConfirmDialog(Owner,
          "WARNING: Your authenticator has not been saved." + Environment.NewLine + Environment.NewLine
          + "If you have added this authenticator to your account, you will not be able to log in later, and you need to click YES to save it." +
          Environment.NewLine + Environment.NewLine
          + "Do you want to save this authenticator?", MessageBoxButtons.YesNoCancel);
        if (result == DialogResult.Yes) {
          DialogResult = DialogResult.OK;
        }
        else if (result == DialogResult.Cancel) {
          DialogResult = DialogResult.None;
        }
      }
    }

    private void okButton_Click(object sender, EventArgs e) {
      var privatekey = secretCodeField.Text.Trim();
      if (privatekey.Length == 0) {
        MainForm.ErrorDialog(Owner, "Please enter the Secret Code");
        DialogResult = DialogResult.None;
        return;
      }

      var first = !newAuthenticatorProgress.Visible;
      // keep a name the user may have typed after verifying
      if (VerifyAuthenticator(privatekey, false) == false) {
        DialogResult = DialogResult.None;
        return;
      }

      if (first) {
        DialogResult = DialogResult.None;
        return;
      }

      if (Authenticator.AuthenticatorData == null) {
        MainForm.ErrorDialog(Owner, "Please enter the Secret Code and click Verify Authenticator");
        DialogResult = DialogResult.None;
      }
    }

    private void iconRadioButton_CheckedChanged(object sender, EventArgs e) {
      if (((RadioButton) sender).Checked) {
        Authenticator.Skin = (string) ((RadioButton) sender).Tag;
      }
    }

    private void icon1_Click(object sender, EventArgs e) {
      icon1RadioButton.Checked = true;
    }

    private void icon2_Click(object sender, EventArgs e) {
      icon2RadioButton.Checked = true;
    }

    private void icon3_Click(object sender, EventArgs e) {
      icon3RadioButton.Checked = true;
    }

    #endregion

    #region Private methods

    private bool VerifyAuthenticator(string privatekey, bool updateNameField = true) {
      if (string.IsNullOrEmpty(privatekey)) {
        return false;
      }

      Authenticator.Name = nameField.Text;

      var authtype = "totp";

      privatekey = AuthHelper.ReadQrCode(Owner, privatekey);
      if (privatekey == null) {
        return false;
      }

      Match match;
      // check for otpauth://, e.g. "otpauth://totp/dc3bf64c-2fd4-40fe-a8cf-83315945f08b@blockchain.info?secret=IHZJDKAEEC774BMUK3GX6SA"
      match = Regex.Match(privatekey, @"otpauth://([^/]+)/([^?]+)\?(.*)", RegexOptions.IgnoreCase);
      if (match.Success) {
        authtype = match.Groups[1].Value; // @todo we only handle totp (not hotp)
        if (string.Compare(authtype, "totp", true) != 0) {
          MainForm.ErrorDialog(Owner,
            "Only time-based (TOTP) authenticators are supported when adding a Google Authenticator. Use the general \"Add Authenticator\" for counter-based (HOTP) authenticators.");
          return false;
        }

        // the label is "issuer:account" and URL-encoded; decoded once before splitting, as in AddAuthenticator
        var label = Uri.UnescapeDataString(match.Groups[2].Value);
        var p = label.IndexOf(':');
        if (p != -1) {
          label = label.Substring(p + 1);
        }
        if (string.IsNullOrEmpty(label) == false && updateNameField) {
          Authenticator.Name = nameField.Text = label;
        }

        var qs = AuthHelper.ParseQueryString(match.Groups[3].Value);
        privatekey = qs["secret"] ?? privatekey;
      }

      // just get the hex chars
      privatekey = Regex.Replace(privatekey, @"[^0-9a-z]", "", RegexOptions.IgnoreCase);
      if (privatekey.Length == 0) {
        MainForm.ErrorDialog(Owner, "The secret code is not valid");
        return false;
      }

      try {
        var authenticator = new MicrosoftAuthenticator();
        authenticator.Enroll(privatekey);
        Authenticator.AuthenticatorData = authenticator;
        Authenticator.Name = nameField.Text;

        codeField.Text = authenticator.CurrentCode;
        newAuthenticatorProgress.Visible = true;
        newAuthenticatorTimer.Enabled = true;
      }
      catch (Exception ex) {
        MainForm.ErrorDialog(Owner, "Unable to create the authenticator: " + ex.Message, ex);
        return false;
      }

      return true;
    }

    #endregion
  }
}
