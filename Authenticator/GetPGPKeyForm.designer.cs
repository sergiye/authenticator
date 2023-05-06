using System.Windows.Forms;

namespace Authenticator
{
	partial class GetPgpKeyForm
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
    private void InitializeComponent() {
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.pgpLabel = new System.Windows.Forms.Label();
      this.browseLabel = new System.Windows.Forms.Label();
      this.passwordLabel = new System.Windows.Forms.Label();
      this.browseButton = new System.Windows.Forms.Button();
      this.pgpField = new System.Windows.Forms.TextBox();
      this.passwordField = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(326, 368);
      this.okButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(112, 35);
      this.okButton.TabIndex = 2;
      this.okButton.Text = "OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(447, 368);
      this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(112, 35);
      this.cancelButton.TabIndex = 2;
      this.cancelButton.Text = "Cancel";
      // 
      // pgpLabel
      // 
      this.pgpLabel.AutoSize = true;
      this.pgpLabel.Location = new System.Drawing.Point(18, 14);
      this.pgpLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.pgpLabel.Name = "pgpLabel";
      this.pgpLabel.Size = new System.Drawing.Size(211, 20);
      this.pgpLabel.TabIndex = 4;
      this.pgpLabel.Text = "Enter or select your PGP key";
      // 
      // browseLabel
      // 
      this.browseLabel.AutoSize = true;
      this.browseLabel.Location = new System.Drawing.Point(18, 225);
      this.browseLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.browseLabel.Name = "browseLabel";
      this.browseLabel.Size = new System.Drawing.Size(162, 20);
      this.browseLabel.TabIndex = 4;
      this.browseLabel.Text = "Or, select your key file";
      // 
      // passwordLabel
      // 
      this.passwordLabel.AutoSize = true;
      this.passwordLabel.Location = new System.Drawing.Point(18, 289);
      this.passwordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.passwordLabel.Name = "passwordLabel";
      this.passwordLabel.Size = new System.Drawing.Size(195, 20);
      this.passwordLabel.TabIndex = 4;
      this.passwordLabel.Text = "If you key has a password ";
      // 
      // browseButton
      // 
      this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.browseButton.Location = new System.Drawing.Point(447, 217);
      this.browseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.browseButton.Name = "browseButton";
      this.browseButton.Size = new System.Drawing.Size(112, 35);
      this.browseButton.TabIndex = 2;
      this.browseButton.Text = "Browse...";
      this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
      // 
      // pgpField
      // 
      this.pgpField.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.pgpField.Location = new System.Drawing.Point(18, 48);
      this.pgpField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.pgpField.Multiline = true;
      this.pgpField.Name = "pgpField";
      this.pgpField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.pgpField.Size = new System.Drawing.Size(540, 156);
      this.pgpField.TabIndex = 1;
      this.pgpField.UseSystemPasswordChar = true;
      // 
      // passwordField
      // 
      this.passwordField.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.passwordField.Location = new System.Drawing.Point(18, 323);
      this.passwordField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.passwordField.Name = "passwordField";
      this.passwordField.PasswordChar = '●';
      this.passwordField.Size = new System.Drawing.Size(540, 26);
      this.passwordField.TabIndex = 1;
      this.passwordField.UseSystemPasswordChar = true;
      // 
      // GetPgpKeyForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(578, 422);
      this.Controls.Add(this.pgpLabel);
      this.Controls.Add(this.browseLabel);
      this.Controls.Add(this.passwordLabel);
      this.Controls.Add(this.browseButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.pgpField);
      this.Controls.Add(this.passwordField);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "GetPgpKeyForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "PGP Key";
      this.Load += new System.EventHandler(this.GetPGPKeyForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

		#endregion

		private TextBox passwordField;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.TextBox pgpField;
		private System.Windows.Forms.Button browseButton;
		private Label passwordLabel;
		private System.Windows.Forms.Label browseLabel;
		private System.Windows.Forms.Label pgpLabel;
	}
}