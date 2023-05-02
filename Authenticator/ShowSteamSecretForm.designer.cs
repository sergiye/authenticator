using System.Windows.Forms;

namespace Authenticator {
  partial class ShowSteamSecretForm {
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      this.allowCopyCheckBox = new System.Windows.Forms.CheckBox();
      this.revocationcodeField = new SecretTextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.btnClose = new System.Windows.Forms.Button();
      this.steamdataField = new SecretTextBox();
      this.metroLabel2 = new System.Windows.Forms.Label();
      this.deviceidField = new SecretTextBox();
      this.metroLabel3 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // allowCopyCheckBox
      // 
      this.allowCopyCheckBox.AutoSize = true;
      this.allowCopyCheckBox.Location = new System.Drawing.Point(211, 102);
      this.allowCopyCheckBox.Name = "allowCopyCheckBox";
      this.allowCopyCheckBox.Size = new System.Drawing.Size(77, 17);
      this.allowCopyCheckBox.TabIndex = 1;
      this.allowCopyCheckBox.Text = "Allow copy";
      this.allowCopyCheckBox.CheckedChanged += new System.EventHandler(this.allowCopyCheckBox_CheckedChanged);
      // 
      // revocationcodeField
      // 
      this.revocationcodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
      this.revocationcodeField.Location = new System.Drawing.Point(93, 95);
      this.revocationcodeField.Multiline = true;
      this.revocationcodeField.Name = "revocationcodeField";
      this.revocationcodeField.SecretMode = false;
      this.revocationcodeField.Size = new System.Drawing.Size(112, 26);
      this.revocationcodeField.SpaceOut = 0;
      this.revocationcodeField.TabIndex = 0;
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(12, 98);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(75, 19);
      this.label2.TabIndex = 1;
      this.label2.Text = "Code";
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label4.Location = new System.Drawing.Point(12, 9);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(413, 72);
      this.label4.TabIndex = 1;
      this.label4.Text = "When your Authenticator was registered with Steam, a code was created that you ca" +
    "n use within the Steam client to remove your authenticator.\r\n\r\nCopy it down and " +
    "keep it somewhere safe.";
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClose.Location = new System.Drawing.Point(350, 330);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      // 
      // steamdataField
      // 
      this.steamdataField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.steamdataField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
      this.steamdataField.ForeColor = System.Drawing.SystemColors.InactiveCaption;
      this.steamdataField.Location = new System.Drawing.Point(12, 196);
      this.steamdataField.Multiline = true;
      this.steamdataField.Name = "steamdataField";
      this.steamdataField.SecretMode = false;
      this.steamdataField.Size = new System.Drawing.Size(413, 116);
      this.steamdataField.SpaceOut = 0;
      this.steamdataField.TabIndex = 0;
      this.steamdataField.TabStop = false;
      this.steamdataField.Text = "WARNING: There is no additional Steam data.\r\n\r\nYour authenticator was created in " +
    "an early version of Authenticator. You need to remove the authenticator from you" +
    "r Steam account and add a new one.";
      // 
      // metroLabel2
      // 
      this.metroLabel2.Location = new System.Drawing.Point(12, 164);
      this.metroLabel2.Name = "metroLabel2";
      this.metroLabel2.Size = new System.Drawing.Size(75, 19);
      this.metroLabel2.TabIndex = 1;
      this.metroLabel2.Text = "Device ID";
      // 
      // deviceidField
      // 
      this.deviceidField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.deviceidField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
      this.deviceidField.Location = new System.Drawing.Point(93, 161);
      this.deviceidField.Multiline = true;
      this.deviceidField.Name = "deviceidField";
      this.deviceidField.SecretMode = false;
      this.deviceidField.Size = new System.Drawing.Size(332, 26);
      this.deviceidField.SpaceOut = 0;
      this.deviceidField.TabIndex = 0;
      this.deviceidField.TabStop = false;
      // 
      // metroLabel3
      // 
      this.metroLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.metroLabel3.Location = new System.Drawing.Point(12, 124);
      this.metroLabel3.Name = "metroLabel3";
      this.metroLabel3.Size = new System.Drawing.Size(413, 34);
      this.metroLabel3.TabIndex = 1;
      this.metroLabel3.Text = "This is the full Steam data for your authenticator that can be used when importin" +
    "g into different software.";
      // 
      // ShowSteamSecretForm
      // 
      this.AcceptButton = this.btnClose;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.ClientSize = new System.Drawing.Size(437, 365);
      this.Controls.Add(this.allowCopyCheckBox);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.steamdataField);
      this.Controls.Add(this.deviceidField);
      this.Controls.Add(this.revocationcodeField);
      this.Controls.Add(this.metroLabel3);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.metroLabel2);
      this.Controls.Add(this.label2);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ShowSteamSecretForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Recovery Code";
      this.Load += new System.EventHandler(this.ShowSteamSecretForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Button btnClose;
    private Label label4;
    private SecretTextBox revocationcodeField;
    private Label label2;
    private CheckBox allowCopyCheckBox;
    private SecretTextBox steamdataField;
    private Label metroLabel2;
    private SecretTextBox deviceidField;
    private Label metroLabel3;
  }
}