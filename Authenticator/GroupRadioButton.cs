using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Authenticator {
  public class GroupMetroRadioButton : RadioButton
	{
		public GroupMetroRadioButton()
			: base()
		{
		}

		public GroupMetroRadioButton(string group)
			: base()
		{
			Group = group;
		}

		public string Group { get; set; }

		protected override void OnCheckedChanged(EventArgs e)
		{
			base.OnCheckedChanged(e);

			var group = Group;
			if (string.IsNullOrEmpty(group))
			{
				return;
			}

			var check = Checked;
			var form = FindParentControl<Form>();
			var radios = FindAllControls<GroupMetroRadioButton>(form);
			foreach (var grb in radios)
			{
				if (grb != this && check && grb.Group == group && grb.Checked)
				{
					grb.Checked = false;
				}
			}
		}

		private T FindParentControl<T>() where T : Control
		{
			var parent = Parent;
			while (parent != null && !(parent is T))
			{
				parent = parent.Parent;
			}
			return (T)parent;
		}

		private static T[] FindAllControls<T>(Control parent) where T : Control
		{
			var controls = new List<T>();
			foreach (Control c in parent.Controls)
			{
				if (c is T)
				{
					controls.Add((T)c);
					if (c.Controls != null && c.Controls.Count != 0)
					{
						controls.AddRange(FindAllControls<T>(c));
					}
				}
			}

			return controls.ToArray();
		}
	}

	public class GroupRadioButton : RadioButton
	{
		public GroupRadioButton()
			: base()
		{
		}

		public GroupRadioButton(string group)
			: base()
		{
			Group = group;
		}

		public string Group { get; set; }

		protected override void OnCheckedChanged(EventArgs e)
		{
			base.OnCheckedChanged(e);

			var group = Group;
			if (string.IsNullOrEmpty(group))
			{
				return;
			}

			var check = Checked;
			var form = FindParentControl<Form>();
			var radios = FindAllControls<GroupRadioButton>(form);
			foreach (var grb in radios)
			{
				if (grb != this && check && grb.Group == group && grb.Checked)
				{
					grb.Checked = false;
				}
			}
		}

		private T FindParentControl<T>() where T : Control
		{
			var parent = Parent;
			while (parent != null && !(parent is T))
			{
				parent = parent.Parent;
			}
			return (T)parent;
		}

		private static T[] FindAllControls<T>(Control parent) where T : Control
		{
			var controls = new List<T>();
			foreach (Control c in parent.Controls)
			{
				if (c is T)
				{
					controls.Add((T)c);
					if (c.Controls != null && c.Controls.Count != 0)
					{
						controls.AddRange(FindAllControls<T>(c));
					}
				}
			}

			return controls.ToArray();
		}
	}
}
