using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Authenticator {
  public class SecretTextBox : TextBox {
    private string mText;
    private bool mSecretMode;
    private string mFontFamily;
    private float mFontSize;

    public bool SecretMode {
      get => mSecretMode;
      set {
        mSecretMode = value;
        Enabled = !value; // we disable so cannot select/copy 
        SetStyle(ControlStyles.UserPaint, value);
        Text = mText;
        //
        // when we disable secret mode we need to reset the font else sometimes it doesn't show corretly
        if (mFontFamily == null) {
          mFontFamily = Font.FontFamily.Name;
          mFontSize = Font.Size;
        }

        if (value == false) {
          Font = new Font(mFontFamily, mFontSize);
        }

        //
        Invalidate(); // force it to redraw
      }
    }

    public int SpaceOut { get; set; }

    public override string Text {
      get => (SecretMode ? mText : base.Text);
      set {
        mText = value;
        base.Text =
          (SecretMode ? (string.IsNullOrEmpty(value) == false ? new string('*', value.Length) : value) : value);
        Invalidate();
      }
    }

    protected override void OnPaint(PaintEventArgs e) {
      var g = e.Graphics;
      using (Brush brush = new SolidBrush(base.ForeColor)) {
        var sf = StringFormat.GenericTypographic;
        sf.Alignment = StringAlignment.Center;
        sf.LineAlignment = StringAlignment.Center;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

        var text = mText;

        // if we have spacing, we add a space in between each set of chars
        if (SpaceOut != 0 && mText != null) {
          var sb = new StringBuilder();
          for (var i = 0; i < mText.Length; i += SpaceOut) {
            if (i >= mText.Length) {
              break;
            }

            if (i + SpaceOut >= mText.Length) {
              sb.Append(mText.Substring(i));
            }
            else {
              sb.Append(mText.Substring(i, SpaceOut)).Append(" ");
            }
          }

          text = sb.ToString().Trim();
        }

        // draw the whole string
        g.DrawString(text ?? string.Empty, base.Font, brush, new RectangleF(0, 0, Width, Height), sf);
      }
    }
  }
}