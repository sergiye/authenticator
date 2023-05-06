namespace Authenticator
{
	partial class ExceptionForm
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
      this.dataText = new System.Windows.Forms.TextBox();
      this.errorLabel = new System.Windows.Forms.Label();
      this.closeButton = new System.Windows.Forms.Button();
      this.panControls = new System.Windows.Forms.Panel();
      this.panControls.SuspendLayout();
      this.SuspendLayout();
      // 
      // dataText
      // 
      this.dataText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataText.Location = new System.Drawing.Point(0, 131);
      this.dataText.Margin = new System.Windows.Forms.Padding(4);
      this.dataText.Multiline = true;
      this.dataText.Name = "dataText";
      this.dataText.ReadOnly = true;
      this.dataText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.dataText.Size = new System.Drawing.Size(624, 260);
      this.dataText.TabIndex = 5;
      // 
      // errorLabel
      // 
      this.errorLabel.Dock = System.Windows.Forms.DockStyle.Top;
      this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.errorLabel.Location = new System.Drawing.Point(0, 0);
      this.errorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.errorLabel.Name = "errorLabel";
      this.errorLabel.Size = new System.Drawing.Size(624, 131);
      this.errorLabel.TabIndex = 4;
      this.errorLabel.Text = "An error has occured.\r\n\r\n{0}\r\n\r\nSome diagnostic information  that might help trac" +
    "k down issues can be found below:";
      // 
      // closeButton
      // 
      this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.closeButton.Location = new System.Drawing.Point(511, 9);
      this.closeButton.Margin = new System.Windows.Forms.Padding(4);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new System.Drawing.Size(100, 28);
      this.closeButton.TabIndex = 0;
      this.closeButton.Text = "Close";
      this.closeButton.UseVisualStyleBackColor = true;
      this.closeButton.Click += new System.EventHandler(this.quitButton_Click);
      // 
      // panControls
      // 
      this.panControls.Controls.Add(this.closeButton);
      this.panControls.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panControls.Location = new System.Drawing.Point(0, 391);
      this.panControls.Name = "panControls";
      this.panControls.Size = new System.Drawing.Size(624, 50);
      this.panControls.TabIndex = 8;
      // 
      // ExceptionForm
      // 
      this.AcceptButton = this.closeButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.CancelButton = this.closeButton;
      this.ClientSize = new System.Drawing.Size(624, 441);
      this.Controls.Add(this.dataText);
      this.Controls.Add(this.errorLabel);
      this.Controls.Add(this.panControls);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(640, 480);
      this.Name = "ExceptionForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Authenticator Error";
      this.Load += new System.EventHandler(this.ExceptionForm_Load);
      this.panControls.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Label errorLabel;
		private System.Windows.Forms.TextBox dataText;
    private System.Windows.Forms.Panel panControls;
  }
}