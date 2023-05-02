using System.Windows.Forms;

namespace Authenticator
{
	partial class ShowSteamTradesForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowSteamTradesForm));
      this.loginButton = new System.Windows.Forms.Button();
      this.loginTabLabel = new System.Windows.Forms.Label();
      this.captchaButton = new System.Windows.Forms.Button();
      this.captchacodeField = new System.Windows.Forms.TextBox();
      this.usernameField = new System.Windows.Forms.TextBox();
      this.captchaTabLabel = new System.Windows.Forms.Label();
      this.cancelButton = new System.Windows.Forms.Button();
      this.tabs = new System.Windows.Forms.TabControl();
      this.loginTab = new System.Windows.Forms.TabPage();
      this.rememberBox = new System.Windows.Forms.CheckBox();
      this.captchaGroup = new System.Windows.Forms.Panel();
      this.captchaBox = new System.Windows.Forms.PictureBox();
      this.passwordField = new System.Windows.Forms.TextBox();
      this.passwordLabel = new System.Windows.Forms.Label();
      this.usernameLabel = new System.Windows.Forms.Label();
      this.tradesTab = new System.Windows.Forms.TabPage();
      this.pollPanel = new System.Windows.Forms.Panel();
      this.pollAction = new System.Windows.Forms.ComboBox();
      this.pollCheckbox = new System.Windows.Forms.CheckBox();
      this.pollNumeric = new System.Windows.Forms.NumericUpDown();
      this.pollMinuesLabel = new System.Windows.Forms.Label();
      this.tradesContainer = new System.Windows.Forms.Panel();
      this.tradePanelMaster = new System.Windows.Forms.Panel();
      this.tradeSep = new System.Windows.Forms.PictureBox();
      this.tradeLabel = new System.Windows.Forms.Label();
      this.tradeImage = new System.Windows.Forms.PictureBox();
      this.tradeReject = new System.Windows.Forms.Button();
      this.tradeAccept = new System.Windows.Forms.Button();
      this.tradeStatus = new System.Windows.Forms.Label();
      this.tradesEmptyLabel = new System.Windows.Forms.Label();
      this.browserContainer = new System.Windows.Forms.Panel();
      this.closeButton = new System.Windows.Forms.Button();
      this.refreshButton = new System.Windows.Forms.Button();
      this.logoutButton = new System.Windows.Forms.Button();
      this.confirmAllButton = new System.Windows.Forms.Button();
      this.cancelAllButton = new System.Windows.Forms.Button();
      this.tabs.SuspendLayout();
      this.loginTab.SuspendLayout();
      this.captchaGroup.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.captchaBox)).BeginInit();
      this.tradesTab.SuspendLayout();
      this.pollPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pollNumeric)).BeginInit();
      this.tradesContainer.SuspendLayout();
      this.tradePanelMaster.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tradeSep)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.tradeImage)).BeginInit();
      this.SuspendLayout();
      // 
      // loginButton
      // 
      this.loginButton.Location = new System.Drawing.Point(104, 126);
      this.loginButton.Name = "loginButton";
      this.loginButton.Size = new System.Drawing.Size(110, 24);
      this.loginButton.TabIndex = 3;
      this.loginButton.Text = "Login";
      this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
      // 
      // loginTabLabel
      // 
      this.loginTabLabel.Location = new System.Drawing.Point(7, 10);
      this.loginTabLabel.Name = "loginTabLabel";
      this.loginTabLabel.Size = new System.Drawing.Size(431, 29);
      this.loginTabLabel.TabIndex = 0;
      this.loginTabLabel.Text = "Enter your steam username and password.";
      // 
      // captchaButton
      // 
      this.captchaButton.Location = new System.Drawing.Point(97, 118);
      this.captchaButton.Name = "captchaButton";
      this.captchaButton.Size = new System.Drawing.Size(110, 23);
      this.captchaButton.TabIndex = 4;
      this.captchaButton.Text = "Login";
      this.captchaButton.Click += new System.EventHandler(this.captchaButton_Click);
      // 
      // captchacodeField
      // 
      this.captchacodeField.Location = new System.Drawing.Point(97, 78);
      this.captchacodeField.Name = "captchacodeField";
      this.captchacodeField.Size = new System.Drawing.Size(206, 20);
      this.captchacodeField.TabIndex = 3;
      // 
      // usernameField
      // 
      this.usernameField.Location = new System.Drawing.Point(104, 41);
      this.usernameField.Name = "usernameField";
      this.usernameField.Size = new System.Drawing.Size(177, 20);
      this.usernameField.TabIndex = 0;
      // 
      // captchaTabLabel
      // 
      this.captchaTabLabel.AutoSize = true;
      this.captchaTabLabel.Location = new System.Drawing.Point(97, 10);
      this.captchaTabLabel.Name = "captchaTabLabel";
      this.captchaTabLabel.Size = new System.Drawing.Size(203, 13);
      this.captchaTabLabel.TabIndex = 0;
      this.captchaTabLabel.Text = "Enter the characters you see in the image";
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(390, 419);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 0;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // tabs
      // 
      this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabs.Controls.Add(this.loginTab);
      this.tabs.Controls.Add(this.tradesTab);
      this.tabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
      this.tabs.ItemSize = new System.Drawing.Size(120, 18);
      this.tabs.Location = new System.Drawing.Point(12, 12);
      this.tabs.Name = "tabs";
      this.tabs.SelectedIndex = 1;
      this.tabs.Size = new System.Drawing.Size(453, 384);
      this.tabs.TabIndex = 0;
      this.tabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
      // 
      // loginTab
      // 
      this.loginTab.BackColor = System.Drawing.SystemColors.Control;
      this.loginTab.Controls.Add(this.rememberBox);
      this.loginTab.Controls.Add(this.captchaGroup);
      this.loginTab.Controls.Add(this.loginButton);
      this.loginTab.Controls.Add(this.passwordField);
      this.loginTab.Controls.Add(this.usernameField);
      this.loginTab.Controls.Add(this.passwordLabel);
      this.loginTab.Controls.Add(this.usernameLabel);
      this.loginTab.Controls.Add(this.loginTabLabel);
      this.loginTab.ForeColor = System.Drawing.SystemColors.ControlText;
      this.loginTab.Location = new System.Drawing.Point(4, 22);
      this.loginTab.Name = "loginTab";
      this.loginTab.Padding = new System.Windows.Forms.Padding(3);
      this.loginTab.Size = new System.Drawing.Size(445, 358);
      this.loginTab.TabIndex = 0;
      this.loginTab.Tag = "";
      this.loginTab.Text = "Login";
      // 
      // rememberBox
      // 
      this.rememberBox.AutoSize = true;
      this.rememberBox.Location = new System.Drawing.Point(104, 97);
      this.rememberBox.Name = "rememberBox";
      this.rememberBox.Size = new System.Drawing.Size(95, 17);
      this.rememberBox.TabIndex = 2;
      this.rememberBox.Text = "remember me?";
      // 
      // captchaGroup
      // 
      this.captchaGroup.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.captchaGroup.Controls.Add(this.captchaBox);
      this.captchaGroup.Controls.Add(this.captchaTabLabel);
      this.captchaGroup.Controls.Add(this.captchacodeField);
      this.captchaGroup.Controls.Add(this.captchaButton);
      this.captchaGroup.Location = new System.Drawing.Point(7, 120);
      this.captchaGroup.Name = "captchaGroup";
      this.captchaGroup.Size = new System.Drawing.Size(431, 167);
      this.captchaGroup.TabIndex = 4;
      this.captchaGroup.Visible = false;
      // 
      // captchaBox
      // 
      this.captchaBox.Location = new System.Drawing.Point(97, 32);
      this.captchaBox.Name = "captchaBox";
      this.captchaBox.Size = new System.Drawing.Size(206, 40);
      this.captchaBox.TabIndex = 3;
      this.captchaBox.TabStop = false;
      // 
      // passwordField
      // 
      this.passwordField.Location = new System.Drawing.Point(104, 69);
      this.passwordField.Name = "passwordField";
      this.passwordField.PasswordChar = '●';
      this.passwordField.Size = new System.Drawing.Size(177, 20);
      this.passwordField.TabIndex = 1;
      this.passwordField.UseSystemPasswordChar = true;
      // 
      // passwordLabel
      // 
      this.passwordLabel.Location = new System.Drawing.Point(18, 69);
      this.passwordLabel.Name = "passwordLabel";
      this.passwordLabel.Size = new System.Drawing.Size(80, 25);
      this.passwordLabel.TabIndex = 1;
      this.passwordLabel.Text = "Password";
      // 
      // usernameLabel
      // 
      this.usernameLabel.Location = new System.Drawing.Point(18, 42);
      this.usernameLabel.Name = "usernameLabel";
      this.usernameLabel.Size = new System.Drawing.Size(80, 25);
      this.usernameLabel.TabIndex = 1;
      this.usernameLabel.Text = "Username";
      // 
      // tradesTab
      // 
      this.tradesTab.AutoScroll = true;
      this.tradesTab.Controls.Add(this.pollPanel);
      this.tradesTab.Controls.Add(this.tradesContainer);
      this.tradesTab.Controls.Add(this.browserContainer);
      this.tradesTab.Location = new System.Drawing.Point(4, 22);
      this.tradesTab.Name = "tradesTab";
      this.tradesTab.Size = new System.Drawing.Size(445, 358);
      this.tradesTab.TabIndex = 2;
      this.tradesTab.Tag = "";
      this.tradesTab.Text = "Confirmations";
      // 
      // pollPanel
      // 
      this.pollPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pollPanel.BackColor = System.Drawing.SystemColors.Window;
      this.pollPanel.Controls.Add(this.pollAction);
      this.pollPanel.Controls.Add(this.pollCheckbox);
      this.pollPanel.Controls.Add(this.pollNumeric);
      this.pollPanel.Controls.Add(this.pollMinuesLabel);
      this.pollPanel.Location = new System.Drawing.Point(5, 321);
      this.pollPanel.Name = "pollPanel";
      this.pollPanel.Size = new System.Drawing.Size(429, 82);
      this.pollPanel.TabIndex = 8;
      this.pollPanel.Visible = false;
      // 
      // pollAction
      // 
      this.pollAction.FormattingEnabled = true;
      this.pollAction.ItemHeight = 13;
      this.pollAction.Items.AddRange(new object[] {
            "Show a notification",
            "Show the Confirmations",
            "Auto-confirm",
            "Auto-cancel"});
      this.pollAction.Location = new System.Drawing.Point(250, 5);
      this.pollAction.Name = "pollAction";
      this.pollAction.Size = new System.Drawing.Size(173, 21);
      this.pollAction.TabIndex = 8;
      this.pollAction.SelectedIndexChanged += new System.EventHandler(this.pollAction_SelectedIndexChanged);
      // 
      // pollCheckbox
      // 
      this.pollCheckbox.AutoSize = true;
      this.pollCheckbox.Location = new System.Drawing.Point(3, 6);
      this.pollCheckbox.Name = "pollCheckbox";
      this.pollCheckbox.Size = new System.Drawing.Size(86, 17);
      this.pollCheckbox.TabIndex = 5;
      this.pollCheckbox.Text = "Check every";
      // 
      // pollNumeric
      // 
      this.pollNumeric.Location = new System.Drawing.Point(105, 8);
      this.pollNumeric.Maximum = new decimal(new int[] {
            10080,
            0,
            0,
            0});
      this.pollNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.pollNumeric.Name = "pollNumeric";
      this.pollNumeric.Size = new System.Drawing.Size(53, 20);
      this.pollNumeric.TabIndex = 6;
      this.pollNumeric.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
      // 
      // pollMinuesLabel
      // 
      this.pollMinuesLabel.AutoSize = true;
      this.pollMinuesLabel.Location = new System.Drawing.Point(164, 6);
      this.pollMinuesLabel.Name = "pollMinuesLabel";
      this.pollMinuesLabel.Size = new System.Drawing.Size(64, 13);
      this.pollMinuesLabel.TabIndex = 7;
      this.pollMinuesLabel.Text = "minutes and";
      // 
      // tradesContainer
      // 
      this.tradesContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tradesContainer.AutoScroll = true;
      this.tradesContainer.BackColor = System.Drawing.SystemColors.Window;
      this.tradesContainer.Controls.Add(this.tradePanelMaster);
      this.tradesContainer.Controls.Add(this.tradesEmptyLabel);
      this.tradesContainer.Location = new System.Drawing.Point(5, 5);
      this.tradesContainer.Name = "tradesContainer";
      this.tradesContainer.Size = new System.Drawing.Size(429, 193);
      this.tradesContainer.TabIndex = 5;
      // 
      // tradePanelMaster
      // 
      this.tradePanelMaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tradePanelMaster.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.tradePanelMaster.Controls.Add(this.tradeSep);
      this.tradePanelMaster.Controls.Add(this.tradeLabel);
      this.tradePanelMaster.Controls.Add(this.tradeImage);
      this.tradePanelMaster.Controls.Add(this.tradeReject);
      this.tradePanelMaster.Controls.Add(this.tradeAccept);
      this.tradePanelMaster.Controls.Add(this.tradeStatus);
      this.tradePanelMaster.Location = new System.Drawing.Point(0, 0);
      this.tradePanelMaster.Name = "tradePanelMaster";
      this.tradePanelMaster.Size = new System.Drawing.Size(429, 79);
      this.tradePanelMaster.TabIndex = 0;
      // 
      // tradeSep
      // 
      this.tradeSep.Image = global::Authenticator.Properties.Resources.BluePixel;
      this.tradeSep.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.tradeSep.Location = new System.Drawing.Point(0, 76);
      this.tradeSep.Name = "tradeSep";
      this.tradeSep.Size = new System.Drawing.Size(448, 1);
      this.tradeSep.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.tradeSep.TabIndex = 6;
      this.tradeSep.TabStop = false;
      // 
      // tradeLabel
      // 
      this.tradeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tradeLabel.Cursor = System.Windows.Forms.Cursors.Hand;
      this.tradeLabel.Location = new System.Drawing.Point(46, 10);
      this.tradeLabel.Name = "tradeLabel";
      this.tradeLabel.Size = new System.Drawing.Size(260, 61);
      this.tradeLabel.TabIndex = 4;
      this.tradeLabel.Text = "metroLabel1";
      // 
      // tradeImage
      // 
      this.tradeImage.Location = new System.Drawing.Point(4, 12);
      this.tradeImage.Name = "tradeImage";
      this.tradeImage.Size = new System.Drawing.Size(36, 36);
      this.tradeImage.TabIndex = 3;
      this.tradeImage.TabStop = false;
      // 
      // tradeReject
      // 
      this.tradeReject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.tradeReject.BackColor = System.Drawing.SystemColors.Control;
      this.tradeReject.Location = new System.Drawing.Point(337, 39);
      this.tradeReject.Name = "tradeReject";
      this.tradeReject.Size = new System.Drawing.Size(75, 23);
      this.tradeReject.TabIndex = 1;
      this.tradeReject.Text = "Cancel";
      this.tradeReject.UseVisualStyleBackColor = false;
      this.tradeReject.Click += new System.EventHandler(this.tradeReject_Click);
      // 
      // tradeAccept
      // 
      this.tradeAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.tradeAccept.BackColor = System.Drawing.SystemColors.Control;
      this.tradeAccept.Location = new System.Drawing.Point(337, 10);
      this.tradeAccept.Name = "tradeAccept";
      this.tradeAccept.Size = new System.Drawing.Size(75, 23);
      this.tradeAccept.TabIndex = 0;
      this.tradeAccept.Text = "Confirm";
      this.tradeAccept.UseVisualStyleBackColor = false;
      this.tradeAccept.Click += new System.EventHandler(this.tradeAccept_Click);
      // 
      // tradeStatus
      // 
      this.tradeStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.tradeStatus.Location = new System.Drawing.Point(312, 11);
      this.tradeStatus.Name = "tradeStatus";
      this.tradeStatus.Size = new System.Drawing.Size(99, 28);
      this.tradeStatus.TabIndex = 4;
      this.tradeStatus.Text = "tradeStatus";
      this.tradeStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
      this.tradeStatus.Visible = false;
      // 
      // tradesEmptyLabel
      // 
      this.tradesEmptyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tradesEmptyLabel.Location = new System.Drawing.Point(0, 0);
      this.tradesEmptyLabel.Name = "tradesEmptyLabel";
      this.tradesEmptyLabel.Size = new System.Drawing.Size(372, 29);
      this.tradesEmptyLabel.TabIndex = 4;
      this.tradesEmptyLabel.Text = "You have no trade confirmations";
      this.tradesEmptyLabel.Visible = false;
      // 
      // browserContainer
      // 
      this.browserContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.browserContainer.BackColor = System.Drawing.SystemColors.Window;
      this.browserContainer.Location = new System.Drawing.Point(5, 204);
      this.browserContainer.Name = "browserContainer";
      this.browserContainer.Size = new System.Drawing.Size(429, 120);
      this.browserContainer.TabIndex = 5;
      // 
      // closeButton
      // 
      this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.closeButton.Location = new System.Drawing.Point(311, 419);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new System.Drawing.Size(75, 23);
      this.closeButton.TabIndex = 1;
      this.closeButton.Text = "Close";
      this.closeButton.Visible = false;
      this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
      // 
      // refreshButton
      // 
      this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.refreshButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("refreshButton.BackgroundImage")));
      this.refreshButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
      this.refreshButton.Location = new System.Drawing.Point(12, 419);
      this.refreshButton.Name = "refreshButton";
      this.refreshButton.Size = new System.Drawing.Size(23, 23);
      this.refreshButton.TabIndex = 4;
      this.refreshButton.Visible = false;
      this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
      // 
      // logoutButton
      // 
      this.logoutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.logoutButton.Location = new System.Drawing.Point(41, 419);
      this.logoutButton.Name = "logoutButton";
      this.logoutButton.Size = new System.Drawing.Size(75, 23);
      this.logoutButton.TabIndex = 4;
      this.logoutButton.Text = "Forget me";
      this.logoutButton.Visible = false;
      this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
      // 
      // confirmAllButton
      // 
      this.confirmAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.confirmAllButton.Location = new System.Drawing.Point(123, 419);
      this.confirmAllButton.Name = "confirmAllButton";
      this.confirmAllButton.Size = new System.Drawing.Size(75, 23);
      this.confirmAllButton.TabIndex = 4;
      this.confirmAllButton.Text = "Confirm All";
      this.confirmAllButton.Visible = false;
      this.confirmAllButton.Click += new System.EventHandler(this.confirmAllButton_Click);
      // 
      // cancelAllButton
      // 
      this.cancelAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.cancelAllButton.Location = new System.Drawing.Point(204, 419);
      this.cancelAllButton.Name = "cancelAllButton";
      this.cancelAllButton.Size = new System.Drawing.Size(75, 23);
      this.cancelAllButton.TabIndex = 4;
      this.cancelAllButton.Text = "Cancel All";
      this.cancelAllButton.Visible = false;
      this.cancelAllButton.Click += new System.EventHandler(this.cancelAllButton_Click);
      // 
      // ShowSteamTradesForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(477, 454);
      this.Controls.Add(this.tabs);
      this.Controls.Add(this.refreshButton);
      this.Controls.Add(this.cancelAllButton);
      this.Controls.Add(this.confirmAllButton);
      this.Controls.Add(this.logoutButton);
      this.Controls.Add(this.closeButton);
      this.Controls.Add(this.cancelButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ShowSteamTradesForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Steam Confirmations";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShowSteamTradesForm_FormClosing);
      this.Load += new System.EventHandler(this.ShowSteamTradesForm_Load);
      this.Shown += new System.EventHandler(this.ShowSteamTradesForm_Shown);
      this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ShowSteamTradesForm_KeyPress);
      this.tabs.ResumeLayout(false);
      this.loginTab.ResumeLayout(false);
      this.loginTab.PerformLayout();
      this.captchaGroup.ResumeLayout(false);
      this.captchaGroup.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.captchaBox)).EndInit();
      this.tradesTab.ResumeLayout(false);
      this.pollPanel.ResumeLayout(false);
      this.pollPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pollNumeric)).EndInit();
      this.tradesContainer.ResumeLayout(false);
      this.tradePanelMaster.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.tradeSep)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.tradeImage)).EndInit();
      this.ResumeLayout(false);

		}

		#endregion

		private Label loginTabLabel;
		private Button loginButton;
		private Label captchaTabLabel;
		private Button cancelButton;
		private TextBox captchacodeField;
		private TextBox usernameField;
		private Button captchaButton;
		private TabControl tabs;
		private TabPage loginTab;
		private Label passwordLabel;
		private Label usernameLabel;
		private TextBox passwordField;
		private TabPage tradesTab;
		private System.Windows.Forms.PictureBox captchaBox;
		private System.Windows.Forms.Panel captchaGroup;
		private Button closeButton;
		private System.Windows.Forms.Panel tradePanelMaster;
		private System.Windows.Forms.PictureBox tradeImage;
		private Button tradeReject;
		private Button tradeAccept;
		private Label tradeLabel;
		private System.Windows.Forms.PictureBox tradeSep;
		private Label tradeStatus;
		private Label tradesEmptyLabel;
		private System.Windows.Forms.Panel browserContainer;
		private System.Windows.Forms.Panel tradesContainer;
		private CheckBox rememberBox;
		private Button refreshButton;
		private Button logoutButton;
		private Label pollMinuesLabel;
		private System.Windows.Forms.NumericUpDown pollNumeric;
		private CheckBox pollCheckbox;
		private System.Windows.Forms.Panel pollPanel;
		private ComboBox pollAction;
		private Button confirmAllButton;
		private Button cancelAllButton;
	}
}