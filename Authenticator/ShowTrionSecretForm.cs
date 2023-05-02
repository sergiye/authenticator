using System;

namespace Authenticator
{
	/// <summary>
	/// Form display initialization confirmation.
	/// </summary>
	public partial class ShowTrionSecretForm : ResourceForm
	{
		/// <summary>
		/// Current authenticator
		/// </summary>
		public AuthAuthenticator CurrentAuthenticator { get; set; }

		/// <summary>
		/// Create a new form
		/// </summary>
		public ShowTrionSecretForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Click OK button to close form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Form loaded event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShowTrionSecretForm_Load(object sender, EventArgs e)
		{
			serialNumberField.SecretMode = true;
			deviceIdField.SecretMode = true;

			var authenticator = CurrentAuthenticator.AuthenticatorData as TrionAuthenticator;
			serialNumberField.Text = authenticator.Serial;
			deviceIdField.Text = authenticator.DeviceId;
		}

		/// <summary>
		/// Click the allow copy chekcbox
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void allowCopyCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			serialNumberField.SecretMode = !allowCopyCheckBox.Checked;
			deviceIdField.SecretMode = !allowCopyCheckBox.Checked;
		}

	}
}
