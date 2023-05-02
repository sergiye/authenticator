using System.Windows.Forms;

namespace Authenticator
{
	partial class ShowTrionSecretForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowTrionSecretForm));
			this.allowCopyCheckBox = new CheckBox();
			this.serialNumberField = new SecretTextBox();
			this.label1 = new Label();
			this.deviceIdField = new SecretTextBox();
			this.label2 = new Label();
			this.label4 = new Label();
			this.btnClose = new Button();
			this.SuspendLayout();
			// 
			// allowCopyCheckBox
			// 
			this.allowCopyCheckBox.AutoSize = true;
			this.allowCopyCheckBox.Location = new System.Drawing.Point(156, 333);
			this.allowCopyCheckBox.Name = "allowCopyCheckBox";
			this.allowCopyCheckBox.Size = new System.Drawing.Size(82, 15);
			this.allowCopyCheckBox.TabIndex = 4;
			this.allowCopyCheckBox.Text = "Allow copy";
			this.allowCopyCheckBox.CheckedChanged += new System.EventHandler(this.allowCopyCheckBox_CheckedChanged);
			// 
			// serialNumberField
			// 
			this.serialNumberField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.serialNumberField.Location = new System.Drawing.Point(156, 261);
			this.serialNumberField.Multiline = true;
			this.serialNumberField.Name = "serialNumberField";
			this.serialNumberField.SecretMode = false;
			this.serialNumberField.Size = new System.Drawing.Size(326, 30);
			this.serialNumberField.SpaceOut = 0;
			this.serialNumberField.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(23, 265);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(118, 19);
			this.label1.TabIndex = 1;
			this.label1.Text = "Serial Number";
			// 
			// deviceIdField
			// 
			this.deviceIdField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.deviceIdField.Location = new System.Drawing.Point(156, 297);
			this.deviceIdField.Multiline = true;
			this.deviceIdField.Name = "deviceIdField";
			this.deviceIdField.SecretMode = false;
			this.deviceIdField.Size = new System.Drawing.Size(326, 30);
			this.deviceIdField.SpaceOut = 0;
			this.deviceIdField.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(23, 301);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(118, 19);
			this.label2.TabIndex = 1;
			this.label2.Text = "Device ID";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(23, 60);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(459, 186);
			this.label4.TabIndex = 1;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(407, 370);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			// 
			// ShowTrionSecretForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(505, 416);
			this.Controls.Add(this.allowCopyCheckBox);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.serialNumberField);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.deviceIdField);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Name = "ShowTrionSecretForm";
			this.ShowIcon = false;
			this.Text = "Restore Code";
			this.Load += new System.EventHandler(this.ShowTrionSecretForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Button btnClose;
		private Label label4;
		private SecretTextBox deviceIdField;
		private Label label2;
		private SecretTextBox serialNumberField;
		private Label label1;
		private CheckBox allowCopyCheckBox;
	}
}