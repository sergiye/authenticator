using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

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
      this.secretCodeField.Location = new System.Drawing.Point(54, 178);
      this.secretCodeField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.secretCodeField.Name = "secretCodeField";
      this.secretCodeField.Size = new System.Drawing.Size(583, 26);
      this.secretCodeField.TabIndex = 1;
      this.secretCodeField.Leave += new System.EventHandler(this.secretCodeField_Leave);
      // 
      // step1Label
      // 
      this.step1Label.Location = new System.Drawing.Point(32, 100);
      this.step1Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.step1Label.Name = "step1Label";
      this.step1Label.Size = new System.Drawing.Size(638, 74);
      this.step1Label.TabIndex = 1;
      this.step1Label.Text = "1. Enter the Secret Code or KeyUri string. Spaces don\'t matter. If you have a QR " + "code, you can paste the URL of the image instead.\r\n";
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(452, 801);
      this.okButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(112, 35);
      this.okButton.TabIndex = 3;
      this.okButton.Text = "OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(574, 801);
      this.cancelButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(112, 35);
      this.cancelButton.TabIndex = 4;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // nameLabel
      // 
      this.nameLabel.AutoSize = true;
      this.nameLabel.Location = new System.Drawing.Point(32, 23);
      this.nameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new System.Drawing.Size(55, 20);
      this.nameLabel.TabIndex = 3;
      this.nameLabel.Text = "Name:";
      // 
      // nameField
      // 
      this.nameField.Location = new System.Drawing.Point(113, 18);
      this.nameField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.nameField.Name = "nameField";
      this.nameField.Size = new System.Drawing.Size(547, 26);
      this.nameField.TabIndex = 0;
      // 
      // step2Label
      // 
      this.step2Label.Location = new System.Drawing.Point(32, 243);
      this.step2Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.step2Label.Name = "step2Label";
      this.step2Label.Size = new System.Drawing.Size(634, 75);
      this.step2Label.TabIndex = 10;
      this.step2Label.Text = "2. Select additional settings. If you don\'t know, it\'s likely the pre-selected on" + "es so just leave the default choice.";
      // 
      // verifyButton
      // 
      this.verifyButton.Location = new System.Drawing.Point(228, 66);
      this.verifyButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.verifyButton.Name = "verifyButton";
      this.verifyButton.Size = new System.Drawing.Size(237, 35);
      this.verifyButton.TabIndex = 2;
      this.verifyButton.Text = "Verify Authenticator";
      this.verifyButton.Click += new System.EventHandler(this.verifyButton_Click);
      // 
      // codeProgress
      // 
      this.codeProgress.Location = new System.Drawing.Point(222, 769);
      this.codeProgress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.codeProgress.Maximum = 30;
      this.codeProgress.Minimum = 1;
      this.codeProgress.Name = "codeProgress";
      this.codeProgress.Size = new System.Drawing.Size(237, 12);
      this.codeProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this.codeProgress.TabIndex = 13;
      this.codeProgress.Value = 1;
      this.codeProgress.Visible = false;
      // 
      // codeField
      // 
      this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.codeField.Location = new System.Drawing.Point(222, 720);
      this.codeField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.codeField.Multiline = true;
      this.codeField.Name = "codeField";
      this.codeField.SecretMode = false;
      this.codeField.Size = new System.Drawing.Size(235, 38);
      this.codeField.SpaceOut = 3;
      this.codeField.TabIndex = 5;
      // 
      // step5Label
      // 
      this.step5Label.AutoSize = true;
      this.step5Label.Location = new System.Drawing.Point(32, 678);
      this.step5Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.step5Label.Name = "step5Label";
      this.step5Label.Size = new System.Drawing.Size(353, 20);
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
      this.step4TimerLabel.Location = new System.Drawing.Point(34, 18);
      this.step4TimerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.step4TimerLabel.Name = "step4TimerLabel";
      this.step4TimerLabel.Size = new System.Drawing.Size(634, 43);
      this.step4TimerLabel.TabIndex = 10;
      this.step4TimerLabel.Text = "3. Click the Verify button to check the first code.";
      // 
      // timeBasedRadio
      // 
      this.timeBasedRadio.AutoSize = true;
      this.timeBasedRadio.Checked = true;
      this.timeBasedRadio.Location = new System.Drawing.Point(269, 318);
      this.timeBasedRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.timeBasedRadio.Name = "timeBasedRadio";
      this.timeBasedRadio.Size = new System.Drawing.Size(117, 24);
      this.timeBasedRadio.TabIndex = 5;
      this.timeBasedRadio.TabStop = true;
      this.timeBasedRadio.Text = "Time-based";
      this.timeBasedRadio.CheckedChanged += new System.EventHandler(this.timeBasedRadio_CheckedChanged);
      // 
      // counterBasedRadio
      // 
      this.counterBasedRadio.AutoSize = true;
      this.counterBasedRadio.Location = new System.Drawing.Point(455, 318);
      this.counterBasedRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.counterBasedRadio.Name = "counterBasedRadio";
      this.counterBasedRadio.Size = new System.Drawing.Size(140, 24);
      this.counterBasedRadio.TabIndex = 6;
      this.counterBasedRadio.Text = "Counter-based";
      this.counterBasedRadio.CheckedChanged += new System.EventHandler(this.counterBasedRadio_CheckedChanged);
      // 
      // timeBasedPanel
      // 
      this.timeBasedPanel.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.timeBasedPanel.Controls.Add(this.step4TimerLabel);
      this.timeBasedPanel.Controls.Add(this.verifyButton);
      this.timeBasedPanel.Location = new System.Drawing.Point(-2, 501);
      this.timeBasedPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.timeBasedPanel.Name = "timeBasedPanel";
      this.timeBasedPanel.Size = new System.Drawing.Size(688, 129);
      this.timeBasedPanel.TabIndex = 15;
      // 
      // counterBasedPanel
      // 
      this.counterBasedPanel.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.counterBasedPanel.Controls.Add(this.verifyCounterButton);
      this.counterBasedPanel.Controls.Add(this.counterField);
      this.counterBasedPanel.Controls.Add(this.step4CounterLabel);
      this.counterBasedPanel.Location = new System.Drawing.Point(-2, 498);
      this.counterBasedPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.counterBasedPanel.Name = "counterBasedPanel";
      this.counterBasedPanel.Size = new System.Drawing.Size(688, 129);
      this.counterBasedPanel.TabIndex = 2;
      this.counterBasedPanel.Visible = false;
      // 
      // verifyCounterButton
      // 
      this.verifyCounterButton.Location = new System.Drawing.Point(306, 86);
      this.verifyCounterButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.verifyCounterButton.Name = "verifyCounterButton";
      this.verifyCounterButton.Size = new System.Drawing.Size(237, 35);
      this.verifyCounterButton.TabIndex = 2;
      this.verifyCounterButton.Text = "Verify Authenticator";
      this.verifyCounterButton.Click += new System.EventHandler(this.verifyButton_Click);
      // 
      // counterField
      // 
      this.counterField.AllowDrop = true;
      this.counterField.CausesValidation = false;
      this.counterField.Location = new System.Drawing.Point(178, 89);
      this.counterField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.counterField.Name = "counterField";
      this.counterField.Size = new System.Drawing.Size(116, 26);
      this.counterField.TabIndex = 0;
      // 
      // step4CounterLabel
      // 
      this.step4CounterLabel.Location = new System.Drawing.Point(34, 18);
      this.step4CounterLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.step4CounterLabel.Name = "step4CounterLabel";
      this.step4CounterLabel.Size = new System.Drawing.Size(634, 66);
      this.step4CounterLabel.TabIndex = 10;
      this.step4CounterLabel.Text = "3. Enter the initial counter value if known. Click the Verify button that will sh" + "ow the last code that was used.";
      // 
      // hashField
      // 
      this.hashField.FormattingEnabled = true;
      this.hashField.ItemHeight = 20;
      this.hashField.Items.AddRange(new object[] {"HMAC-SHA1", "HMAC-SHA256", "HMAC-SHA512"});
      this.hashField.Location = new System.Drawing.Point(269, 357);
      this.hashField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.hashField.Name = "hashField";
      this.hashField.Size = new System.Drawing.Size(146, 28);
      this.hashField.TabIndex = 7;
      // 
      // intervalField
      // 
      this.intervalField.AllowDrop = true;
      this.intervalField.CausesValidation = false;
      this.intervalField.Location = new System.Drawing.Point(269, 410);
      this.intervalField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.intervalField.Name = "intervalField";
      this.intervalField.Size = new System.Drawing.Size(142, 26);
      this.intervalField.TabIndex = 8;
      // 
      // labelTYpe
      // 
      this.labelTYpe.Location = new System.Drawing.Point(113, 313);
      this.labelTYpe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.labelTYpe.Name = "labelTYpe";
      this.labelTYpe.Size = new System.Drawing.Size(108, 32);
      this.labelTYpe.TabIndex = 10;
      this.labelTYpe.Text = "Type";
      // 
      // hashLabel
      // 
      this.hashLabel.Location = new System.Drawing.Point(113, 363);
      this.hashLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.hashLabel.Name = "hashLabel";
      this.hashLabel.Size = new System.Drawing.Size(108, 32);
      this.hashLabel.TabIndex = 10;
      this.hashLabel.Text = "Hash";
      // 
      // periodLabel
      // 
      this.periodLabel.Location = new System.Drawing.Point(113, 410);
      this.periodLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.periodLabel.Name = "periodLabel";
      this.periodLabel.Size = new System.Drawing.Size(108, 32);
      this.periodLabel.TabIndex = 10;
      this.periodLabel.Text = "Interval";
      // 
      // digitsField
      // 
      this.digitsField.AllowDrop = true;
      this.digitsField.CausesValidation = false;
      this.digitsField.Location = new System.Drawing.Point(269, 453);
      this.digitsField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.digitsField.Name = "digitsField";
      this.digitsField.Size = new System.Drawing.Size(142, 26);
      this.digitsField.TabIndex = 9;
      // 
      // digitsLabel
      // 
      this.digitsLabel.Location = new System.Drawing.Point(113, 453);
      this.digitsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.digitsLabel.Name = "digitsLabel";
      this.digitsLabel.Size = new System.Drawing.Size(108, 32);
      this.digitsLabel.TabIndex = 10;
      this.digitsLabel.Text = "Digits";
      // 
      // intervalLabelPost
      // 
      this.intervalLabelPost.Location = new System.Drawing.Point(428, 410);
      this.intervalLabelPost.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.intervalLabelPost.Name = "intervalLabelPost";
      this.intervalLabelPost.Size = new System.Drawing.Size(194, 32);
      this.intervalLabelPost.TabIndex = 10;
      this.intervalLabelPost.Text = "seconds";
      // 
      // AddAuthenticator
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(698, 850);
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
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AddAuthenticator";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Add Authenticator";
      this.Load += new System.EventHandler(this.AddAuthenticator_Load);
      this.timeBasedPanel.ResumeLayout(false);
      this.counterBasedPanel.ResumeLayout(false);
      this.counterBasedPanel.PerformLayout();
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
    private System.Windows.Forms.Label step5Label;
    private System.Windows.Forms.Timer timer;
    private Label step4TimerLabel;
    private System.Windows.Forms.RadioButton timeBasedRadio;
    private System.Windows.Forms.RadioButton counterBasedRadio;
    private System.Windows.Forms.Panel timeBasedPanel;
    private System.Windows.Forms.Panel counterBasedPanel;
    private Label step4CounterLabel;
    private TextBox counterField;
    private Button verifyCounterButton;
    private System.Windows.Forms.ComboBox hashField;
    private System.Windows.Forms.TextBox intervalField;
    private System.Windows.Forms.Label labelTYpe;
    private System.Windows.Forms.Label hashLabel;
    private System.Windows.Forms.Label periodLabel;
    private System.Windows.Forms.TextBox digitsField;
    private System.Windows.Forms.Label digitsLabel;
    private System.Windows.Forms.Label intervalLabelPost;
  }
}