using System.Windows.Forms;

namespace Authenticator
{
    partial class AddAuthenticator
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
      this.nameLabel = new System.Windows.Forms.Label();
      this.nameField = new System.Windows.Forms.TextBox();
      this.step2Label = new System.Windows.Forms.Label();
      this.verifyButton = new System.Windows.Forms.Button();
      this.codeProgress = new System.Windows.Forms.ProgressBar();
      this.codeField = new SecretTextBox();
      this.step5Label = new System.Windows.Forms.Label();
      this.timer = new System.Windows.Forms.Timer(this.components);
      this.step4TimerLabel = new System.Windows.Forms.Label();
      this.timeBasedRadio = new System.Windows.Forms.RadioButton();
      this.counterBasedRadio = new System.Windows.Forms.RadioButton();
      this.timeBasedPanel = new System.Windows.Forms.Panel();
      this.counterBasedPanel = new System.Windows.Forms.Panel();
      this.verifyCounterButton = new System.Windows.Forms.Button();
      this.counterField = new System.Windows.Forms.TextBox();
      this.step4CounterLabel = new System.Windows.Forms.Label();
      this.hashField = new System.Windows.Forms.ComboBox();
      this.intervalField = new System.Windows.Forms.TextBox();
      this.labelTYpe = new System.Windows.Forms.Label();
      this.hashLabel = new System.Windows.Forms.Label();
      this.periodLabel = new System.Windows.Forms.Label();
      this.digitsField = new System.Windows.Forms.TextBox();
      this.digitsLabel = new System.Windows.Forms.Label();
      this.intervalLabelPost = new System.Windows.Forms.Label();
      this.timeBasedPanel.SuspendLayout();
      this.counterBasedPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // secretCodeField
      // 
      this.secretCodeField.AllowDrop = true;
      this.secretCodeField.CausesValidation = false;
      this.secretCodeField.Location = new System.Drawing.Point(43, 171);
      this.secretCodeField.Name = "secretCodeField";
      this.secretCodeField.Size = new System.Drawing.Size(390, 20);
      this.secretCodeField.TabIndex = 1;
      this.secretCodeField.Leave += new System.EventHandler(this.secretCodeField_Leave);
      // 
      // step1Label
      // 
      this.step1Label.Location = new System.Drawing.Point(28, 120);
      this.step1Label.Name = "step1Label";
      this.step1Label.Size = new System.Drawing.Size(425, 48);
      this.step1Label.TabIndex = 1;
      this.step1Label.Text = "1. Enter the Secret Code or KeyUri string. Spaces don\'t matter. If you have a QR " +
    "code, you can paste the URL of the image instead.\r\n";
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(292, 595);
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
      this.cancelButton.Location = new System.Drawing.Point(373, 595);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 4;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // nameLabel
      // 
      this.nameLabel.AutoSize = true;
      this.nameLabel.Location = new System.Drawing.Point(28, 70);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new System.Drawing.Size(38, 13);
      this.nameLabel.TabIndex = 3;
      this.nameLabel.Text = "Name:";
      // 
      // nameField
      // 
      this.nameField.Location = new System.Drawing.Point(82, 67);
      this.nameField.Name = "nameField";
      this.nameField.Size = new System.Drawing.Size(366, 20);
      this.nameField.TabIndex = 0;
      // 
      // step2Label
      // 
      this.step2Label.Location = new System.Drawing.Point(28, 213);
      this.step2Label.Name = "step2Label";
      this.step2Label.Size = new System.Drawing.Size(423, 49);
      this.step2Label.TabIndex = 10;
      this.step2Label.Text = "2. Select additional settings. If you don\'t know, it\'s likely the pre-selected on" +
    "es so just leave the default choice.";
      // 
      // verifyButton
      // 
      this.verifyButton.Location = new System.Drawing.Point(152, 43);
      this.verifyButton.Name = "verifyButton";
      this.verifyButton.Size = new System.Drawing.Size(158, 23);
      this.verifyButton.TabIndex = 2;
      this.verifyButton.Text = "Verify Authenticator";
      this.verifyButton.Click += new System.EventHandler(this.verifyButton_Click);
      // 
      // codeProgress
      // 
      this.codeProgress.Location = new System.Drawing.Point(155, 555);
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
      this.codeField.Location = new System.Drawing.Point(155, 523);
      this.codeField.Multiline = true;
      this.codeField.Name = "codeField";
      this.codeField.SecretMode = false;
      this.codeField.Size = new System.Drawing.Size(158, 26);
      this.codeField.SpaceOut = 3;
      this.codeField.TabIndex = 5;
      // 
      // step5Label
      // 
      this.step5Label.AutoSize = true;
      this.step5Label.Location = new System.Drawing.Point(28, 496);
      this.step5Label.Name = "step5Label";
      this.step5Label.Size = new System.Drawing.Size(240, 13);
      this.step5Label.TabIndex = 11;
      this.step5Label.Text = "4. Verify the following code matches your service.";
      // 
      // timer
      // 
      this.timer.Enabled = true;
      this.timer.Interval = 500;
      this.timer.Tick += new System.EventHandler(this.timer_Tick);
      // 
      // step4TimerLabel
      // 
      this.step4TimerLabel.Location = new System.Drawing.Point(23, 12);
      this.step4TimerLabel.Name = "step4TimerLabel";
      this.step4TimerLabel.Size = new System.Drawing.Size(423, 28);
      this.step4TimerLabel.TabIndex = 10;
      this.step4TimerLabel.Text = "3. Click the Verify button to check the first code.";
      // 
      // timeBasedRadio
      // 
      this.timeBasedRadio.AutoSize = true;
      this.timeBasedRadio.Checked = true;
      this.timeBasedRadio.Location = new System.Drawing.Point(186, 262);
      this.timeBasedRadio.Name = "timeBasedRadio";
      this.timeBasedRadio.Size = new System.Drawing.Size(80, 17);
      this.timeBasedRadio.TabIndex = 5;
      this.timeBasedRadio.TabStop = true;
      this.timeBasedRadio.Text = "Time-based";
      this.timeBasedRadio.CheckedChanged += new System.EventHandler(this.timeBasedRadio_CheckedChanged);
      // 
      // counterBasedRadio
      // 
      this.counterBasedRadio.AutoSize = true;
      this.counterBasedRadio.Location = new System.Drawing.Point(310, 262);
      this.counterBasedRadio.Name = "counterBasedRadio";
      this.counterBasedRadio.Size = new System.Drawing.Size(94, 17);
      this.counterBasedRadio.TabIndex = 6;
      this.counterBasedRadio.Text = "Counter-based";
      this.counterBasedRadio.CheckedChanged += new System.EventHandler(this.counterBasedRadio_CheckedChanged);
      // 
      // timeBasedPanel
      // 
      this.timeBasedPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.timeBasedPanel.Controls.Add(this.step4TimerLabel);
      this.timeBasedPanel.Controls.Add(this.verifyButton);
      this.timeBasedPanel.Location = new System.Drawing.Point(5, 381);
      this.timeBasedPanel.Name = "timeBasedPanel";
      this.timeBasedPanel.Size = new System.Drawing.Size(464, 84);
      this.timeBasedPanel.TabIndex = 15;
      // 
      // counterBasedPanel
      // 
      this.counterBasedPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.counterBasedPanel.Controls.Add(this.verifyCounterButton);
      this.counterBasedPanel.Controls.Add(this.counterField);
      this.counterBasedPanel.Controls.Add(this.step4CounterLabel);
      this.counterBasedPanel.Location = new System.Drawing.Point(5, 379);
      this.counterBasedPanel.Name = "counterBasedPanel";
      this.counterBasedPanel.Size = new System.Drawing.Size(464, 84);
      this.counterBasedPanel.TabIndex = 2;
      this.counterBasedPanel.Visible = false;
      // 
      // verifyCounterButton
      // 
      this.verifyCounterButton.Location = new System.Drawing.Point(204, 56);
      this.verifyCounterButton.Name = "verifyCounterButton";
      this.verifyCounterButton.Size = new System.Drawing.Size(158, 23);
      this.verifyCounterButton.TabIndex = 2;
      this.verifyCounterButton.Text = "Verify Authenticator";
      this.verifyCounterButton.Click += new System.EventHandler(this.verifyButton_Click);
      // 
      // counterField
      // 
      this.counterField.AllowDrop = true;
      this.counterField.CausesValidation = false;
      this.counterField.Location = new System.Drawing.Point(119, 58);
      this.counterField.Name = "counterField";
      this.counterField.Size = new System.Drawing.Size(79, 20);
      this.counterField.TabIndex = 0;
      // 
      // step4CounterLabel
      // 
      this.step4CounterLabel.Location = new System.Drawing.Point(23, 12);
      this.step4CounterLabel.Name = "step4CounterLabel";
      this.step4CounterLabel.Size = new System.Drawing.Size(423, 43);
      this.step4CounterLabel.TabIndex = 10;
      this.step4CounterLabel.Text = "3. Enter the initial counter value if known. Click the Verify button that will sh" +
    "ow the last code that was used.";
      // 
      // hashField
      // 
      this.hashField.FormattingEnabled = true;
      this.hashField.ItemHeight = 13;
      this.hashField.Items.AddRange(new object[] {
            "HMAC-SHA1",
            "HMAC-SHA256",
            "HMAC-SHA512"});
      this.hashField.Location = new System.Drawing.Point(186, 287);
      this.hashField.Name = "hashField";
      this.hashField.Size = new System.Drawing.Size(99, 21);
      this.hashField.TabIndex = 7;
      // 
      // intervalField
      // 
      this.intervalField.AllowDrop = true;
      this.intervalField.CausesValidation = false;
      this.intervalField.Location = new System.Drawing.Point(186, 322);
      this.intervalField.Name = "intervalField";
      this.intervalField.Size = new System.Drawing.Size(96, 20);
      this.intervalField.TabIndex = 8;
      // 
      // labelTYpe
      // 
      this.labelTYpe.Location = new System.Drawing.Point(82, 259);
      this.labelTYpe.Name = "labelTYpe";
      this.labelTYpe.Size = new System.Drawing.Size(72, 21);
      this.labelTYpe.TabIndex = 10;
      this.labelTYpe.Text = "Type";
      // 
      // hashLabel
      // 
      this.hashLabel.Location = new System.Drawing.Point(82, 291);
      this.hashLabel.Name = "hashLabel";
      this.hashLabel.Size = new System.Drawing.Size(72, 21);
      this.hashLabel.TabIndex = 10;
      this.hashLabel.Text = "Hash";
      // 
      // periodLabel
      // 
      this.periodLabel.Location = new System.Drawing.Point(82, 322);
      this.periodLabel.Name = "periodLabel";
      this.periodLabel.Size = new System.Drawing.Size(72, 21);
      this.periodLabel.TabIndex = 10;
      this.periodLabel.Text = "Interval";
      // 
      // digitsField
      // 
      this.digitsField.AllowDrop = true;
      this.digitsField.CausesValidation = false;
      this.digitsField.Location = new System.Drawing.Point(186, 350);
      this.digitsField.Name = "digitsField";
      this.digitsField.Size = new System.Drawing.Size(96, 20);
      this.digitsField.TabIndex = 9;
      // 
      // digitsLabel
      // 
      this.digitsLabel.Location = new System.Drawing.Point(82, 350);
      this.digitsLabel.Name = "digitsLabel";
      this.digitsLabel.Size = new System.Drawing.Size(72, 21);
      this.digitsLabel.TabIndex = 10;
      this.digitsLabel.Text = "Digits";
      // 
      // intervalLabelPost
      // 
      this.intervalLabelPost.Location = new System.Drawing.Point(292, 322);
      this.intervalLabelPost.Name = "intervalLabelPost";
      this.intervalLabelPost.Size = new System.Drawing.Size(129, 21);
      this.intervalLabelPost.TabIndex = 10;
      this.intervalLabelPost.Text = "seconds";
      // 
      // AddAuthenticator
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(471, 641);
      this.Controls.Add(this.hashField);
      this.Controls.Add(this.counterBasedPanel);
      this.Controls.Add(this.timeBasedPanel);
      this.Controls.Add(this.counterBasedRadio);
      this.Controls.Add(this.timeBasedRadio);
      this.Controls.Add(this.codeProgress);
      this.Controls.Add(this.codeField);
      this.Controls.Add(this.step5Label);
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
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Add Authenticator";
      this.Load += new System.EventHandler(this.AddAuthenticator_Load);
      this.timeBasedPanel.ResumeLayout(false);
      this.counterBasedPanel.ResumeLayout(false);
      this.counterBasedPanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private Label step1Label;
        private Button okButton;
        private Button cancelButton;
        private TextBox secretCodeField;
        private Label nameLabel;
        private TextBox nameField;
        private Label step2Label;
        private Button verifyButton;
        private System.Windows.Forms.ProgressBar codeProgress;
        private SecretTextBox codeField;
        private Label step5Label;
        private System.Windows.Forms.Timer timer;
        private Label step4TimerLabel;
        private RadioButton timeBasedRadio;
        private RadioButton counterBasedRadio;
        private System.Windows.Forms.Panel timeBasedPanel;
        private System.Windows.Forms.Panel counterBasedPanel;
        private Label step4CounterLabel;
        private TextBox counterField;
        private Button verifyCounterButton;
        private ComboBox hashField;
    private TextBox intervalField;
    private Label labelTYpe;
    private Label hashLabel;
    private Label periodLabel;
    private TextBox digitsField;
    private Label digitsLabel;
    private Label intervalLabelPost;
  }
}