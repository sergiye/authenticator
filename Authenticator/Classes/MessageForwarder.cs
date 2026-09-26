using sergiye.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Authenticator {

  internal class MessageForwarder : IMessageFilter {

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
      WinApiHelper.SendMessage(control.Handle, (uint) m.Msg, m.WParam, m.LParam);
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
