using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Authenticator {
  /// <summary>
  /// Subclass of the MetroForm that replaces Text properties of any matching controls from the resource file
  /// </summary>
  [ToolboxBitmap(typeof(Form))]
	public class ResourceForm : Form
	{
		/// <summary>
		/// Comparer that checks two types and returns trues if they are the same or one is based on the other
		/// </summary>
		class BasedOnComparer : IEqualityComparer<Type>
		{
			/// <summary>
			/// Check if two types are equal or subclassed
			/// </summary>
			/// <param name="t1"></param>
			/// <param name="t2"></param>
			/// <returns>true if equal or subclassed</returns>
			public bool Equals(Type t1, Type t2)
			{
				return (t1 == t2 || t2.IsSubclassOf(t1));
			}

			/// <summary>
			/// Get the hash code for a Type
			/// </summary>
			/// <param name="t"></param>
			/// <returns></returns>
			public int GetHashCode(Type t)
			{
				return t.GetHashCode();
			}
		}

		/// <summary>
		/// Create the new form
		/// </summary>
		public ResourceForm()
			: base()
		{
		}

		/// <summary>
		/// Search the resources for any _FORMNAME_CONTROLNAME_ strings and replace the text
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			// go through all controls and set any text from resources (including this form)
			//var controls = GetControls(this, new Type[] { typeof(MetroFramework.Controls.MetroLabel), typeof(MetroFramework.Controls.MetroCheckBox) });
			var controls = GetControls(this);

			var formname = "_" + Name + "_";
			var text = AuthMain.StringResources.GetString(formname, System.Threading.Thread.CurrentThread.CurrentCulture);
			if (text != null)
			{
				Text = text;
			}
			foreach (var c in controls)
			{
				var controlname = formname + c.Name + "_";
				text = AuthMain.StringResources.GetString(controlname, System.Threading.Thread.CurrentThread.CurrentCulture);
				if (text != null)
				{
					if (c is TextBox)
					{
						((TextBox)c).Text = text;
					}
					else
					{
						c.Text = text;
					}
				}
			}

			base.OnLoad(e);
		}

		/// <summary>
		/// Get a recursive list of all controls that match the array of types
		/// </summary>
		/// <param name="control">parent control</param>
		/// <param name="controlTypes">array of types to match</param>
		/// <param name="controls">existing list of Controls to add to</param>
		/// <returns>list of Control objects</returns>
		private List<Control> GetControls(Control control, Type[] controlTypes = null, List<Control> controls = null)
		{
			if (controls == null)
			{
				controls = new List<Control>();
			}
			var baseComparer = new BasedOnComparer();
			foreach (Control c in control.Controls)
			{
				if (controlTypes == null || controlTypes.Contains(c.GetType(), baseComparer))
				{
					controls.Add(c);
				}
				if (c.Controls != null && c.Controls.Count != 0)
				{
					GetControls(c, controlTypes, controls);
				}
			}

			return controls;
		}

	}
}
