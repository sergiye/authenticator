using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Authenticator {
  public partial class AddBattleNetAuthenticator : Form {
    public AddBattleNetAuthenticator() {
      InitializeComponent();
      BackColor = SystemColors.Window;
      StartPosition = FormStartPosition.CenterScreen;
    }

    public AuthAuthenticator Authenticator { get; set; }

    #region Form Events

    private void AddBattleNetAuthenticator_Load(object sender, EventArgs e) {
      nameField.Text = Authenticator.Name;

      newSerialNumberField.SecretMode = true;
      newLoginCodeField.SecretMode = true;
      newRestoreCodeField.SecretMode = true;
    }

    private void allowCopyNewButton_CheckedChanged(object sender, EventArgs e) {
      newSerialNumberField.SecretMode = !allowCopyNewButton.Checked;

      if (Authenticator != null && Authenticator.AuthenticatorData != null) {
        // Issue#122: remove dashes if copyable so can be pasted into Battle.net form
        if (allowCopyNewButton.Checked) {
          newSerialNumberField.Text =
            ((BattleNetAuthenticator) Authenticator.AuthenticatorData).Serial.Replace("-", "");
        }
        else {
          newSerialNumberField.Text = ((BattleNetAuthenticator) Authenticator.AuthenticatorData).Serial;
        }
      }

      newLoginCodeField.SecretMode = !allowCopyNewButton.Checked;
      newRestoreCodeField.SecretMode = !allowCopyNewButton.Checked;
    }

    private void newAuthenticatorTimer_Tick(object sender, EventArgs e) {
      if (Authenticator.AuthenticatorData != null && newAuthenticatorProgress.Visible) {
        var time = (int) (Authenticator.AuthenticatorData.ServerTime / 1000L) % 30;
        newAuthenticatorProgress.Value = time + 1;
        if (time == 0) {
          newLoginCodeField.Text = Authenticator.AuthenticatorData.CurrentCode;
        }
      }
    }

    private void cancelButton_Click(object sender, EventArgs e) {
      if (Authenticator.AuthenticatorData != null) {
        var result = MainForm.ConfirmDialog(Owner,
          "You have created a new authenticator. "
          + "If you have attached this authenticator to your account, you might not be able to login in the future." +
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
      if (VerifyAuthenticator() == false) {
        DialogResult = DialogResult.None;
      }
    }

    private void tabControl1_DrawItem(object sender, DrawItemEventArgs e) {
      var page = tabControl1.TabPages[e.Index];
      e.Graphics.FillRectangle(new SolidBrush(page.BackColor), e.Bounds);

      var paddedBounds = e.Bounds;
      var yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
      paddedBounds.Offset(1, yOffset);
      TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, page.ForeColor);
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

    private void iconRadioButton_CheckedChanged(object sender, EventArgs e) {
      if (((RadioButton) sender).Checked) {
        Authenticator.Skin = (string) ((RadioButton) sender).Tag;
      }
    }

    #endregion

    #region Private methods

    private bool VerifyAuthenticator() {
      Authenticator.Name = nameField.Text;

      if (tabControl1.SelectedIndex == 0) {
        if (Authenticator.AuthenticatorData == null) {
          MainForm.ErrorDialog(Owner, "You need to create an authenticator and attach it to your account");
          return false;
        }
      }
      else if (tabControl1.SelectedIndex == 1) {
        var serial = restoreSerialNumberField.Text.Trim();
        var restore = restoreRestoreCodeField.Text.Trim();
        if (serial.Length == 0 || restore.Length == 0) {
          MainForm.ErrorDialog(Owner, "Please enter the Serial number and Restore code");
          return false;
        }

        try {
          var authenticator = new BattleNetAuthenticator();
          authenticator.Restore(serial, restore);
          Authenticator.AuthenticatorData = authenticator;
        }
        catch (InvalidRestoreResponseException irre) {
          MainForm.ErrorDialog(Owner, "Unable to restore the authenticator: " + irre.Message, irre);
          return false;
        }
      }
      else if (tabControl1.SelectedIndex == 2) {
        var privatekey = importPrivateKeyField.Text.Trim();
        if (privatekey.Length == 0) {
          MainForm.ErrorDialog(Owner, "Please enter the Private key");
          return false;
        }

        // just get the hex chars
        privatekey = Regex.Replace(privatekey, @"0x", "", RegexOptions.IgnoreCase);
        privatekey = Regex.Replace(privatekey, @"[^0-9abcdef]", "", RegexOptions.IgnoreCase);
        if (privatekey.Length == 0 || privatekey.Length < 40) {
          MainForm.ErrorDialog(Owner,
            "The private key must be a sequence of at least 40 hexadecimal characters, e.g. 7B0BFA82... or 0x7B, 0x0B, 0xFA, 0x82, ...");
          return false;
        }

        try {
          var authenticator = new BattleNetAuthenticator();
          if (privatekey.Length == 40) // 20 bytes which is key only
          {
            authenticator.SecretKey = global::Authenticator.Authenticator.StringToByteArray(privatekey);
            authenticator.Serial = "US-Imported";
          }
          else {
            authenticator.SecretData = privatekey;
            if (string.IsNullOrEmpty(authenticator.Serial)) {
              authenticator.Serial = "US-Imported";
            }
          }

          authenticator.Sync();
          Authenticator.AuthenticatorData = authenticator;
        }
        catch (Exception irre) {
          MainForm.ErrorDialog(Owner, "Unable to import the authenticator. The private key is probably invalid.", irre);
          return false;
        }
      }

      return true;
    }

    private void ClearAuthenticator(bool showWarning = true) {
      if (Authenticator.AuthenticatorData != null && showWarning) {
        var result = MainForm.ConfirmDialog(Owner,
          "This will clear the authenticator you have just created. "
          + "If you have attached this authenticator to your account, you might not be able to login in the future." +
          Environment.NewLine + Environment.NewLine
          + "Are you sure you want to continue?");
        if (result != DialogResult.Yes) {
          return;
        }

        Authenticator.AuthenticatorData = null;
      }

      newAuthenticatorProgress.Visible = false;
      newAuthenticatorTimer.Enabled = false;
      newSerialNumberField.Text = string.Empty;
      newSerialNumberField.SecretMode = true;
      newLoginCodeField.Text = string.Empty;
      newLoginCodeField.SecretMode = true;
      newRestoreCodeField.Text = string.Empty;
      newRestoreCodeField.SecretMode = true;
      allowCopyNewButton.Checked = false;

      restoreSerialNumberField.Text = string.Empty;
      restoreRestoreCodeField.Text = string.Empty;

      importPrivateKeyField.Text = string.Empty;
    }

    private void enrollAuthenticatorButton_Click(object sender, EventArgs e) {
      do {
        try {
          newSerialNumberField.Text = "creating...";

          var authenticator = new BattleNetAuthenticator();
#if DEBUG
          authenticator.Enroll(System.Diagnostics.Debugger.IsAttached);
#else
					authenticator.Enroll();
#endif
          Authenticator.AuthenticatorData = authenticator;
          newSerialNumberField.Text = authenticator.Serial;
          newLoginCodeField.Text = authenticator.CurrentCode;
          newRestoreCodeField.Text = authenticator.RestoreCode;

          newAuthenticatorProgress.Visible = true;
          newAuthenticatorTimer.Enabled = true;

          return;
        }
        catch (InvalidEnrollResponseException iere) {
          if (MainForm.ErrorDialog(Owner, "An error occured while registering a new authenticator", iere,
                MessageBoxButtons.RetryCancel) != DialogResult.Retry) {
            break;
          }
        }
      } while (true);

      ClearAuthenticator(false);
    }

    #endregion
  }
}