using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Authenticator {

  public class VCenteredTextBox : UserControl {
    private readonly TableLayoutPanel panel;
    private readonly TextBox textBox;

    public VCenteredTextBox() {

      TabStop = false;
      panel = new TableLayoutPanel() {
        Dock = DockStyle.Fill,
        BackColor = SystemColors.Window,
        CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
        ColumnCount = 1,
        RowCount = 1,
      };
      panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
      Controls.Add(panel);

      textBox = new TextBox {
        Anchor = AnchorStyles.Left | AnchorStyles.Right,
        BorderStyle = 0,
        Multiline = true,
        ReadOnly = true,
        TextAlign = HorizontalAlignment.Center,
        TabStop = false,
      };
      panel.Controls.Add(textBox, 0, 0);
    }

    public int SpaceOut { get; set; }

    public override string Text {
      get => textBox.Text; 
      set => textBox.Text = SpaceOut > 0 ? Regex.Replace(value, $".{{{SpaceOut}}}", "$0 ").Trim() : value?.Trim();
    }
  }
}