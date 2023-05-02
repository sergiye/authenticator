using System.Windows.Forms;

namespace Authenticator {
  partial class ShowRestoreCodeForm {
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowRestoreCodeForm));
      this.allowCopyCheckBox = new System.Windows.Forms.CheckBox();
      this.serialNumberField = new SecretTextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.restoreCodeField = new SecretTextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.btnClose = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // allowCopyCheckBox
      // 
      this.allowCopyCheckBox.AutoSize = true;
      this.allowCopyCheckBox.Location = new System.Drawing.Point(365, 157);
      this.allowCopyCheckBox.Name = "allowCopyCheckBox";
      this.allowCopyCheckBox.Size = new System.Drawing.Size(77, 17);
      this.allowCopyCheckBox.TabIndex = 4;
      this.allowCopyCheckBox.Text = "Allow copy";
      this.allowCopyCheckBox.CheckedChanged += new System.EventHandler(this.allowCopyCheckBox_CheckedChanged);
      // 
      // serialNumberField
      // 
      this.serialNumberField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
      this.serialNumberField.Location = new System.Drawing.Point(135, 151);
      this.serialNumberField.Multiline = true;
      this.serialNumberField.Name = "serialNumberField";
      this.serialNumberField.SecretMode = false;
      this.serialNumberField.Size = new System.Drawing.Size(224, 30);
      this.serialNumberField.SpaceOut = 0;
      this.serialNumberField.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(12, 155);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(118, 19);
      this.label1.TabIndex = 1;
      this.label1.Text = "Serial Number";
      // 
      // restoreCodeField
      // 
      this.restoreCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
      this.restoreCodeField.Location = new System.Drawing.Point(135, 188);
      this.restoreCodeField.Multiline = true;
      this.restoreCodeField.Name = "restoreCodeField";
      this.restoreCodeField.SecretMode = false;
      this.restoreCodeField.Size = new System.Drawing.Size(224, 30);
      this.restoreCodeField.SpaceOut = 0;
      this.restoreCodeField.TabIndex = 2;
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(12, 193);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(118, 19);
      this.label2.TabIndex = 1;
      this.label2.Text = "Restore Code";
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label4.Location = new System.Drawing.Point(12, 9);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(459, 134);
      this.label4.TabIndex = 1;
      this.label4.Text = resources.GetString("label4.Text");
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClose.Location = new System.Drawing.Point(396, 229);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 4;
      this.btnClose.Text = "Close";
      // 
      // ShowRestoreCodeForm
      // 
      this.AcceptButton = this.btnClose;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.ClientSize = new System.Drawing.Size(483, 264);
      this.Controls.Add(this.allowCopyCheckBox);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.serialNumberField);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.restoreCodeField);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label2);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ShowRestoreCodeForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Battle.net Restore Code";
      this.Load += new System.EventHandler(this.ShowRestoreCodeForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Button btnClose;
    private Label label4;
    private SecretTextBox restoreCodeField;
    private Label label2;
    private SecretTextBox serialNumberField;
    private Label label1;
    private CheckBox allowCopyCheckBox;
  }
}