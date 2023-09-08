using System.Drawing;
using System.Windows.Forms;

namespace Authenticator {
  public partial class ShortcutEditor : Form {
    private readonly KeysConverter converter = new KeysConverter();

    public Keys Keys { get; set; }

    public ShortcutEditor() {
      InitializeComponent();
      Icon = Icon.ExtractAssociatedIcon(Updater.CurrentFileLocation);
      ShowCurrentShortcut();
    }

    private void ShowCurrentShortcut() {
      txtShortcut.Text = converter.ConvertToString(Keys)?.Replace("+Menu", "").Replace("+ControlKey", "")
        .Replace("+ShiftKey", "");
    }

    private void txtShortcut_KeyDown(object sender, KeyEventArgs e) {
      // var modifierKeys = e.Modifiers;
      // var pressedKey = e.KeyData ^ modifierKeys; //remove modifier keys
      //do stuff with pressed and modifier keys

      if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
        Keys = Keys.None;
      else
        Keys = e.KeyData;

      ShowCurrentShortcut();
      e.SuppressKeyPress = true;
      e.Handled = true;
    }
  }
}