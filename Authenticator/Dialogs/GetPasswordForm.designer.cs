using System.Windows.Forms;

namespace Authenticator
{
	partial class GetPasswordForm
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
      this.passwordField = new System.Windows.Forms.TextBox();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.invalidPasswordLabel = new System.Windows.Forms.Label();
      this.invalidPasswordTimer = new System.Windows.Forms.Timer(this.components);
      this.SuspendLayout();
      // 
      // passwordField
      // 
      this.passwordField.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.passwordField.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
      this.passwordField.Location = new System.Drawing.Point(13, 14);
      this.passwordField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.passwordField.Name = "passwordField";
      this.passwordField.PasswordChar = '●';
      this.passwordField.Size = new System.Drawing.Size(470, 39);
      this.passwordField.TabIndex = 1;
      this.passwordField.UseSystemPasswordChar = true;
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(371, 107);
      this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(112, 35);
      this.cancelButton.TabIndex = 2;
      this.cancelButton.Text = "Cancel";
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(249, 107);
      this.okButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(112, 35);
      this.okButton.TabIndex = 2;
      this.okButton.Text = "OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // invalidPasswordLabel
      // 
      this.invalidPasswordLabel.AutoSize = true;
      this.invalidPasswordLabel.ForeColor = System.Drawing.Color.Red;
      this.invalidPasswordLabel.Location = new System.Drawing.Point(13, 68);
      this.invalidPasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.invalidPasswordLabel.Name = "invalidPasswordLabel";
      this.invalidPasswordLabel.Size = new System.Drawing.Size(126, 20);
      this.invalidPasswordLabel.TabIndex = 3;
      this.invalidPasswordLabel.Text = "Invalid password";
      this.invalidPasswordLabel.Visible = false;
      // 
      // invalidPasswordTimer
      // 
      this.invalidPasswordTimer.Interval = 2000;
      this.invalidPasswordTimer.Tick += new System.EventHandler(this.invalidPasswordTimer_Tick);
      // 
      // GetPasswordForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(496, 156);
      this.Controls.Add(this.invalidPasswordLabel);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.passwordField);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "GetPasswordForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Enter Password";
      this.Load += new System.EventHandler(this.GetPasswordForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

		#endregion

		private System.Windows.Forms.TextBox passwordField;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Label invalidPasswordLabel;
		private System.Windows.Forms.Timer invalidPasswordTimer;
	}
}