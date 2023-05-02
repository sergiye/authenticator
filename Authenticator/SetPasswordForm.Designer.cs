﻿using System.Windows.Forms;

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
		private void InitializeComponent()
		{
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
      this.setPasswordLabel.Location = new System.Drawing.Point(12, 9);
      this.setPasswordLabel.Name = "setPasswordLabel";
      this.setPasswordLabel.Size = new System.Drawing.Size(275, 42);
      this.setPasswordLabel.TabIndex = 0;
      this.setPasswordLabel.Text = "strings.setPasswordLabel";
      // 
      // passwordField
      // 
      this.passwordField.Location = new System.Drawing.Point(12, 54);
      this.passwordField.Name = "passwordField";
      this.passwordField.PasswordChar = '●';
      this.passwordField.Size = new System.Drawing.Size(277, 20);
      this.passwordField.TabIndex = 0;
      this.passwordField.UseSystemPasswordChar = true;
      // 
      // verifyField
      // 
      this.verifyField.Location = new System.Drawing.Point(12, 80);
      this.verifyField.Name = "verifyField";
      this.verifyField.PasswordChar = '●';
      this.verifyField.Size = new System.Drawing.Size(277, 20);
      this.verifyField.TabIndex = 1;
      this.verifyField.UseSystemPasswordChar = true;
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(214, 153);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 4;
      this.cancelButton.Text = "strings.Cancel";
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(133, 153);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 3;
      this.okButton.Text = "strings.OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // showCheckbox
      // 
      this.showCheckbox.AutoSize = true;
      this.showCheckbox.Location = new System.Drawing.Point(12, 117);
      this.showCheckbox.Name = "showCheckbox";
      this.showCheckbox.Size = new System.Drawing.Size(132, 17);
      this.showCheckbox.TabIndex = 2;
      this.showCheckbox.Text = "strings.showCheckbox";
      this.showCheckbox.CheckedChanged += new System.EventHandler(this.showCheckbox_CheckedChanged);
      // 
      // errorLabel
      // 
      this.errorLabel.ForeColor = System.Drawing.Color.Red;
      this.errorLabel.Location = new System.Drawing.Point(85, 118);
      this.errorLabel.Name = "errorLabel";
      this.errorLabel.Size = new System.Drawing.Size(215, 23);
      this.errorLabel.TabIndex = 5;
      this.errorLabel.Text = "strings.errorLabel";
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
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(307, 188);
      this.Controls.Add(this.errorLabel);
      this.Controls.Add(this.showCheckbox);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.verifyField);
      this.Controls.Add(this.passwordField);
      this.Controls.Add(this.setPasswordLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SetPasswordForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "SetPasswordForm";
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private Label setPasswordLabel;
		private TextBox passwordField;
		private TextBox verifyField;
		private Button cancelButton;
		private Button okButton;
		private CheckBox showCheckbox;
		private Label errorLabel;
		private System.Windows.Forms.Timer errorTimer;
	}
}