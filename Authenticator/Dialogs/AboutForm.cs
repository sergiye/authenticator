using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Authenticator {
  public partial class AboutForm : Form {
    public AuthConfig Config { get; set; }

    public AboutForm() {
      InitializeComponent();
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);

      using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Authenticator.LICENSE")) {
        if (stream == null) return;
        using (var s = new StreamReader(stream)) {
          richTextBox1.Text = s.ReadToEnd();
        }
      }
    }

    private void AboutForm_Load(object sender, EventArgs e) {
      // get the version of the application
      var version = Assembly.GetExecutingAssembly().GetName().Version;
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