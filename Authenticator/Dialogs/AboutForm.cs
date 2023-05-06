using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Authenticator {
  public partial class AboutForm : Form {
    public AuthConfig Config { get; set; }

    public AboutForm() {
      InitializeComponent();

      using (var s = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Authenticator.LICENSE"))) {
        richTextBox1.Text = s.ReadToEnd();
      }
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

    private void siteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      try {
        Process.Start(new ProcessStartInfo(siteLink.Text));
      }
      catch {
        //ignore
      }
    }
  }
}