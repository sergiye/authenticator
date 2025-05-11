using System.Windows.Forms;

namespace Authenticator
{
	partial class AddBattleNetAuthenticator
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddBattleNetAuthenticator));
      this.newAuthenticatorProgress = new System.Windows.Forms.ProgressBar();
      this.allowCopyNewButton = new System.Windows.Forms.CheckBox();
      this.label6 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.enrollAuthenticatorButton = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.lnewLabel4 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.newLabel1 = new System.Windows.Forms.Label();
      this.restoreRestoreCodeField = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.restoreSerialNumberField = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.importPrivateKeyField = new System.Windows.Forms.TextBox();
      this.label11 = new System.Windows.Forms.Label();
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
      this.icon3RadioButton = new RadioButton();
      this.icon2RadioButton = new RadioButton();
      this.icon1RadioButton = new RadioButton();
      this.icon3 = new System.Windows.Forms.PictureBox();
      this.icon2 = new System.Windows.Forms.PictureBox();
      this.icon1 = new System.Windows.Forms.PictureBox();
      this.label10 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.nameField = new System.Windows.Forms.TextBox();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.newRestoreCodeField = new SecretTextBox();
      this.newLoginCodeField = new SecretTextBox();
      this.newSerialNumberField = new SecretTextBox();
      this.restoreAuthenticatorTab = new System.Windows.Forms.TabPage();
      this.importAuthenticatorTab = new System.Windows.Forms.TabPage();
      this.label13 = new System.Windows.Forms.Label();
      this.pictureBox2 = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.icon3)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon1)).BeginInit();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.restoreAuthenticatorTab.SuspendLayout();
      this.importAuthenticatorTab.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
      this.SuspendLayout();
      // 
      // newAuthenticatorProgress
      // 
      this.newAuthenticatorProgress.Location = new System.Drawing.Point(126, 246);
      this.newAuthenticatorProgress.Maximum = 30;
      this.newAuthenticatorProgress.Minimum = 1;
      this.newAuthenticatorProgress.Name = "newAuthenticatorProgress";
      this.newAuthenticatorProgress.Size = new System.Drawing.Size(158, 8);
      this.newAuthenticatorProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this.newAuthenticatorProgress.TabIndex = 5;
      this.newAuthenticatorProgress.Value = 1;
      this.newAuthenticatorProgress.Visible = false;
      // 
      // allowCopyNewButton
      // 
      this.allowCopyNewButton.AutoSize = true;
      this.allowCopyNewButton.Location = new System.Drawing.Point(290, 195);
      this.allowCopyNewButton.Name = "allowCopyNewButton";
      this.allowCopyNewButton.Size = new System.Drawing.Size(83, 17);
      this.allowCopyNewButton.TabIndex = 2;
      this.allowCopyNewButton.Text = "Allow copy?";
      this.allowCopyNewButton.CheckedChanged += new System.EventHandler(this.allowCopyNewButton_CheckedChanged);
      // 
      // label6
      // 
      this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(23, 262);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(75, 13);
      this.label6.TabIndex = 1;
      this.label6.Text = "Restore Code:";
      // 
      // label5
      // 
      this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(25, 222);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(64, 13);
      this.label5.TabIndex = 1;
      this.label5.Text = "Login Code:";
      // 
      // enrollAuthenticatorButton
      // 
      this.enrollAuthenticatorButton.Location = new System.Drawing.Point(126, 125);
      this.enrollAuthenticatorButton.Name = "enrollAuthenticatorButton";
      this.enrollAuthenticatorButton.Size = new System.Drawing.Size(166, 23);
      this.enrollAuthenticatorButton.TabIndex = 0;
      this.enrollAuthenticatorButton.Text = "Create Authenticator";
      this.enrollAuthenticatorButton.Click += new System.EventHandler(this.enrollAuthenticatorButton_Click);
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(23, 194);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(76, 13);
      this.label4.TabIndex = 1;
      this.label4.Text = "Serial Number:";
      // 
      // lnewLabel4
      // 
      this.lnewLabel4.Location = new System.Drawing.Point(7, 126);
      this.lnewLabel4.Name = "lnewLabel4";
      this.lnewLabel4.Size = new System.Drawing.Size(66, 25);
      this.lnewLabel4.TabIndex = 1;
      this.lnewLabel4.Text = "4.";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.Location = new System.Drawing.Point(7, 160);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(435, 25);
      this.label2.TabIndex = 1;
      this.label2.Text = "5. You will need to add the following details to your account.";
      // 
      // newLabel1
      // 
      this.newLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.newLabel1.Location = new System.Drawing.Point(7, 14);
      this.newLabel1.Name = "newLabel1";
      this.newLabel1.Size = new System.Drawing.Size(435, 98);
      this.newLabel1.TabIndex = 0;
      this.newLabel1.Text = resources.GetString("newLabel1.Text");
      // 
      // restoreRestoreCodeField
      // 
      this.restoreRestoreCodeField.Location = new System.Drawing.Point(119, 104);
      this.restoreRestoreCodeField.Name = "restoreRestoreCodeField";
      this.restoreRestoreCodeField.Size = new System.Drawing.Size(158, 20);
      this.restoreRestoreCodeField.TabIndex = 1;
      // 
      // label7
      // 
      this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(6, 73);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(76, 13);
      this.label7.TabIndex = 1;
      this.label7.Text = "Serial Number:";
      // 
      // restoreSerialNumberField
      // 
      this.restoreSerialNumberField.Location = new System.Drawing.Point(119, 73);
      this.restoreSerialNumberField.Name = "restoreSerialNumberField";
      this.restoreSerialNumberField.Size = new System.Drawing.Size(158, 20);
      this.restoreSerialNumberField.TabIndex = 0;
      // 
      // label8
      // 
      this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(6, 104);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(75, 13);
      this.label8.TabIndex = 1;
      this.label8.Text = "Restore Code:";
      // 
      // label9
      // 
      this.label9.Location = new System.Drawing.Point(6, 17);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(436, 53);
      this.label9.TabIndex = 1;
      this.label9.Text = "Enter your Serial Number and Restore Code from your previous authenticator.";
      // 
      // importPrivateKeyField
      // 
      this.importPrivateKeyField.Location = new System.Drawing.Point(88, 72);
      this.importPrivateKeyField.Name = "importPrivateKeyField";
      this.importPrivateKeyField.Size = new System.Drawing.Size(354, 20);
      this.importPrivateKeyField.TabIndex = 0;
      // 
      // label11
      // 
      this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(6, 72);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(64, 13);
      this.label11.TabIndex = 1;
      this.label11.Text = "Private Key:";
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(301, 480);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 2;
      this.okButton.Text = "OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(393, 480);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 3;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // newAuthenticatorTimer
      // 
      this.newAuthenticatorTimer.Interval = 500;
      this.newAuthenticatorTimer.Tick += new System.EventHandler(this.newAuthenticatorTimer_Tick);
      // 
      // icon3RadioButton
      // 
      this.icon3RadioButton.Location = new System.Drawing.Point(225, 77);
      this.icon3RadioButton.Name = "icon3RadioButton";
      this.icon3RadioButton.Size = new System.Drawing.Size(14, 13);
      this.icon3RadioButton.TabIndex = 3;
      this.icon3RadioButton.Tag = "DiabloIcon.png";
      this.icon3RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // icon2RadioButton
      // 
      this.icon2RadioButton.Location = new System.Drawing.Point(149, 77);
      this.icon2RadioButton.Name = "icon2RadioButton";
      this.icon2RadioButton.Size = new System.Drawing.Size(14, 13);
      this.icon2RadioButton.TabIndex = 2;
      this.icon2RadioButton.Tag = "WarcraftIcon.png";
      this.icon2RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // icon1RadioButton
      // 
      this.icon1RadioButton.Checked = true;
      this.icon1RadioButton.Location = new System.Drawing.Point(73, 74);
      this.icon1RadioButton.Name = "icon1RadioButton";
      this.icon1RadioButton.Size = new System.Drawing.Size(16, 16);
      this.icon1RadioButton.TabIndex = 1;
      this.icon1RadioButton.TabStop = true;
      this.icon1RadioButton.Tag = "BattleNetAuthenticatorIcon.png";
      this.icon1RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // icon3
      // 
      this.icon3.Image = global::Authenticator.Properties.Resources.DiabloIcon;
      this.icon3.Location = new System.Drawing.Point(245, 59);
      this.icon3.Name = "icon3";
      this.icon3.Size = new System.Drawing.Size(48, 48);
      this.icon3.TabIndex = 4;
      this.icon3.TabStop = false;
      this.icon3.Tag = "DiabloIcon.png";
      this.icon3.Click += new System.EventHandler(this.icon3_Click);
      // 
      // icon2
      // 
      this.icon2.Image = global::Authenticator.Properties.Resources.WarcraftIcon;
      this.icon2.Location = new System.Drawing.Point(169, 59);
      this.icon2.Name = "icon2";
      this.icon2.Size = new System.Drawing.Size(48, 48);
      this.icon2.TabIndex = 4;
      this.icon2.TabStop = false;
      this.icon2.Tag = "WarcraftIcon.png";
      this.icon2.Click += new System.EventHandler(this.icon2_Click);
      // 
      // icon1
      // 
      this.icon1.Image = global::Authenticator.Properties.Resources.BattleNetAuthenticatorIcon;
      this.icon1.Location = new System.Drawing.Point(93, 59);
      this.icon1.Name = "icon1";
      this.icon1.Size = new System.Drawing.Size(48, 48);
      this.icon1.TabIndex = 4;
      this.icon1.TabStop = false;
      this.icon1.Tag = "BattleNetAuthenticatorIcon.png";
      this.icon1.Click += new System.EventHandler(this.icon1_Click);
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(12, 71);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(31, 13);
      this.label10.TabIndex = 3;
      this.label10.Text = "Icon:";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(12, 22);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(38, 13);
      this.label12.TabIndex = 3;
      this.label12.Text = "Name:";
      // 
      // nameField
      // 
      this.nameField.Location = new System.Drawing.Point(73, 21);
      this.nameField.Name = "nameField";
      this.nameField.Size = new System.Drawing.Size(391, 20);
      this.nameField.TabIndex = 0;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Controls.Add(this.restoreAuthenticatorTab);
      this.tabControl1.Controls.Add(this.importAuthenticatorTab);
      this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
      this.tabControl1.Location = new System.Drawing.Point(12, 125);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(456, 337);
      this.tabControl1.TabIndex = 0;
      this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
      // 
      // tabPage1
      // 
      this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
      this.tabPage1.Controls.Add(this.newAuthenticatorProgress);
      this.tabPage1.Controls.Add(this.newLabel1);
      this.tabPage1.Controls.Add(this.allowCopyNewButton);
      this.tabPage1.Controls.Add(this.label2);
      this.tabPage1.Controls.Add(this.newRestoreCodeField);
      this.tabPage1.Controls.Add(this.lnewLabel4);
      this.tabPage1.Controls.Add(this.newLoginCodeField);
      this.tabPage1.Controls.Add(this.label4);
      this.tabPage1.Controls.Add(this.label6);
      this.tabPage1.Controls.Add(this.enrollAuthenticatorButton);
      this.tabPage1.Controls.Add(this.newSerialNumberField);
      this.tabPage1.Controls.Add(this.label5);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(448, 311);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "New Authenticator";
      // 
      // newRestoreCodeField
      // 
      this.newRestoreCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.newRestoreCodeField.Location = new System.Drawing.Point(126, 257);
      this.newRestoreCodeField.Multiline = true;
      this.newRestoreCodeField.Name = "newRestoreCodeField";
      this.newRestoreCodeField.SecretMode = false;
      this.newRestoreCodeField.Size = new System.Drawing.Size(158, 26);
      this.newRestoreCodeField.SpaceOut = 2;
      this.newRestoreCodeField.TabIndex = 4;
      // 
      // newLoginCodeField
      // 
      this.newLoginCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.newLoginCodeField.Location = new System.Drawing.Point(126, 218);
      this.newLoginCodeField.Multiline = true;
      this.newLoginCodeField.Name = "newLoginCodeField";
      this.newLoginCodeField.SecretMode = false;
      this.newLoginCodeField.Size = new System.Drawing.Size(158, 26);
      this.newLoginCodeField.SpaceOut = 4;
      this.newLoginCodeField.TabIndex = 3;
      // 
      // newSerialNumberField
      // 
      this.newSerialNumberField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.newSerialNumberField.Location = new System.Drawing.Point(126, 189);
      this.newSerialNumberField.Multiline = true;
      this.newSerialNumberField.Name = "newSerialNumberField";
      this.newSerialNumberField.SecretMode = false;
      this.newSerialNumberField.Size = new System.Drawing.Size(158, 26);
      this.newSerialNumberField.SpaceOut = 0;
      this.newSerialNumberField.TabIndex = 1;
      // 
      // restoreAuthenticatorTab
      // 
      this.restoreAuthenticatorTab.BackColor = System.Drawing.SystemColors.Control;
      this.restoreAuthenticatorTab.Controls.Add(this.restoreRestoreCodeField);
      this.restoreAuthenticatorTab.Controls.Add(this.label9);
      this.restoreAuthenticatorTab.Controls.Add(this.label7);
      this.restoreAuthenticatorTab.Controls.Add(this.label8);
      this.restoreAuthenticatorTab.Controls.Add(this.restoreSerialNumberField);
      this.restoreAuthenticatorTab.Location = new System.Drawing.Point(4, 22);
      this.restoreAuthenticatorTab.Name = "restoreAuthenticatorTab";
      this.restoreAuthenticatorTab.Padding = new System.Windows.Forms.Padding(3);
      this.restoreAuthenticatorTab.Size = new System.Drawing.Size(448, 311);
      this.restoreAuthenticatorTab.TabIndex = 1;
      this.restoreAuthenticatorTab.Text = "Restore Authenticator";
      // 
      // importAuthenticatorTab
      // 
      this.importAuthenticatorTab.BackColor = System.Drawing.SystemColors.Control;
      this.importAuthenticatorTab.Controls.Add(this.label13);
      this.importAuthenticatorTab.Controls.Add(this.importPrivateKeyField);
      this.importAuthenticatorTab.Controls.Add(this.label11);
      this.importAuthenticatorTab.Location = new System.Drawing.Point(4, 22);
      this.importAuthenticatorTab.Name = "importAuthenticatorTab";
      this.importAuthenticatorTab.Padding = new System.Windows.Forms.Padding(3);
      this.importAuthenticatorTab.Size = new System.Drawing.Size(448, 311);
      this.importAuthenticatorTab.TabIndex = 2;
      this.importAuthenticatorTab.Text = "Import Authenticator";
      // 
      // label13
      // 
      this.label13.Location = new System.Drawing.Point(6, 17);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(436, 55);
      this.label13.TabIndex = 2;
      this.label13.Text = "Enter the private key that has been exported from another authenticator program.";
      // 
      // pictureBox2
      // 
      this.pictureBox2.Image = global::Authenticator.Properties.Resources.BluePixel;
      this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.pictureBox2.Location = new System.Drawing.Point(16, 463);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new System.Drawing.Size(452, 1);
      this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox2.TabIndex = 5;
      this.pictureBox2.TabStop = false;
      // 
      // AddBattleNetAuthenticator
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(481, 515);
      this.Controls.Add(this.pictureBox2);
      this.Controls.Add(this.icon3RadioButton);
      this.Controls.Add(this.icon2RadioButton);
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.icon1RadioButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.icon3);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.icon2);
      this.Controls.Add(this.label12);
      this.Controls.Add(this.icon1);
      this.Controls.Add(this.nameField);
      this.Controls.Add(this.label10);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AddBattleNetAuthenticator";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Battle.net Authenticator";
      this.Load += new System.EventHandler(this.AddBattleNetAuthenticator_Load);
      ((System.ComponentModel.ISupportInitialize)(this.icon3)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon1)).EndInit();
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.restoreAuthenticatorTab.ResumeLayout(false);
      this.restoreAuthenticatorTab.PerformLayout();
      this.importAuthenticatorTab.ResumeLayout(false);
      this.importAuthenticatorTab.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private Label newLabel1;
		private SecretTextBox newRestoreCodeField;
		private SecretTextBox newLoginCodeField;
		private Label label6;
		private SecretTextBox newSerialNumberField;
		private Label label5;
		private Button enrollAuthenticatorButton;
		private Label label4;
		private Label lnewLabel4;
		private Label label2;
		private CheckBox allowCopyNewButton;
		private TextBox restoreRestoreCodeField;
		private Label label7;
		private TextBox restoreSerialNumberField;
		private Label label8;
		private Label label9;
		private TextBox importPrivateKeyField;
		private Label label11;
		private Button okButton;
		private Button cancelButton;
		private System.Windows.Forms.ProgressBar newAuthenticatorProgress;
		private System.Windows.Forms.Timer newAuthenticatorTimer;
		private RadioButton icon3RadioButton;
		private RadioButton icon2RadioButton;
		private RadioButton icon1RadioButton;
		private System.Windows.Forms.PictureBox icon3;
		private System.Windows.Forms.PictureBox icon2;
		private System.Windows.Forms.PictureBox icon1;
		private Label label10;
		private Label label12;
		private TextBox nameField;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private TabPage restoreAuthenticatorTab;
		private TabPage importAuthenticatorTab;
		private Label label13;
		private System.Windows.Forms.PictureBox pictureBox2;
	}
}