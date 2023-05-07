using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Authenticator {
  public partial class ExceptionForm : Form {
    public Exception Error { get; set; }

    public ExceptionForm(Exception error) {
      InitializeComponent();
      Error = error;
      TopMost = true;
    }

    private void ExceptionForm_Load(object sender, EventArgs e) {
      Icon = SystemIcons.Error;
      errorLabel.Text = string.Format(errorLabel.Text, Error != null ? Error.Message : "An unknown error occured");
      if (Error != null) dataText.Text = $"{Error.Message}\n\n{new StackTrace(Error)}";
    }
  }
}