using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Authenticator {
  public sealed class SnippingTool : Form {

    #region Windows Form Designer code

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.SuspendLayout();
      //this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = "SnippingTool";
      this.ResumeLayout(false);
    }

    #endregion

    private Rectangle rcSelect;
    private Point pntStart;
    //private Brush textBackBrush = new SolidBrush(Color.White);
    //private Brush textForeBrush = new SolidBrush(Color.Red);
    private Brush areaBrush = new SolidBrush(Color.FromArgb(120, Color.Black));
    private Pen areaPen = new Pen(Color.OrangeRed, 1);

    public Image Image { get; set; }

    //public static Image GetPrimaryScreenImage() {
    //  var rc = Screen.PrimaryScreen.Bounds;
    //  var bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppPArgb);
    //  using (var gr = Graphics.FromImage(bmp))
    //    gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
    //  return bmp;
    //}

    //public static Image GetMultipleScreenImage() {
    //  var screenLeft = SystemInformation.VirtualScreen.Left;
    //  var screenTop = SystemInformation.VirtualScreen.Top;
    //  var screenWidth = SystemInformation.VirtualScreen.Width;
    //  var screenHeight = SystemInformation.VirtualScreen.Height;
    //  var bmp = new Bitmap(screenWidth, screenHeight, PixelFormat.Format32bppPArgb);
    //  using (var gr = Graphics.FromImage(bmp))
    //    gr.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
    //  return bmp;
    //}

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
      var x1 = rcSelect.X;
      var x2 = rcSelect.X + rcSelect.Width;
      var y1 = rcSelect.Y;
      var y2 = rcSelect.Y + rcSelect.Height;
      e.Graphics.FillRectangle(areaBrush, new Rectangle(0, 0, x1, Height));
      e.Graphics.FillRectangle(areaBrush, new Rectangle(x2, 0, Width - x2, Height));
      e.Graphics.FillRectangle(areaBrush, new Rectangle(x1, 0, x2 - x1, y1));
      e.Graphics.FillRectangle(areaBrush, new Rectangle(x1, y2, x2 - x1, Height - y2));
      e.Graphics.DrawRectangle(areaPen, rcSelect);

      //if (rcSelect.Width == 0 && rcSelect.Height == 0) {
      //  var size = e.Graphics.MeasureString("Select area", Font);
      //  e.Graphics.FillRectangle(textBackBrush, new Rectangle(Cursor.Position.X, Cursor.Position.Y + 10, (int)size.Width, (int)size.Height));
      //  e.Graphics.DrawString("Select area", Font, textForeBrush, Cursor.Position.X, Cursor.Position.Y + 10);
      //}
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
      if (keyData == Keys.Escape) DialogResult = DialogResult.Cancel;
      return base.ProcessCmdKey(ref msg, keyData);
    }
  }
}