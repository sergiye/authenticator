using System;

namespace Authenticator {
  /// <summary>
  /// Show the About form
  /// </summary>
  public partial class AboutForm : ResourceForm {
    /// <summary>
    /// Current config object
    /// </summary>
    public AuthConfig Config { get; set; }

    /// <summary>
    /// Create the form
    /// </summary>
    public AboutForm() {
      InitializeComponent();
    }

    /// <summary>
    /// Load the about form
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AboutForm_Load(object sender, EventArgs e) {
      // get the version of the application
      var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
      var debug = string.Empty;
#if DEBUG
      debug += " (DEBUG)";
#endif
      aboutLabel.Text = string.Format(aboutLabel.Text, version.ToString(3) + debug, DateTime.Today.Year);
    }

    /// <summary>
    /// Click the close button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void closeButton_Click(object sender, EventArgs e) {
      Close();
    }
  }
}