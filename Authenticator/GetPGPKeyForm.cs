using System;
using System.IO;
using System.Windows.Forms;

namespace Authenticator {
  public partial class GetPgpKeyForm : ResourceForm {

    public GetPgpKeyForm() {
      InitializeComponent();
    }

    public string PgpKey { get; private set; }

    public string Password { get; private set; }

    private void GetPGPKeyForm_Load(object sender, EventArgs e) {
      // force this window to the front and topmost
      // see: http://stackoverflow.com/questions/278237/keep-window-on-top-and-steal-focus-in-winforms
      var oldtopmost = TopMost;
      TopMost = true;
      TopMost = oldtopmost;
      Activate();
    }

    private void browseButton_Click(object sender, EventArgs e) {
      var ofd = new OpenFileDialog();
      ofd.CheckFileExists = true;
      ofd.Filter = "All Files (*.*)|*.*";
      ofd.Title = "Choose PGP Key File";

      if (ofd.ShowDialog(Parent) == DialogResult.OK) {
        pgpField.Text = File.ReadAllText(ofd.FileName);
      }
    }

    private void okButton_Click(object sender, EventArgs e) {
      // it isn't empty
      if (pgpField.Text.Length == 0) {
        DialogResult = DialogResult.None;
        return;
      }

      PgpKey = pgpField.Text;
      Password = passwordField.Text;
    }
  }
}