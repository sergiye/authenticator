using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Authenticator {
  public class WinApi {
    public const int WM_SETREDRAW = 0x0b;
    public const int WM_MOUSEWHEEL = 0x020A;
    public const int WM_USER = 0x0400;

    public const int WM_VSCROLL = 0x115;
    public const int SB_THUMBTRACK = 5;
    public const int SB_ENDSCROLL = 8;
    public const int SIF_TRACKPOS = 0x10;
    public const int SIF_RANGE = 0x1;
    public const int SIF_POS = 0x4;
    public const int SIF_PAGE = 0x2;
    public const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;

    public struct ScrollInfoStruct {
      public int CbSize;
      public int FMask;
      public int NMin;
      public int NMax;
      public int NPage;
      public int NPos;
      public int NTrackPos;
    }

    [DllImport("user32.dll", EntryPoint = "SendMessageA", SetLastError = true)]
    internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, int wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    internal static extern IntPtr SetActiveWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    internal static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    internal static extern IntPtr FindWindow(string className, string windowName);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
    internal static extern IntPtr GetParent(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern int GetScrollInfo(IntPtr hWnd, int n, ref ScrollInfoStruct lpScrollInfo);

    [DllImport("user32.dll")]
    internal static extern IntPtr GetOpenClipboardWindow();

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    internal static extern int GetWindowTextLength(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    public class MessageForwarder : IMessageFilter {
      private Control control;
      private Control previousParent;
      private HashSet<int> messages;
      private bool isMouseOverControl;

      public MessageForwarder(Control control, int message) : this(control, new[] {message}) {
      }

      public MessageForwarder(Control control, IEnumerable<int> messages) {
        this.control = control;
        this.messages = new HashSet<int>(messages);
        previousParent = control.Parent;
        isMouseOverControl = false;

        control.ParentChanged += control_ParentChanged;
        control.MouseEnter += (sender, e) => isMouseOverControl = true;
        control.MouseLeave += (sender, e) => isMouseOverControl = false;
        control.Leave += (sender, e) => isMouseOverControl = false;

        if (control.Parent != null) {
          Application.AddMessageFilter(this);
        }
      }

      public bool PreFilterMessage(ref Message m) {
        if (!messages.Contains(m.Msg) || !control.CanFocus || control.Focused || !isMouseOverControl) return false;
        try {
          SendMessage(control.Handle, (uint) m.Msg, (int) m.WParam, m.LParam);
        }
        catch (OverflowException) {
        }
        return true;
      }

      private void control_ParentChanged(object sender, EventArgs e) {
        if (control.Parent == null) {
          Application.RemoveMessageFilter(this);
        }
        else if (previousParent == null) {
          Application.AddMessageFilter(this);
        }
        previousParent = control.Parent;
      }
    }
  }
}