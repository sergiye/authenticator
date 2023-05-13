using System.Windows.Forms;

namespace Authenticator {
  partial class MainForm {
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
      this.mainTimer = new System.Windows.Forms.Timer(this.components);
      this.authenticatorMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.passwordTimer = new System.Windows.Forms.Timer(this.components);
      this.passwordPanel = new System.Windows.Forms.Panel();
      this.passwordButton = new System.Windows.Forms.Button();
      this.passwordErrorLabel = new System.Windows.Forms.Label();
      this.passwordLabel = new System.Windows.Forms.Label();
      this.passwordField = new System.Windows.Forms.TextBox();
      this.introLabel = new System.Windows.Forms.Label();
      this.authenticatorList = new AuthenticatorListBox();
      this.loadingPanel = new System.Windows.Forms.Panel();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.mainMenu = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.authenticatorMenu.SuspendLayout();
      this.passwordPanel.SuspendLayout();
      this.loadingPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.mainMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainTimer
      // 
      this.mainTimer.Enabled = true;
      this.mainTimer.Interval = 500;
      this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
      // 
      // authenticatorMenu
      // 
      this.authenticatorMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.authenticatorMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
      this.authenticatorMenu.Name = "authenticatorMenu";
      this.authenticatorMenu.Size = new System.Drawing.Size(95, 26);
      // 
      // testToolStripMenuItem
      // 
      this.testToolStripMenuItem.Name = "testToolStripMenuItem";
      this.testToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
      this.testToolStripMenuItem.Text = "Test";
      // 
      // passwordTimer
      // 
      this.passwordTimer.Interval = 500;
      this.passwordTimer.Tick += new System.EventHandler(this.passwordTimer_Tick);
      // 
      // passwordPanel
      // 
      this.passwordPanel.BackColor = System.Drawing.SystemColors.Window;
      this.passwordPanel.Controls.Add(this.passwordButton);
      this.passwordPanel.Controls.Add(this.passwordErrorLabel);
      this.passwordPanel.Controls.Add(this.passwordLabel);
      this.passwordPanel.Controls.Add(this.passwordField);
      this.passwordPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.passwordPanel.Location = new System.Drawing.Point(0, 0);
      this.passwordPanel.Name = "passwordPanel";
      this.passwordPanel.Size = new System.Drawing.Size(297, 149);
      this.passwordPanel.TabIndex = 2;
      // 
      // passwordButton
      // 
      this.passwordButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.passwordButton.Location = new System.Drawing.Point(215, 114);
      this.passwordButton.Name = "passwordButton";
      this.passwordButton.Size = new System.Drawing.Size(75, 23);
      this.passwordButton.TabIndex = 3;
      this.passwordButton.Text = "OK";
      this.passwordButton.Click += new System.EventHandler(this.passwordButton_Click);
      // 
      // passwordErrorLabel
      // 
      this.passwordErrorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.passwordErrorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.passwordErrorLabel.ForeColor = System.Drawing.Color.Red;
      this.passwordErrorLabel.Location = new System.Drawing.Point(9, 92);
      this.passwordErrorLabel.Name = "passwordErrorLabel";
      this.passwordErrorLabel.Size = new System.Drawing.Size(281, 19);
      this.passwordErrorLabel.TabIndex = 2;
      // 
      // passwordLabel
      // 
      this.passwordLabel.AutoSize = true;
      this.passwordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.passwordLabel.Location = new System.Drawing.Point(3, 30);
      this.passwordLabel.Name = "passwordLabel";
      this.passwordLabel.Size = new System.Drawing.Size(235, 20);
      this.passwordLabel.TabIndex = 0;
      this.passwordLabel.Text = "Please verify your password:";
      // 
      // passwordField
      // 
      this.passwordField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.passwordField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.passwordField.Location = new System.Drawing.Point(7, 63);
      this.passwordField.Name = "passwordField";
      this.passwordField.PasswordChar = '●';
      this.passwordField.Size = new System.Drawing.Size(281, 26);
      this.passwordField.TabIndex = 1;
      this.passwordField.UseSystemPasswordChar = true;
      this.passwordField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.passwordField_KeyPress);
      // 
      // introLabel
      // 
      this.introLabel.BackColor = System.Drawing.SystemColors.Window;
      this.introLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.introLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.introLabel.Location = new System.Drawing.Point(0, 0);
      this.introLabel.Name = "introLabel";
      this.introLabel.Size = new System.Drawing.Size(297, 149);
      this.introLabel.TabIndex = 3;
      this.introLabel.Text = "Use \"Add\" menu item to create new authenticator or \"File\"-\"Import\" to import your" +
    " authenticators";
      this.introLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.introLabel.Visible = false;
      // 
      // authenticatorList
      // 
      this.authenticatorList.AllowDrop = true;
      this.authenticatorList.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.authenticatorList.CurrentItem = null;
      this.authenticatorList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.authenticatorList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.authenticatorList.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
      this.authenticatorList.IntegralHeight = false;
      this.authenticatorList.ItemHeight = 80;
      this.authenticatorList.Location = new System.Drawing.Point(0, 0);
      this.authenticatorList.Name = "authenticatorList";
      this.authenticatorList.ReadOnly = false;
      this.authenticatorList.SelectionMode = System.Windows.Forms.SelectionMode.None;
      this.authenticatorList.Size = new System.Drawing.Size(297, 149);
      this.authenticatorList.TabIndex = 0;
      this.authenticatorList.Visible = false;
      this.authenticatorList.ItemRemoved += new System.EventHandler<AuthenticatorListBox.ListItem>(this.authenticatorList_ItemRemoved);
      this.authenticatorList.Reordered += new System.EventHandler(this.authenticatorList_Reordered);
      this.authenticatorList.DoubleClick += new System.EventHandler<AuthAuthenticator>(this.authenticatorList_DoubleClick);
      // 
      // loadingPanel
      // 
      this.loadingPanel.BackColor = System.Drawing.SystemColors.Window;
      this.loadingPanel.Controls.Add(this.pictureBox1);
      this.loadingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.loadingPanel.Location = new System.Drawing.Point(0, 0);
      this.loadingPanel.Name = "loadingPanel";
      this.loadingPanel.Size = new System.Drawing.Size(297, 149);
      this.loadingPanel.TabIndex = 4;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.pictureBox1.Image = global::Authenticator.Properties.Resources.spinner24;
      this.pictureBox1.Location = new System.Drawing.Point(139, 64);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(25, 25);
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      // 
      // mainMenu
      // 
      this.mainMenu.BackColor = System.Drawing.SystemColors.Window;
      this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.addToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.mainMenu.Location = new System.Drawing.Point(0, 0);
      this.mainMenu.Name = "mainMenu";
      this.mainMenu.Size = new System.Drawing.Size(297, 24);
      this.mainMenu.TabIndex = 5;
      this.mainMenu.Visible = false;
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // addToolStripMenuItem
      // 
      this.addToolStripMenuItem.Name = "addToolStripMenuItem";
      this.addToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
      this.addToolStripMenuItem.Text = "Add";
      // 
      // optionsToolStripMenuItem
      // 
      this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
      this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
      this.optionsToolStripMenuItem.Text = "Options";
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
      this.helpToolStripMenuItem.Text = "Help";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.ClientSize = new System.Drawing.Size(297, 149);
      this.Controls.Add(this.passwordPanel);
      this.Controls.Add(this.introLabel);
      this.Controls.Add(this.loadingPanel);
      this.Controls.Add(this.authenticatorList);
      this.Controls.Add(this.mainMenu);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MainMenuStrip = this.mainMenu;
      this.MaximizeBox = false;
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Authenticator";
      this.TopMost = true;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.Shown += new System.EventHandler(this.MainForm_Shown);
      this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
      this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
      this.authenticatorMenu.ResumeLayout(false);
      this.passwordPanel.ResumeLayout(false);
      this.passwordPanel.PerformLayout();
      this.loadingPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.mainMenu.ResumeLayout(false);
      this.mainMenu.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private AuthenticatorListBox authenticatorList;
    private System.Windows.Forms.Timer mainTimer;
    private System.Windows.Forms.ContextMenuStrip authenticatorMenu;
    private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
    private System.Windows.Forms.Label introLabel;
    private System.Windows.Forms.Panel passwordPanel;
    private TextBox passwordField;
    private Button passwordButton;
    private Label passwordErrorLabel;
    private System.Windows.Forms.Timer passwordTimer;
    private System.Windows.Forms.Label passwordLabel;
    private System.Windows.Forms.Panel loadingPanel;
    private System.Windows.Forms.PictureBox pictureBox1;
    private MenuStrip mainMenu;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem addToolStripMenuItem;
    private ToolStripMenuItem optionsToolStripMenuItem;
    private ToolStripMenuItem helpToolStripMenuItem;
  }
}