using System.Windows.Forms;

namespace Authenticator {
  partial class AddOktaVerifyAuthenticator {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddOktaVerifyAuthenticator));
      this.newAuthenticatorProgress = new System.Windows.Forms.ProgressBar();
      this.codeField = new SecretTextBox();
      this.verifyAuthenticatorButton = new System.Windows.Forms.Button();
      this.secretCodeField = new System.Windows.Forms.TextBox();
      this.setupLabel = new System.Windows.Forms.Label();
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.iconRadioButton = new GroupRadioButton();
      this.oktaIcon = new System.Windows.Forms.PictureBox();
      this.iconLabel = new System.Windows.Forms.Label();
      this.nameLabel = new System.Windows.Forms.Label();
      this.nameField = new System.Windows.Forms.TextBox();
      this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
      this.step8Label = new System.Windows.Forms.Label();
      this.step9Label = new System.Windows.Forms.Label();
      this.linkLabel1 = new System.Windows.Forms.LinkLabel();
      this.label1 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.oktaIcon)).BeginInit();
      this.SuspendLayout();
      // 
      // newAuthenticatorProgress
      // 
      this.newAuthenticatorProgress.Location = new System.Drawing.Point(15, 351);
      this.newAuthenticatorProgress.Maximum = 30;
      this.newAuthenticatorProgress.Minimum = 1;
      this.newAuthenticatorProgress.Name = "newAuthenticatorProgress";
      this.newAuthenticatorProgress.Size = new System.Drawing.Size(158, 8);
      this.newAuthenticatorProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this.newAuthenticatorProgress.TabIndex = 0;
      this.newAuthenticatorProgress.Value = 1;
      this.newAuthenticatorProgress.Visible = false;
      // 
      // codeField
      // 
      this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.codeField.Location = new System.Drawing.Point(15, 323);
      this.codeField.Multiline = true;
      this.codeField.Name = "codeField";
      this.codeField.SecretMode = false;
      this.codeField.Size = new System.Drawing.Size(158, 26);
      this.codeField.SpaceOut = 3;
      this.codeField.TabIndex = 0;
      this.codeField.TabStop = false;
      // 
      // verifyAuthenticatorButton
      // 
      this.verifyAuthenticatorButton.Location = new System.Drawing.Point(278, 266);
      this.verifyAuthenticatorButton.Name = "verifyAuthenticatorButton";
      this.verifyAuthenticatorButton.Size = new System.Drawing.Size(159, 23);
      this.verifyAuthenticatorButton.TabIndex = 2;
      this.verifyAuthenticatorButton.Text = "Verify Authenticator";
      this.verifyAuthenticatorButton.Click += new System.EventHandler(this.verifyAuthenticatorButton_Click);
      // 
      // secretCodeField
      // 
      this.secretCodeField.AllowDrop = true;
      this.secretCodeField.CausesValidation = false;
      this.secretCodeField.Location = new System.Drawing.Point(15, 232);
      this.secretCodeField.Name = "secretCodeField";
      this.secretCodeField.Size = new System.Drawing.Size(423, 20);
      this.secretCodeField.TabIndex = 1;
      // 
      // setupLabel
      // 
      this.setupLabel.BackColor = System.Drawing.Color.Transparent;
      this.setupLabel.Location = new System.Drawing.Point(12, 129);
      this.setupLabel.Name = "setupLabel";
      this.setupLabel.Size = new System.Drawing.Size(425, 100);
      this.setupLabel.TabIndex = 0;
      this.setupLabel.Text = resources.GetString("setupLabel.Text");
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(287, 408);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 3;
      this.okButton.Text = "OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(368, 408);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 4;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // iconRadioButton
      // 
      this.iconRadioButton.AutoSize = true;
      this.iconRadioButton.Checked = true;
      this.iconRadioButton.Group = "ICON";
      this.iconRadioButton.Location = new System.Drawing.Point(67, 74);
      this.iconRadioButton.Name = "iconRadioButton";
      this.iconRadioButton.Size = new System.Drawing.Size(14, 13);
      this.iconRadioButton.TabIndex = 1;
      this.iconRadioButton.TabStop = true;
      // 
      // oktaIcon
      // 
      this.oktaIcon.Image = ((System.Drawing.Image)(resources.GetObject("oktaIcon.Image")));
      this.oktaIcon.Location = new System.Drawing.Point(87, 62);
      this.oktaIcon.Name = "oktaIcon";
      this.oktaIcon.Size = new System.Drawing.Size(35, 35);
      this.oktaIcon.TabIndex = 4;
      this.oktaIcon.TabStop = false;
      // 
      // iconLabel
      // 
      this.iconLabel.AutoSize = true;
      this.iconLabel.Location = new System.Drawing.Point(12, 70);
      this.iconLabel.Name = "iconLabel";
      this.iconLabel.Size = new System.Drawing.Size(31, 13);
      this.iconLabel.TabIndex = 3;
      this.iconLabel.Text = "Icon:";
      // 
      // nameLabel
      // 
      this.nameLabel.AutoSize = true;
      this.nameLabel.Location = new System.Drawing.Point(12, 22);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new System.Drawing.Size(38, 13);
      this.nameLabel.TabIndex = 3;
      this.nameLabel.Text = "Name:";
      // 
      // nameField
      // 
      this.nameField.Location = new System.Drawing.Point(66, 22);
      this.nameField.Name = "nameField";
      this.nameField.Size = new System.Drawing.Size(371, 20);
      this.nameField.TabIndex = 0;
      // 
      // newAuthenticatorTimer
      // 
      this.newAuthenticatorTimer.Tick += new System.EventHandler(this.newAuthenticatorTimer_Tick);
      // 
      // step8Label
      // 
      this.step8Label.BackColor = System.Drawing.Color.Transparent;
      this.step8Label.Location = new System.Drawing.Point(12, 298);
      this.step8Label.Name = "step8Label";
      this.step8Label.Size = new System.Drawing.Size(425, 22);
      this.step8Label.TabIndex = 0;
      this.step8Label.Text = "8. Click \"Next\" in the setup flow and enter the code provided here:";
      // 
      // step9Label
      // 
      this.step9Label.BackColor = System.Drawing.Color.Transparent;
      this.step9Label.Location = new System.Drawing.Point(12, 369);
      this.step9Label.Name = "step9Label";
      this.step9Label.Size = new System.Drawing.Size(425, 35);
      this.step9Label.TabIndex = 0;
      this.step9Label.Text = "9. Click \"Verify\" to finish.";
      // 
      // linkLabel1
      // 
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Location = new System.Drawing.Point(130, 526);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new System.Drawing.Size(0, 13);
      this.linkLabel1.TabIndex = 13;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(23, 526);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(0, 13);
      this.label1.TabIndex = 14;
      // 
      // AddOktaVerifyAuthenticator
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(455, 443);
      this.Controls.Add(this.linkLabel1);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.step9Label);
      this.Controls.Add(this.step8Label);
      this.Controls.Add(this.newAuthenticatorProgress);
      this.Controls.Add(this.iconRadioButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.codeField);
      this.Controls.Add(this.oktaIcon);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.iconLabel);
      this.Controls.Add(this.nameLabel);
      this.Controls.Add(this.nameField);
      this.Controls.Add(this.verifyAuthenticatorButton);
      this.Controls.Add(this.setupLabel);
      this.Controls.Add(this.secretCodeField);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AddOktaVerifyAuthenticator";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Okta Verify Authenticator";
      this.Load += new System.EventHandler(this.AddOktaVerifyAuthenticator_Load);
      ((System.ComponentModel.ISupportInitialize)(this.oktaIcon)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Label setupLabel;
    private Button okButton;
    private Button cancelButton;
    private TextBox secretCodeField;
    private System.Windows.Forms.PictureBox oktaIcon;
    private Label iconLabel;
    private Label nameLabel;
    private TextBox nameField;
    private Button verifyAuthenticatorButton;
    private System.Windows.Forms.ProgressBar newAuthenticatorProgress;
    private SecretTextBox codeField;
    private System.Windows.Forms.Timer newAuthenticatorTimer;
    private GroupRadioButton iconRadioButton;
    private Label step8Label;
    private Label step9Label;
    private System.Windows.Forms.LinkLabel linkLabel1;
    private System.Windows.Forms.Label label1;
  }
}