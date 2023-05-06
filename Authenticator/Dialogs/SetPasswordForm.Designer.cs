using System.Windows.Forms;

namespace Authenticator
{
	partial class SetPasswordForm
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
      this.components = new System.ComponentModel.Container();
      this.setPasswordLabel = new System.Windows.Forms.Label();
      this.passwordField = new System.Windows.Forms.TextBox();
      this.verifyField = new System.Windows.Forms.TextBox();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.showCheckbox = new System.Windows.Forms.CheckBox();
      this.errorLabel = new System.Windows.Forms.Label();
      this.errorTimer = new System.Windows.Forms.Timer(this.components);
      this.SuspendLayout();
      // 
      // setPasswordLabel
      // 
      this.setPasswordLabel.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.setPasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
      this.setPasswordLabel.Location = new System.Drawing.Point(13, 14);
      this.setPasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.setPasswordLabel.Name = "setPasswordLabel";
      this.setPasswordLabel.Size = new System.Drawing.Size(393, 50);
      this.setPasswordLabel.TabIndex = 0;
      this.setPasswordLabel.Text = "Enter a password to protect this authenticator or leave blank to remove it";
      // 
      // passwordField
      // 
      this.passwordField.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.passwordField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
      this.passwordField.Location = new System.Drawing.Point(13, 69);
      this.passwordField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.passwordField.Name = "passwordField";
      this.passwordField.PasswordChar = '●';
      this.passwordField.Size = new System.Drawing.Size(390, 35);
      this.passwordField.TabIndex = 0;
      this.passwordField.UseSystemPasswordChar = true;
      // 
      // verifyField
      // 
      this.verifyField.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.verifyField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
      this.verifyField.Location = new System.Drawing.Point(13, 114);
      this.verifyField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.verifyField.Name = "verifyField";
      this.verifyField.PasswordChar = '●';
      this.verifyField.Size = new System.Drawing.Size(390, 35);
      this.verifyField.TabIndex = 1;
      this.verifyField.UseSystemPasswordChar = true;
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(291, 223);
      this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(112, 35);
      this.cancelButton.TabIndex = 4;
      this.cancelButton.Text = "Cancel";
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(170, 223);
      this.okButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(112, 35);
      this.okButton.TabIndex = 3;
      this.okButton.Text = "OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // showCheckbox
      // 
      this.showCheckbox.AutoSize = true;
      this.showCheckbox.Location = new System.Drawing.Point(13, 180);
      this.showCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.showCheckbox.Name = "showCheckbox";
      this.showCheckbox.Size = new System.Drawing.Size(75, 24);
      this.showCheckbox.TabIndex = 2;
      this.showCheckbox.Text = "Show";
      this.showCheckbox.CheckedChanged += new System.EventHandler(this.showCheckbox_CheckedChanged);
      // 
      // errorLabel
      // 
      this.errorLabel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.errorLabel.ForeColor = System.Drawing.Color.Red;
      this.errorLabel.Location = new System.Drawing.Point(96, 180);
      this.errorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.errorLabel.Name = "errorLabel";
      this.errorLabel.Size = new System.Drawing.Size(307, 22);
      this.errorLabel.TabIndex = 5;
      this.errorLabel.Text = "Your passwords do not match.";
      this.errorLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      this.errorLabel.Visible = false;
      // 
      // errorTimer
      // 
      this.errorTimer.Interval = 2000;
      this.errorTimer.Tick += new System.EventHandler(this.errorTimer_Tick);
      // 
      // SetPasswordForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(416, 272);
      this.Controls.Add(this.errorLabel);
      this.Controls.Add(this.showCheckbox);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.verifyField);
      this.Controls.Add(this.passwordField);
      this.Controls.Add(this.setPasswordLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SetPasswordForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Set Password";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

		#endregion

		private System.Windows.Forms.Label setPasswordLabel;
		private System.Windows.Forms.TextBox passwordField;
		private System.Windows.Forms.TextBox verifyField;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.CheckBox showCheckbox;
		private System.Windows.Forms.Label errorLabel;
		private System.Windows.Forms.Timer errorTimer;
	}
}