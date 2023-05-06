using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Authenticator {
  public sealed partial class SnippingTool : Form {
    private Rectangle rcSelect;
    private Point pntStart;

    public Image Image { get; set; }

    public static Image Snip() {
      var rc = Screen.PrimaryScreen.Bounds;
      using (var bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppPArgb)) {
        using (var gr = Graphics.FromImage(bmp))
          gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
        using (var snipper = new SnippingTool(bmp)) {
          if (snipper.ShowDialog() == DialogResult.OK) {
            return snipper.Image;
          }
        }

        return null;
      }
    }

    public static Image SnipMultiple() {
      var screenLeft = SystemInformation.VirtualScreen.Left;
      var screenTop = SystemInformation.VirtualScreen.Top;
      var screenWidth = SystemInformation.VirtualScreen.Width;
      var screenHeight = SystemInformation.VirtualScreen.Height;

      using (var bmp = new Bitmap(screenWidth, screenHeight, PixelFormat.Format32bppPArgb)) {
        using (var gr = Graphics.FromImage(bmp))
          gr.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
        using (var snipper = new SnippingTool(bmp)) {
//                    snipper.Size = new Size(screenWidth, screenHeight);
//                    snipper.Location = new Point(screenLeft, screenTop);
//                    snipper.Left = screenLeft;
//                    snipper.Top = screenTop;
//                    snipper.Width = screenWidth;
//                    snipper.Height = screenHeight;
          snipper.Size = SystemInformation.VirtualScreen.Size;
          snipper.Location = SystemInformation.VirtualScreen.Location;
          snipper.WindowState = FormWindowState.Normal;
          if (snipper.ShowDialog() == DialogResult.OK) {
            return snipper.Image;
          }
        }

        return null;
      }
    }

    private SnippingTool(Image screenShot) {
      InitializeComponent();
      BackgroundImage = screenShot;
      ShowInTaskbar = false;
      FormBorderStyle = FormBorderStyle.None;
      WindowState = FormWindowState.Maximized;
      DoubleBuffered = true;
      Cursor = Cursors.Cross;
      TopMost = true;
    }

    protected override void OnMouseDown(MouseEventArgs e) {
      if (e.Button != MouseButtons.Left) return;
      pntStart = e.Location;
      rcSelect = new Rectangle(e.Location, new Size(0, 0));
      Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e) {
      if (e.Button != MouseButtons.Left) return;
      var x1 = Math.Min(e.X, pntStart.X);
      var y1 = Math.Min(e.Y, pntStart.Y);
      var x2 = Math.Max(e.X, pntStart.X);
      var y2 = Math.Max(e.Y, pntStart.Y);
      rcSelect = new Rectangle(x1, y1, x2 - x1, y2 - y1);
      Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e) {
      if (rcSelect.Width <= 0 || rcSelect.Height <= 0) return;
      Image = new Bitmap(rcSelect.Width, rcSelect.Height);
      using (var gr = Graphics.FromImage(Image)) {
        gr.DrawImage(BackgroundImage, new Rectangle(0, 0, Image.Width, Image.Height),
          rcSelect, GraphicsUnit.Pixel);
      }

      DialogResult = DialogResult.OK;
    }

    protected override void OnPaint(PaintEventArgs e) {
      using (Brush br = new SolidBrush(Color.FromArgb(120, Color.Black))) {
        var x1 = rcSelect.X;
        var x2 = rcSelect.X + rcSelect.Width;
        var y1 = rcSelect.Y;
        var y2 = rcSelect.Y + rcSelect.Height;
        e.Graphics.FillRectangle(br, new Rectangle(0, 0, x1, Height));
        e.Graphics.FillRectangle(br, new Rectangle(x2, 0, Width - x2, Height));
        e.Graphics.FillRectangle(br, new Rectangle(x1, 0, x2 - x1, y1));
        e.Graphics.FillRectangle(br, new Rectangle(x1, y2, x2 - x1, Height - y2));
      }

      using (var pen = new Pen(Color.OrangeRed, 1)) {
        e.Graphics.DrawRectangle(pen, rcSelect);
      }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
      if (keyData == Keys.Escape) DialogResult = DialogResult.Cancel;
      return base.ProcessCmdKey(ref msg, keyData);
    }
  }
}