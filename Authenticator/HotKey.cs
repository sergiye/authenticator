using System;
using System.Xml;

namespace Authenticator {
  /// <summary>
  /// A hot key sequence and command containing the key, modifier and script
  /// </summary>
  public class HotKey {
    public enum HotKeyActions {
      Inject,
      Copy,
      Notify,
      Advanced
    };

    /// <summary>
    /// Modifier for hotkey
    /// </summary>
    public WinApi.KeyModifiers Modifiers;

    /// <summary>
    /// Hotkey code
    /// </summary>
    public WinApi.VirtualKeyCode Key;

    /// <summary>
    /// Action to be perform on hotkey
    /// </summary>
    public HotKeyActions Action;

    /// <summary>
    /// Specific window title or process name
    /// </summary>
    public string Window;

    /// <summary>
    /// Copy of advanced script from authenticator data
    /// </summary>
    public string Advanced;

    /// <summary>
    /// Create a new HotKey
    /// </summary>
    public HotKey() {
      Action = HotKeyActions.Notify;
    }

    /// <summary>
    /// Read the saved Xml to load the hotkey
    /// </summary>
    /// <param name="reader">XmlReader</param>
    public void ReadXml(XmlReader reader) {
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

            case "key":
              Key = (WinApi.VirtualKeyCode) BitConverter.ToUInt16(
                Authenticator.StringToByteArray(reader.ReadElementContentAsString()), 0);
              break;

            case "action":
              Action = (HotKeyActions) Enum.Parse(typeof(HotKeyActions), reader.ReadElementContentAsString(), true);
              break;

            case "window":
              Window = reader.ReadElementContentAsString();
              break;

            case "advanced":
              Advanced = reader.ReadElementContentAsString();
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
      writer.WriteStartElement("hotkey");

      writer.WriteStartElement("modifiers");
      writer.WriteString(Authenticator.ByteArrayToString(BitConverter.GetBytes((int) Modifiers)));
      writer.WriteEndElement();
      //
      writer.WriteStartElement("key");
      writer.WriteString(Authenticator.ByteArrayToString(BitConverter.GetBytes((ushort) Key)));
      writer.WriteEndElement();
      //
      writer.WriteStartElement("action");
      writer.WriteString(Enum.GetName(typeof(HotKeyActions), Action));
      writer.WriteEndElement();
      //
      if (String.IsNullOrEmpty(Window) == false) {
        writer.WriteStartElement("window");
        writer.WriteString(Window);
        writer.WriteEndElement();
      }

      //
      if (String.IsNullOrEmpty(Advanced) == false) {
        writer.WriteStartElement("advanced");
        writer.WriteString(Advanced);
        writer.WriteEndElement();
      }

      writer.WriteEndElement();
    }

    /// <summary>
    /// Get the display string for this shortcut
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var shortcut = Key.ToString().Substring(3);
      if ((Modifiers & WinApi.KeyModifiers.Alt) != 0) {
        shortcut = "Alt-" + shortcut;
      }

      if ((Modifiers & WinApi.KeyModifiers.Control) != 0) {
        shortcut = "Ctrl-" + shortcut;
      }

      if ((Modifiers & WinApi.KeyModifiers.Shift) != 0) {
        shortcut = "Shift-" + shortcut;
      }

      return shortcut;
    }
  }
}