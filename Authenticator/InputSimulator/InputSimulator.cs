using System;
using System.Runtime.InteropServices;

namespace Authenticator {
  /// <summary>
  /// Provides a useful wrapper around the User32 SendInput and related native Windows functions.
  /// </summary>
  public static class InputSimulator {
    #region DllImports

    /// <summary>
    /// The SendInput function synthesizes keystrokes, mouse motions, and button clicks.
    /// </summary>
    /// <param name="numberOfInputs">Number of structures in the Inputs array.</param>
    /// <param name="inputs">Pointer to an array of INPUT structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.</param>
    /// <param name="sizeOfInputStructure">Specifies the size, in bytes, of an INPUT structure. If cbSize is not the size of an INPUT structure, the function fails.</param>
    /// <returns>The function returns the number of events that it successfully inserted into the keyboard or mouse input stream. If the function returns zero, the input was already blocked by another thread. To get extended error information, call GetLastError.Microsoft Windows Vista. This function fails when it is blocked by User Interface Privilege Isolation (UIPI). Note that neither GetLastError nor the return value will indicate the failure was caused by UIPI blocking.</returns>
    /// <remarks>
    /// Microsoft Windows Vista. This function is subject to UIPI. Applications are permitted to inject input only into applications that are at an equal or lesser integrity level.
    /// The SendInput function inserts the events in the INPUT structures serially into the keyboard or mouse input stream. These events are not interspersed with other keyboard or mouse input events inserted either by the user (with the keyboard or mouse) or by calls to keybd_event, mouse_event, or other calls to SendInput.
    /// This function does not reset the keyboard's current state. Any keys that are already pressed when the function is called might interfere with the events that this function generates. To avoid this problem, check the keyboard's state with the GetAsyncKeyState function and correct as necessary.
    /// </remarks>
    [DllImport("user32.dll", SetLastError = true)]
    static extern UInt32 SendInput(UInt32 numberOfInputs, Input[] inputs, Int32 sizeOfInputStructure);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern short VkKeyScan(char ch);

    #endregion

    #region Methods

    /// <summary>
    /// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted text entry via the keyboard.
    /// </summary>
    /// <param name="text">The text to be simulated.</param>
    public static void SimulateTextEntry(string text) {
      var chars = text.ToCharArray(); // UTF8Encoding.ASCII.GetBytes(text);
      var len = chars.Length;
      var inputList = new Input[len * 2];
      for (var x = 0; x < len; x++) {
        UInt16 scanCode = chars[x];
        var down = new Input();
        var up = new Input();

        if (scanCode < 32) {
          // map the scan code to a Vk so we can deal with special control chars, e.g. Tab and Enter
          var vk = (ushort) VkKeyScan(chars[x]);
          var state = (ushort) ((vk & (ushort) 0xff00) >> 8);
          vk = (ushort) (vk & (ushort) 0xff);

          down.Type = (UInt32) InputType.KEYBOARD;
          down.Data.Keyboard = new Keybdinput();
          down.Data.Keyboard.Vk = vk;
          down.Data.Keyboard.Scan = 0;
          down.Data.Keyboard.Flags = (UInt32) 0;
          down.Data.Keyboard.Time = 0;
          down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

          up.Type = (UInt32) InputType.KEYBOARD;
          up.Data.Keyboard = new Keybdinput();
          up.Data.Keyboard.Vk = vk;
          up.Data.Keyboard.Scan = 0;
          up.Data.Keyboard.Flags = (UInt32) (KeyboardFlag.KEYUP);
          up.Data.Keyboard.Time = 0;
          up.Data.Keyboard.ExtraInfo = IntPtr.Zero;
        }
        else {
          down.Type = (UInt32) InputType.KEYBOARD;
          down.Data.Keyboard = new Keybdinput();
          down.Data.Keyboard.Vk = 0;
          down.Data.Keyboard.Scan = scanCode;
          down.Data.Keyboard.Flags = (UInt32) KeyboardFlag.UNICODE;
          down.Data.Keyboard.Time = 0;
          down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

          up.Type = (UInt32) InputType.KEYBOARD;
          up.Data.Keyboard = new Keybdinput();
          up.Data.Keyboard.Vk = 0;
          up.Data.Keyboard.Scan = scanCode;
          up.Data.Keyboard.Flags = (UInt32) (KeyboardFlag.KEYUP | KeyboardFlag.UNICODE);
          up.Data.Keyboard.Time = 0;
          up.Data.Keyboard.ExtraInfo = IntPtr.Zero;
        }

        // Handle extended keys:
        // If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
        // we need to include the KEYEVENTF_EXTENDEDKEY flag in the Flags property. 
        if ((scanCode & 0xFF00) == 0xE000) {
          down.Data.Keyboard.Flags |= (UInt32) KeyboardFlag.EXTENDEDKEY;
          up.Data.Keyboard.Flags |= (UInt32) KeyboardFlag.EXTENDEDKEY;
        }

        inputList[2 * x] = down;
        inputList[2 * x + 1] = up;
      }

      var numberOfSuccessfulSimulatedInputs = SendInput((UInt32) len * 2, inputList, Marshal.SizeOf(typeof(Input)));
    }

    #endregion
  }
}