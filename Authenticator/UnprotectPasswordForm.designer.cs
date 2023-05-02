using System.Windows.Forms;

namespace Authenticator
{
	partial class UnprotectPasswordForm
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
      this.passwordField.Location = new System.Drawing.Point(12, 21);
      this.passwordField.Name = "passwordField";
      this.passwordField.PasswordChar = '●';
      this.passwordField.Size = new System.Drawing.Size(277, 20);
      this.passwordField.TabIndex = 1;
      this.passwordField.UseSystemPasswordChar = true;
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(214, 85);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 2;
      this.cancelButton.Text = "strings.Cancel";
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(133, 85);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 2;
      this.okButton.Text = "strings.OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // invalidPasswordLabel
      // 
      this.invalidPasswordLabel.AutoSize = true;
      this.invalidPasswordLabel.ForeColor = System.Drawing.Color.Red;
      this.invalidPasswordLabel.Location = new System.Drawing.Point(12, 54);
      this.invalidPasswordLabel.Name = "invalidPasswordLabel";
      this.invalidPasswordLabel.Size = new System.Drawing.Size(117, 13);
      this.invalidPasswordLabel.TabIndex = 3;
      this.invalidPasswordLabel.Text = "strings.InvalidPassword";
      this.invalidPasswordLabel.Visible = false;
      // 
      // invalidPasswordTimer
      // 
      this.invalidPasswordTimer.Interval = 2000;
      this.invalidPasswordTimer.Tick += new System.EventHandler(this.invalidPasswordTimer_Tick);
      // 
      // UnprotectPasswordForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(299, 120);
      this.Controls.Add(this.invalidPasswordLabel);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.passwordField);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "UnprotectPasswordForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "_UnprotectPasswordForm_";
      this.Load += new System.EventHandler(this.UnprotectPasswordForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private TextBox passwordField;
		private Button cancelButton;
		private Button okButton;
		private Label invalidPasswordLabel;
		private System.Windows.Forms.Timer invalidPasswordTimer;
	}
}