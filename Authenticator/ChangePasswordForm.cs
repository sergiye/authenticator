using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Authenticator.Resources;

namespace Authenticator {
  /// <summary>
  /// Form for setting the password and encryption for the current authenticators
  /// </summary>
  public partial class ChangePasswordForm : ResourceForm {
    /// <summary>
    /// Used to show a filled password box
    /// </summary>
    private const string EXISTING_PASSWORD = "******";

    /// <summary>
    /// Create the form
    /// </summary>
    public ChangePasswordForm() {
      InitializeComponent();
    }

    /// <summary>
    /// Current and new password type
    /// </summary>
    public Authenticator.PasswordTypes PasswordType { get; set; }

    /// <summary>
    /// Current and new password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// If have a current password
    /// </summary>
    public bool HasPassword { get; set; }

    /// <summary>
    /// List of seedwords
    /// </summary>
    private List<string> seedWords = new List<string>();

    /// <summary>
    /// Load the form and pretick checkboxes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangePasswordForm_Load(object sender, EventArgs e) {
      if ((PasswordType & Authenticator.PasswordTypes.Machine) != 0 ||
          (PasswordType & Authenticator.PasswordTypes.User) != 0) {
        machineCheckbox.Checked = true;
      }

      if ((PasswordType & Authenticator.PasswordTypes.User) != 0) {
        userCheckbox.Checked = true;
      }

      userCheckbox.Enabled = machineCheckbox.Checked;

      if ((PasswordType & Authenticator.PasswordTypes.Explicit) != 0) {
        passwordCheckbox.Checked = true;
        if (HasPassword) {
          passwordField.Text = EXISTING_PASSWORD;
          verifyField.Text = EXISTING_PASSWORD;
        }
      }
    }

    /// <summary>
    /// Form has been shown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangePasswordForm_Shown(object sender, EventArgs e) {
      // Buf in MetroFrame where focus is not set correcty during Load, so we do it here
      if (passwordField.Enabled) {
        passwordField.Focus();
      }
    }

    /// <summary>
    /// Machine encryption is ticked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void machineCheckbox_CheckedChanged(object sender, EventArgs e) {
      if (machineCheckbox.Checked == false) {
        userCheckbox.Checked = false;
      }

      userCheckbox.Enabled = machineCheckbox.Checked;
    }


    /// <summary>
    /// Password encrpytion is ticked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void passwordCheckbox_CheckedChanged(object sender, EventArgs e) {
      passwordField.Enabled = (passwordCheckbox.Checked);
      verifyField.Enabled = (passwordCheckbox.Checked);
      if (passwordCheckbox.Checked) {
        passwordField.Focus();
      }
    }

    /// <summary>
    /// OK button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void okButton_Click(object sender, EventArgs e) {
      // check password is set if requried
      if (passwordCheckbox.Checked && passwordField.Text.Trim().Length == 0) {
        MainForm.ErrorDialog(this, strings.EnterPassword);
        DialogResult = System.Windows.Forms.DialogResult.None;
        return;
      }

      if (passwordCheckbox.Checked && string.Compare(passwordField.Text.Trim(), verifyField.Text.Trim()) != 0) {
        MainForm.ErrorDialog(this, strings.PasswordsDontMatch);
        DialogResult = System.Windows.Forms.DialogResult.None;
        return;
      }

      // set the valid password type property
      PasswordType = Authenticator.PasswordTypes.None;
      Password = null;
      if (userCheckbox.Checked) {
        PasswordType |= Authenticator.PasswordTypes.User;
      }
      else if (machineCheckbox.Checked) {
        PasswordType |= Authenticator.PasswordTypes.Machine;
      }

      if (passwordCheckbox.Checked) {
        PasswordType |= Authenticator.PasswordTypes.Explicit;
        if (passwordField.Text != EXISTING_PASSWORD) {
          Password = passwordField.Text.Trim();
        }
      }
    }

    /// <summary>
    /// Get a new random seed
    /// </summary>
    /// <param name="wordCount">number of words in seed</param>
    /// <returns></returns>
    private string GetSeed(int wordCount) {
      if (seedWords.Count == 0) {
        using (var s = new StreamReader(Assembly.GetExecutingAssembly()
                 .GetManifestResourceStream("Authenticator.Resources.SeedWords.txt"))) {
          string line;
          while ((line = s.ReadLine()) != null) {
            if (line.Length != 0) {
              seedWords.Add(line);
            }
          }
        }
      }

      var words = new List<string>();
      var random = new RNGCryptoServiceProvider();
      var buffer = new byte[4];
      for (var i = 0; i < wordCount; i++) {
        random.GetBytes(buffer);
        var pos = (int) (BitConverter.ToUInt32(buffer, 0) % (uint) seedWords.Count());

        // check for duplicates
        var word = seedWords[pos].ToLower();
        if (words.IndexOf(word) != -1) {
          i--;
          continue;
        }

        words.Add(word);
      }

      return string.Join(" ", words.ToArray());
    }
  }
}