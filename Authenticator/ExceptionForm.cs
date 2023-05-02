using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using Authenticator.Resources;

namespace Authenticator {
  public partial class ExceptionForm : ResourceForm {
    public Exception ErrorException { get; set; }

    public AuthConfig Config { get; set; }

    public ExceptionForm() {
      InitializeComponent();
    }

    private void ExceptionForm_Load(object sender, EventArgs e) {
      errorIcon.Image = SystemIcons.Error.ToBitmap();
      Height = detailsButton.Top + detailsButton.Height + 45;

      errorLabel.Text = string.Format(errorLabel.Text, (ErrorException != null ? ErrorException.Message : strings.UnknownError));

      // build data
#if DEBUG
      dataText.Text = string.Format("{0}\n\n{1}", ErrorException.Message, new StackTrace(ErrorException));
#else
			try
			{
				dataText.Text = AuthHelper.PGPEncrypt(BuildDiagnostics(), AuthHelper.AUTH_PGP_PUBLICKEY);
			}
			catch (Exception ex)
			{
				dataText.Text = string.Format("{0}\n\n{1}", ex.Message, new System.Diagnostics.StackTrace(ex).ToString());
			}
#endif
    }

    private string BuildDiagnostics() {
      var diag = new StringBuilder();

      if (Version.TryParse(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion, out var version)) {
        diag.Append("Version:" + version.ToString(4));
      }

      // add log
      try {
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AuthMain.APPLICATION_NAME);
        if (Directory.Exists(dir)) {
          var log = Path.Combine(dir, "Authenticator.log");
          if (File.Exists(log)) {
            diag.Append("--AUTHENTICATOR.LOG--").Append(Environment.NewLine);
            diag.Append(File.ReadAllText(log)).Append(Environment.NewLine).Append(Environment.NewLine);
          }

          // add authenticator.xml
          foreach (var file in Directory.GetFiles(dir, "*.xml")) {
            diag.Append("--" + file + "--").Append(Environment.NewLine);
            diag.Append(File.ReadAllText(file)).Append(Environment.NewLine).Append(Environment.NewLine);
          }
        }
      }
      catch (Exception) { }

      // add the current config
      if (Config != null) {
        using (var ms = new MemoryStream()) {
          var settings = new XmlWriterSettings();
          settings.Indent = true;
          using (var xml = XmlWriter.Create(ms, settings)) {
            Config.WriteXmlString(xml);
          }

          ms.Position = 0;

          diag.Append("-- Config --").Append(Environment.NewLine);
          diag.Append(new StreamReader(ms).ReadToEnd()).Append(Environment.NewLine).Append(Environment.NewLine);
        }
      }

      // add the exception
      if (ErrorException != null) {
        diag.Append("--EXCEPTION--").Append(Environment.NewLine);

        var ex = ErrorException;
        while (ex != null) {
          diag.Append("Stack: ").Append(ex.Message).Append(Environment.NewLine).Append(new StackTrace(ex)).Append(Environment.NewLine);
          ex = ex.InnerException;
        }
        if (ErrorException is InvalidEncryptionException) {
          diag.Append("Plain: " + ((InvalidEncryptionException)ErrorException).Plain).Append(Environment.NewLine);
          diag.Append("Password: " + ((InvalidEncryptionException)ErrorException).Password).Append(Environment.NewLine);
          diag.Append("Encrypted: " + ((InvalidEncryptionException)ErrorException).Encrypted).Append(Environment.NewLine);
          diag.Append("Decrypted: " + ((InvalidEncryptionException)ErrorException).Decrypted).Append(Environment.NewLine);
        }
        else if (ErrorException is InvalidSecretDataException) {
          diag.Append("EncType: " + ((InvalidSecretDataException)ErrorException).EncType).Append(Environment.NewLine);
          diag.Append("Password: " + ((InvalidSecretDataException)ErrorException).Password).Append(Environment.NewLine);
          foreach (var data in ((InvalidSecretDataException)ErrorException).Decrypted) {
            diag.Append("Data: " + data).Append(Environment.NewLine);
          }
        }
      }

      return diag.ToString();
    }

    private void quitButton_Click(object sender, EventArgs e) {
      Close();
    }

    private void continueButton_Click(object sender, EventArgs e) {
      Close();
    }

    private void detailsButton_Click(object sender, EventArgs e) {
      dataText.Visible = !dataText.Visible;
      if (dataText.Visible) {
        detailsButton.Text = strings.HideDetails;
        Height += 160;
      }
      else {
        detailsButton.Text = strings._ExceptionForm_detailsButton_;
        Height -= 160;
      }
    }

  }
}
