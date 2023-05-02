﻿using System.Windows.Forms;

namespace Authenticator
{
	partial class ShowRestoreCodeForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowRestoreCodeForm));
			this.allowCopyCheckBox = new CheckBox();
			this.serialNumberField = new SecretTextBox();
			this.label1 = new Label();
			this.restoreCodeField = new SecretTextBox();
			this.label2 = new Label();
			this.label4 = new Label();
			this.btnClose = new Button();
			this.SuspendLayout();
			// 
			// allowCopyCheckBox
			// 
			this.allowCopyCheckBox.AutoSize = true;
			this.allowCopyCheckBox.Location = new System.Drawing.Point(376, 274);
			this.allowCopyCheckBox.Name = "allowCopyCheckBox";
			this.allowCopyCheckBox.Size = new System.Drawing.Size(77, 17);
			this.allowCopyCheckBox.TabIndex = 4;
			this.allowCopyCheckBox.Text = "Allow copy";
			this.allowCopyCheckBox.CheckedChanged += new System.EventHandler(this.allowCopyCheckBox_CheckedChanged);
			// 
			// serialNumberField
			// 
			this.serialNumberField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.serialNumberField.Location = new System.Drawing.Point(146, 268);
			this.serialNumberField.Multiline = true;
			this.serialNumberField.Name = "serialNumberField";
			this.serialNumberField.SecretMode = false;
			this.serialNumberField.Size = new System.Drawing.Size(224, 30);
			this.serialNumberField.SpaceOut = 0;
			this.serialNumberField.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(23, 272);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(118, 19);
			this.label1.TabIndex = 1;
			this.label1.Text = "Serial Number";
			// 
			// restoreCodeField
			// 
			this.restoreCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.restoreCodeField.Location = new System.Drawing.Point(146, 305);
			this.restoreCodeField.Multiline = true;
			this.restoreCodeField.Name = "restoreCodeField";
			this.restoreCodeField.SecretMode = false;
			this.restoreCodeField.Size = new System.Drawing.Size(224, 30);
			this.restoreCodeField.SpaceOut = 0;
			this.restoreCodeField.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(23, 310);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(118, 19);
			this.label2.TabIndex = 1;
			this.label2.Text = "Restore Code";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(23, 60);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(451, 205);
			this.label4.TabIndex = 1;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(399, 353);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			// 
			// ShowRestoreCodeForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(497, 399);
			this.Controls.Add(this.allowCopyCheckBox);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.serialNumberField);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.restoreCodeField);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Name = "ShowRestoreCodeForm";
			this.ShowIcon = false;
			this.Text = "Battle.net Restore Code";
			this.Load += new System.EventHandler(this.ShowRestoreCodeForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Button btnClose;
		private Label label4;
		private SecretTextBox restoreCodeField;
		private Label label2;
		private SecretTextBox serialNumberField;
		private Label label1;
		private CheckBox allowCopyCheckBox;
	}
}