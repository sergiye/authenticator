using System;
using System.Windows.Forms;

namespace Authenticator
{
	/// <summary>
	/// Form class for create a new Battle.net authenticator
	/// </summary>
	public partial class AddGuildWarsAuthenticator : ResourceForm
	{
		/// <summary>
		/// Form instantiation
		/// </summary>
		public AddGuildWarsAuthenticator()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Current authenticator
		/// </summary>
		public AuthAuthenticator Authenticator { get; set; }

#region Form Events

		/// <summary>
		/// Load the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddGuildWarsAuthenticator_Load(object sender, EventArgs e)
		{
			nameField.Text = Authenticator.Name;
			codeField.SecretMode = true;
		}

		/// <summary>
		/// Ticker event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void newAuthenticatorTimer_Tick(object sender, EventArgs e)
		{
			if (Authenticator.AuthenticatorData != null && newAuthenticatorProgress.Visible)
			{
				var time = (int)(Authenticator.AuthenticatorData.ServerTime / 1000L) % 30;
				newAuthenticatorProgress.Value = time + 1;
				if (time == 0)
				{
					codeField.Text = Authenticator.AuthenticatorData.CurrentCode;
				}
			}
		}

		private void allowCopyButton_CheckedChanged(object sender, EventArgs e)
		{
			codeField.SecretMode = !allowCopyButton.Checked;
		}

		/// <summary>
		/// Click to add the code
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void verifyAuthenticatorButton_Click(object sender, EventArgs e)
		{
			var privatekey = secretCodeField.Text.Trim();
			if (string.IsNullOrEmpty(privatekey))
			{
				MainForm.ErrorDialog(this, "Please enter the secret code");
				return;
			}

			VerifyAuthenticator(privatekey);
		}

		/// <summary>
		/// Click the cancel button and show a warning
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelButton_Click(object sender, EventArgs e)
		{
			if (Authenticator.AuthenticatorData != null)
			{
				var result = MainForm.ConfirmDialog(Owner,
					"WARNING: Your authenticator has not been saved." + Environment.NewLine + Environment.NewLine
					+ "If you have added this authenticator to your account, you will not be able to login in the future, and you need to click YES to save it." + Environment.NewLine + Environment.NewLine
					+ "Do you want to save this authenticator?", MessageBoxButtons.YesNoCancel);
				if (result == DialogResult.Yes)
				{
					DialogResult = DialogResult.OK;
					return;
				}
				else if (result == DialogResult.Cancel)
				{
					DialogResult = DialogResult.None;
					return;
				}
			}
		}

		/// <summary>
		/// Click the OK button to verify and add the authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			var privatekey = secretCodeField.Text.Trim();
			if (privatekey.Length == 0)
			{
				MainForm.ErrorDialog(Owner, "Please enter the Secret Code");
				DialogResult = DialogResult.None;
				return;
			}
			var first = !newAuthenticatorProgress.Visible;
			if (VerifyAuthenticator(privatekey) == false)
			{
				DialogResult = DialogResult.None;
				return;
			}
			if (first)
			{
				DialogResult = DialogResult.None;
				return;
			}
			if (Authenticator.AuthenticatorData == null)
			{
				MainForm.ErrorDialog(Owner, "Please enter the Secret Code and click Verify Authenticator");
				DialogResult = DialogResult.None;
				return;
			}
		}

		/// <summary>
		/// Select one of the icons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void iconRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (((RadioButton)sender).Checked)
			{
				Authenticator.Skin = (string)((RadioButton)sender).Tag;
			}
		}

		/// <summary>
		/// Click the icon1
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void icon1_Click(object sender, EventArgs e)
		{
			icon1RadioButton.Checked = true;
		}

		/// <summary>
		/// Click the icon2
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void icon2_Click(object sender, EventArgs e)
		{
			icon2RadioButton.Checked = true;
		}

#endregion

#region Private methods

		/// <summary>
		/// Verify and create the authenticator if needed
		/// </summary>
		/// <returns>true is successful</returns>
		private bool VerifyAuthenticator(string privatekey)
		{
			Authenticator.Name = nameField.Text;

			try
			{
				var authenticator = new GuildWarsAuthenticator();
				authenticator.Enroll(privatekey);
				Authenticator.AuthenticatorData = authenticator;
				Authenticator.Name = nameField.Text;

				codeField.Text = authenticator.CurrentCode;
				newAuthenticatorProgress.Visible = true;
				newAuthenticatorTimer.Enabled = true;
			}
			catch (Exception ex)
			{
				MainForm.ErrorDialog(Owner, "Unable to create the authenticator: " + ex.Message, ex);
				return false;
			}

			return true;
		}

#endregion

	}
}