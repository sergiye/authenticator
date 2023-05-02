using System.Windows.Forms;

namespace Authenticator
{
	partial class MainForm
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
      this.mainTimer = new System.Windows.Forms.Timer(this.components);
      this.authenticatorMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addAuthenticatorMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.optionsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.passwordTimer = new System.Windows.Forms.Timer(this.components);
      this.hotkeyTimer = new System.Windows.Forms.Timer(this.components);
      this.passwordPanel = new System.Windows.Forms.Panel();
      this.passwordButton = new System.Windows.Forms.Button();
      this.passwordErrorLabel = new System.Windows.Forms.Label();
      this.passwordLabel = new System.Windows.Forms.Label();
      this.passwordField = new System.Windows.Forms.TextBox();
      this.introLabel = new System.Windows.Forms.Label();
      this.commandPanel = new System.Windows.Forms.Panel();
      this.optionsButton = new System.Windows.Forms.Button();
      this.addAuthenticatorButton = new System.Windows.Forms.Button();
      this.authenticatorList = new AuthenticatorListBox();
      this.loadingPanel = new System.Windows.Forms.Panel();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.authenticatorMenu.SuspendLayout();
      this.passwordPanel.SuspendLayout();
      this.commandPanel.SuspendLayout();
      this.loadingPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
      // addAuthenticatorMenu
      // 
      this.addAuthenticatorMenu.Name = "addMenu";
      this.addAuthenticatorMenu.Size = new System.Drawing.Size(61, 4);
      // 
      // optionsMenu
      // 
      this.optionsMenu.Name = "addMenu";
      this.optionsMenu.Size = new System.Drawing.Size(61, 4);
      this.optionsMenu.Opening += new System.ComponentModel.CancelEventHandler(this.optionsMenu_Opening);
      // 
      // notifyIcon
      // 
      this.notifyIcon.ContextMenuStrip = this.notifyMenu;
      this.notifyIcon.Text = "Authenticator";
      this.notifyIcon.Visible = true;
      this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
      // 
      // notifyMenu
      // 
      this.notifyMenu.Name = "notifyMenu";
      this.notifyMenu.Size = new System.Drawing.Size(61, 4);
      this.notifyMenu.Opening += new System.ComponentModel.CancelEventHandler(this.notifyMenu_Opening);
      // 
      // passwordTimer
      // 
      this.passwordTimer.Interval = 500;
      this.passwordTimer.Tick += new System.EventHandler(this.passwordTimer_Tick);
      // 
      // hotkeyTimer
      // 
      this.hotkeyTimer.Interval = 250;
      this.hotkeyTimer.Tick += new System.EventHandler(this.hotkeyTimer_Tick);
      // 
      // passwordPanel
      // 
      this.passwordPanel.Controls.Add(this.passwordButton);
      this.passwordPanel.Controls.Add(this.passwordErrorLabel);
      this.passwordPanel.Controls.Add(this.passwordLabel);
      this.passwordPanel.Controls.Add(this.passwordField);
      this.passwordPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.passwordPanel.Location = new System.Drawing.Point(0, 0);
      this.passwordPanel.Name = "passwordPanel";
      this.passwordPanel.Size = new System.Drawing.Size(420, 145);
      this.passwordPanel.TabIndex = 4;
      // 
      // passwordButton
      // 
      this.passwordButton.Location = new System.Drawing.Point(278, 40);
      this.passwordButton.Name = "passwordButton";
      this.passwordButton.Size = new System.Drawing.Size(75, 23);
      this.passwordButton.TabIndex = 1;
      this.passwordButton.Text = "OK";
      this.passwordButton.Click += new System.EventHandler(this.passwordButton_Click);
      // 
      // passwordErrorLabel
      // 
      this.passwordErrorLabel.ForeColor = System.Drawing.Color.Red;
      this.passwordErrorLabel.Location = new System.Drawing.Point(27, 68);
      this.passwordErrorLabel.Name = "passwordErrorLabel";
      this.passwordErrorLabel.Size = new System.Drawing.Size(326, 19);
      this.passwordErrorLabel.TabIndex = 3;
      // 
      // passwordLabel
      // 
      this.passwordLabel.AutoSize = true;
      this.passwordLabel.Location = new System.Drawing.Point(24, 16);
      this.passwordLabel.Name = "passwordLabel";
      this.passwordLabel.Size = new System.Drawing.Size(53, 13);
      this.passwordLabel.TabIndex = 3;
      this.passwordLabel.Text = "Password";
      // 
      // passwordField
      // 
      this.passwordField.Location = new System.Drawing.Point(27, 42);
      this.passwordField.Name = "passwordField";
      this.passwordField.PasswordChar = '●';
      this.passwordField.Size = new System.Drawing.Size(236, 20);
      this.passwordField.TabIndex = 0;
      this.passwordField.UseSystemPasswordChar = true;
      this.passwordField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.passwordField_KeyPress);
      // 
      // introLabel
      // 
      this.introLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.introLabel.Location = new System.Drawing.Point(0, 0);
      this.introLabel.Name = "introLabel";
      this.introLabel.Size = new System.Drawing.Size(420, 145);
      this.introLabel.TabIndex = 3;
      this.introLabel.Text = "Click the \"Add\" button to create or import your authenticator";
      this.introLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.introLabel.Visible = false;
      // 
      // commandPanel
      // 
      this.commandPanel.Controls.Add(this.optionsButton);
      this.commandPanel.Controls.Add(this.addAuthenticatorButton);
      this.commandPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.commandPanel.Location = new System.Drawing.Point(0, 145);
      this.commandPanel.Name = "commandPanel";
      this.commandPanel.Size = new System.Drawing.Size(420, 35);
      this.commandPanel.TabIndex = 1;
      this.commandPanel.Visible = false;
      this.commandPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.commandPanel_MouseDown);
      // 
      // optionsButton
      // 
      this.optionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.optionsButton.BackColor = System.Drawing.SystemColors.ControlLight;
      this.optionsButton.BackgroundImage = global::Authenticator.Properties.Resources.OptionsIcon;
      this.optionsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.optionsButton.Location = new System.Drawing.Point(389, 6);
      this.optionsButton.Name = "optionsButton";
      this.optionsButton.Size = new System.Drawing.Size(28, 23);
      this.optionsButton.TabIndex = 0;
      this.optionsButton.UseVisualStyleBackColor = false;
      this.optionsButton.Click += new System.EventHandler(this.optionsButton_Click);
      // 
      // addAuthenticatorButton
      // 
      this.addAuthenticatorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.addAuthenticatorButton.Location = new System.Drawing.Point(3, 6);
      this.addAuthenticatorButton.Name = "addAuthenticatorButton";
      this.addAuthenticatorButton.Size = new System.Drawing.Size(70, 23);
      this.addAuthenticatorButton.TabIndex = 0;
      this.addAuthenticatorButton.Text = "Add";
      this.addAuthenticatorButton.Click += new System.EventHandler(this.addAuthenticatorButton_Click);
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
      this.authenticatorList.Size = new System.Drawing.Size(420, 180);
      this.authenticatorList.TabIndex = 0;
      this.authenticatorList.TabStop = false;
      this.authenticatorList.Visible = false;
      this.authenticatorList.ItemRemoved += new AuthenticatorListItemRemovedHandler(this.authenticatorList_ItemRemoved);
      this.authenticatorList.Reordered += new AuthenticatorListReorderedHandler(this.authenticatorList_Reordered);
      this.authenticatorList.DoubleClick += new AuthenticatorListDoubleClickHandler(this.authenticatorList_DoubleClick);
      // 
      // loadingPanel
      // 
      this.loadingPanel.Controls.Add(this.pictureBox1);
      this.loadingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.loadingPanel.Location = new System.Drawing.Point(0, 0);
      this.loadingPanel.Name = "loadingPanel";
      this.loadingPanel.Size = new System.Drawing.Size(420, 145);
      this.loadingPanel.TabIndex = 6;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = global::Authenticator.Properties.Resources.spinner24;
      this.pictureBox1.Location = new System.Drawing.Point(187, 48);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(25, 25);
      this.pictureBox1.TabIndex = 2;
      this.pictureBox1.TabStop = false;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.ClientSize = new System.Drawing.Size(420, 180);
      this.Controls.Add(this.loadingPanel);
      this.Controls.Add(this.passwordPanel);
      this.Controls.Add(this.introLabel);
      this.Controls.Add(this.commandPanel);
      this.Controls.Add(this.authenticatorList);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(1620, 1080);
      this.MinimumSize = new System.Drawing.Size(300, 180);
      this.Name = "MainForm";
      this.Text = "Authenticator";
      this.TopMost = true;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.Shown += new System.EventHandler(this.MainForm_Shown);
      this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
      this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
      this.Resize += new System.EventHandler(this.MainForm_Resize);
      this.authenticatorMenu.ResumeLayout(false);
      this.passwordPanel.ResumeLayout(false);
      this.passwordPanel.PerformLayout();
      this.commandPanel.ResumeLayout(false);
      this.loadingPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);

    }

		#endregion

		private AuthenticatorListBox authenticatorList;
		private System.Windows.Forms.Timer mainTimer;
		private System.Windows.Forms.ContextMenuStrip authenticatorMenu;
		private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
		private Panel commandPanel;
		private Button addAuthenticatorButton;
		private System.Windows.Forms.ContextMenuStrip addAuthenticatorMenu;
		private Button optionsButton;
		private System.Windows.Forms.ContextMenuStrip optionsMenu;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private Label introLabel;
		private Panel passwordPanel;
		private TextBox passwordField;
		private Button passwordButton;
		private Label passwordErrorLabel;
		private System.Windows.Forms.Timer passwordTimer;
		private Label passwordLabel;
		private System.Windows.Forms.ContextMenuStrip notifyMenu;
		private System.Windows.Forms.Timer hotkeyTimer;
		private Panel loadingPanel;
		private System.Windows.Forms.PictureBox pictureBox1;

  }
}

