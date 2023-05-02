using System.Windows.Forms;

namespace Authenticator {
  partial class AddSteamAuthenticator {
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddSteamAuthenticator));
      this.loginButton = new System.Windows.Forms.Button();
      this.authoriseTabLabel = new System.Windows.Forms.Label();
      this.loginTabLabel = new System.Windows.Forms.Label();
      this.captchaButton = new System.Windows.Forms.Button();
      this.captchacodeField = new System.Windows.Forms.TextBox();
      this.usernameField = new System.Windows.Forms.TextBox();
      this.captchaTabLabel = new System.Windows.Forms.Label();
      this.confirmButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.steamIconRadioButton = new GroupMetroRadioButton();
      this.steamAuthenticatorIconRadioButton = new GroupMetroRadioButton();
      this.iconLabel = new System.Windows.Forms.Label();
      this.nameLabel = new System.Windows.Forms.Label();
      this.nameField = new System.Windows.Forms.TextBox();
      this.tabs = new System.Windows.Forms.TabControl();
      this.loginTab = new System.Windows.Forms.TabPage();
      this.captchaGroup = new System.Windows.Forms.Panel();
      this.captchaBox = new System.Windows.Forms.PictureBox();
      this.passwordField = new System.Windows.Forms.TextBox();
      this.passwordLabel = new System.Windows.Forms.Label();
      this.usernameLabel = new System.Windows.Forms.Label();
      this.authTab = new System.Windows.Forms.TabPage();
      this.authcodeLabel = new System.Windows.Forms.Label();
      this.authcodeButton = new System.Windows.Forms.Button();
      this.authcodeField = new System.Windows.Forms.TextBox();
      this.confirmTab = new System.Windows.Forms.TabPage();
      this.revocationCheckbox = new System.Windows.Forms.CheckBox();
      this.revocationcodeCopy = new System.Windows.Forms.CheckBox();
      this.revocationcodeText = new System.Windows.Forms.Label();
      this.revocationcodeField = new SecretTextBox();
      this.revocationcodeLabel = new System.Windows.Forms.Label();
      this.activationcodeLabel = new System.Windows.Forms.Label();
      this.activationcodeField = new System.Windows.Forms.TextBox();
      this.confirmTabLabel = new System.Windows.Forms.Label();
      this.addedTab = new System.Windows.Forms.TabPage();
      this.revocationcode2Copy = new System.Windows.Forms.CheckBox();
      this.revocationcode2Label = new System.Windows.Forms.Label();
      this.revocationcode2Field = new SecretTextBox();
      this.metroLabel1 = new System.Windows.Forms.Label();
      this.importAndroidTab = new System.Windows.Forms.TabPage();
      this.importSteamguard = new System.Windows.Forms.TextBox();
      this.metroLabel4 = new System.Windows.Forms.Label();
      this.importUuid = new System.Windows.Forms.TextBox();
      this.metroLabel2 = new System.Windows.Forms.Label();
      this.metroLabel3 = new System.Windows.Forms.Label();
      this.importSDATab = new System.Windows.Forms.TabPage();
      this.metroLabel8 = new System.Windows.Forms.Label();
      this.metroLabel9 = new System.Windows.Forms.Label();
      this.metroLabel7 = new System.Windows.Forms.Label();
      this.metroLabel6 = new System.Windows.Forms.Label();
      this.importSDAList = new System.Windows.Forms.ListBox();
      this.importSDAPassword = new System.Windows.Forms.TextBox();
      this.importSDAPath = new System.Windows.Forms.TextBox();
      this.metroLabel5 = new System.Windows.Forms.Label();
      this.importSDALoad = new System.Windows.Forms.Button();
      this.importSDABrowse = new System.Windows.Forms.Button();
      this.closeButton = new System.Windows.Forms.Button();
      this.steamIcon = new System.Windows.Forms.PictureBox();
      this.steamAuthenticatorIcon = new System.Windows.Forms.PictureBox();
      this.tabs.SuspendLayout();
      this.loginTab.SuspendLayout();
      this.captchaGroup.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.captchaBox)).BeginInit();
      this.authTab.SuspendLayout();
      this.confirmTab.SuspendLayout();
      this.addedTab.SuspendLayout();
      this.importAndroidTab.SuspendLayout();
      this.importSDATab.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.steamIcon)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.steamAuthenticatorIcon)).BeginInit();
      this.SuspendLayout();
      // 
      // loginButton
      // 
      this.loginButton.Location = new System.Drawing.Point(104, 127);
      this.loginButton.Name = "loginButton";
      this.loginButton.Size = new System.Drawing.Size(110, 24);
      this.loginButton.TabIndex = 2;
      this.loginButton.Text = "Login";
      this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
      // 
      // authoriseTabLabel
      // 
      this.authoriseTabLabel.Location = new System.Drawing.Point(4, 12);
      this.authoriseTabLabel.Name = "authoriseTabLabel";
      this.authoriseTabLabel.Size = new System.Drawing.Size(449, 45);
      this.authoriseTabLabel.TabIndex = 1;
      this.authoriseTabLabel.Text = "Please enter the code sent to your {0} email address approving access from a new " +
    "device.";
      // 
      // loginTabLabel
      // 
      this.loginTabLabel.Location = new System.Drawing.Point(7, 10);
      this.loginTabLabel.Name = "loginTabLabel";
      this.loginTabLabel.Size = new System.Drawing.Size(431, 46);
      this.loginTabLabel.TabIndex = 0;
      this.loginTabLabel.Text = "Enter your steam username and password. This is needed to login to your account t" +
    "o add a new authenticator.";
      // 
      // captchaButton
      // 
      this.captchaButton.Location = new System.Drawing.Point(97, 118);
      this.captchaButton.Name = "captchaButton";
      this.captchaButton.Size = new System.Drawing.Size(110, 23);
      this.captchaButton.TabIndex = 1;
      this.captchaButton.Text = "Login";
      this.captchaButton.Click += new System.EventHandler(this.captchaButton_Click);
      // 
      // captchacodeField
      // 
      this.captchacodeField.Location = new System.Drawing.Point(97, 78);
      this.captchacodeField.Name = "captchacodeField";
      this.captchacodeField.Size = new System.Drawing.Size(206, 20);
      this.captchacodeField.TabIndex = 0;
      // 
      // usernameField
      // 
      this.usernameField.Location = new System.Drawing.Point(104, 59);
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
      // confirmButton
      // 
      this.confirmButton.Enabled = false;
      this.confirmButton.Location = new System.Drawing.Point(137, 243);
      this.confirmButton.Name = "confirmButton";
      this.confirmButton.Size = new System.Drawing.Size(116, 23);
      this.confirmButton.TabIndex = 2;
      this.confirmButton.Text = "Confirm";
      this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(387, 477);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 0;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // steamIconRadioButton
      // 
      this.steamIconRadioButton.Group = "ICON";
      this.steamIconRadioButton.Location = new System.Drawing.Point(145, 76);
      this.steamIconRadioButton.Name = "steamIconRadioButton";
      this.steamIconRadioButton.Size = new System.Drawing.Size(14, 13);
      this.steamIconRadioButton.TabIndex = 2;
      this.steamIconRadioButton.Tag = "SteamIcon.png";
      this.steamIconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // steamAuthenticatorIconRadioButton
      // 
      this.steamAuthenticatorIconRadioButton.Checked = true;
      this.steamAuthenticatorIconRadioButton.Group = "ICON";
      this.steamAuthenticatorIconRadioButton.Location = new System.Drawing.Point(69, 76);
      this.steamAuthenticatorIconRadioButton.Name = "steamAuthenticatorIconRadioButton";
      this.steamAuthenticatorIconRadioButton.Size = new System.Drawing.Size(14, 13);
      this.steamAuthenticatorIconRadioButton.TabIndex = 1;
      this.steamAuthenticatorIconRadioButton.TabStop = true;
      this.steamAuthenticatorIconRadioButton.Tag = "SteamAuthenticatorIcon.png";
      this.steamAuthenticatorIconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
      // 
      // iconLabel
      // 
      this.iconLabel.AutoSize = true;
      this.iconLabel.Location = new System.Drawing.Point(12, 72);
      this.iconLabel.Name = "iconLabel";
      this.iconLabel.Size = new System.Drawing.Size(31, 13);
      this.iconLabel.TabIndex = 3;
      this.iconLabel.Text = "Icon:";
      // 
      // nameLabel
      // 
      this.nameLabel.AutoSize = true;
      this.nameLabel.Location = new System.Drawing.Point(12, 24);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new System.Drawing.Size(38, 13);
      this.nameLabel.TabIndex = 3;
      this.nameLabel.Text = "Name:";
      // 
      // nameField
      // 
      this.nameField.Location = new System.Drawing.Point(66, 23);
      this.nameField.Name = "nameField";
      this.nameField.Size = new System.Drawing.Size(388, 20);
      this.nameField.TabIndex = 0;
      // 
      // tabs
      // 
      this.tabs.Controls.Add(this.loginTab);
      this.tabs.Controls.Add(this.authTab);
      this.tabs.Controls.Add(this.confirmTab);
      this.tabs.Controls.Add(this.addedTab);
      this.tabs.Controls.Add(this.importAndroidTab);
      this.tabs.Controls.Add(this.importSDATab);
      this.tabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
      this.tabs.ItemSize = new System.Drawing.Size(120, 18);
      this.tabs.Location = new System.Drawing.Point(4, 134);
      this.tabs.Name = "tabs";
      this.tabs.SelectedIndex = 0;
      this.tabs.Size = new System.Drawing.Size(464, 327);
      this.tabs.TabIndex = 0;
      this.tabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
      this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
      // 
      // loginTab
      // 
      this.loginTab.BackColor = System.Drawing.SystemColors.Control;
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
      this.loginTab.Size = new System.Drawing.Size(456, 301);
      this.loginTab.TabIndex = 0;
      this.loginTab.Tag = "";
      this.loginTab.Text = "Login";
      // 
      // captchaGroup
      // 
      this.captchaGroup.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.captchaGroup.Controls.Add(this.captchaBox);
      this.captchaGroup.Controls.Add(this.captchaTabLabel);
      this.captchaGroup.Controls.Add(this.captchacodeField);
      this.captchaGroup.Controls.Add(this.captchaButton);
      this.captchaGroup.Location = new System.Drawing.Point(7, 115);
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
      this.passwordField.Location = new System.Drawing.Point(104, 87);
      this.passwordField.Name = "passwordField";
      this.passwordField.PasswordChar = '●';
      this.passwordField.Size = new System.Drawing.Size(177, 20);
      this.passwordField.TabIndex = 1;
      this.passwordField.UseSystemPasswordChar = true;
      // 
      // passwordLabel
      // 
      this.passwordLabel.Location = new System.Drawing.Point(18, 87);
      this.passwordLabel.Name = "passwordLabel";
      this.passwordLabel.Size = new System.Drawing.Size(80, 25);
      this.passwordLabel.TabIndex = 1;
      this.passwordLabel.Text = "Password";
      // 
      // usernameLabel
      // 
      this.usernameLabel.Location = new System.Drawing.Point(18, 60);
      this.usernameLabel.Name = "usernameLabel";
      this.usernameLabel.Size = new System.Drawing.Size(80, 25);
      this.usernameLabel.TabIndex = 1;
      this.usernameLabel.Text = "Username";
      // 
      // authTab
      // 
      this.authTab.Controls.Add(this.authcodeLabel);
      this.authTab.Controls.Add(this.authcodeButton);
      this.authTab.Controls.Add(this.authcodeField);
      this.authTab.Controls.Add(this.authoriseTabLabel);
      this.authTab.Location = new System.Drawing.Point(4, 22);
      this.authTab.Name = "authTab";
      this.authTab.Size = new System.Drawing.Size(456, 301);
      this.authTab.TabIndex = 2;
      this.authTab.Tag = "";
      this.authTab.Text = "Authorise";
      // 
      // authcodeLabel
      // 
      this.authcodeLabel.Location = new System.Drawing.Point(18, 60);
      this.authcodeLabel.Name = "authcodeLabel";
      this.authcodeLabel.Size = new System.Drawing.Size(80, 25);
      this.authcodeLabel.TabIndex = 2;
      this.authcodeLabel.Text = "Code";
      // 
      // authcodeButton
      // 
      this.authcodeButton.Location = new System.Drawing.Point(104, 101);
      this.authcodeButton.Name = "authcodeButton";
      this.authcodeButton.Size = new System.Drawing.Size(110, 24);
      this.authcodeButton.TabIndex = 1;
      this.authcodeButton.Text = "Continue";
      this.authcodeButton.Click += new System.EventHandler(this.authcodeButton_Click);
      // 
      // authcodeField
      // 
      this.authcodeField.Location = new System.Drawing.Point(104, 60);
      this.authcodeField.Name = "authcodeField";
      this.authcodeField.Size = new System.Drawing.Size(206, 20);
      this.authcodeField.TabIndex = 0;
      // 
      // confirmTab
      // 
      this.confirmTab.Controls.Add(this.revocationCheckbox);
      this.confirmTab.Controls.Add(this.revocationcodeCopy);
      this.confirmTab.Controls.Add(this.revocationcodeText);
      this.confirmTab.Controls.Add(this.revocationcodeField);
      this.confirmTab.Controls.Add(this.revocationcodeLabel);
      this.confirmTab.Controls.Add(this.activationcodeLabel);
      this.confirmTab.Controls.Add(this.confirmButton);
      this.confirmTab.Controls.Add(this.activationcodeField);
      this.confirmTab.Controls.Add(this.confirmTabLabel);
      this.confirmTab.Location = new System.Drawing.Point(4, 22);
      this.confirmTab.Name = "confirmTab";
      this.confirmTab.Size = new System.Drawing.Size(456, 301);
      this.confirmTab.TabIndex = 3;
      this.confirmTab.Tag = "";
      this.confirmTab.Text = "Confirm";
      // 
      // revocationCheckbox
      // 
      this.revocationCheckbox.AutoSize = true;
      this.revocationCheckbox.Location = new System.Drawing.Point(137, 208);
      this.revocationCheckbox.Name = "revocationCheckbox";
      this.revocationCheckbox.Size = new System.Drawing.Size(215, 17);
      this.revocationCheckbox.TabIndex = 1;
      this.revocationCheckbox.Text = "I have written down my revocation code";
      this.revocationCheckbox.CheckedChanged += new System.EventHandler(this.revocationCheckbox_CheckedChanged);
      // 
      // revocationcodeCopy
      // 
      this.revocationcodeCopy.AutoSize = true;
      this.revocationcodeCopy.Location = new System.Drawing.Point(320, 169);
      this.revocationcodeCopy.Name = "revocationcodeCopy";
      this.revocationcodeCopy.Size = new System.Drawing.Size(83, 17);
      this.revocationcodeCopy.TabIndex = 3;
      this.revocationcodeCopy.Text = "Allow copy?";
      this.revocationcodeCopy.CheckedChanged += new System.EventHandler(this.revocationcodeCopy_CheckedChanged);
      // 
      // revocationcodeText
      // 
      this.revocationcodeText.Location = new System.Drawing.Point(4, 95);
      this.revocationcodeText.Name = "revocationcodeText";
      this.revocationcodeText.Size = new System.Drawing.Size(442, 65);
      this.revocationcodeText.TabIndex = 17;
      this.revocationcodeText.Text = "This is your Recovery code. It can be used to remove the authenticator from your " +
    "account if you ever lose it. Please write it down now and put it somewhere safe." +
    "";
      // 
      // revocationcodeField
      // 
      this.revocationcodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.revocationcodeField.Location = new System.Drawing.Point(137, 163);
      this.revocationcodeField.Multiline = true;
      this.revocationcodeField.Name = "revocationcodeField";
      this.revocationcodeField.SecretMode = false;
      this.revocationcodeField.Size = new System.Drawing.Size(177, 26);
      this.revocationcodeField.SpaceOut = 0;
      this.revocationcodeField.TabIndex = 16;
      // 
      // revocationcodeLabel
      // 
      this.revocationcodeLabel.Location = new System.Drawing.Point(20, 164);
      this.revocationcodeLabel.Name = "revocationcodeLabel";
      this.revocationcodeLabel.Size = new System.Drawing.Size(111, 25);
      this.revocationcodeLabel.TabIndex = 5;
      this.revocationcodeLabel.Text = "Recovery code";
      // 
      // activationcodeLabel
      // 
      this.activationcodeLabel.Location = new System.Drawing.Point(4, 49);
      this.activationcodeLabel.Name = "activationcodeLabel";
      this.activationcodeLabel.Size = new System.Drawing.Size(127, 25);
      this.activationcodeLabel.TabIndex = 5;
      this.activationcodeLabel.Text = "Confirmation code";
      // 
      // activationcodeField
      // 
      this.activationcodeField.Location = new System.Drawing.Point(137, 49);
      this.activationcodeField.Name = "activationcodeField";
      this.activationcodeField.Size = new System.Drawing.Size(177, 20);
      this.activationcodeField.TabIndex = 0;
      // 
      // confirmTabLabel
      // 
      this.confirmTabLabel.Location = new System.Drawing.Point(4, 12);
      this.confirmTabLabel.Name = "confirmTabLabel";
      this.confirmTabLabel.Size = new System.Drawing.Size(449, 34);
      this.confirmTabLabel.TabIndex = 2;
      this.confirmTabLabel.Text = "Enter the confirmation code you received by SMS to your phone.";
      // 
      // addedTab
      // 
      this.addedTab.Controls.Add(this.revocationcode2Copy);
      this.addedTab.Controls.Add(this.revocationcode2Label);
      this.addedTab.Controls.Add(this.revocationcode2Field);
      this.addedTab.Controls.Add(this.metroLabel1);
      this.addedTab.Location = new System.Drawing.Point(4, 22);
      this.addedTab.Name = "addedTab";
      this.addedTab.Size = new System.Drawing.Size(456, 301);
      this.addedTab.TabIndex = 4;
      this.addedTab.Tag = "";
      this.addedTab.Text = "Added";
      // 
      // revocationcode2Copy
      // 
      this.revocationcode2Copy.AutoSize = true;
      this.revocationcode2Copy.Location = new System.Drawing.Point(320, 127);
      this.revocationcode2Copy.Name = "revocationcode2Copy";
      this.revocationcode2Copy.Size = new System.Drawing.Size(83, 17);
      this.revocationcode2Copy.TabIndex = 0;
      this.revocationcode2Copy.Text = "Allow copy?";
      this.revocationcode2Copy.CheckedChanged += new System.EventHandler(this.revocationcode2Copy_CheckedChanged);
      // 
      // revocationcode2Label
      // 
      this.revocationcode2Label.Location = new System.Drawing.Point(20, 121);
      this.revocationcode2Label.Name = "revocationcode2Label";
      this.revocationcode2Label.Size = new System.Drawing.Size(111, 25);
      this.revocationcode2Label.TabIndex = 8;
      this.revocationcode2Label.Text = "Recovery code";
      // 
      // revocationcode2Field
      // 
      this.revocationcode2Field.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.revocationcode2Field.Location = new System.Drawing.Point(137, 121);
      this.revocationcode2Field.Multiline = true;
      this.revocationcode2Field.Name = "revocationcode2Field";
      this.revocationcode2Field.SecretMode = false;
      this.revocationcode2Field.Size = new System.Drawing.Size(177, 26);
      this.revocationcode2Field.SpaceOut = 0;
      this.revocationcode2Field.TabIndex = 7;
      // 
      // metroLabel1
      // 
      this.metroLabel1.Location = new System.Drawing.Point(4, 12);
      this.metroLabel1.Name = "metroLabel1";
      this.metroLabel1.Size = new System.Drawing.Size(449, 90);
      this.metroLabel1.TabIndex = 2;
      this.metroLabel1.Text = "Your authenticator has been added to your Steam account.\r\n\r\nPlease make sure you " +
    "have copied down your recovery code so you can remove your authenticator from yo" +
    "ur Steam account in the future.\r\n";
      // 
      // importAndroidTab
      // 
      this.importAndroidTab.Controls.Add(this.importSteamguard);
      this.importAndroidTab.Controls.Add(this.metroLabel4);
      this.importAndroidTab.Controls.Add(this.importUuid);
      this.importAndroidTab.Controls.Add(this.metroLabel2);
      this.importAndroidTab.Controls.Add(this.metroLabel3);
      this.importAndroidTab.Location = new System.Drawing.Point(4, 22);
      this.importAndroidTab.Name = "importAndroidTab";
      this.importAndroidTab.Size = new System.Drawing.Size(456, 301);
      this.importAndroidTab.TabIndex = 5;
      this.importAndroidTab.Text = "Import Android";
      // 
      // importSteamguard
      // 
      this.importSteamguard.Location = new System.Drawing.Point(4, 243);
      this.importSteamguard.Multiline = true;
      this.importSteamguard.Name = "importSteamguard";
      this.importSteamguard.Size = new System.Drawing.Size(449, 55);
      this.importSteamguard.TabIndex = 11;
      // 
      // metroLabel4
      // 
      this.metroLabel4.Location = new System.Drawing.Point(4, 213);
      this.metroLabel4.Name = "metroLabel4";
      this.metroLabel4.Size = new System.Drawing.Size(449, 26);
      this.metroLabel4.TabIndex = 10;
      this.metroLabel4.Text = "From the files folder, paste contents of \'SteamGuard-NNNNNNNNNN\'";
      // 
      // importUuid
      // 
      this.importUuid.Location = new System.Drawing.Point(4, 182);
      this.importUuid.Multiline = true;
      this.importUuid.Name = "importUuid";
      this.importUuid.Size = new System.Drawing.Size(449, 22);
      this.importUuid.TabIndex = 9;
      // 
      // metroLabel2
      // 
      this.metroLabel2.Location = new System.Drawing.Point(4, 154);
      this.metroLabel2.Name = "metroLabel2";
      this.metroLabel2.Size = new System.Drawing.Size(449, 26);
      this.metroLabel2.TabIndex = 8;
      this.metroLabel2.Text = "From the shared_prefs folder, paste contents of \'steam_uuid.xml\'";
      // 
      // metroLabel3
      // 
      this.metroLabel3.Location = new System.Drawing.Point(4, 12);
      this.metroLabel3.Name = "metroLabel3";
      this.metroLabel3.Size = new System.Drawing.Size(449, 124);
      this.metroLabel3.TabIndex = 7;
      this.metroLabel3.Text = resources.GetString("metroLabel3.Text");
      // 
      // importSDATab
      // 
      this.importSDATab.Controls.Add(this.metroLabel8);
      this.importSDATab.Controls.Add(this.metroLabel9);
      this.importSDATab.Controls.Add(this.metroLabel7);
      this.importSDATab.Controls.Add(this.metroLabel6);
      this.importSDATab.Controls.Add(this.importSDAList);
      this.importSDATab.Controls.Add(this.importSDAPassword);
      this.importSDATab.Controls.Add(this.importSDAPath);
      this.importSDATab.Controls.Add(this.metroLabel5);
      this.importSDATab.Controls.Add(this.importSDALoad);
      this.importSDATab.Controls.Add(this.importSDABrowse);
      this.importSDATab.Location = new System.Drawing.Point(4, 22);
      this.importSDATab.Name = "importSDATab";
      this.importSDATab.Size = new System.Drawing.Size(456, 301);
      this.importSDATab.TabIndex = 6;
      this.importSDATab.Text = "Import SDA";
      // 
      // metroLabel8
      // 
      this.metroLabel8.Location = new System.Drawing.Point(3, 107);
      this.metroLabel8.Name = "metroLabel8";
      this.metroLabel8.Size = new System.Drawing.Size(259, 27);
      this.metroLabel8.TabIndex = 12;
      this.metroLabel8.Text = "2. Enter your password (if you have one):";
      // 
      // metroLabel9
      // 
      this.metroLabel9.Location = new System.Drawing.Point(4, 148);
      this.metroLabel9.Name = "metroLabel9";
      this.metroLabel9.Size = new System.Drawing.Size(60, 27);
      this.metroLabel9.TabIndex = 12;
      this.metroLabel9.Text = "3. Click";
      // 
      // metroLabel7
      // 
      this.metroLabel7.Location = new System.Drawing.Point(151, 148);
      this.metroLabel7.Name = "metroLabel7";
      this.metroLabel7.Size = new System.Drawing.Size(301, 27);
      this.metroLabel7.TabIndex = 12;
      this.metroLabel7.Text = "and choose which account to import.";
      // 
      // metroLabel6
      // 
      this.metroLabel6.Location = new System.Drawing.Point(3, 41);
      this.metroLabel6.Name = "metroLabel6";
      this.metroLabel6.Size = new System.Drawing.Size(449, 28);
      this.metroLabel6.TabIndex = 12;
      this.metroLabel6.Text = "1. Select the manifest.json file where your maFiles are located.";
      // 
      // importSDAList
      // 
      this.importSDAList.FormattingEnabled = true;
      this.importSDAList.Location = new System.Drawing.Point(4, 178);
      this.importSDAList.Name = "importSDAList";
      this.importSDAList.Size = new System.Drawing.Size(449, 82);
      this.importSDAList.TabIndex = 11;
      // 
      // importSDAPassword
      // 
      this.importSDAPassword.Location = new System.Drawing.Point(268, 107);
      this.importSDAPassword.Name = "importSDAPassword";
      this.importSDAPassword.PasswordChar = '*';
      this.importSDAPassword.Size = new System.Drawing.Size(184, 20);
      this.importSDAPassword.TabIndex = 10;
      // 
      // importSDAPath
      // 
      this.importSDAPath.Location = new System.Drawing.Point(4, 70);
      this.importSDAPath.Name = "importSDAPath";
      this.importSDAPath.Size = new System.Drawing.Size(368, 20);
      this.importSDAPath.TabIndex = 10;
      // 
      // metroLabel5
      // 
      this.metroLabel5.Location = new System.Drawing.Point(4, 12);
      this.metroLabel5.Name = "metroLabel5";
      this.metroLabel5.Size = new System.Drawing.Size(449, 29);
      this.metroLabel5.TabIndex = 8;
      this.metroLabel5.Text = "You can import an authenticator from SteamDesktopAuthenticator.";
      // 
      // importSDALoad
      // 
      this.importSDALoad.Location = new System.Drawing.Point(70, 147);
      this.importSDALoad.Name = "importSDALoad";
      this.importSDALoad.Size = new System.Drawing.Size(75, 23);
      this.importSDALoad.TabIndex = 1;
      this.importSDALoad.Text = "Load";
      this.importSDALoad.Click += new System.EventHandler(this.importSDALoad_Click);
      // 
      // importSDABrowse
      // 
      this.importSDABrowse.Location = new System.Drawing.Point(378, 70);
      this.importSDABrowse.Name = "importSDABrowse";
      this.importSDABrowse.Size = new System.Drawing.Size(75, 23);
      this.importSDABrowse.TabIndex = 1;
      this.importSDABrowse.Text = "Browse...";
      this.importSDABrowse.Click += new System.EventHandler(this.importSDABrowse_Click);
      // 
      // closeButton
      // 
      this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.closeButton.Location = new System.Drawing.Point(308, 477);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new System.Drawing.Size(75, 23);
      this.closeButton.TabIndex = 1;
      this.closeButton.Text = "Close";
      this.closeButton.Visible = false;
      this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
      // 
      // steamIcon
      // 
      this.steamIcon.Image = global::Authenticator.Properties.Resources.SteamIcon;
      this.steamIcon.Location = new System.Drawing.Point(165, 61);
      this.steamIcon.Name = "steamIcon";
      this.steamIcon.Size = new System.Drawing.Size(48, 48);
      this.steamIcon.TabIndex = 4;
      this.steamIcon.TabStop = false;
      this.steamIcon.Tag = "SteamIcon.png";
      this.steamIcon.Click += new System.EventHandler(this.iconRift_Click);
      // 
      // steamAuthenticatorIcon
      // 
      this.steamAuthenticatorIcon.Image = global::Authenticator.Properties.Resources.SteamAuthenticatorIcon;
      this.steamAuthenticatorIcon.Location = new System.Drawing.Point(89, 61);
      this.steamAuthenticatorIcon.Name = "steamAuthenticatorIcon";
      this.steamAuthenticatorIcon.Size = new System.Drawing.Size(48, 48);
      this.steamAuthenticatorIcon.TabIndex = 4;
      this.steamAuthenticatorIcon.TabStop = false;
      this.steamAuthenticatorIcon.Tag = "SteamAuthenticatorIcon.png";
      this.steamAuthenticatorIcon.Click += new System.EventHandler(this.iconGlyph_Click);
      // 
      // AddSteamAuthenticator
      // 
      this.AcceptButton = this.closeButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(474, 512);
      this.Controls.Add(this.tabs);
      this.Controls.Add(this.steamIconRadioButton);
      this.Controls.Add(this.closeButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.steamAuthenticatorIconRadioButton);
      this.Controls.Add(this.nameLabel);
      this.Controls.Add(this.steamIcon);
      this.Controls.Add(this.nameField);
      this.Controls.Add(this.steamAuthenticatorIcon);
      this.Controls.Add(this.iconLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AddSteamAuthenticator";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Add Steam Authenticator";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddSteamAuthenticator_FormClosing);
      this.Load += new System.EventHandler(this.AddSteamAuthenticator_Load);
      this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AddSteamAuthenticator_KeyPress);
      this.tabs.ResumeLayout(false);
      this.loginTab.ResumeLayout(false);
      this.loginTab.PerformLayout();
      this.captchaGroup.ResumeLayout(false);
      this.captchaGroup.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.captchaBox)).EndInit();
      this.authTab.ResumeLayout(false);
      this.authTab.PerformLayout();
      this.confirmTab.ResumeLayout(false);
      this.confirmTab.PerformLayout();
      this.addedTab.ResumeLayout(false);
      this.addedTab.PerformLayout();
      this.importAndroidTab.ResumeLayout(false);
      this.importAndroidTab.PerformLayout();
      this.importSDATab.ResumeLayout(false);
      this.importSDATab.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.steamIcon)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.steamAuthenticatorIcon)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Label loginTabLabel;
    private Button loginButton;
    private Label authoriseTabLabel;
    private Label captchaTabLabel;
    private Button confirmButton;
    private Button cancelButton;
    private TextBox captchacodeField;
    private TextBox usernameField;
    private Button captchaButton;
    private GroupMetroRadioButton steamIconRadioButton;
    private GroupMetroRadioButton steamAuthenticatorIconRadioButton;
    private System.Windows.Forms.PictureBox steamIcon;
    private System.Windows.Forms.PictureBox steamAuthenticatorIcon;
    private Label nameLabel;
    private TextBox nameField;
    private Label iconLabel;
    private TabControl tabs;
    private TabPage loginTab;
    private Label passwordLabel;
    private Label usernameLabel;
    private Button authcodeButton;
    private TextBox passwordField;
    private TabPage authTab;
    private TabPage confirmTab;
    private System.Windows.Forms.PictureBox captchaBox;
    private Label authcodeLabel;
    private TextBox authcodeField;
    private Label confirmTabLabel;
    private Label activationcodeLabel;
    private TextBox activationcodeField;
    private System.Windows.Forms.Panel captchaGroup;
    private TabPage addedTab;
    private Label metroLabel1;
    private Label revocationcode2Label;
    private SecretTextBox revocationcode2Field;
    private Button closeButton;
    private CheckBox revocationcodeCopy;
    private Label revocationcodeText;
    private SecretTextBox revocationcodeField;
    private CheckBox revocationCheckbox;
    private Label revocationcodeLabel;
    private CheckBox revocationcode2Copy;
    private TabPage importAndroidTab;
    private Label metroLabel2;
    private Label metroLabel3;
    private TextBox importUuid;
    private TextBox importSteamguard;
    private Label metroLabel4;
    private TabPage importSDATab;
    private Label metroLabel5;
    private TextBox importSDAPath;
    private Button importSDABrowse;
    private System.Windows.Forms.ListBox importSDAList;
    private Label metroLabel7;
    private Label metroLabel6;
    private Label metroLabel8;
    private TextBox importSDAPassword;
    private Label metroLabel9;
    private Button importSDALoad;
  }
}