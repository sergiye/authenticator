using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Authenticator {
  [ToolboxBitmap(typeof(Form))]
  public class ResourceForm : Form {
    class BasedOnComparer : IEqualityComparer<Type> {
      public bool Equals(Type t1, Type t2) {
        return (t1 == t2 || t2.IsSubclassOf(t1));
      }

      public int GetHashCode(Type t) {
        return t.GetHashCode();
      }
    }

    public ResourceForm()
      : base() {
    }

    protected override void OnLoad(EventArgs e) {
      // go through all controls and set any text from resources (including this form)
      //var controls = GetControls(this, new Type[] { typeof(MetroFramework.Controls.MetroLabel), typeof(MetroFramework.Controls.MetroCheckBox) });
      var controls = GetControls(this);

      var formname = "_" + Name + "_";
      var text = AuthMain.StringResources.GetString(formname, System.Threading.Thread.CurrentThread.CurrentCulture);
      if (text != null) {
        Text = text;
      }

      foreach (var c in controls) {
        var controlname = formname + c.Name + "_";
        text = AuthMain.StringResources.GetString(controlname, System.Threading.Thread.CurrentThread.CurrentCulture);
        if (text != null) {
          if (c is TextBox) {
            ((TextBox) c).Text = text;
          }
          else {
            c.Text = text;
          }
        }
      }

      base.OnLoad(e);
    }

    private List<Control> GetControls(Control control, Type[] controlTypes = null, List<Control> controls = null) {
      if (controls == null) {
        controls = new List<Control>();
      }

      var baseComparer = new BasedOnComparer();
      foreach (Control c in control.Controls) {
        if (controlTypes == null || controlTypes.Contains(c.GetType(), baseComparer)) {
          controls.Add(c);
        }

        if (c.Controls != null && c.Controls.Count != 0) {
          GetControls(c, controlTypes, controls);
        }
      }

      return controls;
    }
  }
}