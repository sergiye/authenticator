using sergiye.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Authenticator {
  public partial class AddTrionAuthenticator : Form {
    public AddTrionAuthenticator() {
      InitializeComponent();
      BackColor = SystemColors.Window;
      StartPosition = FormStartPosition.CenterScreen;
      Theme.Current.Apply(this);
    }

    public AuthAuthenticator Authenticator { get; set; }

    #region Form Events

    private void AddTrionAuthenticator_Load(object sender, EventArgs e) {
      nameField.Text = Authenticator.Name;

      newSerialNumberField.SecretMode = true;
      newLoginCodeField.SecretMode = true;
      newRestoreCodeField.SecretMode = true;
    }

    private void allowCopyNewButton_CheckedChanged(object sender, EventArgs e) {
      newSerialNumberField.SecretMode = !allowCopyNewButton.Checked;
      //newLoginCodeField.SecretMode = !allowCopyNewButton.Checked;
      newRestoreCodeField.SecretMode = !allowCopyNewButton.Checked;
    }

    private void newAuthenticatorTimer_Tick(object sender, EventArgs e) {
      if (Authenticator != null && Authenticator.AuthenticatorData != null) {
        var time = (int) (Authenticator.AuthenticatorData.ServerTime / 1000L) % 30;
        if (time == 0) {
          newLoginCodeField.Text = Authenticator.AuthenticatorData.CurrentCode;
        }
      }
    }

    private void restoreGetQuestionsButton_Click(object sender, EventArgs e) {
      var email = restoreEmailField.Text.Trim();
      var password = restorePasswordField.Text.Trim();
      if (email.Length == 0 || password.Length == 0) {
        MainForm.ErrorDialog(Owner, "Please enter your account email and password");
        return;
      }

      try {
        TrionAuthenticator.SecurityQuestions(email, password, out var question1, out var question2);
        restoreQuestion1Label.Text = question1 ?? string.Empty;
        restoreQuestion2Label.Text = question2 ?? string.Empty;
      }
      catch (InvalidRestoreResponseException irre) {
        MainForm.ErrorDialog(Owner, irre.Message, irre);
      }
      catch (Exception ex) {
        MainForm.ErrorDialog(Owner, "Unable to access account: " + ex.Message, ex);
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

    private void iconRift_Click(object sender, EventArgs e) {
      riftIconRadioButton.Checked = true;
    }

    private void iconGlyph_Click(object sender, EventArgs e) {
      glyphIconRadioButton.Checked = true;
    }

    private void iconArcheAge_Click(object sender, EventArgs e) {
      archeageIconRadioButton.Checked = true;
    }

    private void trionIcon_Click(object sender, EventArgs e) {
      trionAuthenticatorRadioButton.Checked = true;
    }

    private void iconRadioButton_CheckedChanged(object sender, EventArgs e) {
      if (((RadioButton) sender).Checked) {
        Authenticator.Skin = (string) ((RadioButton) sender).Tag;
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

    #endregion

    #region Private methods

    private bool VerifyAuthenticator() {
      if (tabControl1.SelectedIndex == 0) {
        if (Authenticator.AuthenticatorData == null) {
          MainForm.ErrorDialog(Owner, "You need to create an authenticator and attach it to your account");
          return false;
        }
      }
      else {
        if (restoreQuestion1Label.Text.Length == 0) {
          restoreGetQuestionsButton_Click(null, null);
          return false;
        }

        var email = restoreEmailField.Text.Trim();
        var password = restorePasswordField.Text.Trim();
        var deviceId = restoreDeviceIdField.Text.Trim();
        var answer1 = restoreAnswer1Field.Text;
        var answer2 = restoreAnswer2Field.Text;
        if (deviceId.Length == 0 || (restoreQuestion2Label.Text.Length != 0 && answer1.Length == 0) ||
            (restoreQuestion2Label.Text.Length != 0 && answer2.Length == 0)) {
          MainForm.ErrorDialog(Owner, "Please enter the device ID and answers to your secret questions");
          return false;
        }

        try {
          var authenticator = new TrionAuthenticator();
          authenticator.Restore(email, password, deviceId, answer1, answer2);
          Authenticator.AuthenticatorData = authenticator;
        }
        catch (InvalidRestoreResponseException irre) {
          MainForm.ErrorDialog(Owner, "Unable to restore the authenticator: " + irre.Message, irre);
          return false;
        }
        catch (Exception ex) {
          MainForm.ErrorDialog(Owner, "An error occured restoring the authenticator: " + ex.Message, ex);
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

      newSerialNumberField.Text = string.Empty;
      newSerialNumberField.SecretMode = true;
      newLoginCodeField.Text = string.Empty;
      newLoginCodeField.SecretMode = true;
      newRestoreCodeField.Text = string.Empty;
      newRestoreCodeField.SecretMode = true;
      allowCopyNewButton.Checked = false;

      restoreEmailField.Text = string.Empty;
      restorePasswordField.Text = string.Empty;
      restoreDeviceIdField.Text = string.Empty;
      restoreQuestion1Label.Text = string.Empty;
      restoreQuestion2Label.Text = string.Empty;
      restoreAnswer1Field.Text = string.Empty;
      restoreAnswer2Field.Text = string.Empty;
    }

    private void enrollAuthenticatorButton_Click(object sender, EventArgs e) {
      do {
        try {
          newSerialNumberField.Text = "creating...";

          var authenticator = new TrionAuthenticator();
#if DEBUG
          authenticator.Enroll(System.Diagnostics.Debugger.IsAttached);
#else
					authenticator.Enroll();
#endif
          Authenticator.AuthenticatorData = authenticator;
          newSerialNumberField.Text = authenticator.Serial;
          newLoginCodeField.Text = authenticator.CurrentCode;
          newRestoreCodeField.Text = authenticator.DeviceId;

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