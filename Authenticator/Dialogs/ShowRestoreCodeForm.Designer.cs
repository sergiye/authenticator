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

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowRestoreCodeForm));
      this.serialNumberField = new VCenteredTextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.restoreCodeField = new VCenteredTextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.btnClose = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // serialNumberField
      // 
      this.serialNumberField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
      this.serialNumberField.Location = new System.Drawing.Point(202, 232);
      this.serialNumberField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.serialNumberField.Name = "serialNumberField";
      this.serialNumberField.Size = new System.Drawing.Size(334, 44);
      this.serialNumberField.SpaceOut = 0;
      this.serialNumberField.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(18, 238);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(177, 29);
      this.label1.TabIndex = 1;
      this.label1.Text = "Serial Number";
      // 
      // restoreCodeField
      // 
      this.restoreCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
      this.restoreCodeField.Location = new System.Drawing.Point(202, 289);
      this.restoreCodeField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.restoreCodeField.Name = "restoreCodeField";
      this.restoreCodeField.Size = new System.Drawing.Size(334, 44);
      this.restoreCodeField.SpaceOut = 0;
      this.restoreCodeField.TabIndex = 2;
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(18, 297);
      this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(177, 29);
      this.label2.TabIndex = 1;
      this.label2.Text = "Restore Code";
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
      this.label4.Location = new System.Drawing.Point(18, 14);
      this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(688, 206);
      this.label4.TabIndex = 1;
      this.label4.Text = resources.GetString("label4.Text");
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClose.Location = new System.Drawing.Point(594, 352);
      this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(112, 35);
      this.btnClose.TabIndex = 4;
      this.btnClose.Text = "Close";
      // 
      // ShowRestoreCodeForm
      // 
      this.AcceptButton = this.btnClose;
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.CancelButton = this.btnClose;
      this.ClientSize = new System.Drawing.Size(724, 406);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.serialNumberField);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.restoreCodeField);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label2);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ShowRestoreCodeForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Battle.net Restore Code";
      this.Load += new System.EventHandler(this.ShowRestoreCodeForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Button btnClose;
    private Label label4;
    private VCenteredTextBox restoreCodeField;
    private Label label2;
    private VCenteredTextBox serialNumberField;
    private Label label1;
  }
}