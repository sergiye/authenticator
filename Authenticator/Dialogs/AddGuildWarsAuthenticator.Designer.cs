using System.Windows.Forms;

namespace Authenticator {
  partial class AddGuildWarsAuthenticator {
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddGuildWarsAuthenticator));
      this.newAuthenticatorProgress = new System.Windows.Forms.ProgressBar();
      this.codeField = new sergiye.Common.VCenteredTextBox();
      this.verifyAuthenticatorButton = new System.Windows.Forms.Button();
      this.secretCodeField = new System.Windows.Forms.TextBox();
      this.step8Label = new System.Windows.Forms.Label();
      this.step7Label = new System.Windows.Forms.Label();
      this.step1Label = new System.Windows.Forms.Label();
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.icon2RadioButton = new RadioButton();
      this.icon1RadioButton = new RadioButton();
      this.icon2 = new System.Windows.Forms.PictureBox();
      this.icon1 = new System.Windows.Forms.PictureBox();
      this.iconLabel = new System.Windows.Forms.Label();
      this.nameLabel = new System.Windows.Forms.Label();
      this.nameField = new System.Windows.Forms.TextBox();
      this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
      this.step6Label = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.icon2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.icon1)).BeginInit();
      this.SuspendLayout();
      // 
      // newAuthenticatorProgress
      // 
      this.newAuthenticatorProgress.Location = new System.Drawing.Point(72, 329);
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
      this.codeField.Location = new System.Drawing.Point(72, 301);
      this.codeField.Name = "codeField";
      this.codeField.Size = new System.Drawing.Size(158, 26);
      this.codeField.SpaceOut = 3;
      this.codeField.TabIndex = 8;
      // 
      // verifyAuthenticatorButton
      // 
      this.verifyAuthenticatorButton.Location = new System.Drawing.Point(72, 238);
      this.verifyAuthenticatorButton.Name = "verifyAuthenticatorButton";
      this.verifyAuthenticatorButton.Size = new System.Drawing.Size(159, 23);
      this.verifyAuthenticatorButton.TabIndex = 4;
      this.verifyAuthenticatorButton.Text = "Verify Authenticator";
      this.verifyAuthenticatorButton.Click += new System.EventHandler(this.verifyAuthenticatorButton_Click);
      // 
      // secretCodeField
      // 
      this.secretCodeField.AllowDrop = true;
      this.secretCodeField.CausesValidation = false;
      this.secretCodeField.Location = new System.Drawing.Point(12, 203);
      this.secretCodeField.Multiline = true;
      this.secretCodeField.Name = "secretCodeField";
      this.secretCodeField.Size = new System.Drawing.Size(428, 22);
      this.secretCodeField.TabIndex = 3;
      // 
      // step8Label
      // 
      this.step8Label.Location = new System.Drawing.Point(12, 358);
      this.step8Label.Name = "step8Label";
      this.step8Label.Size = new System.Drawing.Size(428, 56);
      this.step8Label.TabIndex = 1;
      this.step8Label.Text = "8. IMPORTANT: Write down your Secret Code and store it somewhere safe and secure." +
    " You will need it if you ever need to restore your authenticator.";
      // 
      // step7Label
      // 
      this.step7Label.Location = new System.Drawing.Point(12, 272);
      this.step7Label.Name = "step7Label";
      this.step7Label.Size = new System.Drawing.Size(428, 26);
      this.step7Label.TabIndex = 1;
      this.step7Label.Text = "7. Enter the following code to verify it is working";
      // 
      // step1Label
      // 
      this.step1Label.Location = new System.Drawing.Point(12, 125);
      this.step1Label.Name = "step1Label";
      this.step1Label.Size = new System.Drawing.Size(428, 75);
      this.step1Label.TabIndex = 1;
      this.step1Label.Text = resources.GetString("step1Label.Text");
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(285, 406);
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
      this.cancelButton.Location = new System.Drawing.Point(366, 406);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 7;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // icon2RadioButton
      // 
      this.icon2RadioButton.Location = new System.Drawing.Point(148, 79);
      this.icon2RadioButton.Name = "icon2RadioButton";
      this.icon2RadioButton.Size = new System.Drawing.Size(14, 13);
      this.icon2RadioButton.TabIndex = 2;
      this.icon2RadioButton.Tag = "ArenaNetIcon.png";
      this.icon2RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // icon1RadioButton
      // 
      this.icon1RadioButton.Checked = true;
      this.icon1RadioButton.Location = new System.Drawing.Point(72, 79);
      this.icon1RadioButton.Name = "icon1RadioButton";
      this.icon1RadioButton.Size = new System.Drawing.Size(14, 13);
      this.icon1RadioButton.TabIndex = 1;
      this.icon1RadioButton.TabStop = true;
      this.icon1RadioButton.Tag = "GuildWarsAuthenticatorIcon.png";
      this.icon1RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // icon2
      // 
      this.icon2.Image = global::Authenticator.Properties.Resources.ArenaNetIcon;
      this.icon2.Location = new System.Drawing.Point(168, 62);
      this.icon2.Name = "icon2";
      this.icon2.Size = new System.Drawing.Size(48, 48);
      this.icon2.TabIndex = 4;
      this.icon2.TabStop = false;
      this.icon2.Tag = "";
      this.icon2.Click += new System.EventHandler(this.icon2_Click);
      // 
      // icon1
      // 
      this.icon1.Image = global::Authenticator.Properties.Resources.GuildWarsAuthenticatorIcon;
      this.icon1.Location = new System.Drawing.Point(92, 62);
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
      this.iconLabel.Location = new System.Drawing.Point(12, 75);
      this.iconLabel.Name = "iconLabel";
      this.iconLabel.Size = new System.Drawing.Size(31, 13);
      this.iconLabel.TabIndex = 3;
      this.iconLabel.Text = "Icon:";
      // 
      // nameLabel
      // 
      this.nameLabel.AutoSize = true;
      this.nameLabel.Location = new System.Drawing.Point(12, 27);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new System.Drawing.Size(38, 13);
      this.nameLabel.TabIndex = 3;
      this.nameLabel.Text = "Name:";
      // 
      // nameField
      // 
      this.nameField.Location = new System.Drawing.Point(69, 26);
      this.nameField.Name = "nameField";
      this.nameField.Size = new System.Drawing.Size(371, 20);
      this.nameField.TabIndex = 0;
      // 
      // newAuthenticatorTimer
      // 
      this.newAuthenticatorTimer.Tick += new System.EventHandler(this.newAuthenticatorTimer_Tick);
      // 
      // step6Label
      // 
      this.step6Label.Location = new System.Drawing.Point(12, 240);
      this.step6Label.Name = "step6Label";
      this.step6Label.Size = new System.Drawing.Size(51, 25);
      this.step6Label.TabIndex = 11;
      this.step6Label.Text = "6. Click";
      // 
      // AddGuildWarsAuthenticator
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(453, 441);
      this.Controls.Add(this.step6Label);
      this.Controls.Add(this.icon2RadioButton);
      this.Controls.Add(this.newAuthenticatorProgress);
      this.Controls.Add(this.icon1RadioButton);
      this.Controls.Add(this.icon2);
      this.Controls.Add(this.icon1);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.iconLabel);
      this.Controls.Add(this.codeField);
      this.Controls.Add(this.nameLabel);
      this.Controls.Add(this.nameField);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.verifyAuthenticatorButton);
      this.Controls.Add(this.step1Label);
      this.Controls.Add(this.secretCodeField);
      this.Controls.Add(this.step7Label);
      this.Controls.Add(this.step8Label);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AddGuildWarsAuthenticator";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Guild Wars 2 Authenticator";
      this.Load += new System.EventHandler(this.AddGuildWarsAuthenticator_Load);
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
    private RadioButton icon2RadioButton;
    private RadioButton icon1RadioButton;
    private System.Windows.Forms.PictureBox icon2;
    private System.Windows.Forms.PictureBox icon1;
    private Label iconLabel;
    private Label nameLabel;
    private TextBox nameField;
    private Button verifyAuthenticatorButton;
    private Label step7Label;
    private System.Windows.Forms.ProgressBar newAuthenticatorProgress;
    private sergiye.Common.VCenteredTextBox codeField;
    private Label step8Label;
    private System.Windows.Forms.Timer newAuthenticatorTimer;
    private Label step6Label;
  }
}