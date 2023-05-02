using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Authenticator.Resources;

namespace Authenticator {
  public partial class ChangePasswordForm : ResourceForm {
    private const string EXISTING_PASSWORD = "******";

    public ChangePasswordForm() {
      InitializeComponent();
    }

    public Authenticator.PasswordTypes PasswordType { get; set; }

    public string Password { get; set; }

    public bool HasPassword { get; set; }

    private List<string> seedWords = new List<string>();

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

    private void ChangePasswordForm_Shown(object sender, EventArgs e) {
      // Buf in MetroFrame where focus is not set correcty during Load, so we do it here
      if (passwordField.Enabled) {
        passwordField.Focus();
      }
    }

    private void machineCheckbox_CheckedChanged(object sender, EventArgs e) {
      if (machineCheckbox.Checked == false) {
        userCheckbox.Checked = false;
      }

      userCheckbox.Enabled = machineCheckbox.Checked;
    }


    private void passwordCheckbox_CheckedChanged(object sender, EventArgs e) {
      passwordField.Enabled = (passwordCheckbox.Checked);
      verifyField.Enabled = (passwordCheckbox.Checked);
      if (passwordCheckbox.Checked) {
        passwordField.Focus();
      }
    }

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