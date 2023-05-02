using System;
using Authenticator.Resources;

namespace Authenticator {
  /// <summary>
  /// Class for form that prompts for password and unprotects authenticator
  /// </summary>
  public partial class UnprotectPasswordForm : ResourceForm
	{
		/// <summary>
		/// Create new form
		/// </summary>
		public UnprotectPasswordForm() : base()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Authenticator to unprotect
		/// </summary>
		public AuthAuthenticator Authenticator { get; set; }

		/// <summary>
		/// Load the form and make it topmost
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UnprotectPasswordForm_Load(object sender, EventArgs e)
		{
			// window text is "{0} Password" 
			Text = string.Format(Text, Authenticator.Name);

			// force this window to the front and topmost
			// see: http://stackoverflow.com/questions/278237/keep-window-on-top-and-steal-focus-in-winforms
			var oldtopmost = TopMost;
			TopMost = true;
			TopMost = oldtopmost;
			Activate();
		}

		/// <summary>
		/// Click the OK button to unprotect the authenticator with given password
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			// it isn't empty
			var password = passwordField.Text;
			if (password.Length == 0)
			{
				invalidPasswordLabel.Text = strings.EnterPassword;
				invalidPasswordLabel.Visible = true;
				invalidPasswordTimer.Enabled = true;
				DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			// try to unprotect
			try
			{
				if (Authenticator.AuthenticatorData.Unprotect(password))
				{
					Authenticator.MarkChanged();
				}
			}
			catch (BadYubiKeyException)
			{
				invalidPasswordLabel.Text = "Please insert your YubiKey";
				invalidPasswordLabel.Visible = true;
				invalidPasswordTimer.Enabled = true;
				DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}
			catch (BadPasswordException)
			{
				invalidPasswordLabel.Text = strings.InvalidPassword;
				invalidPasswordLabel.Visible = true;
				invalidPasswordTimer.Enabled = true;
				DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}
		}

		/// <summary>
		/// Display error message for couple seconds
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void invalidPasswordTimer_Tick(object sender, EventArgs e)
		{
			invalidPasswordTimer.Enabled = false;
			invalidPasswordLabel.Visible = false;
		}

	}
}
