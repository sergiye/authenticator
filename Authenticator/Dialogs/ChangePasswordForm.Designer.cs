using System.Windows.Forms;

namespace Authenticator {
  partial class ChangePasswordForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePasswordForm));
      this.introLabel = new System.Windows.Forms.Label();
      this.machineCheckbox = new System.Windows.Forms.CheckBox();
      this.userCheckbox = new System.Windows.Forms.CheckBox();
      this.passwordCheckbox = new System.Windows.Forms.CheckBox();
      this.passwordLabel = new System.Windows.Forms.Label();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.pictureBox2 = new System.Windows.Forms.PictureBox();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.passwordField = new System.Windows.Forms.TextBox();
      this.verifyField = new System.Windows.Forms.TextBox();
      this.verifyFieldLabel = new System.Windows.Forms.Label();
      this.passwordFieldLabel = new System.Windows.Forms.Label();
      this.machineLabel = new System.Windows.Forms.Label();
      this.metroLabel4 = new System.Windows.Forms.Label();
      this.metroLabel3 = new System.Windows.Forms.Label();
      this.metroLabel1 = new System.Windows.Forms.Label();
      this.metroLabel5 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize) (this.pictureBox2)).BeginInit();
      this.SuspendLayout();
      // 
      // introLabel
      // 
      this.introLabel.Location = new System.Drawing.Point(18, 14);
      this.introLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.introLabel.Name = "introLabel";
      this.introLabel.Size = new System.Drawing.Size(1077, 65);
      this.introLabel.TabIndex = 1;
      this.introLabel.Text = "Select how you would like to protect your authenticators. Using a password is str" + "ongly recommended, otherwise your data could be read and stolen by malware runni" + "ng on your computer.";
      // 
      // machineCheckbox
      // 
      this.machineCheckbox.AutoSize = true;
      this.machineCheckbox.Location = new System.Drawing.Point(48, 458);
      this.machineCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.machineCheckbox.Name = "machineCheckbox";
      this.machineCheckbox.Size = new System.Drawing.Size(343, 24);
      this.machineCheckbox.TabIndex = 3;
      this.machineCheckbox.Text = "Encrypt to only be useable on this computer";
      this.machineCheckbox.CheckedChanged += new System.EventHandler(this.machineCheckbox_CheckedChanged);
      // 
      // userCheckbox
      // 
      this.userCheckbox.AutoSize = true;
      this.userCheckbox.Enabled = false;
      this.userCheckbox.Location = new System.Drawing.Point(80, 506);
      this.userCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.userCheckbox.Name = "userCheckbox";
      this.userCheckbox.Size = new System.Drawing.Size(354, 24);
      this.userCheckbox.TabIndex = 4;
      this.userCheckbox.Text = "And only by the current user on this computer";
      // 
      // passwordCheckbox
      // 
      this.passwordCheckbox.AutoSize = true;
      this.passwordCheckbox.Location = new System.Drawing.Point(24, 105);
      this.passwordCheckbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.passwordCheckbox.Name = "passwordCheckbox";
      this.passwordCheckbox.Size = new System.Drawing.Size(247, 24);
      this.passwordCheckbox.TabIndex = 0;
      this.passwordCheckbox.Text = "Protect with my own password";
      this.passwordCheckbox.CheckedChanged += new System.EventHandler(this.passwordCheckbox_CheckedChanged);
      // 
      // passwordLabel
      // 
      this.passwordLabel.Location = new System.Drawing.Point(24, 138);
      this.passwordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.passwordLabel.Name = "passwordLabel";
      this.passwordLabel.Size = new System.Drawing.Size(1071, 77);
      this.passwordLabel.TabIndex = 1;
      this.passwordLabel.Text = resources.GetString("passwordLabel.Text");
      // 
      // pictureBox1
      // 
      this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox1.Image = global::Authenticator.Properties.Resources.BluePixel;
      this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.pictureBox1.Location = new System.Drawing.Point(18, 325);
      this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(1035, 2);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 2;
      this.pictureBox1.TabStop = false;
      // 
      // pictureBox2
      // 
      this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox2.Image = global::Authenticator.Properties.Resources.BluePixel;
      this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.pictureBox2.Location = new System.Drawing.Point(20, 566);
      this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.pictureBox2.Name = "pictureBox2";
      this.pictureBox2.Size = new System.Drawing.Size(1035, 2);
      this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox2.TabIndex = 3;
      this.pictureBox2.TabStop = false;
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(978, 591);
      this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(112, 35);
      this.cancelButton.TabIndex = 6;
      this.cancelButton.Text = "Cancel";
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(856, 591);
      this.okButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(112, 35);
      this.okButton.TabIndex = 5;
      this.okButton.Text = "OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // passwordField
      // 
      this.passwordField.Enabled = false;
      this.passwordField.Location = new System.Drawing.Point(170, 220);
      this.passwordField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.passwordField.Name = "passwordField";
      this.passwordField.PasswordChar = '●';
      this.passwordField.Size = new System.Drawing.Size(391, 26);
      this.passwordField.TabIndex = 1;
      this.passwordField.UseSystemPasswordChar = true;
      // 
      // verifyField
      // 
      this.verifyField.Enabled = false;
      this.verifyField.Location = new System.Drawing.Point(170, 265);
      this.verifyField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.verifyField.Name = "verifyField";
      this.verifyField.PasswordChar = '●';
      this.verifyField.Size = new System.Drawing.Size(391, 26);
      this.verifyField.TabIndex = 2;
      this.verifyField.UseSystemPasswordChar = true;
      // 
      // verifyFieldLabel
      // 
      this.verifyFieldLabel.AutoSize = true;
      this.verifyFieldLabel.Location = new System.Drawing.Point(45, 266);
      this.verifyFieldLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.verifyFieldLabel.Name = "verifyFieldLabel";
      this.verifyFieldLabel.Size = new System.Drawing.Size(49, 20);
      this.verifyFieldLabel.TabIndex = 5;
      this.verifyFieldLabel.Text = "Verify";
      // 
      // passwordFieldLabel
      // 
      this.passwordFieldLabel.AutoSize = true;
      this.passwordFieldLabel.Location = new System.Drawing.Point(45, 222);
      this.passwordFieldLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.passwordFieldLabel.Name = "passwordFieldLabel";
      this.passwordFieldLabel.Size = new System.Drawing.Size(78, 20);
      this.passwordFieldLabel.TabIndex = 5;
      this.passwordFieldLabel.Text = "Password";
      // 
      // machineLabel
      // 
      this.machineLabel.Location = new System.Drawing.Point(18, 345);
      this.machineLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.machineLabel.Name = "machineLabel";
      this.machineLabel.Size = new System.Drawing.Size(1077, 97);
      this.machineLabel.TabIndex = 1;
      this.machineLabel.Text = resources.GetString("machineLabel.Text");
      // 
      // metroLabel4
      // 
      this.metroLabel4.Location = new System.Drawing.Point(78, 43);
      this.metroLabel4.Name = "metroLabel4";
      this.metroLabel4.Size = new System.Drawing.Size(135, 23);
      this.metroLabel4.TabIndex = 1;
      this.metroLabel4.Text = "Require button press";
      // 
      // metroLabel3
      // 
      this.metroLabel3.Location = new System.Drawing.Point(3, 49);
      this.metroLabel3.Name = "metroLabel3";
      this.metroLabel3.Size = new System.Drawing.Size(42, 23);
      this.metroLabel3.TabIndex = 1;
      this.metroLabel3.Text = "Slot";
      // 
      // metroLabel1
      // 
      this.metroLabel1.Location = new System.Drawing.Point(3, 1);
      this.metroLabel1.Name = "metroLabel1";
      this.metroLabel1.Size = new System.Drawing.Size(692, 47);
      this.metroLabel1.TabIndex = 1;
      this.metroLabel1.Text = "Your YubiKey must support Challenge-Response using HMAC-SHA1 in one of its slots." + " Use the YubiKey personalization tool to configure the slot or click the Configu" + "re Slot button.";
      // 
      // metroLabel5
      // 
      this.metroLabel5.Location = new System.Drawing.Point(-1, 11);
      this.metroLabel5.Name = "metroLabel5";
      this.metroLabel5.Size = new System.Drawing.Size(73, 23);
      this.metroLabel5.TabIndex = 12;
      this.metroLabel5.Text = "Passphrase";
      // 
      // ChangePasswordForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(1108, 645);
      this.Controls.Add(this.verifyField);
      this.Controls.Add(this.passwordField);
      this.Controls.Add(this.verifyFieldLabel);
      this.Controls.Add(this.passwordFieldLabel);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.pictureBox2);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.passwordLabel);
      this.Controls.Add(this.machineLabel);
      this.Controls.Add(this.passwordCheckbox);
      this.Controls.Add(this.userCheckbox);
      this.Controls.Add(this.machineCheckbox);
      this.Controls.Add(this.introLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ChangePasswordForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Protection";
      this.Load += new System.EventHandler(this.ChangePasswordForm_Load);
      this.Shown += new System.EventHandler(this.ChangePasswordForm_Shown);
      ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize) (this.pictureBox2)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    #endregion

    private Label introLabel;
    private CheckBox machineCheckbox;
    private CheckBox userCheckbox;
    private CheckBox passwordCheckbox;
    private Label passwordLabel;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.PictureBox pictureBox2;
    private Button cancelButton;
    private Button okButton;
    private TextBox passwordField;
    private TextBox verifyField;
    private Label verifyFieldLabel;
    private Label passwordFieldLabel;
    private Label machineLabel;
    private Label metroLabel1;
    private Label metroLabel4;
    private Label metroLabel3;
    private Label metroLabel5;

  }
}