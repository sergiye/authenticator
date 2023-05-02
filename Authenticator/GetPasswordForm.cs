using System;
using Authenticator.Resources;

namespace Authenticator
{
	/// <summary>
	/// Class for form that prompts for password and unprotects authenticator
	/// </summary>
	public partial class GetPasswordForm : ResourceForm
	{
		/// <summary>
		/// Create new form
		/// </summary>
		public GetPasswordForm()
			: base()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; private set; }

		public bool InvalidPassword { get; set; }

		/// <summary>
		/// Load the form and make it topmost
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GetPasswordForm_Load(object sender, EventArgs e)
		{
			// force this window to the front and topmost
			// see: http://stackoverflow.com/questions/278237/keep-window-on-top-and-steal-focus-in-winforms
			var oldtopmost = TopMost;
			TopMost = true;
			TopMost = oldtopmost;
			Activate();

			if (InvalidPassword)
			{
				invalidPasswordLabel.Text = strings.InvalidPassword;
				invalidPasswordLabel.Visible = true;
				invalidPasswordTimer.Enabled = true;
			}
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

			Password = password;
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
