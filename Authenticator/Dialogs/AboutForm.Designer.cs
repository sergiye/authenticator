using System.Windows.Forms;

namespace Authenticator {
	partial class AboutForm {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
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
      this.aboutLabel = new System.Windows.Forms.Label();
      this.licenseLabel = new System.Windows.Forms.Label();
      this.richTextBox1 = new System.Windows.Forms.RichTextBox();
      this.closeButton = new System.Windows.Forms.Button();
      this.siteLink = new System.Windows.Forms.LinkLabel();
      this.SuspendLayout();
      // 
      // aboutLabel
      // 
      this.aboutLabel.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.aboutLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
      this.aboutLabel.Location = new System.Drawing.Point(18, 14);
      this.aboutLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.aboutLabel.Name = "aboutLabel";
      this.aboutLabel.Size = new System.Drawing.Size(585, 51);
      this.aboutLabel.TabIndex = 2;
      this.aboutLabel.Text = "Authenticator {0}\r\nCopyright {1}. Sergiy Egoshin. All rights reserved.\r\n";
      // 
      // licenseLabel
      // 
      this.licenseLabel.AutoSize = true;
      this.licenseLabel.Location = new System.Drawing.Point(18, 115);
      this.licenseLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.licenseLabel.Name = "licenseLabel";
      this.licenseLabel.Size = new System.Drawing.Size(64, 20);
      this.licenseLabel.TabIndex = 3;
      this.licenseLabel.Text = "License";
      // 
      // richTextBox1
      // 
      this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.richTextBox1.Location = new System.Drawing.Point(22, 140);
      this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.ReadOnly = true;
      this.richTextBox1.Size = new System.Drawing.Size(579, 251);
      this.richTextBox1.TabIndex = 5;
      this.richTextBox1.Text = "";
      // 
      // closeButton
      // 
      this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.closeButton.Location = new System.Drawing.Point(491, 406);
      this.closeButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new System.Drawing.Size(112, 35);
      this.closeButton.TabIndex = 8;
      this.closeButton.Text = "Close";
      // 
      // siteLink
      // 
      this.siteLink.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.siteLink.Location = new System.Drawing.Point(22, 75);
      this.siteLink.Name = "siteLink";
      this.siteLink.Size = new System.Drawing.Size(581, 23);
      this.siteLink.TabIndex = 9;
      this.siteLink.TabStop = true;
      this.siteLink.Text = "https://github.com/sergiye/authenticator";
      this.siteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.siteLink_LinkClicked);
      // 
      // AboutForm
      // 
      this.AcceptButton = this.closeButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.CancelButton = this.closeButton;
      this.ClientSize = new System.Drawing.Size(621, 457);
      this.Controls.Add(this.siteLink);
      this.Controls.Add(this.closeButton);
      this.Controls.Add(this.richTextBox1);
      this.Controls.Add(this.licenseLabel);
      this.Controls.Add(this.aboutLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "About";
      this.Load += new System.EventHandler(this.AboutForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private System.Windows.Forms.LinkLabel siteLink;

    #endregion

		private System.Windows.Forms.Label aboutLabel;
		private System.Windows.Forms.Label licenseLabel;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Button closeButton;
	}
}