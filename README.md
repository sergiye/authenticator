# Authenticator

[![Stars](https://img.shields.io/github/stars/sergiye/authenticator?style=flat-square)](https://github.com/sergiye/authenticator/stargazers)
[![Fork](https://img.shields.io/github/forks/sergiye/authenticator?style=flat-square)](https://github.com/sergiye/authenticator/fork)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/sergiye/authenticator?style=plastic)
![GitHub all releases](https://img.shields.io/github/downloads/sergiye/authenticator/total?style=plastic)
![GitHub last commit](https://img.shields.io/github/last-commit/sergiye/authenticator?style=plastic)

*Authenticator is a portable, open-source Authenticator for Windows that provides counter or time-based RFC 6238 authenticators and common implementations, such as the Google Authenticator.*

----

## Download Latest Version

Authenticator provides an alternative solution to combine various two-factor authenticator services in one convenient place.

This is the latest stable version and can be downloaded from the [releases](https://github.com/sergiye/authenticator/releases) page, or get the newer one directly from:
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

# Build

**The recommended way to get the program is BUILD from source**
- Install git, Visual Studio 2019 (or higher)
- `git clone https://github.com/sergiye/authenticator.git`
- build


### How To Use

To use:
  * Download the latest version from releases
  * There is no installation required, just start executable file from anywhere on your computer
  * Use the Add menu item to add or import an authenticator
  * Right-click any authenticator to bring up context menu
  * Click the icon on the right to show the current code, if auto-refresh is not enabled
  * Use options menu for program options

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

Unlike some other authenticator applications, Authenticator does not store/send your information to any 3rd party servers. Your authenticator information can be stored near executable file (portable mode), or (by default) is saved in your account roaming profile, i.e. `c:\Users\<username>\AppData\Roaming\Authenticator`. However, this file can be moved anywhere and passed into Authenticator when run.

----

## Thanks

Authenticator is based on open source WinAuth application written by Colin Mackie.

----

## License

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License  along with this program.  If not, see http://www.gnu.org/licenses/.


## Donate

<a href=https://www.buymeacoffee.com/sergiye>
<img src="https://www.buymeacoffee.com/assets/img/custom_images/yellow_img.png" alt="Buy Me A Coffee" style="height: auto !important;width: auto !important;" />
</a>

**If you like the program, you can support the author and transfer money to**
- Card number: 5169 3600 1644 3834 (https://easypay.ua/moneytransfer/transfer2card?id=99445709)
- IBAN number: UA113052990000026208909644481
