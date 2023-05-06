using System;
using System.Windows.Forms;

namespace Authenticator {
  public partial class AboutForm : Form {
    public AuthConfig Config { get; set; }

    public AboutForm() {
      InitializeComponent();
    }

    private void AboutForm_Load(object sender, EventArgs e) {
      // get the version of the application
      var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
      var debug = string.Empty;
#if DEBUG
      debug += " (DEBUG)";
#endif
      aboutLabel.Text = string.Format(aboutLabel.Text, version.ToString(3) + debug, DateTime.Today.Year);
    }

    private void closeButton_Click(object sender, EventArgs e) {
      Close();
    }
  }
}