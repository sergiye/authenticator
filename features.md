# Authenticator features

This document describes everything Authenticator can do: the supported authenticator types, how to add and manage them, how your data is protected, import and export, and all application options.

## Supported authenticators

Authenticator generates one-time passwords for any service that supports standard two-factor authentication.

| Type | Description |
|---|---|
| Time-based (TOTP, RFC 6238) | The common type used by Google Authenticator and most services. The code changes every period (30 seconds by default). |
| Counter-based (HOTP, RFC 4226) | The code changes each time a new one is requested; the counter is stored and increased on every generated code. |
| Microsoft | Time-based authenticator for Microsoft accounts, with Microsoft icons. |
| Okta Verify | Time-based authenticator for Okta, with the Okta icon. |

For time-based and counter-based authenticators you can set:

* the number of digits (6 by default, up to 9);
* the period in seconds (30 by default);
* the HMAC algorithm: SHA1 (default), SHA256 or SHA512.

## Adding an authenticator

Use **File → Add** (the first entry has the `Ctrl+A` shortcut) and choose the type. In the dialog, enter a name and provide the secret in any of these forms:

* the secret key as shown by the service (Base32, spaces and dashes are ignored);
* an `otpauth://totp/...` or `otpauth://hotp/...` URI; the name, issuer, digits, period, algorithm and counter are taken from it;
* a Google Authenticator export (`otpauth-migration://offline?data=...`); only the first account of the export is added and you are warned if it contains more;
* an `https://` address of a QR code image (plain `http://` is refused);
* a `data:image/...;base64,...` QR code image;
* the path of a QR code image file, or pick one with the browse button.

You can also capture a QR code directly from the screen: the dialog hides and you select the area of the screen that contains the code.

Click **Verify** to check the secret and see the first code. For counter-based authenticators you can enter the current counter value. The icon is detected automatically from the issuer when possible.

When the first authenticator is added, you are asked how the configuration should be protected (see [Protection](#protection)).

## Working with authenticators

### Showing codes

* With **Auto Refresh** enabled, the code is always visible and refreshed automatically; the pie on the right shows the time left.
* Without auto refresh, click the code area or the refresh icon on the right to show the code for 10 seconds.
* For counter-based authenticators each shown code increases the counter.
* Password-protected authenticators ask for their password when the code is requested and lock again after the code is hidden.

### Copying codes

* Double-click an authenticator to copy its current code, even when the code is hidden.
* **Copy on New Code** copies the code to the clipboard each time a new code is shown.
* Copied codes are excluded from the Windows clipboard history and cloud clipboard, and are removed from the clipboard after 30 seconds if they are still there.

### Organizing

* Drag and drop authenticators to change their order.
* **Show Filter** (`Ctrl+F`) shows a filter box that searches by name and icon; `Esc` clears it.
* **Rename** (`F2`) edits the name directly in the list.
* **Icon** offers automatic detection by issuer, more than a hundred built-in service icons grouped alphabetically, or your own image file.

### Context menu

Right-click an authenticator to open its context menu:

| Item | Shortcut | Description |
|---|---|---|
| Show Code | `Ctrl+Space` | Shows the current code (only when auto refresh is off). |
| Copy Code | `Ctrl+Shift+C` | Copies the current code to the clipboard. |
| Show Secret Key... | `Ctrl+Shift+V` | Shows the secret key and its QR code, for time-based and counter-based authenticators. Requires the main password if the configuration is protected with one. |
| Auto Refresh | `Ctrl+Shift+A` | Keeps the code visible and refreshes it automatically. Not available for counter-based or password-protected authenticators. |
| Copy on New Code | | Copies each newly shown code to the clipboard. |
| Icon | | Selects an automatic, built-in or custom icon. |
| Rename | `F2` | Renames the authenticator. |
| Set Password... | `Ctrl+P` | Adds, changes or removes a password for this authenticator only. |
| Delete | | Deletes the authenticator after confirmation. |

## Protection

### Configuration protection

**File → Change Protection** selects how the whole configuration file is encrypted. The options can be combined:

* **Password**: the configuration is encrypted with your password (AES-256-GCM with a PBKDF2-HMAC-SHA256 key). The password is requested at startup.
* **Windows account**: the configuration can be decrypted only by your Windows user account (DPAPI).
* **This computer**: the configuration can be decrypted only on this computer (DPAPI).

Changing the protection requires the current password when one is set. If no protection is chosen, the configuration is stored unencrypted.

### Per-authenticator password

Any authenticator can have its own additional password (**Set Password...** in the context menu). Its secret stays encrypted in memory and on disk; the password is requested each time a code is needed, and the authenticator is locked again 10 seconds after the code is shown.

### Other safeguards

* The configuration is saved atomically, so an interrupted save cannot corrupt it.
* Changes are saved automatically shortly after they are made.
* A configuration saved by a newer version of Authenticator is not overwritten by an older one.

## Import and export

### Import

**File → Import** (`Ctrl+I`) adds authenticators from:

| Format | Description |
|---|---|
| Text file (`.txt`) | One `otpauth://` URI per line; empty lines and lines starting with `#` are ignored. |
| Zip file (`.zip`) | Text files with `otpauth://` URIs inside a zip archive, optionally password protected. |
| PGP file (`.pgp`) | An armored PGP message decrypted with your private key and its password. |
| Authenticator config (`.config`) | Another Authenticator (or older version 2) configuration file. |

Imported authenticators with a name that already exists are renamed with a number.

### Export

**File → Export** (`Ctrl+E`) writes all authenticators as `otpauth://` URIs. The main password is requested first if the configuration is protected with one. The export can be:

* a plain text file;
* a password-protected zip file (AES-256 encryption); choosing a password always produces a zip file;
* a PGP message encrypted with a public key.

Password-protected authenticators must be unlocked to be exported; if you skip any of them, you are told which ones will be missing and can cancel the export.

## Main window and system tray

### File menu

| Item | Shortcut |
|---|---|
| Add | `Ctrl+A` for the first authenticator type |
| Export | `Ctrl+E` |
| Import | `Ctrl+I` |
| Change Protection | |
| Exit | `Ctrl+W` |

The Add, Import and Change Protection items are hidden when the configuration file is read-only.

### Options menu

| Item | Shortcut | Description |
|---|---|---|
| Run On User Login | | Starts Authenticator when you sign in to Windows. |
| Start Minimized | | Starts minimized (or hidden in the tray when the tray icon is used). |
| Use System Tray Icon | | Shows an icon in the notification area; closing the window hides it to the tray. |
| Auto-Update App | | Checks for and installs new versions automatically. |
| Auto-Size | `Ctrl+S` | Sizes the window to fit the authenticators (up to half of the screen height). |
| Show Filter | `Ctrl+F` | Shows the filter box. |
| Item Size | `Ctrl+1`, `Ctrl+2`, `Ctrl+3` | Small, medium or large list items. |
| Always On Top | `Ctrl+T` | Keeps the window above other windows. |
| Hide Menu | `Ctrl+M` | Hides the menu bar; press `Alt` to show it temporarily. |
| Theme | | Auto, Light, Dark and custom themes. |

### Help menu

| Item | Shortcut |
|---|---|
| Site | `Ctrl+F1` |
| Check For Updates | `Ctrl+U` |
| About | `F1` |

### System tray

When **Use System Tray Icon** is enabled, the tray icon menu offers:

* **Show/Hide** the main window (also by double-clicking the icon);
* **Click Action**: whether clicking an authenticator in the tray menu shows the code in a notification or copies it to the clipboard;
* the first 30 authenticators, for quick access to their codes;
* **Exit**.

## Themes

Auto (follows Windows), Light and Dark themes are built in. Custom themes can be added as `{themeName}.json` files in a `themes` folder next to the executable; see the [README](README.md) for the file format.

## Configuration file and portable mode

The configuration is stored in `Authenticator.config`. At startup it is searched for in this order:

1. the file passed on the command line;
2. the current directory;
3. the folder of the executable (portable mode);
4. `%AppData%\Authenticator`, where a new configuration is created by default.

To use Authenticator as a portable application, keep `Authenticator.config` next to the executable. A read-only configuration file is opened without saving any changes.

## Command line

```
Authenticator.exe [options] [config file]
```

| Option | Description |
|---|---|
| `-min`, `--minimize` | Start minimized. |
| `-p <password>`, `--password <password>` | Password used to decrypt the configuration. |
| `config file` | Path of the configuration file to use. |

Only one instance of Authenticator can run at a time.

## Developer libraries

The `TwoFactorAuth.Core` and `TwoFactorAuth` NuGet packages let you add two-factor authentication to your own applications: generate secret keys and provisioning URIs, generate and validate PIN codes (with a configurable clock drift tolerance, 30 seconds by default), and create QR code images without internet access. See the [README](README.md#developer-information) for sample code.
