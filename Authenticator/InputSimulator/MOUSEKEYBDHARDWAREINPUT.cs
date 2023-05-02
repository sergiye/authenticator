using System.Runtime.InteropServices;

namespace Authenticator {
#pragma warning disable 649
  /// <summary>
  /// The combined/overlayed structure that includes Mouse, Keyboard and Hardware Input message data (see: http://msdn.microsoft.com/en-us/library/ms646270(VS.85).aspx)
  /// </summary>
  [StructLayout(LayoutKind.Explicit)]
  struct Mousekeybdhardwareinput {
    [FieldOffset(0)] public Mouseinput Mouse;

    [FieldOffset(0)] public Keybdinput Keyboard;

    [FieldOffset(0)] public Hardwareinput Hardware;
  }
#pragma warning restore 649
}