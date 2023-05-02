using System.Windows.Forms;

namespace Authenticator
{
	partial class AboutForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
      this.aboutLabel = new System.Windows.Forms.Label();
      this.licenseLabel = new System.Windows.Forms.Label();
      this.richTextBox1 = new System.Windows.Forms.RichTextBox();
      this.closeButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // aboutLabel
      // 
      this.aboutLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.aboutLabel.Location = new System.Drawing.Point(12, 9);
      this.aboutLabel.Name = "aboutLabel";
      this.aboutLabel.Size = new System.Drawing.Size(391, 45);
      this.aboutLabel.TabIndex = 2;
      this.aboutLabel.Text = AuthMain.APPLICATION_TITLE + " {0}\r\nCopyright {1}. Sergiy Egoshin. All rights reserved.\r\n";
      // 
      // licenseLabel
      // 
      this.licenseLabel.AutoSize = true;
      this.licenseLabel.Location = new System.Drawing.Point(12, 54);
      this.licenseLabel.Name = "licenseLabel";
      this.licenseLabel.Size = new System.Drawing.Size(44, 13);
      this.licenseLabel.TabIndex = 3;
      this.licenseLabel.Text = "License";
      // 
      // richTextBox1
      // 
      this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.richTextBox1.Location = new System.Drawing.Point(15, 70);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.ReadOnly = true;
      this.richTextBox1.Size = new System.Drawing.Size(388, 258);
      this.richTextBox1.TabIndex = 5;
      this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
      // 
      // closeButton
      // 
      this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.closeButton.Location = new System.Drawing.Point(328, 336);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new System.Drawing.Size(75, 23);
      this.closeButton.TabIndex = 8;
      this.closeButton.Text = "Close";
      this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
      // 
      // AboutForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(415, 369);
      this.Controls.Add(this.closeButton);
      this.Controls.Add(this.richTextBox1);
      this.Controls.Add(this.licenseLabel);
      this.Controls.Add(this.aboutLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "About";
      this.Load += new System.EventHandler(this.AboutForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private Label aboutLabel;
		private Label licenseLabel;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private Button closeButton;
	}
}