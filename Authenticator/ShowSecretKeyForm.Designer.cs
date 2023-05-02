using System.Windows.Forms;

namespace Authenticator {
  partial class ShowSecretKeyForm {
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowSecretKeyForm));
      this.allowCopyCheckBox = new System.Windows.Forms.CheckBox();
      this.secretKeyField = new SecretTextBox();
      this.qrImage = new System.Windows.Forms.PictureBox();
      this.label4 = new System.Windows.Forms.Label();
      this.btnClose = new System.Windows.Forms.Button();
      this.metroLabel1 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.qrImage)).BeginInit();
      this.SuspendLayout();
      // 
      // allowCopyCheckBox
      // 
      this.allowCopyCheckBox.AutoSize = true;
      this.allowCopyCheckBox.Location = new System.Drawing.Point(12, 177);
      this.allowCopyCheckBox.Name = "allowCopyCheckBox";
      this.allowCopyCheckBox.Size = new System.Drawing.Size(77, 17);
      this.allowCopyCheckBox.TabIndex = 5;
      this.allowCopyCheckBox.Text = "Allow copy";
      this.allowCopyCheckBox.CheckedChanged += new System.EventHandler(this.allowCopyCheckBox_CheckedChanged);
      // 
      // secretKeyField
      // 
      this.secretKeyField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.secretKeyField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
      this.secretKeyField.Location = new System.Drawing.Point(12, 121);
      this.secretKeyField.Multiline = true;
      this.secretKeyField.Name = "secretKeyField";
      this.secretKeyField.SecretMode = false;
      this.secretKeyField.Size = new System.Drawing.Size(414, 50);
      this.secretKeyField.SpaceOut = 0;
      this.secretKeyField.TabIndex = 2;
      // 
      // qrImage
      // 
      this.qrImage.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.qrImage.BackColor = System.Drawing.SystemColors.ControlDark;
      this.qrImage.Location = new System.Drawing.Point(144, 248);
      this.qrImage.Name = "qrImage";
      this.qrImage.Size = new System.Drawing.Size(150, 150);
      this.qrImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.qrImage.TabIndex = 3;
      this.qrImage.TabStop = false;
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label4.Location = new System.Drawing.Point(12, 9);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(414, 109);
      this.label4.TabIndex = 1;
      this.label4.Text = resources.GetString("label4.Text");
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClose.Location = new System.Drawing.Point(351, 429);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 4;
      this.btnClose.Text = "Close";
      this.btnClose.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // metroLabel1
      // 
      this.metroLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.metroLabel1.Location = new System.Drawing.Point(12, 212);
      this.metroLabel1.Name = "metroLabel1";
      this.metroLabel1.Size = new System.Drawing.Size(414, 33);
      this.metroLabel1.TabIndex = 1;
      this.metroLabel1.Text = "You can also scan the QR code with your mobile device.";
      // 
      // ShowSecretKeyForm
      // 
      this.AcceptButton = this.btnClose;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.ClientSize = new System.Drawing.Size(438, 464);
      this.Controls.Add(this.allowCopyCheckBox);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.secretKeyField);
      this.Controls.Add(this.qrImage);
      this.Controls.Add(this.metroLabel1);
      this.Controls.Add(this.label4);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ShowSecretKeyForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Secret Key";
      this.Load += new System.EventHandler(this.ShowSecretKeyForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.qrImage)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnClose;
    private Label label4;
    private SecretTextBox secretKeyField;
    private System.Windows.Forms.PictureBox qrImage;
    private CheckBox allowCopyCheckBox;
    private Label metroLabel1;
  }
}