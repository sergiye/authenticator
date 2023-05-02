using System.Windows.Forms;

namespace Authenticator
{
	partial class AddGoogleAuthenticator
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
      this.secretCodeField = new System.Windows.Forms.TextBox();
      this.step1Label = new System.Windows.Forms.Label();
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.icon3RadioButton = new GroupMetroRadioButton();
      this.icon2RadioButton = new GroupMetroRadioButton();
      this.icon1RadioButton = new GroupMetroRadioButton();
      this.icon3 = new System.Windows.Forms.PictureBox();
      this.icon2 = new System.Windows.Forms.PictureBox();
      this.icon1 = new System.Windows.Forms.PictureBox();
      this.iconLabel = new System.Windows.Forms.Label();
      this.nameLabel = new System.Windows.Forms.Label();
      this.nameField = new System.Windows.Forms.TextBox();
      this.step2Label = new System.Windows.Forms.Label();
      this.verifyButton = new System.Windows.Forms.Button();
      this.codeProgress = new System.Windows.Forms.ProgressBar();
      this.codeField = new SecretTextBox();
      this.step3Label = new System.Windows.Forms.Label();
      this.timer = new System.Windows.Forms.Timer(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.icon3)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon1)).BeginInit();
      this.SuspendLayout();
      // 
      // secretCodeField
      // 
      this.secretCodeField.AllowDrop = true;
      this.secretCodeField.CausesValidation = false;
      this.secretCodeField.Location = new System.Drawing.Point(14, 186);
      this.secretCodeField.Name = "secretCodeField";
      this.secretCodeField.Size = new System.Drawing.Size(423, 20);
      this.secretCodeField.TabIndex = 4;
      // 
      // step1Label
      // 
      this.step1Label.Location = new System.Drawing.Point(12, 134);
      this.step1Label.Name = "step1Label";
      this.step1Label.Size = new System.Drawing.Size(425, 48);
      this.step1Label.TabIndex = 1;
      this.step1Label.Text = "1. Enter the Secret Code for your authenticator. Spaces don\'t matter. If you have" +
    " a QR code, you can paste the URL of the image instead.\r\n";
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(278, 363);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 6;
      this.okButton.Text = "OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(359, 363);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 7;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // icon3RadioButton
      // 
      this.icon3RadioButton.Group = "ICON";
      this.icon3RadioButton.Location = new System.Drawing.Point(218, 74);
      this.icon3RadioButton.Name = "icon3RadioButton";
      this.icon3RadioButton.Size = new System.Drawing.Size(14, 13);
      this.icon3RadioButton.TabIndex = 3;
      this.icon3RadioButton.Tag = "ChromeIcon.png";
      this.icon3RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // icon2RadioButton
      // 
      this.icon2RadioButton.Group = "ICON";
      this.icon2RadioButton.Location = new System.Drawing.Point(142, 74);
      this.icon2RadioButton.Name = "icon2RadioButton";
      this.icon2RadioButton.Size = new System.Drawing.Size(14, 13);
      this.icon2RadioButton.TabIndex = 2;
      this.icon2RadioButton.Tag = "GoogleIcon.png";
      this.icon2RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // icon1RadioButton
      // 
      this.icon1RadioButton.Checked = true;
      this.icon1RadioButton.Group = "ICON";
      this.icon1RadioButton.Location = new System.Drawing.Point(66, 74);
      this.icon1RadioButton.Name = "icon1RadioButton";
      this.icon1RadioButton.Size = new System.Drawing.Size(14, 13);
      this.icon1RadioButton.TabIndex = 1;
      this.icon1RadioButton.TabStop = true;
      this.icon1RadioButton.Tag = "GoogleAuthenticatorIcon.png";
      this.icon1RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // icon3
      // 
      this.icon3.Image = global::Authenticator.Properties.Resources.ChromeIcon;
      this.icon3.Location = new System.Drawing.Point(238, 57);
      this.icon3.Name = "icon3";
      this.icon3.Size = new System.Drawing.Size(48, 48);
      this.icon3.TabIndex = 4;
      this.icon3.TabStop = false;
      this.icon3.Tag = "";
      this.icon3.Click += new System.EventHandler(this.icon3_Click);
      // 
      // icon2
      // 
      this.icon2.Image = global::Authenticator.Properties.Resources.GoogleIcon;
      this.icon2.Location = new System.Drawing.Point(162, 57);
      this.icon2.Name = "icon2";
      this.icon2.Size = new System.Drawing.Size(48, 48);
      this.icon2.TabIndex = 4;
      this.icon2.TabStop = false;
      this.icon2.Tag = "";
      this.icon2.Click += new System.EventHandler(this.icon2_Click);
      // 
      // icon1
      // 
      this.icon1.Image = global::Authenticator.Properties.Resources.GoogleAuthenticatorIcon;
      this.icon1.Location = new System.Drawing.Point(86, 57);
      this.icon1.Name = "icon1";
      this.icon1.Size = new System.Drawing.Size(48, 48);
      this.icon1.TabIndex = 4;
      this.icon1.TabStop = false;
      this.icon1.Tag = "";
      this.icon1.Click += new System.EventHandler(this.icon1_Click);
      // 
      // iconLabel
      // 
      this.iconLabel.AutoSize = true;
      this.iconLabel.Location = new System.Drawing.Point(12, 71);
      this.iconLabel.Name = "iconLabel";
      this.iconLabel.Size = new System.Drawing.Size(31, 13);
      this.iconLabel.TabIndex = 3;
      this.iconLabel.Text = "Icon:";
      // 
      // nameLabel
      // 
      this.nameLabel.AutoSize = true;
      this.nameLabel.Location = new System.Drawing.Point(12, 23);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new System.Drawing.Size(38, 13);
      this.nameLabel.TabIndex = 3;
      this.nameLabel.Text = "Name:";
      // 
      // nameField
      // 
      this.nameField.Location = new System.Drawing.Point(66, 20);
      this.nameField.Name = "nameField";
      this.nameField.Size = new System.Drawing.Size(371, 20);
      this.nameField.TabIndex = 0;
      // 
      // step2Label
      // 
      this.step2Label.Location = new System.Drawing.Point(12, 228);
      this.step2Label.Name = "step2Label";
      this.step2Label.Size = new System.Drawing.Size(51, 25);
      this.step2Label.TabIndex = 10;
      this.step2Label.Text = "2. Click";
      // 
      // verifyButton
      // 
      this.verifyButton.Location = new System.Drawing.Point(128, 223);
      this.verifyButton.Name = "verifyButton";
      this.verifyButton.Size = new System.Drawing.Size(158, 23);
      this.verifyButton.TabIndex = 5;
      this.verifyButton.Text = "Verify Authenticator";
      this.verifyButton.Click += new System.EventHandler(this.verifyButton_Click);
      // 
      // codeProgress
      // 
      this.codeProgress.Location = new System.Drawing.Point(128, 325);
      this.codeProgress.Maximum = 30;
      this.codeProgress.Minimum = 1;
      this.codeProgress.Name = "codeProgress";
      this.codeProgress.Size = new System.Drawing.Size(158, 8);
      this.codeProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this.codeProgress.TabIndex = 13;
      this.codeProgress.Value = 1;
      this.codeProgress.Visible = false;
      // 
      // codeField
      // 
      this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.codeField.Location = new System.Drawing.Point(128, 297);
      this.codeField.Multiline = true;
      this.codeField.Name = "codeField";
      this.codeField.SecretMode = false;
      this.codeField.Size = new System.Drawing.Size(158, 26);
      this.codeField.SpaceOut = 3;
      this.codeField.TabIndex = 12;
      // 
      // step3Label
      // 
      this.step3Label.AutoSize = true;
      this.step3Label.Location = new System.Drawing.Point(12, 266);
      this.step3Label.Name = "step3Label";
      this.step3Label.Size = new System.Drawing.Size(237, 13);
      this.step3Label.TabIndex = 11;
      this.step3Label.Text = "3. Verify the following code matches your service";
      // 
      // timer
      // 
      this.timer.Enabled = true;
      this.timer.Interval = 500;
      this.timer.Tick += new System.EventHandler(this.timer_Tick);
      // 
      // AddGoogleAuthenticator
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(446, 398);
      this.Controls.Add(this.codeProgress);
      this.Controls.Add(this.codeField);
      this.Controls.Add(this.step3Label);
      this.Controls.Add(this.step2Label);
      this.Controls.Add(this.verifyButton);
      this.Controls.Add(this.icon3RadioButton);
      this.Controls.Add(this.icon2RadioButton);
      this.Controls.Add(this.icon1RadioButton);
      this.Controls.Add(this.icon3);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.icon2);
      this.Controls.Add(this.icon1);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.iconLabel);
      this.Controls.Add(this.secretCodeField);
      this.Controls.Add(this.nameLabel);
      this.Controls.Add(this.nameField);
      this.Controls.Add(this.step1Label);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AddGoogleAuthenticator";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Add Google Authenticator";
      this.Load += new System.EventHandler(this.AddGoogleAuthenticator_Load);
      ((System.ComponentModel.ISupportInitialize)(this.icon3)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private Label step1Label;
		private Button okButton;
		private Button cancelButton;
		private TextBox secretCodeField;
		private GroupMetroRadioButton icon3RadioButton;
		private GroupMetroRadioButton icon2RadioButton;
		private GroupMetroRadioButton icon1RadioButton;
		private System.Windows.Forms.PictureBox icon3;
		private System.Windows.Forms.PictureBox icon2;
		private System.Windows.Forms.PictureBox icon1;
		private Label iconLabel;
		private Label nameLabel;
		private TextBox nameField;
		private Label step2Label;
		private Button verifyButton;
		private System.Windows.Forms.ProgressBar codeProgress;
		private SecretTextBox codeField;
		private Label step3Label;
		private System.Windows.Forms.Timer timer;
	}
}