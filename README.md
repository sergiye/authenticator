# Authenticator

[![Release (latest)](https://img.shields.io/github/v/release/sergiye/authenticator)](https://github.com/sergiye/authenticator/releases/latest)
![Downloads](https://img.shields.io/github/downloads/sergiye/authenticator/total?color=ff4f42)
![GitHub last commit](https://img.shields.io/github/last-commit/sergiye/authenticator?color=00AD00)

[![Nuget](https://img.shields.io/nuget/v/TwoFactorAuth)](https://www.nuget.org/packages/TwoFactorAuth/)
[![Nuget](https://img.shields.io/nuget/dt/TwoFactorAuth?label=nuget-downloads)](https://www.nuget.org/packages/TwoFactorAuth/)

[![Nuget](https://img.shields.io/nuget/v/TwoFactorAuth.Core)](https://www.nuget.org/packages/TwoFactorAuth.Core/)
[![Nuget](https://img.shields.io/nuget/dt/TwoFactorAuth.Core?label=nuget-downloads)](https://www.nuget.org/packages/TwoFactorAuth.Core/)

*Authenticator is a portable OTP/Two-Factor Authentication tool for Windows that provides a user-friendly interface and allows you to copy the login code instead of manually entering it from your phone.*

Authenticator provides an alternative solution to combine various two-factor authenticator services in one convenient place.

It works with counter or time-based RFC 6238 authenticators and common implementations, such as the Google Authenticator.

----
## What does it look like?

Here's a preview of the app's UI running on Windows 10:

[<img src="https://github.com/sergiye/authenticator/raw/master/preview.png" alt="preview"/>](https://github.com/sergiye/authenticator/raw/master/preview.png)

Also there are:
 - `Auto`/`Light`/`Dark` themes integrated into executable.
 - Custom `themes` supported from external files

You can find custom theme examples [here](https://github.com/sergiye/authenticator/tree/master/themes)
To add custom theme to the app, just create a `themes` folder next to the executable file and place all theme files there.
Don't forget to restart the app to scan for new theme files!

## Download Latest Version

The published version can be obtained from [releases](https://github.com/sergiye/authenticator/releases) page, or get the newer one directly from:
[Latest Version](https://github.com/sergiye/authenticator/releases/latest)

Features include:

  * Support for time-based RFC 6238 authenticators (e.g. Google Authenticator) and HOTP counter-based authenticators
  * Displays multiple authenticators simultaneously
  * Codes displayed and refreshed automatically or on demand
  * Data is protected with your password, locked to Windows machine or account
  * Additional password protection per authenticator
  * Selection of standard or custom icons
  * Capture a QR code from a selected area of the screen, image URL or parse from 'otpauth' string
  * Portable mode preventing changes to other files or registry settings
  * Import and export in UriKeyFormat and from Authenticator Plus for Android 


## How To Use

To use:
  * Download the latest version from releases
  * There is no installation required, just start executable file from anywhere on your computer
  * Use the Add menu item to add or import an authenticator
  * Right-click any authenticator to bring up context menu
  * Click the icon on the right to show the current code, if auto-refresh is not enabled
  * Use options menu for program options

## Developer information
**Integrate the TwoFactorAuth/TwoFactorAuth.Core library in own application**
1. Add the [TwoFactorAuth](https://www.nuget.org/packages/TwoFactorAuth/) NuGet package to your application.
2. Use the sample code below

**Sample code**
```c#
//generate secret key
var accountId = 12345;
var accountName = "user@mail.org";
var applicationName = "Your Application Name";
var tfa = new TwoFactorAuthenticator(accountId);
var secretKey = tfa.ManualSecretKey;

//generate QR code image by google api
var qrCodeImageLink = QrGenerator.GetQrCodeLink(applicationName, $"{applicationName} - {accountName}", secretKey);
var qrCodeImageBytes = QrGenerator.GenerateQrCodeByGoogle(applicationName, $"{applicationName} - {accountName}", secretKey);

//base64 image string to use on web page (<img src="data:image/png;base64, ...) - no internet required
var qrCodeImageString = QrGeneratorEx.GenerateQrCode(applicationName, $"{applicationName} - {accountName}", secretKey);

//generate recovery codes
const int codesCount = 10;
var recoveryCodes = new string[codesCount];
for (var i = 0; i < codesCount; i++)
  recoveryCodes[i] = tfa.GenerateHashedCode(Environment.TickCount + i, 10);

//generate and validate PIN
var currentPin = tfa.GetCurrentPin();
var isValid = tfa.ValidateTwoFactorPin(currentPin);
```

----

## COMMON QUESTIONS

#### Is it secure? Is it safe?

All authenticators just provide another layer of security. None are 100% effective.

A physical/keychain device is by far the best protection. Although still subject to any man-in-the-middle attack, there is no way to get at the secret key stored within it. If you are at all concerned, get one of these.

An iPhone app or app on a non-rooted Android device is also secure. There is no way to get at the secret key stored on the device, however, some apps provides way to export the key that could compromise your authenticator if you do not physically protect your phone. Also if those apps backup their data elsewhere, that data could be vulnerable.

A rooted-Android phone can have your secret key read off it by an app with access. Some apps also do not encrypt the keys and so this should be considered risky.

Authenticator stores you secret key in an encrypted file on your computer. Whilst it cannot therefore provide the same security as a separate physical device, as much as possible has been done to protect the key on your machine. As above, physical access to your machine would be the only way to compromise any authenticator.

#### I'm concerned this might be a virus / malware / keylogger

Authenticator is an open-source allowing everyone to inspect and review the code. A binary is provided, but the source code is always released simultaneously so that you can review the code and build it yourself.

No personal information is sent out to any other 3rd party servers. It never even sees your account information, only your authenticator details.

There are no other executables installed on your machine. There is no installer doing things you are unable to monitor. Authenticator is portable so you can just run it from anywhere.

#### I found Authenticator on another website, is it the same thing?

Authenticator source code is uploaded to GitHub at http://github.com/sergiey/authenticator and pre-built binaries are in [releases](https://github.com/sergiye/authenticator/releases). It is not published anywhere else, so please do not download any other programs claiming to be Authenticator.

#### Where does Authenticator save my authenticator information?

Unlike some other authenticator applications, Authenticator does not store/send your information to any 3rd party servers. Your authenticator information can be stored near executable file (portable mode), or (by default) is saved in your account roaming profile, i.e. `%AppData%\Authenticator`. However, this file can be moved anywhere and passed into Authenticator when run.

----

## Thanks
Authenticator is inspired by on open source [WinAuth](https://github.com/winauth/winauth) application written by Colin Mackie.

## License
This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License  along with this program.  If not, see http://www.gnu.org/licenses/.

## How can I help improve it?
The authenticator team welcomes feedback and contributions!<br/>
You can check if it works properly on your PC. If you notice any inaccuracies, please send us a pull request. If you have any suggestions or improvements, don't hesitate to create an issue.

Also, don't forget to ★ star ★ the repository to help other people find it.

<!-- [![Star History Chart](https://api.star-history.com/svg?repos=sergiye/authenticator&type=Date)](https://star-history.com/#sergiye/authenticator&Date) -->

[//]: # ([![Stargazers over time]&#40;https://starchart.cc/sergiye/authenticator.svg?variant=adaptive&#41;]&#40;https://starchart.cc/sergiye/authenticator&#41;)

[![Stargazers](https://reporoster.com/stars/sergiye/authenticator)](https://star-history.com/#sergiye/authenticator&Date)

<!-- [![Forkers](https://reporoster.com/forks/sergiye/authenticator)](https://github.com/sergiye/authenticator/network/members) -->

## Donate!
Every [cup of coffee](https://patreon.com/SergiyE) you donate will help this app become better and let me know that this project is in demand.