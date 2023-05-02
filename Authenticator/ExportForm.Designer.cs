using System.Windows.Forms;

namespace Authenticator
{
	partial class ExportForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.introLabel = new System.Windows.Forms.Label();
      this.passwordCheckbox = new System.Windows.Forms.CheckBox();
      this.pictureBox2 = new System.Windows.Forms.PictureBox();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.passwordField = new System.Windows.Forms.TextBox();
      this.verifyField = new System.Windows.Forms.TextBox();
      this.verifyFieldLabel = new System.Windows.Forms.Label();
      this.passwordFieldLabel = new System.Windows.Forms.Label();
      this.pgpCheckbox = new System.Windows.Forms.CheckBox();
      this.pgpBrowse = new System.Windows.Forms.Button();
      this.pgpField = new System.Windows.Forms.TextBox();
      this.orLabel = new System.Windows.Forms.Label();
      this.fileField = new System.Windows.Forms.TextBox();
      this.metroLabel1 = new System.Windows.Forms.Label();
      this.browseButton = new System.Windows.Forms.Button();
      this.metroLabel2 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
      this.SuspendLayout();
      // 
      // introLabel
      // 
      this.introLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.introLabel.Location = new System.Drawing.Point(23, 60);
      this.introLabel.Name = "introLabel";
      this.introLabel.Size = new System.Drawing.Size(484, 50);
      this.introLabel.TabIndex = 1;
      this.introLabel.Text = "This will export a text file with an authenticator per line in Google\'s KeyUriFor" +
    "mat that can be imported into Authenticator or some other authenticator applicat" +
    "ions.";
      // 
      // passwordCheckbox
      // 
      this.passwordCheckbox.AutoSize = true;
      this.passwordCheckbox.Location = new System.Drawing.Point(25, 112);
      this.passwordCheckbox.Name = "passwordCheckbox";
      this.passwordCheckbox.Size = new System.Drawing.Size(199, 17);
      this.passwordCheckbox.TabIndex = 0;
      this.passwordCheckbox.Text = "Protect with a password (zip file only)";
      this.passwordCheckbox.CheckedChanged += new System.EventHandler(this.passwordCheckbox_CheckedChanged);
      // 
      // pictureBox2
      // 
      this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox2.Image = global::Authenticator.Properties.Resources.BluePixel;
      this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.pictureBox2.Location = new System.Drawing.Point(24, 563);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new System.Drawing.Size(484, 1);
      this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox2.TabIndex = 3;
      this.pictureBox2.TabStop = false;
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(432, 580);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 6;
      this.cancelButton.Text = "Cancel";
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(351, 580);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 5;
      this.okButton.Text = "OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // passwordField
      // 
      this.passwordField.Enabled = false;
      this.passwordField.Location = new System.Drawing.Point(128, 143);
      this.passwordField.Name = "passwordField";
      this.passwordField.PasswordChar = '●';
      this.passwordField.Size = new System.Drawing.Size(262, 20);
      this.passwordField.TabIndex = 1;
      this.passwordField.UseSystemPasswordChar = true;
      // 
      // verifyField
      // 
      this.verifyField.Enabled = false;
      this.verifyField.Location = new System.Drawing.Point(128, 172);
      this.verifyField.Name = "verifyField";
      this.verifyField.PasswordChar = '●';
      this.verifyField.Size = new System.Drawing.Size(262, 20);
      this.verifyField.TabIndex = 2;
      this.verifyField.UseSystemPasswordChar = true;
      // 
      // verifyFieldLabel
      // 
      this.verifyFieldLabel.AutoSize = true;
      this.verifyFieldLabel.Location = new System.Drawing.Point(45, 173);
      this.verifyFieldLabel.Name = "verifyFieldLabel";
      this.verifyFieldLabel.Size = new System.Drawing.Size(33, 13);
      this.verifyFieldLabel.TabIndex = 5;
      this.verifyFieldLabel.Text = "Verify";
      // 
      // passwordFieldLabel
      // 
      this.passwordFieldLabel.AutoSize = true;
      this.passwordFieldLabel.Location = new System.Drawing.Point(45, 144);
      this.passwordFieldLabel.Name = "passwordFieldLabel";
      this.passwordFieldLabel.Size = new System.Drawing.Size(53, 13);
      this.passwordFieldLabel.TabIndex = 5;
      this.passwordFieldLabel.Text = "Password";
      // 
      // pgpCheckbox
      // 
      this.pgpCheckbox.AutoSize = true;
      this.pgpCheckbox.Location = new System.Drawing.Point(23, 212);
      this.pgpCheckbox.Name = "pgpCheckbox";
      this.pgpCheckbox.Size = new System.Drawing.Size(168, 17);
      this.pgpCheckbox.TabIndex = 0;
      this.pgpCheckbox.Text = "Protect with a PGP Public key";
      this.pgpCheckbox.CheckedChanged += new System.EventHandler(this.pgpCheckbox_CheckedChanged);
      // 
      // pgpBrowse
      // 
      this.pgpBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.pgpBrowse.Location = new System.Drawing.Point(73, 363);
      this.pgpBrowse.Name = "pgpBrowse";
      this.pgpBrowse.Size = new System.Drawing.Size(186, 23);
      this.pgpBrowse.TabIndex = 8;
      this.pgpBrowse.Text = "Browse for the PGP key file...";
      this.pgpBrowse.Click += new System.EventHandler(this.pgpBrowseButton_Click);
      // 
      // pgpField
      // 
      this.pgpField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pgpField.Enabled = false;
      this.pgpField.Location = new System.Drawing.Point(45, 237);
      this.pgpField.Multiline = true;
      this.pgpField.Name = "pgpField";
      this.pgpField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.pgpField.Size = new System.Drawing.Size(462, 113);
      this.pgpField.TabIndex = 7;
      // 
      // orLabel
      // 
      this.orLabel.AutoSize = true;
      this.orLabel.Location = new System.Drawing.Point(45, 363);
      this.orLabel.Name = "orLabel";
      this.orLabel.Size = new System.Drawing.Size(16, 13);
      this.orLabel.TabIndex = 5;
      this.orLabel.Text = "or";
      // 
      // fileField
      // 
      this.fileField.Location = new System.Drawing.Point(25, 467);
      this.fileField.Name = "fileField";
      this.fileField.Size = new System.Drawing.Size(401, 20);
      this.fileField.TabIndex = 9;
      // 
      // metroLabel1
      // 
      this.metroLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.metroLabel1.Location = new System.Drawing.Point(23, 414);
      this.metroLabel1.Name = "metroLabel1";
      this.metroLabel1.Size = new System.Drawing.Size(484, 50);
      this.metroLabel1.TabIndex = 1;
      this.metroLabel1.Text = "Select the file to be saved. This will be a \".zip\" file if you have used a passwo" +
    "rd, a .pgp file if you have used a PGP key, otherwise a plain .txt file";
      // 
      // browseButton
      // 
      this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.browseButton.Location = new System.Drawing.Point(432, 467);
      this.browseButton.Name = "browseButton";
      this.browseButton.Size = new System.Drawing.Size(75, 23);
      this.browseButton.TabIndex = 8;
      this.browseButton.Text = "Browse...";
      this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
      // 
      // metroLabel2
      // 
      this.metroLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.metroLabel2.Location = new System.Drawing.Point(23, 505);
      this.metroLabel2.Name = "metroLabel2";
      this.metroLabel2.Size = new System.Drawing.Size(484, 50);
      this.metroLabel2.TabIndex = 1;
      this.metroLabel2.Text = "WARNING: If your authenticators have their own password, you will be asked for ea" +
    "ch in turn. Cancelling any password will exclude it from the export.\r\n";
      // 
      // ExportForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(538, 626);
      this.Controls.Add(this.fileField);
      this.Controls.Add(this.browseButton);
      this.Controls.Add(this.pgpBrowse);
      this.Controls.Add(this.pgpField);
      this.Controls.Add(this.verifyField);
      this.Controls.Add(this.passwordField);
      this.Controls.Add(this.orLabel);
      this.Controls.Add(this.verifyFieldLabel);
      this.Controls.Add(this.passwordFieldLabel);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.pictureBox2);
      this.Controls.Add(this.pgpCheckbox);
      this.Controls.Add(this.passwordCheckbox);
      this.Controls.Add(this.metroLabel2);
      this.Controls.Add(this.metroLabel1);
      this.Controls.Add(this.introLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ExportForm";
      this.Text = "Export";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private Label introLabel;
		private CheckBox passwordCheckbox;
		private System.Windows.Forms.PictureBox pictureBox2;
		private Button cancelButton;
		private Button okButton;
		private TextBox passwordField;
		private TextBox verifyField;
		private Label verifyFieldLabel;
		private Label passwordFieldLabel;
		private CheckBox pgpCheckbox;
		private Button pgpBrowse;
		private TextBox pgpField;
		private Label orLabel;
		private TextBox fileField;
		private Label metroLabel1;
		private Button browseButton;
		private Label metroLabel2;

	}
}