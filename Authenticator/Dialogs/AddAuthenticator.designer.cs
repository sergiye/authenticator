using System.Windows.Forms;

namespace Authenticator {
  partial class AddAuthenticator {
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
      this.components = new System.ComponentModel.Container();
      this.secretCodeField = new System.Windows.Forms.TextBox();
      this.step1Label = new System.Windows.Forms.Label();
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.nameLabel = new System.Windows.Forms.Label();
      this.nameField = new System.Windows.Forms.TextBox();
      this.step2Label = new System.Windows.Forms.Label();
      this.verifyButton = new System.Windows.Forms.Button();
      this.codeProgress = new System.Windows.Forms.ProgressBar();
      this.codeField = new SecretTextBox();
      this.step4Label = new System.Windows.Forms.Label();
      this.timer = new System.Windows.Forms.Timer(this.components);
      this.step3Label = new System.Windows.Forms.Label();
      this.timeBasedRadio = new System.Windows.Forms.RadioButton();
      this.counterBasedRadio = new System.Windows.Forms.RadioButton();
      this.counterField = new System.Windows.Forms.TextBox();
      this.hashField = new System.Windows.Forms.ComboBox();
      this.intervalField = new System.Windows.Forms.TextBox();
      this.labelTYpe = new System.Windows.Forms.Label();
      this.hashLabel = new System.Windows.Forms.Label();
      this.periodLabel = new System.Windows.Forms.Label();
      this.digitsField = new System.Windows.Forms.TextBox();
      this.digitsLabel = new System.Windows.Forms.Label();
      this.intervalLabelPost = new System.Windows.Forms.Label();
      this.getFromScreenButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // secretCodeField
      // 
      this.secretCodeField.AllowDrop = true;
      this.secretCodeField.CausesValidation = false;
      this.secretCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.secretCodeField.Location = new System.Drawing.Point(15, 102);
      this.secretCodeField.Name = "secretCodeField";
      this.secretCodeField.Size = new System.Drawing.Size(417, 26);
      this.secretCodeField.TabIndex = 3;
      this.secretCodeField.Leave += new System.EventHandler(this.secretCodeField_Leave);
      // 
      // step1Label
      // 
      this.step1Label.Location = new System.Drawing.Point(12, 54);
      this.step1Label.Name = "step1Label";
      this.step1Label.Size = new System.Drawing.Size(425, 45);
      this.step1Label.TabIndex = 2;
      this.step1Label.Text = "1. Enter the Secret Code or KeyUri string. Spaces don\'t matter. If you have a QR " +
    "code, you can paste the URL of the image instead. Or you can take QR code from y" +
    "our dislay like a screenshot";
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(279, 500);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 21;
      this.okButton.Text = "OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(361, 500);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 22;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // nameLabel
      // 
      this.nameLabel.AutoSize = true;
      this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.nameLabel.Location = new System.Drawing.Point(12, 18);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new System.Drawing.Size(43, 13);
      this.nameLabel.TabIndex = 0;
      this.nameLabel.Text = "Name:";
      // 
      // nameField
      // 
      this.nameField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.nameField.Location = new System.Drawing.Point(66, 12);
      this.nameField.Name = "nameField";
      this.nameField.Size = new System.Drawing.Size(366, 26);
      this.nameField.TabIndex = 1;
      // 
      // step2Label
      // 
      this.step2Label.Location = new System.Drawing.Point(12, 169);
      this.step2Label.Name = "step2Label";
      this.step2Label.Size = new System.Drawing.Size(423, 32);
      this.step2Label.TabIndex = 4;
      this.step2Label.Text = "2. Select additional settings. If you don\'t know, it\'s likely the pre-selected on" +
    "es so just leave the default choice.";
      // 
      // verifyButton
      // 
      this.verifyButton.Location = new System.Drawing.Point(139, 377);
      this.verifyButton.Name = "verifyButton";
      this.verifyButton.Size = new System.Drawing.Size(158, 23);
      this.verifyButton.TabIndex = 17;
      this.verifyButton.Text = "Verify Authenticator";
      this.verifyButton.Click += new System.EventHandler(this.verifyButton_Click);
      // 
      // codeProgress
      // 
      this.codeProgress.Location = new System.Drawing.Point(139, 472);
      this.codeProgress.Maximum = 30;
      this.codeProgress.Minimum = 1;
      this.codeProgress.Name = "codeProgress";
      this.codeProgress.Size = new System.Drawing.Size(158, 8);
      this.codeProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this.codeProgress.TabIndex = 20;
      this.codeProgress.Value = 1;
      this.codeProgress.Visible = false;
      // 
      // codeField
      // 
      this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.codeField.Location = new System.Drawing.Point(139, 440);
      this.codeField.Multiline = true;
      this.codeField.Name = "codeField";
      this.codeField.SecretMode = false;
      this.codeField.Size = new System.Drawing.Size(158, 26);
      this.codeField.SpaceOut = 3;
      this.codeField.TabIndex = 19;
      // 
      // step4Label
      // 
      this.step4Label.AutoSize = true;
      this.step4Label.Location = new System.Drawing.Point(12, 413);
      this.step4Label.Name = "step4Label";
      this.step4Label.Size = new System.Drawing.Size(240, 13);
      this.step4Label.TabIndex = 18;
      this.step4Label.Text = "4. Verify the following code matches your service.";
      // 
      // timer
      // 
      this.timer.Enabled = true;
      this.timer.Interval = 500;
      this.timer.Tick += new System.EventHandler(this.timer_Tick);
      // 
      // step3Label
      // 
      this.step3Label.Location = new System.Drawing.Point(12, 331);
      this.step3Label.Name = "step3Label";
      this.step3Label.Size = new System.Drawing.Size(423, 40);
      this.step3Label.TabIndex = 15;
      this.step3Label.Text = "3. Click the Verify button to check the first code.";
      // 
      // timeBasedRadio
      // 
      this.timeBasedRadio.AutoSize = true;
      this.timeBasedRadio.Checked = true;
      this.timeBasedRadio.Location = new System.Drawing.Point(153, 212);
      this.timeBasedRadio.Name = "timeBasedRadio";
      this.timeBasedRadio.Size = new System.Drawing.Size(80, 17);
      this.timeBasedRadio.TabIndex = 6;
      this.timeBasedRadio.TabStop = true;
      this.timeBasedRadio.Text = "Time-based";
      this.timeBasedRadio.CheckedChanged += new System.EventHandler(this.basementRadio_CheckedChanged);
      // 
      // counterBasedRadio
      // 
      this.counterBasedRadio.AutoSize = true;
      this.counterBasedRadio.Location = new System.Drawing.Point(277, 212);
      this.counterBasedRadio.Name = "counterBasedRadio";
      this.counterBasedRadio.Size = new System.Drawing.Size(94, 17);
      this.counterBasedRadio.TabIndex = 7;
      this.counterBasedRadio.Text = "Counter-based";
      this.counterBasedRadio.CheckedChanged += new System.EventHandler(this.basementRadio_CheckedChanged);
      // 
      // counterField
      // 
      this.counterField.AllowDrop = true;
      this.counterField.CausesValidation = false;
      this.counterField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.counterField.Location = new System.Drawing.Point(23, 376);
      this.counterField.Name = "counterField";
      this.counterField.Size = new System.Drawing.Size(110, 26);
      this.counterField.TabIndex = 16;
      this.counterField.Visible = false;
      // 
      // hashField
      // 
      this.hashField.FormattingEnabled = true;
      this.hashField.ItemHeight = 13;
      this.hashField.Items.AddRange(new object[] {
            "HMAC-SHA1",
            "HMAC-SHA256",
            "HMAC-SHA512"});
      this.hashField.Location = new System.Drawing.Point(153, 237);
      this.hashField.Name = "hashField";
      this.hashField.Size = new System.Drawing.Size(99, 21);
      this.hashField.TabIndex = 9;
      // 
      // intervalField
      // 
      this.intervalField.AllowDrop = true;
      this.intervalField.CausesValidation = false;
      this.intervalField.Location = new System.Drawing.Point(153, 264);
      this.intervalField.Name = "intervalField";
      this.intervalField.Size = new System.Drawing.Size(100, 20);
      this.intervalField.TabIndex = 11;
      // 
      // labelTYpe
      // 
      this.labelTYpe.Location = new System.Drawing.Point(49, 214);
      this.labelTYpe.Name = "labelTYpe";
      this.labelTYpe.Size = new System.Drawing.Size(72, 20);
      this.labelTYpe.TabIndex = 5;
      this.labelTYpe.Text = "Type";
      // 
      // hashLabel
      // 
      this.hashLabel.Location = new System.Drawing.Point(49, 240);
      this.hashLabel.Name = "hashLabel";
      this.hashLabel.Size = new System.Drawing.Size(72, 20);
      this.hashLabel.TabIndex = 8;
      this.hashLabel.Text = "Hash";
      // 
      // periodLabel
      // 
      this.periodLabel.Location = new System.Drawing.Point(49, 267);
      this.periodLabel.Name = "periodLabel";
      this.periodLabel.Size = new System.Drawing.Size(72, 20);
      this.periodLabel.TabIndex = 10;
      this.periodLabel.Text = "Interval";
      // 
      // digitsField
      // 
      this.digitsField.AllowDrop = true;
      this.digitsField.CausesValidation = false;
      this.digitsField.Location = new System.Drawing.Point(153, 290);
      this.digitsField.Name = "digitsField";
      this.digitsField.Size = new System.Drawing.Size(100, 20);
      this.digitsField.TabIndex = 14;
      // 
      // digitsLabel
      // 
      this.digitsLabel.Location = new System.Drawing.Point(49, 293);
      this.digitsLabel.Name = "digitsLabel";
      this.digitsLabel.Size = new System.Drawing.Size(72, 20);
      this.digitsLabel.TabIndex = 13;
      this.digitsLabel.Text = "Digits";
      // 
      // intervalLabelPost
      // 
      this.intervalLabelPost.Location = new System.Drawing.Point(259, 267);
      this.intervalLabelPost.Name = "intervalLabelPost";
      this.intervalLabelPost.Size = new System.Drawing.Size(69, 20);
      this.intervalLabelPost.TabIndex = 12;
      this.intervalLabelPost.Text = "seconds";
      // 
      // getFromScreenButton
      // 
      this.getFromScreenButton.Location = new System.Drawing.Point(274, 134);
      this.getFromScreenButton.Name = "getFromScreenButton";
      this.getFromScreenButton.Size = new System.Drawing.Size(158, 23);
      this.getFromScreenButton.TabIndex = 23;
      this.getFromScreenButton.Text = "Take from screen";
      this.getFromScreenButton.Click += new System.EventHandler(this.getFromScreenButton_Click);
      // 
      // AddAuthenticator
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(443, 531);
      this.Controls.Add(this.getFromScreenButton);
      this.Controls.Add(this.counterField);
      this.Controls.Add(this.hashField);
      this.Controls.Add(this.step3Label);
      this.Controls.Add(this.verifyButton);
      this.Controls.Add(this.counterBasedRadio);
      this.Controls.Add(this.timeBasedRadio);
      this.Controls.Add(this.codeProgress);
      this.Controls.Add(this.codeField);
      this.Controls.Add(this.step4Label);
      this.Controls.Add(this.intervalLabelPost);
      this.Controls.Add(this.digitsLabel);
      this.Controls.Add(this.periodLabel);
      this.Controls.Add(this.hashLabel);
      this.Controls.Add(this.labelTYpe);
      this.Controls.Add(this.step2Label);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.digitsField);
      this.Controls.Add(this.intervalField);
      this.Controls.Add(this.secretCodeField);
      this.Controls.Add(this.nameLabel);
      this.Controls.Add(this.nameField);
      this.Controls.Add(this.step1Label);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AddAuthenticator";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Add Authenticator";
      this.Load += new System.EventHandler(this.AddAuthenticator_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label step1Label;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.TextBox secretCodeField;
    private System.Windows.Forms.Label nameLabel;
    private System.Windows.Forms.TextBox nameField;
    private System.Windows.Forms.Label step2Label;
    private Button verifyButton;
    private System.Windows.Forms.ProgressBar codeProgress;
    private SecretTextBox codeField;
    private System.Windows.Forms.Label step4Label;
    private System.Windows.Forms.Timer timer;
    private Label step3Label;
    private System.Windows.Forms.RadioButton timeBasedRadio;
    private System.Windows.Forms.RadioButton counterBasedRadio;
    private System.Windows.Forms.TextBox counterField;
    private System.Windows.Forms.ComboBox hashField;
    private System.Windows.Forms.TextBox intervalField;
    private System.Windows.Forms.Label labelTYpe;
    private System.Windows.Forms.Label hashLabel;
    private System.Windows.Forms.Label periodLabel;
    private System.Windows.Forms.TextBox digitsField;
    private System.Windows.Forms.Label digitsLabel;
    private System.Windows.Forms.Label intervalLabelPost;
    private Button getFromScreenButton;
  }
}