using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Authenticator {
  public partial class ExportForm : Form {
    public ExportForm() {
      InitializeComponent();
      BackColor = SystemColors.Window;
      StartPosition = FormStartPosition.CenterScreen;
    }

    public string Password { get; protected set; }

    public string PgpKey { get; protected set; }

    public string ExportFile { get; protected set; }

    private void passwordCheckbox_CheckedChanged(object sender, EventArgs e) {
      if (passwordCheckbox.Checked) {
        pgpCheckbox.Checked = false;
      }

      passwordField.Enabled = (passwordCheckbox.Checked);
      verifyField.Enabled = (passwordCheckbox.Checked);
      if (passwordCheckbox.Checked) {
        passwordField.Focus();
      }
    }

    private void pgpCheckbox_CheckedChanged(object sender, EventArgs e) {
      if (pgpCheckbox.Checked) {
        passwordCheckbox.Checked = false;
      }

      pgpField.Enabled = pgpCheckbox.Checked;
      if (pgpCheckbox.Checked) {
        pgpField.Focus();
      }
    }

    private void pgpBrowseButton_Click(object sender, EventArgs e) {
      var ofd = new OpenFileDialog();
      ofd.CheckFileExists = true;
      ofd.Filter = "All Files (*.*)|*.*";
      ofd.Title = "Choose PGP Key File";

      if (ofd.ShowDialog(Parent) == DialogResult.OK) {
        pgpField.Text = File.ReadAllText(ofd.FileName);
      }
    }

    private void browseButton_Click(object sender, EventArgs e) {
      var sfd = new SaveFileDialog();
      sfd.AddExtension = true;
      sfd.CheckPathExists = true;
      if (passwordCheckbox.Checked) {
        sfd.Filter = "Zip File (*.zip)|*.zip";
        sfd.FileName = "authenticator-" + DateTime.Today.ToString("yyyy-MM-dd") + ".zip";
      }
      else if (pgpCheckbox.Checked) {
        sfd.Filter = "PGP File (*.pgp)|*.pgp";
        sfd.FileName = "authenticator-" + DateTime.Today.ToString("yyyy-MM-dd") + ".pgp";
      }
      else {
        sfd.Filter = "Text File (*.txt)|*.txt|Zip File (*.zip)|*.zip|All Files (*.*)|*.*";
        sfd.FileName = "authenticator-" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
      }

      sfd.OverwritePrompt = true;
      if (sfd.ShowDialog(Parent) != DialogResult.OK) {
        return;
      }

      fileField.Text = sfd.FileName;
    }

    private void okButton_Click(object sender, EventArgs e) {
      // check password is set if required
      if (passwordCheckbox.Checked && passwordField.Text.Trim().Length == 0) {
        MainForm.ErrorDialog(this, "Please enter a password");
        DialogResult = DialogResult.None;
        return;
      }

      if (passwordCheckbox.Checked && string.Compare(passwordField.Text, verifyField.Text) != 0) {
        MainForm.ErrorDialog(this, "Passwords do not match");
        DialogResult = DialogResult.None;
        return;
      }

      if (pgpCheckbox.Checked && pgpField.Text.Length == 0) {
        MainForm.ErrorDialog(this, "Please enter a PGP key");
        DialogResult = DialogResult.None;
        return;
      }

      if (fileField.Text.Length == 0) {
        MainForm.ErrorDialog(this, "Select or enter a file");
        DialogResult = DialogResult.None;
        return;
      }

      // set the valid password type property
      ExportFile = fileField.Text;
      if (passwordCheckbox.Checked && passwordField.Text.Length != 0) {
        Password = passwordField.Text;
      }

      if (pgpCheckbox.Checked && pgpField.Text.Length != 0) {
        PgpKey = pgpField.Text;
      }
    }
  }
}