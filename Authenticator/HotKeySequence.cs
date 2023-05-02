﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Authenticator {
  /// <summary>
  /// A hot key sequence and command containing the key, modifier and script
  /// </summary>
  public class HoyKeySequence {
    /// <summary>
    /// Modifier for hotkey
    /// </summary>
    public WinApi.KeyModifiers Modifiers;

    /// <summary>
    /// Hotkey code
    /// </summary>
    public WinApi.VirtualKeyCode HotKey;

    /// <summary>
    /// Flag if custom script
    /// </summary>
    public bool Advanced;

    /// <summary>
    /// Any custom script
    /// </summary>
    public string AdvancedScript;

    /// <summary>
    /// Windows title for script
    /// </summary>
    public string WindowTitle;

    /// <summary>
    /// Use regular expression for window title
    /// </summary>
    public bool WindowTitleRegex;

    /// <summary>
    /// Process name for script
    /// </summary>
    public string ProcessName;

    /// <summary>
    /// Create a new blank HotKeySequcen
    /// </summary>
    public HoyKeySequence() {
    }

    /// <summary>
    /// Create a new HotKeySequence from a loaded string
    /// </summary>
    /// <param name="data"></param>
    public HoyKeySequence(string data) {
      if (string.IsNullOrEmpty(data) == false) {
        var match = Regex.Match(data, @"([0-9a-fA-F]{8})([0-9a-fA-F]{4})\t([^\t]*)\t(Y|N)(.*)", RegexOptions.Multiline);
        if (match.Success) {
          Modifiers = (WinApi.KeyModifiers) BitConverter.ToInt32(Authenticator.StringToByteArray(match.Groups[1].Value),
            0);
          HotKey = (WinApi.VirtualKeyCode) BitConverter.ToUInt16(Authenticator.StringToByteArray(match.Groups[2].Value),
            0);
          WindowTitle = match.Groups[3].Value;
          Advanced = (match.Groups[4].Value == "Y");
          if (Advanced) {
            AdvancedScript = match.Groups[5].Value;
          }
        }
      }
    }

    /// <summary>
    /// Create a new HotKeySequence from a loaded string
    /// </summary>
    /// <param name="data">XmlNode from config</param>
    public HoyKeySequence(XmlNode autoLoginNode, string password, decimal version) {
      var boolVal = false;
      var node = autoLoginNode.SelectSingleNode("modifiers");
      if (node != null && node.InnerText.Length != 0) {
        Modifiers = (WinApi.KeyModifiers) BitConverter.ToInt32(Authenticator.StringToByteArray(node.InnerText), 0);
      }

      node = autoLoginNode.SelectSingleNode("hotkey");
      if (node != null && node.InnerText.Length != 0) {
        HotKey = (WinApi.VirtualKeyCode) BitConverter.ToUInt16(Authenticator.StringToByteArray(node.InnerText), 0);
      }

      node = autoLoginNode.SelectSingleNode("windowtitle");
      if (node != null && node.InnerText.Length != 0) {
        WindowTitle = node.InnerText;
      }

      node = autoLoginNode.SelectSingleNode("windowtitleregex");
      if (node != null && bool.TryParse(node.InnerText, out boolVal)) {
        WindowTitleRegex = boolVal;
      }

      node = autoLoginNode.SelectSingleNode("processname");
      if (node != null && node.InnerText.Length != 0) {
        ProcessName = node.InnerText;
      }

      node = autoLoginNode.SelectSingleNode("advanced");
      if (node != null && bool.TryParse(node.InnerText, out boolVal)) {
        Advanced = boolVal;
      }

      node = autoLoginNode.SelectSingleNode("script");
      if (node != null && node.InnerText.Length != 0) {
        var data = node.InnerText;

        var attr = node.Attributes["encrypted"];
        if (attr != null && attr.InnerText.Length != 0) {
          var encTypes = attr.InnerText.ToCharArray();
          // we read the string in reverse order (the order they were encrypted)
          for (var i = encTypes.Length - 1; i >= 0; i--) {
            var encryptedType = encTypes[i];
            switch (encryptedType) {
              case 'u': {
                // we are going to decrypt with the Windows User account key
                var cipher = Authenticator.StringToByteArray(data);
                var plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.CurrentUser);
                data = Encoding.UTF8.GetString(plain, 0, plain.Length);
                break;
              }
              case 'm': {
                // we are going to decrypt with the Windows local machine key
                var cipher = Authenticator.StringToByteArray(data);
                var plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.LocalMachine);
                data = Encoding.UTF8.GetString(plain, 0, plain.Length);
                break;
              }
              case 'y': {
                // we use an explicit password to encrypt data
                if (string.IsNullOrEmpty(password)) {
                  throw new EncryptedSecretDataException();
                }

                data = Authenticator.Decrypt(data, password, (version >= (decimal) 1.7)); // changed encrypted in 1.7
                var plain = Authenticator.StringToByteArray(data);
                data = Encoding.UTF8.GetString(plain, 0, plain.Length);
                break;
              }
              default:
                break;
            }
          }
        }

        AdvancedScript = data;
      }
    }

    public void ReadXml(XmlReader reader, string password = null) {
      reader.MoveToContent();

      if (reader.IsEmptyElement) {
        reader.Read();
        return;
      }

      reader.Read();
      while (reader.EOF == false) {
        if (reader.IsStartElement()) {
          switch (reader.Name) {
            case "modifiers":
              Modifiers = (WinApi.KeyModifiers) BitConverter.ToInt32(
                Authenticator.StringToByteArray(reader.ReadElementContentAsString()), 0);
              break;

            case "hotkey":
              HotKey = (WinApi.VirtualKeyCode) BitConverter.ToUInt16(
                Authenticator.StringToByteArray(reader.ReadElementContentAsString()), 0);
              break;

            case "windowtitle":
              WindowTitle = reader.ReadElementContentAsString();
              break;

            case "windowtitleregex":
              WindowTitleRegex = reader.ReadElementContentAsBoolean();
              break;

            case "processname":
              ProcessName = reader.ReadElementContentAsString();
              break;

            case "advanced":
              Advanced = reader.ReadElementContentAsBoolean();
              break;

            case "script":
              var encrypted = reader.GetAttribute("encrypted");
              var data = reader.ReadElementContentAsString();

              if (string.IsNullOrEmpty(encrypted) == false) {
                var passwordType = Authenticator.DecodePasswordTypes(encrypted);
                data = Authenticator.DecryptSequence(data, passwordType, password, true);
                //byte[] plain = Authenticator.StringToByteArray(data);
                //data = Encoding.UTF8.GetString(plain, 0, plain.Length);

/*
                char[] encTypes = encrypted.ToCharArray();
                // we read the string in reverse order (the order they were encrypted)
                for (int i = encTypes.Length - 1; i >= 0; i--)
                {
                  char encryptedType = encTypes[i];
                  switch (encryptedType)
                  {
                    case 'u':
                      {
                        // we are going to decrypt with the Windows User account key
                        byte[] cipher = Authenticator.StringToByteArray(data);
                        byte[] plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.CurrentUser);
                        data = Encoding.UTF8.GetString(plain, 0, plain.Length);
                        break;
                      }
                    case 'm':
                      {
                        // we are going to decrypt with the Windows local machine key
                        byte[] cipher = Authenticator.StringToByteArray(data);
                        byte[] plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.LocalMachine);
                        data = Encoding.UTF8.GetString(plain, 0, plain.Length);
                        break;
                      }
                    case 'y':
                      {
                        // we use an explicit password to encrypt data
                        if (string.IsNullOrEmpty(password) == true)
                        {
                          throw new EncryptedSecretDataException();
                        }
                        data = Authenticator.Decrypt(data, password, true);
                        byte[] plain = Authenticator.StringToByteArray(data);
                        data = Encoding.UTF8.GetString(plain, 0, plain.Length);
                        break;
                      }
                    default:
                      break;
                  }
                }
*/
              }

              AdvancedScript = data;

              break;

            default:
              reader.Skip();
              break;
          }
        }
        else {
          reader.Read();
          break;
        }
      }
    }

    /// <summary>
    /// Write data into the XmlWriter
    /// </summary>
    /// <param name="writer">XmlWriter to write to</param>
    public void WriteXmlString(XmlWriter writer) {
      writer.WriteStartElement("autologin");

      writer.WriteStartElement("modifiers");
      writer.WriteString(Authenticator.ByteArrayToString(BitConverter.GetBytes((int) Modifiers)));
      writer.WriteEndElement();
      //
      writer.WriteStartElement("hotkey");
      writer.WriteString(Authenticator.ByteArrayToString(BitConverter.GetBytes((ushort) HotKey)));
      writer.WriteEndElement();
      //
      writer.WriteStartElement("windowtitle");
      writer.WriteCData(WindowTitle ?? string.Empty);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("processname");
      writer.WriteCData(ProcessName ?? string.Empty);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("windowtitleregex");
      writer.WriteValue(WindowTitleRegex);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("advanced");
      writer.WriteValue(Advanced);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("script");
      var script = AdvancedScript.Replace("\n", string.Empty);
      writer.WriteCData(script);
      writer.WriteEndElement();

      writer.WriteEndElement();
    }

    /// <summary>
    /// Get an xml string representation of the HotKeySequence
    /// </summary>
    /// <returns>string representation</returns>
    //public override string ToString()
    //{
    //	StringBuilder sb = new StringBuilder();
    //	sb.Append("<modifiers>").Append(Authenticator.ByteArrayToString(BitConverter.GetBytes((int)Modifiers))).Append("</modifiers>");
    //	sb.Append("<hotkey>").Append(Authenticator.ByteArrayToString(BitConverter.GetBytes((ushort)HotKey))).Append("</hotkey>");
    //	sb.Append("<windowtitle><![CDATA[").Append(WindowTitle ?? string.Empty).Append("]]></windowtitle>");
    //	sb.Append("<processname><![CDATA[").Append(ProcessName ?? string.Empty).Append("]]></processname>");
    //	sb.Append("<windowtitleregex>").Append(WindowTitleRegex.ToString()).Append("</windowtitleregex>");
    //	sb.Append("<advanced>").Append(Advanced.ToString()).Append("</advanced>");
    //	sb.Append("<script><![CDATA[").Append(AdvancedScript.Replace("\n", string.Empty)).Append("]]></script>");

    //	return sb.ToString();
    //}
  }
}