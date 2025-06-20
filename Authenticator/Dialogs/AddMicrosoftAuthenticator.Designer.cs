using System.Windows.Forms;

namespace Authenticator
{
	partial class AddMicrosoftAuthenticator
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddMicrosoftAuthenticator));
      this.newAuthenticatorProgress = new System.Windows.Forms.ProgressBar();
      this.codeField = new sergiye.Common.VCenteredTextBox();
      this.verifyAuthenticatorButton = new System.Windows.Forms.Button();
      this.secretCodeField = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.step9Label = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.icon3RadioButton = new RadioButton();
      this.icon2RadioButton = new RadioButton();
      this.icon1RadioButton = new RadioButton();
      this.icon3 = new System.Windows.Forms.PictureBox();
      this.icon2 = new System.Windows.Forms.PictureBox();
      this.icon1 = new System.Windows.Forms.PictureBox();
      this.label10 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.nameField = new System.Windows.Forms.TextBox();
      this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
      this.step8Label = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.icon3)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon1)).BeginInit();
      this.SuspendLayout();
      // 
      // newAuthenticatorProgress
      // 
      this.newAuthenticatorProgress.Location = new System.Drawing.Point(87, 389);
      this.newAuthenticatorProgress.Maximum = 30;
      this.newAuthenticatorProgress.Minimum = 1;
      this.newAuthenticatorProgress.Name = "newAuthenticatorProgress";
      this.newAuthenticatorProgress.Size = new System.Drawing.Size(158, 8);
      this.newAuthenticatorProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this.newAuthenticatorProgress.TabIndex = 9;
      this.newAuthenticatorProgress.Value = 1;
      this.newAuthenticatorProgress.Visible = false;
      // 
      // codeField
      // 
      this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.codeField.Location = new System.Drawing.Point(87, 361);
      this.codeField.Name = "codeField";
      this.codeField.Size = new System.Drawing.Size(158, 26);
      this.codeField.SpaceOut = 3;
      this.codeField.TabIndex = 8;
      // 
      // verifyAuthenticatorButton
      // 
      this.verifyAuthenticatorButton.Location = new System.Drawing.Point(87, 291);
      this.verifyAuthenticatorButton.Name = "verifyAuthenticatorButton";
      this.verifyAuthenticatorButton.Size = new System.Drawing.Size(159, 23);
      this.verifyAuthenticatorButton.TabIndex = 5;
      this.verifyAuthenticatorButton.Text = "Verify Authenticator";
      this.verifyAuthenticatorButton.Click += new System.EventHandler(this.verifyAuthenticatorButton_Click);
      // 
      // secretCodeField
      // 
      this.secretCodeField.AllowDrop = true;
      this.secretCodeField.CausesValidation = false;
      this.secretCodeField.Location = new System.Drawing.Point(12, 262);
      this.secretCodeField.Name = "secretCodeField";
      this.secretCodeField.Size = new System.Drawing.Size(391, 20);
      this.secretCodeField.TabIndex = 4;
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(12, 406);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(391, 43);
      this.label3.TabIndex = 1;
      this.label3.Text = "10. IMPORTANT: Write down you Secret Code and store it somewhere safe and secure." +
    " You will need it if you ever need to restore your authenticator.";
      // 
      // step9Label
      // 
      this.step9Label.Location = new System.Drawing.Point(12, 332);
      this.step9Label.Name = "step9Label";
      this.step9Label.Size = new System.Drawing.Size(391, 26);
      this.step9Label.TabIndex = 1;
      this.step9Label.Text = "9. Enter the following code to verify it is working.";
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(12, 126);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(391, 133);
      this.label2.TabIndex = 1;
      this.label2.Text = resources.GetString("label2.Text");
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(253, 465);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 7;
      this.okButton.Text = "OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(334, 465);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 8;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // icon3RadioButton
      // 
      this.icon3RadioButton.AutoSize = true;
      this.icon3RadioButton.Location = new System.Drawing.Point(216, 71);
      this.icon3RadioButton.Name = "icon3RadioButton";
      this.icon3RadioButton.Size = new System.Drawing.Size(14, 13);
      this.icon3RadioButton.TabIndex = 3;
      this.icon3RadioButton.Tag = "Windows7Icon.png";
      this.icon3RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // icon2RadioButton
      // 
      this.icon2RadioButton.AutoSize = true;
      this.icon2RadioButton.Location = new System.Drawing.Point(143, 71);
      this.icon2RadioButton.Name = "icon2RadioButton";
      this.icon2RadioButton.Size = new System.Drawing.Size(14, 13);
      this.icon2RadioButton.TabIndex = 2;
      this.icon2RadioButton.Tag = "Windows8Icon.png";
      this.icon2RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // icon1RadioButton
      // 
      this.icon1RadioButton.AutoSize = true;
      this.icon1RadioButton.Checked = true;
      this.icon1RadioButton.Location = new System.Drawing.Point(67, 71);
      this.icon1RadioButton.Name = "icon1RadioButton";
      this.icon1RadioButton.Size = new System.Drawing.Size(14, 13);
      this.icon1RadioButton.TabIndex = 1;
      this.icon1RadioButton.TabStop = true;
      this.icon1RadioButton.Tag = "MicrosoftAuthenticatorIcon.png";
      this.icon1RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // icon3
      // 
      this.icon3.Image = global::Authenticator.Properties.Resources.Windows7Icon;
      this.icon3.Location = new System.Drawing.Point(236, 56);
      this.icon3.Name = "icon3";
      this.icon3.Size = new System.Drawing.Size(48, 48);
      this.icon3.TabIndex = 4;
      this.icon3.TabStop = false;
      this.icon3.Tag = "";
      this.icon3.Click += new System.EventHandler(this.icon3_Click);
      // 
      // icon2
      // 
      this.icon2.Image = global::Authenticator.Properties.Resources.Windows8Icon;
      this.icon2.Location = new System.Drawing.Point(163, 56);
      this.icon2.Name = "icon2";
      this.icon2.Size = new System.Drawing.Size(48, 48);
      this.icon2.TabIndex = 4;
      this.icon2.TabStop = false;
      this.icon2.Tag = "";
      this.icon2.Click += new System.EventHandler(this.icon2_Click);
      // 
      // icon1
      // 
      this.icon1.Image = global::Authenticator.Properties.Resources.MicrosoftAuthenticatorIcon;
      this.icon1.Location = new System.Drawing.Point(87, 56);
      this.icon1.Name = "icon1";
      this.icon1.Size = new System.Drawing.Size(48, 48);
      this.icon1.TabIndex = 4;
      this.icon1.TabStop = false;
      this.icon1.Tag = "";
      this.icon1.Click += new System.EventHandler(this.icon1_Click);
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(12, 67);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(31, 13);
      this.label10.TabIndex = 3;
      this.label10.Text = "Icon:";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(12, 19);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(38, 13);
      this.label12.TabIndex = 3;
      this.label12.Text = "Name:";
      // 
      // nameField
      // 
      this.nameField.Location = new System.Drawing.Point(66, 19);
      this.nameField.Name = "nameField";
      this.nameField.Size = new System.Drawing.Size(337, 20);
      this.nameField.TabIndex = 0;
      // 
      // newAuthenticatorTimer
      // 
      this.newAuthenticatorTimer.Tick += new System.EventHandler(this.newAuthenticatorTimer_Tick);
      // 
      // step8Label
      // 
      this.step8Label.Location = new System.Drawing.Point(12, 296);
      this.step8Label.Name = "step8Label";
      this.step8Label.Size = new System.Drawing.Size(51, 25);
      this.step8Label.TabIndex = 11;
      this.step8Label.Text = "8. Click.";
      // 
      // AddMicrosoftAuthenticator
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(421, 500);
      this.Controls.Add(this.step8Label);
      this.Controls.Add(this.icon3RadioButton);
      this.Controls.Add(this.newAuthenticatorProgress);
      this.Controls.Add(this.icon2RadioButton);
      this.Controls.Add(this.icon1RadioButton);
      this.Controls.Add(this.icon3);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.icon2);
      this.Controls.Add(this.codeField);
      this.Controls.Add(this.icon1);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.label10);
      this.Controls.Add(this.label12);
      this.Controls.Add(this.nameField);
      this.Controls.Add(this.verifyAuthenticatorButton);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.secretCodeField);
      this.Controls.Add(this.step9Label);
      this.Controls.Add(this.label3);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AddMicrosoftAuthenticator";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Microsoft Authenticator";
      this.Load += new System.EventHandler(this.AddMicrosoftAuthenticator_Load);
      ((System.ComponentModel.ISupportInitialize)(this.icon3)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private Label label2;
		private Button okButton;
		private Button cancelButton;
		private TextBox secretCodeField;
		private System.Windows.Forms.PictureBox icon2;
		private System.Windows.Forms.PictureBox icon1;
		private Label label10;
		private Label label12;
		private TextBox nameField;
		private Button verifyAuthenticatorButton;
		private Label step9Label;
		private System.Windows.Forms.ProgressBar newAuthenticatorProgress;
		private sergiye.Common.VCenteredTextBox codeField;
		private Label label3;
		private System.Windows.Forms.Timer newAuthenticatorTimer;
		private System.Windows.Forms.PictureBox icon3;
		private RadioButton icon2RadioButton;
		private RadioButton icon1RadioButton;
		private RadioButton icon3RadioButton;
		private Label step8Label;
	}
}