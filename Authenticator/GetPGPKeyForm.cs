using System;
using System.IO;
using System.Windows.Forms;

namespace Authenticator
{
	/// <summary>
	/// Class for form that prompts for password and unprotects authenticator
	/// </summary>
	public partial class GetPgpKeyForm : ResourceForm
	{
		/// <summary>
		/// Create new form
		/// </summary>
		public GetPgpKeyForm()
			: base()
		{
			InitializeComponent();
		}

		/// <summary>
		/// PGPKey
		/// </summary>
		public string PgpKey { get; private set; }

		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; private set; }

		/// <summary>
		/// Load the form and make it topmost
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GetPGPKeyForm_Load(object sender, EventArgs e)
		{
			// force this window to the front and topmost
			// see: http://stackoverflow.com/questions/278237/keep-window-on-top-and-steal-focus-in-winforms
			var oldtopmost = TopMost;
			TopMost = true;
			TopMost = oldtopmost;
			Activate();
		}

		/// <summary>
		/// Browse the PGP key
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void browseButton_Click(object sender, EventArgs e)
		{
			var ofd = new OpenFileDialog();
			ofd.CheckFileExists = true;
			ofd.Filter = "All Files (*.*)|*.*";
			ofd.Title = "Choose PGP Key File";

			if (ofd.ShowDialog(Parent) == DialogResult.OK)
			{
				pgpField.Text = File.ReadAllText(ofd.FileName);
			}
		}

		/// <summary>
		/// Click the OK button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			// it isn't empty
			if (pgpField.Text.Length == 0)
			{
				DialogResult = DialogResult.None;
				return;
			}

			PgpKey = pgpField.Text;
			Password = passwordField.Text;
		}

	}
}
