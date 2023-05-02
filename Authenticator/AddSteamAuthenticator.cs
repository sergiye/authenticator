using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace Authenticator {
  /// <summary>
  /// Form class for create a new Battle.net authenticator
  /// </summary>
  public partial class AddSteamAuthenticator : ResourceForm
	{
		/// <summary>
		/// Entry for a single SDA account
		/// </summary>
		class ImportedSdaEntry
		{
			public const int PBKDF2_ITERATIONS = 50000;
			public const int SALT_LENGTH = 8;
			public const int KEY_SIZE_BYTES = 32;
			public const int IV_LENGTH = 16;

			public string Username;
			public string SteamId;
			public string Json;

			public override string ToString()
			{
				return Username + " (" + SteamId + ")";
			}
		}

		/// <summary>
		/// Form instantiation
		/// </summary>
		public AddSteamAuthenticator()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Current authenticator
		/// </summary>
		public AuthAuthenticator Authenticator { get; set; }

		/// <summary>
		/// Enrolling state
		/// </summary>
		private SteamAuthenticator.EnrollState mEnroll = new SteamAuthenticator.EnrollState();

		/// <summary>
		/// Current enrolling authenticator
		/// </summary>
		private SteamAuthenticator mSteamAuthenticator = new SteamAuthenticator();

		/// <summary>
		/// Set of tab pages taken from the tab control so we can hide and show them
		/// </summary>
		private Dictionary<string, TabPage> mTabPages = new Dictionary<string, TabPage>();

		#region Form Events

		/// <summary>
		/// Load the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddSteamAuthenticator_Load(object sender, EventArgs e)
		{
			nameField.Text = Authenticator.Name;

			for (var i = 0; i < tabs.TabPages.Count; i++)
			{
				mTabPages.Add(tabs.TabPages[i].Name, tabs.TabPages[i]);
			}
			tabs.TabPages.RemoveByKey("authTab");
			tabs.TabPages.RemoveByKey("confirmTab");
			tabs.TabPages.RemoveByKey("addedTab");
			tabs.SelectedTab = tabs.TabPages[0];

			revocationcodeField.SecretMode = true;
			revocationcode2Field.SecretMode = true;

			importSDAList.Font = Font;

			nameField.Focus();
		}

		/// <summary>
		/// If we close after adding, make sure we save it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddSteamAuthenticator_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (mEnroll.Success)
			{
				Authenticator.Name = nameField.Text;
				DialogResult = DialogResult.OK;
			}
		}

		/// <summary>
		/// Press the form's cancel button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelButton_Click(object sender, EventArgs e)
		{
			// if we press ESC after adding, make sure we save it
			if (mEnroll.Success)
			{
				DialogResult = DialogResult.OK;
			}
		}

		/// <summary>
		/// Click the OK button to verify and add the authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void confirmButton_Click(object sender, EventArgs e)
		{
			if (activationcodeField.Text.Trim().Length == 0)
			{
				MainForm.ErrorDialog(this, "Please enter the activation code from your email");
				DialogResult = DialogResult.None;
				return;
			}

			mEnroll.ActivationCode = activationcodeField.Text.Trim();

			ProcessEnroll();
		}

		/// <summary>
		/// Select one of the icons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void iconRift_Click(object sender, EventArgs e)
		{
			steamIconRadioButton.Checked = true;
		}

		/// <summary>
		/// Select one of the icons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void iconGlyph_Click(object sender, EventArgs e)
		{
			steamAuthenticatorIconRadioButton.Checked = true;
		}

		/// <summary>
		/// Set the authenticator icon
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void iconRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (((RadioButton)sender).Checked)
			{
				Authenticator.Skin = (string)((RadioButton)sender).Tag;
			}
		}

		/// <summary>
		/// Draw the tabs of the tabcontrol so they aren't white
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
		{
			var page = tabs.TabPages[e.Index];
			e.Graphics.FillRectangle(new SolidBrush(page.BackColor), e.Bounds);

			var paddedBounds = e.Bounds;
			var yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
			paddedBounds.Offset(1, yOffset);
			TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, page.ForeColor);

			captchaGroup.BackColor = page.BackColor;
		}

		/// <summary>
		/// Answer the captcha
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void captchaButton_Click(object sender, EventArgs e)
		{
			if (captchacodeField.Text.Trim().Length == 0)
			{
				MainForm.ErrorDialog(this, "Please enter the characters in the image", null, MessageBoxButtons.OK);
				return;
			}

			mEnroll.Username = usernameField.Text.Trim();
			mEnroll.Password = passwordField.Text.Trim();
			mEnroll.CaptchaText = captchacodeField.Text.Trim();

			ProcessEnroll();
		}

		/// <summary>
		/// Login to steam account
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void loginButton_Click(object sender, EventArgs e)
		{
			if (usernameField.Text.Trim().Length == 0 || passwordField.Text.Trim().Length == 0)
			{
				MainForm.ErrorDialog(this, "Please enter your username and password", null, MessageBoxButtons.OK);
				return;
			}

			mEnroll.Username = usernameField.Text.Trim();
			mEnroll.Password = passwordField.Text.Trim();

			ProcessEnroll();
		}

		/// <summary>
		/// Confirm with the code sent by email
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void authcodeButton_Click(object sender, EventArgs e)
		{
			if (authcodeField.Text.Trim().Length == 0)
			{
				MainForm.ErrorDialog(this, "Please enter the authorisation code", null, MessageBoxButtons.OK);
				return;
			}

			mEnroll.EmailAuthText = authcodeField.Text.Trim();

			ProcessEnroll();
		}

		/// <summary>
		/// CLick the close button to save
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void closeButton_Click(object sender, EventArgs e)
		{
			Authenticator.Name = nameField.Text;

			if (tabs.SelectedTab.Name == "importAndroidTab")
			{
				if (ImportSteamGuard() == false)
				{
					DialogResult = DialogResult.None;
					return;
				}
			}
			if (tabs.SelectedTab.Name == "importSDATab")
			{
				if (ImportSda() == false)
				{
					DialogResult = DialogResult.None;
					return;
				}
			}

			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// Handle the enter key on the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddSteamAuthenticator_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				switch (tabs.SelectedTab.Name)
				{
					case "loginTab":
						e.Handled = true;
						if (mEnroll.RequiresCaptcha)
						{
							captchaButton_Click(sender, new EventArgs());
						}
						else
						{
							loginButton_Click(sender, new EventArgs());
						}
						break;
					case "authTab":
						e.Handled = true;
						authcodeButton_Click(sender, new EventArgs());
						break;
					case "confirmTab":
						e.Handled = true;
						confirmButton_Click(sender, new EventArgs());
						break;
					case "importAndroidTab":
						e.Handled = true;
						closeButton_Click(sender, new EventArgs());
						break;
					case "importSDATab":
						e.Handled = true;
						closeButton_Click(sender, new EventArgs());
						break;
					default:
						e.Handled = false;
						break;
				}

				return;
			}

			e.Handled = false;
		}

		/// <summary>
		/// Enable the button when we have confirmed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void revocationCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			confirmButton.Enabled = revocationCheckbox.Checked;
		}

		/// <summary>
		/// Allow the field to be copied
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void revocationcodeCopy_CheckedChanged(object sender, EventArgs e)
		{
			revocationcodeField.SecretMode = !revocationcodeCopy.Checked;
		}

		/// <summary>
		/// Allow the field to be copied
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void revocationcode2Copy_CheckedChanged(object sender, EventArgs e)
		{
			revocationcode2Field.SecretMode = !revocationcode2Copy.Checked;
		}

		/// <summary>
		/// When changing tabs, set the correct buttons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabs.SelectedTab != null && (tabs.SelectedTab.Name == "importAndroidTab" || tabs.SelectedTab.Name == "importSDATab"))
			{
				closeButton.Text = "OK";
				closeButton.Visible = true;
			}
			else
			{
				closeButton.Text = "Close";
				closeButton.Visible = false;
			}
		}

		/// <summary>
		/// Browse the SDA folder
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void importSDABrowse_Click(object sender, EventArgs e)
		{
			var ofd = new OpenFileDialog();
			ofd.AddExtension = true;
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;
			ofd.DefaultExt = "*.json";
			ofd.FileName = "manifest.json";
			ofd.Filter = "Manifest file|manifest.json|maFile (*.maFile)|*.maFile";
			ofd.FilterIndex = 0;
			ofd.RestoreDirectory = true;
			ofd.Title = "SteamDesktopAuthenticator";
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				importSDAPath.Text = ofd.FileName;
			}
		}

		/// <summary>
		/// Click the load the SDA accounts
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void importSDALoad_Click(object sender, EventArgs e)
		{
			LoadSda();
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Import an authenticator from the uuid and steamguard files
		/// </summary>
		/// <returns>true if successful</returns>
		private bool ImportSteamGuard()
		{
			var uuid = importUuid.Text.Trim();
			if (uuid.Length == 0)
			{
				MainForm.ErrorDialog(this, "Please enter the contents of the steam.uuid.xml file or your DeviceId");
				return false;
			}
			var steamguard = importSteamguard.Text.Trim();
			if (steamguard.Length == 0)
			{
				MainForm.ErrorDialog(this, "Please enter the contents of your SteamGuard file");
				return false;
			}

			// check the deviceid
			string deviceId;
			if (uuid.IndexOf("?xml") != -1)
			{
				try
				{
					var doc = new XmlDocument();
					doc.LoadXml(uuid);
					var node = doc.SelectSingleNode("//string[@name='uuidKey']");
					if (node == null)
					{
						MainForm.ErrorDialog(this, "Cannot find uuidKey in xml");
						return false;
					}

					deviceId = node.InnerText;
				}
				catch (Exception ex)
				{
					MainForm.ErrorDialog(this, "Invalid uuid xml: " + ex.Message);
					return false;
				}
			}
			else
			{
				deviceId = uuid;
			}
			if (string.IsNullOrEmpty(deviceId) || Regex.IsMatch(deviceId, @"android:[0-9abcdef-]+", RegexOptions.Singleline | RegexOptions.IgnoreCase) == false)
			{
				MainForm.ErrorDialog(this, "Invalid deviceid, expecting \"android:NNNN...\"");
				return false;
			}

			// check the steamguard
			byte[] secret;
			string serial;
			try
			{
				var json = JObject.Parse(steamguard);

				var node = json.SelectToken("shared_secret");
				if (node == null)
				{
					throw new ApplicationException("no shared_secret");
				}			
				secret = Convert.FromBase64String(node.Value<string>());

				node = json.SelectToken("serial_number");
				if (node == null)
				{
					throw new ApplicationException("no serial_number");
				}
				serial = node.Value<string>();
			}
			catch (Exception ex)
			{
				MainForm.ErrorDialog(this, "Invalid SteamGuard JSON contents: " + ex.Message);
				return false;
			}

			var auth = new SteamAuthenticator();
			auth.SecretKey = secret;
			auth.Serial = serial;
			auth.SteamData = steamguard;
			auth.DeviceId = deviceId;

			Authenticator.AuthenticatorData = auth;

			return true;
		}

		/// <summary>
		/// Import the selected SDA account
		/// </summary>
		/// <returns>true if successful</returns>
		private bool ImportSda()
		{
			var entry = importSDAList.SelectedItem as ImportedSdaEntry;
			if (entry == null)
			{
				MainForm.ErrorDialog(this, "Please load and select a Steam account");
				return false;
			}

			var auth = new SteamAuthenticator();
			var token = JObject.Parse(entry.Json);
			foreach (var prop in token.Root.Children().ToList())
			{
				var child = token.SelectToken(prop.Path);

				var lkey = prop.Path.ToLower();
				if (lkey == "fully_enrolled" || lkey == "session")
				{
					prop.Remove();
				}
				else if (lkey == "device_id")
				{
					auth.DeviceId = child.Value<string>();
					prop.Remove();
				}
				else if (lkey == "serial_number")
				{
					auth.Serial = child.Value<string>();
				}
				else if (lkey == "account_name")
				{
					if (nameField.Text.Length == 0)
					{
						nameField.Text = "Steam (" + child.Value<string>() + ")";
					}
				}
				else if (lkey == "shared_secret")
				{
					auth.SecretKey = Convert.FromBase64String(child.Value<string>());
				}
			}
			auth.SteamData = token.ToString(Newtonsoft.Json.Formatting.None);

			Authenticator.AuthenticatorData = auth;

			return true;
		}

		/// <summary>
		/// Load all the accounts from the SDA manifest into the listbox
		/// </summary>
		private void LoadSda()
		{
			var manifestfile = importSDAPath.Text.Trim();
			if (string.IsNullOrEmpty(manifestfile) || File.Exists(manifestfile) == false)
			{
				MainForm.ErrorDialog(this, "Enter a path for SteamDesktopAuthenticator");
				return;
			}

			var password = importSDAPassword.Text.Trim();

			importSDAList.Items.Clear();
			try
			{
				var path = Path.GetDirectoryName(manifestfile);

				if (manifestfile.IndexOf("manifest.json") != -1)
				{
					var manifest = JObject.Parse(File.ReadAllText(manifestfile));
					var token = manifest.SelectToken("encrypted");
					var encrypted = token != null ? token.Value<bool>() : false;
					if (encrypted && password.Length == 0)
					{
						throw new ApplicationException("Please enter your password");
					}

					var entries = manifest["entries"] as JArray;
					if (entries == null || entries.Count == 0)
					{
						throw new ApplicationException("SteamDesktopAuthenticator has no SteamGuard authenticators");
					}

					foreach (var entry in entries)
					{
						token = entry.SelectToken("filename");
						if (token != null)
						{
							var filename = token.Value<string>();
							string steamid = null;
							string iv = null;
							string salt = null;

							token = entry.SelectToken("steamid");
							if (token != null)
							{
								steamid = token.Value<string>();
							}
							token = entry.SelectToken("encryption_iv");
							if (token != null)
							{
								iv = token.Value<string>();
							}
							token = entry.SelectToken("encryption_salt");
							if (token != null)
							{
								salt = token.Value<string>();
							}

							LoadSdaFile(Path.Combine(path, filename), password, steamid, iv, salt);
						}
					}
				}
				else if (string.IsNullOrEmpty(password) == false)
				{
					throw new ApplicationException("Cannot load an single maFile that has been encrypted");
				}
				else
				{
					LoadSdaFile(manifestfile);
				}
			}
			catch (ApplicationException ex)
			{
				MainForm.ErrorDialog(this, ex.Message);
			}
			catch (Exception ex)
			{
				MainForm.ErrorDialog(this, "Error while importing: " + ex.Message, ex);
			}
		}

		/// <summary>
		/// Load a single maFile with the security credentials
		/// </summary>
		/// <param name="mafile">filename</param>
		/// <param name="password">optional password</param>
		/// <param name="steamid">steamid if known</param>
		/// <param name="iv">optional iv for decryption</param>
		/// <param name="salt">optional salt</param>
		private void LoadSdaFile(string mafile, string password = null, string steamid = null, string iv = null, string salt = null)
		{
			string data;
			if (File.Exists(mafile) == false || (data = File.ReadAllText(mafile)) == null)
			{
				throw new ApplicationException("Cannot read file " + mafile);
			}

			// decrypt
			if (string.IsNullOrEmpty(password) == false)
			{
				var ciphertext = Convert.FromBase64String(data);

				using (var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), ImportedSdaEntry.PBKDF2_ITERATIONS))
				{
					var key = pbkdf2.GetBytes(ImportedSdaEntry.KEY_SIZE_BYTES);

					using (var aes256 = new RijndaelManaged())
					{
						aes256.IV = Convert.FromBase64String(iv);
						aes256.Key = key;
						aes256.Padding = PaddingMode.PKCS7;
						aes256.Mode = CipherMode.CBC;

						try
						{
							using (var decryptor = aes256.CreateDecryptor(aes256.Key, aes256.IV))
							{
								using (var ms = new MemoryStream(ciphertext))
								{
									using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
									{
										using (var sr = new StreamReader(cs))
										{
											data = sr.ReadToEnd();
										}
									}
								}
							}
						}
						catch (CryptographicException )
						{
							throw new ApplicationException("Invalid password");
						}
					}
				}
			}

			var token = JObject.Parse(data);
			var sdaentry = new ImportedSdaEntry();
			sdaentry.Username = token.SelectToken("account_name") != null ? token.SelectToken("account_name").Value<string>() : null;
			sdaentry.SteamId = steamid;
			if (string.IsNullOrEmpty(sdaentry.SteamId))
			{
				sdaentry.SteamId = token.SelectToken("Session.SteamID") != null ? token.SelectToken("Session.SteamID").Value<string>() : null;
			}
			if (string.IsNullOrEmpty(sdaentry.SteamId))
			{
				sdaentry.SteamId = mafile.Split('.')[0];
			}
			sdaentry.Json = data;

			importSDAList.Items.Add(sdaentry);
		}

		/// <summary>
		/// Process the enrolling calling the authenticator method, checking the state and displaying appropriate tab
		/// </summary>
		private void ProcessEnroll()
		{
			do
			{
				try
				{
					var cursor = Cursor.Current;
					Cursor.Current = Cursors.WaitCursor;
					Application.DoEvents();

					var result = mSteamAuthenticator.Enroll(mEnroll);
					Cursor.Current = cursor;
					if (result == false)
					{
						if (string.IsNullOrEmpty(mEnroll.Error) == false)
						{
							MainForm.ErrorDialog(this, mEnroll.Error, null, MessageBoxButtons.OK);
						}

						if (mEnroll.Requires2Fa)
						{
							MainForm.ErrorDialog(this, "It looks like you already have an authenticator added to you account", null, MessageBoxButtons.OK);
							return;
						}

						if (mEnroll.RequiresCaptcha)
						{
							using (var web = new WebClient())
							{
								var data = web.DownloadData(mEnroll.CaptchaUrl);

								using (var ms = new MemoryStream(data))
								{
									captchaBox.Image = Image.FromStream(ms);
								}
							}
							loginButton.Enabled = false;
							captchaGroup.Visible = true;
							captchacodeField.Text = "";
							captchacodeField.Focus();
							return;
						}
						loginButton.Enabled = true;
						captchaGroup.Visible = false;

						if (mEnroll.RequiresEmailAuth)
						{
							if (authoriseTabLabel.Tag == null || string.IsNullOrEmpty((string)authoriseTabLabel.Tag))
							{
								authoriseTabLabel.Tag = authoriseTabLabel.Text;
							}
							var email = string.IsNullOrEmpty(mEnroll.EmailDomain) == false ? "***@" + mEnroll.EmailDomain : string.Empty;
							authoriseTabLabel.Text = string.Format((string)authoriseTabLabel.Tag, email);
							authcodeField.Text = "";
							ShowTab("authTab");
							authcodeField.Focus();
							return;
						}
						if (tabs.TabPages.ContainsKey("authTab"))
						{
							tabs.TabPages.RemoveByKey("authTab");
						}

						if (mEnroll.RequiresLogin)
						{
							ShowTab("loginTab");
							usernameField.Focus();
							return;
						}

						if (mEnroll.RequiresActivation)
						{
							mEnroll.Error = null;

							Authenticator.AuthenticatorData = mSteamAuthenticator;
							revocationcodeField.Text = mEnroll.RevocationCode;

							ShowTab("confirmTab");

							activationcodeField.Focus();
							return;
						}

						var error = mEnroll.Error;
						if (string.IsNullOrEmpty(error))
						{
							error = "Unable to add the add the authenticator to your account. Please try again later.";
						}
						MainForm.ErrorDialog(this, error, null, MessageBoxButtons.OK);

						return;
					}

					ShowTab("addedTab");

					revocationcode2Field.Text = mEnroll.RevocationCode;
					tabs.SelectedTab = tabs.TabPages["addedTab"];

					closeButton.Location = cancelButton.Location;
					closeButton.Visible = true;
					cancelButton.Visible = false;

					break;
				}
				catch (InvalidEnrollResponseException iere)
				{
					if (MainForm.ErrorDialog(this, "An error occurred while registering the authenticator", iere, MessageBoxButtons.RetryCancel) != DialogResult.Retry)
					{
						break;
					}
				}
			} while (true);
		}

		/// <summary>
		/// Show the named tab hiding all others
		/// </summary>
		/// <param name="name">name of tab to show</param>
		/// <param name="only">hide all others, or append if false</param>
		private void ShowTab(string name, bool only = true)
		{
			if (only)
			{
				tabs.TabPages.Clear();
			}

			if (tabs.TabPages.ContainsKey(name) == false)
			{
				tabs.TabPages.Add(mTabPages[name]);
			}

			tabs.SelectedTab = tabs.TabPages[name];
		}


#endregion

	}
}
