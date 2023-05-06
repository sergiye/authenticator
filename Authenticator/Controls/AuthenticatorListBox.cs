using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Authenticator {
  public class AuthenticatorListBox : ListBox {

    public class ListItem {

      public ListItem(AuthAuthenticator auth, int index) {
        Authenticator = auth;
        LastUpdate = DateTime.MinValue;
        Index = index;
        DisplayUntil = DateTime.MinValue;
      }

      public int Index { get; set; }
      public AuthAuthenticator Authenticator { get; set; }
      public DateTime LastUpdate { get; set; }
      public DateTime DisplayUntil { get; set; }
      public string LastCode { get; set; }
      public bool Dragging { get; set; }
      public int UnprotectCount { get; set; }
      public int AutoWidth { get; set; }
    }

    private const int MARGIN_LEFT = 4;
    private const int MARGIN_TOP = 8;
    private const int MARGIN_RIGHT = 8;
    private const int ICON_WIDTH = 48;
    private const int ICON_HEIGHT = 48;
    private const int ICON_MARGIN_RIGHT = 12;

    private const int LABEL_MARGIN_BOTTOM = 2;

    private const int FONT_SIZE = 12;

    private const int PIE_WIDTH = 46;
    private const int PIE_HEIGHT = 46;
    private const int PIE_MARGIN = 2;
    private const int PIE_STARTANGLE = 270;
    private const int PIE_SWEEPANGLE = 360;

    public event EventHandler<ListItem> ItemRemoved;

    public event EventHandler Reordered;

    [Category("Action")]
    public event ScrollEventHandler Scrolled;

    public new event EventHandler<AuthAuthenticator> DoubleClick;

    private TextBox renameTextbox;
    private ListItem currentItem;
    private Point mouseDownLocation = Point.Empty;
    // private Point mouseMoveLocation = Point.Empty;
    private Bitmap draggedBitmap;
    private ListItem draggedItem;
    private Rectangle draggedBitmapRect;
    private int draggedBitmapOffsetY;
    private int? lastDragTopIndex;
    private DateTime lastDragScroll;

    public AuthenticatorListBox() {
      // set owner draw stlying
      SetStyle(
        ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
        ControlStyles.UserPaint, true);
      DrawMode = DrawMode.OwnerDrawFixed;
      ReadOnly = true;
      AllowDrop = true;
      DoubleBuffered = true;

      // hook the scroll event
      Scrolled += AuthenticatorListBox_Scrolled;

      // hook the context menu
      ContextMenuStrip = new ContextMenuStrip();
      ContextMenuStrip.Opening += ContextMenuStrip_Opening;

      // preload the content menu
      LoadContextMenuStrip();
    }

    protected override void OnMouseWheel(MouseEventArgs e) {
      base.OnMouseWheel(e);

      var y = e.Delta * ItemHeight + MARGIN_TOP;
      var sargs = new ScrollEventArgs(ScrollEventType.ThumbPosition, y, ScrollOrientation.VerticalScroll);
      Scrolled?.Invoke(this, sargs);
    }

    protected override void OnMouseDoubleClick(MouseEventArgs e) {
      var item = CurrentItem;
      if (item != null) {
        DoubleClick?.Invoke(this, item.Authenticator);
      }
    }

    #region Control Events

    protected override void OnResize(EventArgs e) {
      base.OnResize(e);
      SetRenameTextboxLocation();
    }

    void AuthenticatorListBox_Scrolled(object sender, ScrollEventArgs e) {
      if (e.Type == ScrollEventType.EndScroll || e.Type == ScrollEventType.ThumbPosition) {
        SetRenameTextboxLocation();
      }
    }

    void ContextMenu_Click(object sender, EventArgs e) {
      ProcessMenu((ToolStripItem)sender);
    }

    void ContextMenuStrip_Opening(object sender, CancelEventArgs e) {
      SetContextMenuItems();
    }

    public void Tick(object sender, EventArgs e) {
      for (var index = 0; index < Items.Count; index++) {
        // get the item
        var item = Items[index] as ListItem;
        var auth = item.Authenticator;

        var y = ItemHeight * index - TopIndex * ItemHeight;
        if (auth.AutoRefresh) {
          // for autorefresh we repaint the pie or the code too
          //int tillUpdate = (int)((auth.AuthenticatorData.ServerTime % ((long)auth.AuthenticatorData.Period * 1000L)) / 1000L);
          var tillUpdate =
            (int)Math.Round(
              (decimal)(auth.AuthenticatorData.ServerTime % ((long)auth.AuthenticatorData.Period * 1000L) / 1000L) *
              (360M / (decimal)auth.AuthenticatorData.Period));
          if (item.LastUpdate == DateTime.MinValue || tillUpdate == 0) {
            Invalidate(new Rectangle(0, y, Width, ItemHeight), false);
            item.LastUpdate = DateTime.Now;
          }
          else {
            Invalidate(new Rectangle(0, y, Width, ItemHeight), false);
            item.LastUpdate = DateTime.Now;
          }
        }
        else {
          // check if we need to redraw
          if (item.DisplayUntil != DateTime.MinValue) {
            // clear the item
            if (item.DisplayUntil <= DateTime.Now) {
              item.DisplayUntil = DateTime.MinValue;
              item.LastUpdate = DateTime.MinValue;
              item.LastCode = null;

              if (item.Authenticator.AuthenticatorData != null && item.Authenticator.AuthenticatorData.PasswordType ==
                  Authenticator.PasswordTypes.Explicit) {
                ProtectAuthenticator(item);
              }

              SetCursor(PointToClient(MousePosition));
            }

            Invalidate(new Rectangle(0, y, Width, ItemHeight), false);
          }
        }
      }
    }

    protected override void OnMouseDown(MouseEventArgs e) {
      // set the current item based on position
      SetCurrentItem(e.Location);
      mouseDownLocation = e.Location;

      base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e) {
      base.OnMouseUp(e);

      if ((e.Button & MouseButtons.Left) != 0) {
        // if this was in a refresh icon, we do a refresh
        var index = IndexFromPoint(e.Location);
        if (index >= 0 && index < Items.Count) {
          var item = Items[index] as ListItem;
          var y = ItemHeight * index - ItemHeight * TopIndex;
          var hasvscroll = Height < Items.Count * ItemHeight;
          if (item.Authenticator.AutoRefresh == false && item.DisplayUntil < DateTime.Now
                                                      && new Rectangle(
                                                        Width - (ICON_WIDTH + MARGIN_RIGHT) -
                                                        (hasvscroll ? SystemInformation.VerticalScrollBarWidth : 0),
                                                        y + MARGIN_TOP, ICON_WIDTH, ICON_HEIGHT).Contains(e.Location)) {
            if (UnprotectAuthenticator(item) == DialogResult.Cancel) {
              return;
            }

            item.LastCode = item.Authenticator.CurrentCode;
            item.LastUpdate = DateTime.Now;
            item.DisplayUntil = DateTime.Now.AddSeconds(10);

            if (item.Authenticator.CopyOnCode) {
              // copy to clipboard
              item.Authenticator.CopyCodeToClipboard(Parent as Form);
            }

            RefreshCurrentItem();
          }
        }
      }

      // dispose and reset the dragging
      mouseDownLocation = Point.Empty;
      if (draggedBitmap != null) {
        draggedBitmap.Dispose();
        draggedBitmap = null;
        Invalidate(draggedBitmapRect);
      }

      draggedItem = null;
    }

    protected override void OnMouseMove(MouseEventArgs e) {
      // mouseMoveLocation = e.Location;

      // if we are moving with LeftMouse down and moved more than 2 pixles then we are dragging
      if (e.Button == MouseButtons.Left && mouseDownLocation != Point.Empty && Items.Count > 1) {
        var dx = Math.Abs(mouseDownLocation.X - e.Location.X);
        var dy = Math.Abs(mouseDownLocation.Y - e.Location.Y);
        if (dx > 2 || dy > 2) {
          draggedItem = CurrentItem;

          // get a snapshot of the current item for the drag
          var hasvscroll = Height < Items.Count * ItemHeight;
          draggedBitmap = new Bitmap(Width - (hasvscroll ? SystemInformation.VerticalScrollBarWidth : 0), ItemHeight);
          draggedBitmapRect = Rectangle.Empty;
          using (var g = Graphics.FromImage(draggedBitmap)) {
            var y = ItemHeight * CurrentItem.Index - ItemHeight * TopIndex;

            var screen = Parent.PointToScreen(new Point(Location.X, Location.Y + y));
            g.CopyFromScreen(screen.X, screen.Y, 0, 0,
              new Size(Width - (hasvscroll ? SystemInformation.VerticalScrollBarWidth : 0), ItemHeight),
              CopyPixelOperation.SourceCopy);

            // save offset in Y from top of item
            draggedBitmapOffsetY = e.Y - y;
          }

          // make the bitmap darker
          Lighten(draggedBitmap, -10);

          draggedItem.Dragging = true;
          RefreshItem(draggedItem);

          // moved enough so start drag
          DoDragDrop(draggedItem, DragDropEffects.Move);
        }
      }
      else if (e.Button == MouseButtons.None) {
        SetCursor(e.Location);
      }

      base.OnMouseMove(e);
    }

    protected override void OnDragOver(DragEventArgs e) {
      var screen = Parent.RectangleToScreen(new Rectangle(Location.X, Location.Y, Width, Height));
      var mousePoint = new Point(e.X, e.Y);
      if (screen.Contains(mousePoint) == false) {
        e.Effect = DragDropEffects.None;
      }
      else if (draggedBitmap != null) {
        e.Effect = DragDropEffects.Move;
        using (var g = CreateGraphics()) {
          var mouseClientPoint = PointToClient(mousePoint);
          var x = 0;
          var y = Math.Max(mouseClientPoint.Y - draggedBitmapOffsetY, 0);
          var rect = new Rectangle(x, y, draggedBitmap.Width, draggedBitmap.Height);
          g.DrawImageUnscaled(draggedBitmap, rect);

          if (draggedBitmapRect != Rectangle.Empty) {
            // invalidate the extent between the old rect and this one
            var region = new Region(rect);
            region.Union(draggedBitmapRect);
            region.Exclude(rect);
            Invalidate(region);
          }

          draggedBitmapRect = rect;
        }
      }
    }

    protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e) {
      var screen = Parent.RectangleToScreen(new Rectangle(Location.X, Location.Y, Width, Height));
      var mousePoint = Cursor.Position;

      // if ESC is pressed, always stop
      if (e.EscapePressed || ((e.KeyState & 1) == 0 && screen.Contains(mousePoint) == false)) {
        e.Action = DragAction.Cancel;

        if (draggedItem != null) {
          draggedItem.Dragging = false;
          RefreshItem(draggedItem);
          draggedItem = null;
        }

        if (draggedBitmap != null) {
          draggedBitmap.Dispose();
          draggedBitmap = null;
          Invalidate(draggedBitmapRect);
        }

        mouseDownLocation = Point.Empty;
      }
      else {
        var now = DateTime.Now;

        // if we are at the top or bottom, scroll every 150ms
        if (mousePoint.Y >= screen.Bottom) {
          var visible = ClientSize.Height / ItemHeight;
          var maxtopindex = Math.Max(Items.Count - visible + 1, 0);
          if (TopIndex < maxtopindex && now.Subtract(lastDragScroll).TotalMilliseconds >= 150) {
            TopIndex++;
            lastDragScroll = now;
            Refresh();
          }
        }
        else if (mousePoint.Y <= screen.Top) {
          // var visible = ClientSize.Height / ItemHeight;
          if (TopIndex > 0 && now.Subtract(lastDragScroll).TotalMilliseconds >= 150) {
            TopIndex--;
            lastDragScroll = now;
            Refresh();
          }
        }

        lastDragTopIndex = TopIndex;

        base.OnQueryContinueDrag(e);
      }
    }

    protected override void OnDragDrop(DragEventArgs e) {
      var item = e.Data.GetData(typeof(ListItem)) as ListItem;
      if (item != null) {
        // stop paiting as we reorder to reduce flicker
        WinApi.SendMessage(Handle, WinApi.WM_SETREDRAW, 0, IntPtr.Zero);
        try {
          // get the new index
          var point = PointToClient(new Point(e.X, e.Y));
          var index = IndexFromPoint(point);
          if (index < 0) {
            index = Items.Count - 1;
          }

          // move the item
          Items.Remove(item);
          Items.Insert(index, item);

          // set the correct indexes of our items
          for (var i = 0; i < Items.Count; i++) {
            ((ListItem) Items[i]).Index = i;
          }

          // fire the reordered event
          Reordered?.Invoke(this, EventArgs.Empty);

          // clear state
          item.Dragging = false;
          draggedItem = null;
          if (draggedBitmap != null) {
            draggedBitmap.Dispose();
            draggedBitmap = null;
          }

          if (lastDragTopIndex != null) {
            TopIndex = lastDragTopIndex.Value;
          }
        }
        finally {
          // resume painting
          WinApi.SendMessage(Handle, WinApi.WM_SETREDRAW, 1, IntPtr.Zero);
        }

        Refresh();
      }
      else {
        base.OnDragDrop(e);
      }
    }

    protected override void WndProc(ref Message msg) {
      if (msg.Msg == WinApi.WM_VSCROLL) {
        if (Scrolled != null) {
          var si = new WinApi.ScrollInfoStruct();
          si.FMask = WinApi.SIF_ALL;
          si.CbSize = Marshal.SizeOf(si);
          WinApi.GetScrollInfo(msg.HWnd, 0, ref si);

          if (msg.WParam.ToInt32() == WinApi.SB_ENDSCROLL) {
            var sargs = new ScrollEventArgs(ScrollEventType.EndScroll, si.NPos);
            Scrolled(this, sargs);
          }
          else if (msg.WParam.ToInt32() == WinApi.SB_THUMBTRACK) {
            var sargs = new ScrollEventArgs(ScrollEventType.ThumbTrack, si.NPos);
            Scrolled(this, sargs);
          }
        }
      }

      base.WndProc(ref msg);
    }

    #endregion

    #region Item renaming

    private void SetRenameTextboxLocation() {
      if (renameTextbox != null && renameTextbox.Visible) {
        var item = renameTextbox.Tag as ListItem;
        if (item != null) {
          var y = ItemHeight * item.Index - TopIndex * ItemHeight + MARGIN_TOP;
          if (RenameTextbox.Location.Y != y) {
            RenameTextbox.Location = new Point(RenameTextbox.Location.X, y);
          }

          Refresh();
        }
      }
    }

    public TextBox RenameTextbox {
      get {
        var hasvscroll = Height < Items.Count * ItemHeight;
        var labelMaxWidth = GetMaxAvailableLabelWidth(Width - Margin.Horizontal - DefaultPadding.Horizontal -
                                                      (hasvscroll ? SystemInformation.VerticalScrollBarWidth : 0));
        if (renameTextbox == null) {
          renameTextbox = new TextBox();
          renameTextbox.Name = "renameTextBox";
          renameTextbox.AllowDrop = true;
          renameTextbox.CausesValidation = false;
          renameTextbox.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
          renameTextbox.Location = new Point(0, 0);
          renameTextbox.Multiline = false;
          renameTextbox.Name = "secretCodeField";
          renameTextbox.Size = new Size(labelMaxWidth, 22);
          renameTextbox.TabIndex = 0;
          renameTextbox.Visible = false;
          renameTextbox.Leave += RenameTextbox_Leave;
          renameTextbox.KeyPress += _renameTextbox_KeyPress;

          Controls.Add(renameTextbox);
        }
        else {
          renameTextbox.Width = labelMaxWidth;
        }

        return renameTextbox;
      }
    }

    public bool IsRenaming {
      get { return RenameTextbox.Visible; }
    }

    public void EndRenaming(bool save = true) {
      if (save == false) {
        RenameTextbox.Tag = null;
      }

      RenameTextbox.Visible = false;
    }

    void _renameTextbox_KeyPress(object sender, KeyPressEventArgs e) {
      if (e.KeyChar == 27) {
        RenameTextbox.Tag = null;
        RenameTextbox.Visible = false;
        e.Handled = true;
      }
      else if (e.KeyChar == 13 || e.KeyChar == 9) {
        RenameTextbox.Visible = false;
        e.Handled = true;
      }
    }

    void RenameTextbox_Leave(object sender, EventArgs e) {
      RenameTextbox.Visible = false;
      var item = RenameTextbox.Tag as ListItem;
      if (item != null) {
        var newname = RenameTextbox.Text.Trim();
        if (newname.Length != 0) {
          // force the autowidth to be recaculated when we set the name
          item.AutoWidth = 0;
          item.Authenticator.Name = newname;
          RefreshItem(item.Index);
        }
      }
    }

    #endregion

    #region Public Properties

    public bool ReadOnly { get; set; }

    public ListItem CurrentItem {
      get {
        if (currentItem == null && Items.Count != 0) {
          currentItem = (ListItem)Items[0];
        }

        return currentItem;
      }
      set { currentItem = value; }
    }

    #endregion

    #region Private methods

    private void SetCurrentItem(Point mouseLocation) {
      var index = IndexFromPoint(mouseLocation);
      if (index < 0) {
        index = 0;
      }
      else if (index >= Items.Count) {
        index = Items.Count - 1;
      }

      if (index >= Items.Count) {
        CurrentItem = null;
      }
      else {
        CurrentItem = Items[index] as ListItem;
      }
    }

    private void SetCursor(Point mouseLocation, Cursor force = null) {
      // set cursor if we are over a refresh icon
      var cursor = Cursor.Current;
      if (force == null) {
        var index = IndexFromPoint(mouseLocation);
        if (index >= 0 && index < Items.Count) {
          var item = Items[index] as ListItem;
          var y = ItemHeight * index - TopIndex * ItemHeight;
          var hasvscroll = Height < Items.Count * ItemHeight;
          if (item.Authenticator.AutoRefresh == false && item.DisplayUntil < DateTime.Now
                                                      && new Rectangle(
                                                          Width - (ICON_WIDTH + MARGIN_RIGHT) -
                                                          (hasvscroll ? SystemInformation.VerticalScrollBarWidth : 0),
                                                          y + MARGIN_TOP, ICON_WIDTH, ICON_HEIGHT)
                                                        .Contains(mouseLocation)) {
            cursor = Cursors.Hand;
          }
        }
      }
      else {
        cursor = force;
      }

      if (Cursor.Current != cursor) {
        Cursor.Current = cursor;
      }
    }

    private DialogResult UnprotectAuthenticator(ListItem item, Screen screen = null) {
      // keep a count so we can have multiples
      if (item.UnprotectCount > 0) {
        item.UnprotectCount++;
        return DialogResult.OK;
      }

      // if there is no protection return None
      var auth = item.Authenticator;
      if (auth.AuthenticatorData == null || auth.AuthenticatorData.RequiresPassword == false) {
        return DialogResult.None;
      }

      // request the password
      var getPassForm = new UnprotectPasswordForm {
        Authenticator = auth
      };
      if (screen != null) {
        // center on the current windows screen (in case of multiple monitors)
        getPassForm.StartPosition = FormStartPosition.Manual;
        var left = screen.Bounds.Width / 2 - getPassForm.Width / 2 + screen.Bounds.Left;
        var top = screen.Bounds.Height / 2 - getPassForm.Height / 2 + screen.Bounds.Top;
        getPassForm.Location = new Point(left, top);
      }

      var result = getPassForm.ShowDialog();
      if (result == DialogResult.OK) {
        item.UnprotectCount++;
      }

      return result;
    }

    private void ProtectAuthenticator(ListItem item) {
      // if already protected just decrement counter
      item.UnprotectCount--;
      if (item.UnprotectCount > 0) {
        return;
      }

      // reprotect the authenticator
      var auth = item.Authenticator;
      if (auth.AuthenticatorData == null) {
        return;
      }

      auth.AuthenticatorData.Protect();
      item.UnprotectCount = 0;
    }

    private void LoadContextMenuStrip() {
      ContextMenuStrip.Items.Clear();

      var label = new ToolStripLabel();
      label.Name = "contextMenuItemName";
      label.ForeColor = SystemColors.HotTrack;
      ContextMenuStrip.Items.Add(label);
      ContextMenuStrip.Items.Add(new ToolStripSeparator());
      //
      // EventHandler onclick = ContextMenu_Click;
      //
      //
      var menuitem = new ToolStripMenuItem("Set Password...");
      menuitem.Name = "setPasswordMenuItem";
      menuitem.Click += ContextMenu_Click;
      ContextMenuStrip.Items.Add(menuitem);
      ContextMenuStrip.Items.Add(new ToolStripSeparator());
      //
      menuitem = new ToolStripMenuItem("Show Code");
      menuitem.Name = "showCodeMenuItem";
      menuitem.Click += ContextMenu_Click;
      ContextMenuStrip.Items.Add(menuitem);
      //
      menuitem = new ToolStripMenuItem("Copy Code");
      menuitem.Name = "copyCodeMenuItem";
      menuitem.Click += ContextMenu_Click;
      ContextMenuStrip.Items.Add(menuitem);
      //
      ContextMenuStrip.Items.Add(new ToolStripSeparator());
      //
      menuitem = new ToolStripMenuItem("Show Serial && Restore Code...");
      menuitem.Name = "showRestoreCodeMenuItem";
      menuitem.Click += ContextMenu_Click;
      ContextMenuStrip.Items.Add(menuitem);
      //
      menuitem = new ToolStripMenuItem("Show Secret Key...");
      menuitem.Name = "showGoogleSecretMenuItem";
      menuitem.Click += ContextMenu_Click;
      ContextMenuStrip.Items.Add(menuitem);
      //
      menuitem = new ToolStripMenuItem("Show Serial Key and Device ID...");
      menuitem.Name = "showTrionSecretMenuItem";
      menuitem.Click += ContextMenu_Click;
      ContextMenuStrip.Items.Add(menuitem);
      //
      ContextMenuStrip.Items.Add(new ToolStripSeparator());
      //
      menuitem = new ToolStripMenuItem("Delete");
      menuitem.Name = "deleteMenuItem";
      menuitem.Click += ContextMenu_Click;
      ContextMenuStrip.Items.Add(menuitem);
      //
      menuitem = new ToolStripMenuItem("Rename");
      menuitem.Name = "renameMenuItem";
      menuitem.Click += ContextMenu_Click;
      ContextMenuStrip.Items.Add(menuitem);
      //
      ContextMenuStrip.Items.Add(new ToolStripSeparator());
      //
      menuitem = new ToolStripMenuItem("Auto Refresh");
      menuitem.Name = "autoRefreshMenuItem";
      menuitem.Click += ContextMenu_Click;
      ContextMenuStrip.Items.Add(menuitem);
      //
      menuitem = new ToolStripMenuItem("Copy on New Code");
      menuitem.Name = "copyOnCodeMenuItem";
      menuitem.Click += ContextMenu_Click;
      ContextMenuStrip.Items.Add(menuitem);
      //
      menuitem = new ToolStripMenuItem("Icon");
      menuitem.Name = "iconMenuItem";
      var subitem = new ToolStripMenuItem();
      subitem.Text = "Auto";
      subitem.Name = "iconMenuItem_default";
      subitem.Tag = string.Empty;
      subitem.Click += ContextMenu_Click;
      menuitem.DropDownItems.Add(subitem);
      menuitem.DropDownItems.Add("-");
      ContextMenuStrip.Items.Add(menuitem);
      var iconindex = 1;
      var parentItem = menuitem;
      foreach (var entry in AuthMain.AuthenticatorIcons) {
        var icon = entry.Key;
        var iconfile = entry.Value;
        if (iconfile.Length == 0) {
          parentItem.DropDownItems.Add(new ToolStripSeparator());
        }
        else if (icon.StartsWith("+")) {
          if (parentItem.Tag is ToolStripMenuItem) {
            parentItem = parentItem.Tag as ToolStripMenuItem;
          }

          subitem = new ToolStripMenuItem();
          subitem.Text = icon.Substring(1);
          //subitem.Name = "iconMenuItem_" + iconindex++;
          subitem.Tag = parentItem;
          subitem.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Authenticator.Resources." + iconfile));
          subitem.ImageAlign = ContentAlignment.MiddleLeft;
          subitem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
          //subitem.Click += ContextMenu_Click;
          parentItem.DropDownItems.Add(subitem);
          parentItem = subitem;
        }
        else {
          subitem = new ToolStripMenuItem();
          subitem.Text = icon;
          subitem.Name = "iconMenuItem_" + iconindex++;
          subitem.Tag = iconfile;
          subitem.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("Authenticator.Resources." + iconfile));
          subitem.ImageAlign = ContentAlignment.MiddleLeft;
          subitem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
          subitem.Click += ContextMenu_Click;
          parentItem.DropDownItems.Add(subitem);
        }
      }

      menuitem.DropDownItems.Add("-");
      subitem = new ToolStripMenuItem();
      subitem.Text = "Other...";
      subitem.Name = "iconMenuItem_0";
      subitem.Tag = "OTHER";
      subitem.Click += ContextMenu_Click;
      menuitem.DropDownItems.Add(subitem);
      ContextMenuStrip.Items.Add(menuitem);
      //
      var sepitem = new ToolStripSeparator();
      sepitem.Name = "syncMenuSep";
      ContextMenuStrip.Items.Add(sepitem);
      //
      menuitem = new ToolStripMenuItem("Sync Time");
      menuitem.Name = "syncMenuItem";
      menuitem.Click += ContextMenu_Click;
      ContextMenuStrip.Items.Add(menuitem);
    }

    private void SetContextMenuItems() {
      var menu = ContextMenuStrip;
      var item = CurrentItem;
      AuthAuthenticator auth;
      if (item == null || (auth = item.Authenticator) == null || auth.AuthenticatorData == null)
        return;

      var labelItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "contextMenuItemName") as ToolStripLabel;
      if (labelItem != null) labelItem.Text = item.Authenticator.Name;

      var menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "setPasswordMenuItem") as ToolStripMenuItem;
      if (menuItem != null)
        menuItem.Text = item.Authenticator.AuthenticatorData.PasswordType == Authenticator.PasswordTypes.Explicit
          ? "Change or Remove Password..."
          : "Set Password...";

      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "showCodeMenuItem") as ToolStripMenuItem;
      if (menuItem != null) menuItem.Visible = !auth.AutoRefresh;
      //
      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "showRestoreCodeMenuItem") as ToolStripMenuItem;
      if (menuItem != null) menuItem.Visible = auth.AuthenticatorData is BattleNetAuthenticator;
      //
      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "showGoogleSecretMenuItem") as ToolStripMenuItem;
      if (menuItem != null)
        menuItem.Visible = auth.AuthenticatorData is GoogleAuthenticator || auth.AuthenticatorData is HotpAuthenticator;
      //
      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "showTrionSecretMenuItem") as ToolStripMenuItem;
      if (menuItem != null) menuItem.Visible = auth.AuthenticatorData is TrionAuthenticator;
      
      //
      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "autoRefreshMenuItem") as ToolStripMenuItem;
      if (menuItem != null) {
        menuItem.Visible = !(auth.AuthenticatorData is HotpAuthenticator);
        menuItem.CheckState = auth.AutoRefresh ? CheckState.Checked : CheckState.Unchecked;
        menuItem.Enabled = auth.AuthenticatorData.RequiresPassword == false &&
                           auth.AuthenticatorData.PasswordType != Authenticator.PasswordTypes.Explicit;
      }

      //
      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "copyOnCodeMenuItem") as ToolStripMenuItem;
      if (menuItem != null) menuItem.CheckState = auth.CopyOnCode ? CheckState.Checked : CheckState.Unchecked;
      //
      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "iconMenuItem") as ToolStripMenuItem;
      if (menuItem.DropDownItems.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "iconMenuItem_default") is
          ToolStripMenuItem subItem) subItem.CheckState = CheckState.Checked;
      foreach (ToolStripItem iconItem in menuItem.DropDownItems) {
        if (iconItem is ToolStripMenuItem toolStripItem && toolStripItem.Tag is string tag) {
          if (string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(auth.Skin)) {
            toolStripItem.CheckState = CheckState.Checked;
          }
          else if (string.Compare(tag, auth.Skin) == 0) {
            toolStripItem.CheckState = CheckState.Checked;
          }
          else {
            toolStripItem.CheckState = CheckState.Unchecked;
          }
        }
      }

      //
      var sepItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "syncMenuSep");
      sepItem.Visible = !(auth.AuthenticatorData is HotpAuthenticator);

      menuItem = menu.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "syncMenuItem") as ToolStripMenuItem;
      menuItem.Visible = !(auth.AuthenticatorData is HotpAuthenticator);
    }

    private void ProcessMenu(ToolStripItem menuitem) {
      var item = CurrentItem;
      var auth = item.Authenticator;

      // check and perform each menu
      switch (menuitem.Name) {
        case "setPasswordMenuItem": {
            // check if the authentcated is still protected
            var wasprotected = UnprotectAuthenticator(item);
            if (wasprotected == DialogResult.Cancel) {
              return;
            }

            try {
              // show the new password form
              var form = new SetPasswordForm();
              if (form.ShowDialog(Parent as Form) != DialogResult.OK) {
                return;
              }

              // set the encrpytion
              var password = form.Password;
              if (string.IsNullOrEmpty(password) == false) {
                auth.AuthenticatorData.SetEncryption(Authenticator.PasswordTypes.Explicit, password);
                // can't have autorefresh on
                auth.AutoRefresh = false;

                item.UnprotectCount = 0;
                item.DisplayUntil = DateTime.MinValue;
                item.LastUpdate = DateTime.MinValue;
                item.LastCode = null;
              }
              else {
                auth.AuthenticatorData.SetEncryption(Authenticator.PasswordTypes.None);
              }

              // make sure authenticator is saved
              auth.MarkChanged();
              RefreshCurrentItem();
            }
            finally {
              if (wasprotected == DialogResult.OK) {
                ProtectAuthenticator(item);
              }
            }

            break;
          }

        case "showCodeMenuItem": {
            // check if the authentcated is still protected
            if (UnprotectAuthenticator(item) == DialogResult.Cancel) {
              return;
            }

            // reduce unprotect count if already displayed
            if (item.DisplayUntil != DateTime.MinValue) {
              ProtectAuthenticator(item);
            }

            item.LastCode = auth.CurrentCode;
            item.LastUpdate = DateTime.Now;
            item.DisplayUntil = DateTime.Now.AddSeconds(10);
            RefreshCurrentItem();
            break;
          }

        case "copyCodeMenuItem": {
            // check if the authentcated is still protected
            var wasprotected = UnprotectAuthenticator(item);
            if (wasprotected == DialogResult.Cancel) {
              return;
            }

            try {
              auth.CopyCodeToClipboard(Parent as Form, item.LastCode, true);
            }
            finally {
              if (wasprotected == DialogResult.OK) {
                ProtectAuthenticator(item);
              }
            }

            break;
          }

        case "autoRefreshMenuItem": {
            auth.AutoRefresh = !auth.AutoRefresh;
            item.LastUpdate = DateTime.Now;
            item.DisplayUntil = DateTime.MinValue;
            RefreshCurrentItem();
            break;
          }

        case "copyOnCodeMenuItem": {
            auth.CopyOnCode = !auth.CopyOnCode;
            break;
          }

        case "showRestoreCodeMenuItem": {
            // check if the authentcated is still protected
            var wasprotected = UnprotectAuthenticator(item);
            if (wasprotected == DialogResult.Cancel) {
              return;
            }

            try {
              if (wasprotected != DialogResult.OK) {
                // confirm current main password
                var mainform = Parent as MainForm;
                if ((mainform.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0) {
                  var invalidPassword = false;
                  while (true) {
                    var checkform = new GetPasswordForm();
                    checkform.InvalidPassword = invalidPassword;
                    var result = checkform.ShowDialog(this);
                    if (result == DialogResult.Cancel) {
                      return;
                    }

                    if (mainform.Config.IsPassword(checkform.Password)) {
                      break;
                    }

                    invalidPassword = true;
                  }
                }
              }

              // show the serial and restore code for Battle.net authenticator				
              var form = new ShowRestoreCodeForm();
              form.CurrentAuthenticator = auth;
              form.ShowDialog(Parent as Form);
            }
            finally {
              if (wasprotected == DialogResult.OK) {
                ProtectAuthenticator(item);
              }
            }

            break;
          }

        case "showGoogleSecretMenuItem": {
            // check if the authentcated is still protected
            var wasprotected = UnprotectAuthenticator(item);
            if (wasprotected == DialogResult.Cancel) {
              return;
            }

            try {
              if (wasprotected != DialogResult.OK) {
                // confirm current main password
                var mainform = Parent as MainForm;
                if ((mainform.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0) {
                  var invalidPassword = false;
                  while (true) {
                    var checkform = new GetPasswordForm();
                    checkform.InvalidPassword = invalidPassword;
                    var result = checkform.ShowDialog(this);
                    if (result == DialogResult.Cancel) {
                      return;
                    }

                    if (mainform.Config.IsPassword(checkform.Password)) {
                      break;
                    }

                    invalidPassword = true;
                  }
                }
              }

              // show the secret key for Google authenticator				
              var form = new ShowSecretKeyForm();
              form.CurrentAuthenticator = auth;
              form.ShowDialog(Parent as Form);
            }
            finally {
              if (wasprotected == DialogResult.OK) {
                ProtectAuthenticator(item);
              }
            }

            break;
          }

        case "showTrionSecretMenuItem": {
            // check if the authenticator is still protected
            var wasprotected = UnprotectAuthenticator(item);
            if (wasprotected == DialogResult.Cancel) {
              return;
            }

            try {
              // show the secret key for Trion authenticator				
              var form = new ShowTrionSecretForm();
              form.CurrentAuthenticator = auth;
              form.ShowDialog(Parent as Form);
            }
            finally {
              if (wasprotected == DialogResult.OK) {
                ProtectAuthenticator(item);
              }
            }

            break;
          }

        case "deleteMenuItem": {
            if (MainForm.ConfirmDialog(Parent as Form, "Are you sure you want to delete this authenticator?\nThis will permanently remove it and you may no longer be able to access you account.", MessageBoxButtons.YesNo,
                  MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
              var index = item.Index;
              Items.Remove(item);
              ItemRemoved?.Invoke(this, item);
              if (index >= Items.Count) {
                index = Items.Count - 1;
              }

              CurrentItem = Items.Count != 0 ? Items[index] as ListItem : null;
              // reset the correct indexes of our items
              for (var i = 0; i < Items.Count; i++) {
                ((ListItem) Items[i]).Index = i;
              }
            }

            break;
          }

        case "renameMenuItem": {
            var y = ItemHeight * item.Index - TopIndex * ItemHeight + 8;
            RenameTextbox.Location = new Point(64, y);
            RenameTextbox.Text = auth.Name;
            RenameTextbox.Tag = item;
            RenameTextbox.Visible = true;
            RenameTextbox.Focus();
            break;
          }

        case "syncMenuItem": {
            // check if the authentcated is still protected
            var wasprotected = UnprotectAuthenticator(item);
            if (wasprotected == DialogResult.Cancel) {
              return;
            }

            var cursor = Cursor.Current;
            try {
              Cursor.Current = Cursors.WaitCursor;
              auth.Sync();
              RefreshItem(item);
            }
            finally {
              Cursor.Current = cursor;
              if (wasprotected == DialogResult.OK) {
                ProtectAuthenticator(item);
              }
            }

            break;
          }

        default:
          if (menuitem.Name.StartsWith("iconMenuItem_")) {
            if (menuitem.Tag is string && string.Compare((string)menuitem.Tag, "OTHER") == 0) {
              do {
                // other..choose an image file
                var ofd = new OpenFileDialog();
                ofd.AddExtension = true;
                ofd.CheckFileExists = true;
                ofd.DefaultExt = "png";
                ofd.InitialDirectory = Directory.GetCurrentDirectory();
                ofd.FileName = string.Empty;
                ofd.Filter = "PNG Image Files (*.png)|*.png|GIF Image Files (*.gif)|*.gif|All Files (*.*)|*.*";
                ofd.RestoreDirectory = true;
                ofd.ShowReadOnly = false;
                ofd.Title = "Load Icon Image (png or gif @ 48x48)";
                var result = ofd.ShowDialog(this);
                if (result != DialogResult.OK) {
                  return;
                }

                try {
                  // get the image and create an icon if not already the right size
                  using (var iconimage = (Bitmap)Image.FromFile(ofd.FileName)) {
                    if (iconimage.Width != ICON_WIDTH || iconimage.Height != ICON_HEIGHT) {
                      using (var scaled = new Bitmap(ICON_WIDTH, ICON_HEIGHT)) {
                        using (var g = Graphics.FromImage(scaled)) {
                          g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                          g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                          g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                          g.DrawImage(iconimage, new Rectangle(0, 0, ICON_WIDTH, ICON_HEIGHT));
                        }

                        auth.Icon = scaled;
                      }
                    }
                    else {
                      auth.Icon = iconimage;
                    }

                    RefreshCurrentItem();
                  }
                }
                catch (Exception ex) {
                  if (MessageBox.Show(Parent as Form,
                        $"Error loading image file: {ex.Message}\nDo you want to try again?",
                        AuthMain.APPLICATION_NAME,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) ==
                      DialogResult.Yes) {
                    continue;
                  }
                }

                break;
              } while (true);
            }
            else {
              auth.Skin = ((string)menuitem.Tag).Length != 0 ? (string)menuitem.Tag : null;
              RefreshCurrentItem();
            }
          }

          break;
      }
    }

    public string GetItemCode(ListItem item, Screen screen = null) {
      var auth = item.Authenticator;

      // check if the authentcated is still protected
      var wasprotected = UnprotectAuthenticator(item, screen);
      if (wasprotected == DialogResult.Cancel) {
        return null;
      }

      try {
        return auth.CurrentCode;
      }
      finally {
        if (wasprotected == DialogResult.OK) {
          ProtectAuthenticator(item);
        }
      }
    }

    private void RefreshCurrentItem() {
      RefreshItem(CurrentItem);
    }

    private void RefreshItem(ListItem item) {
      // var hasvscroll = Height < Items.Count * ItemHeight;
      var y = ItemHeight * item.Index - ItemHeight * TopIndex;
      var rect = new Rectangle(0, y, Width, Height);
      Invalidate(rect, false);
    }

    private static void Lighten(Bitmap bmp, int amount) {
      Color c;
      int r, g, b;

      for (var y = 0; y < bmp.Height; y++) {
        for (var x = 0; x < bmp.Width; x++) {
          c = bmp.GetPixel(x, y);
          r = Math.Max(Math.Min(c.R + amount, 255), 0);
          g = Math.Max(Math.Min(c.G + amount, 255), 0);
          b = Math.Max(Math.Min(c.B + amount, 255), 0);
          bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
        }
      }
    }

    #endregion

    #region Owner Draw

    protected int GetMaxAvailableLabelWidth(int totalWidth) {
      return totalWidth - MARGIN_LEFT - ICON_WIDTH - ICON_MARGIN_RIGHT - PIE_WIDTH - MARGIN_RIGHT;
    }

    public int GetMaxItemWidth() {
      var items = Items.Cast<ListItem>().Where(i => i.AutoWidth == 0).ToArray();
      if (items.Any()) {
        using (var g = CreateGraphics()) {
          using (var font = new Font(Font.FontFamily, FONT_SIZE, FontStyle.Regular)) {
            foreach (var item in items) {
              var auth = item.Authenticator;
              var labelSize = g.MeasureString(auth.Name, font);
              item.AutoWidth = MARGIN_LEFT + ICON_WIDTH + ICON_MARGIN_RIGHT + (int)Math.Ceiling(labelSize.Width) + PIE_WIDTH + MARGIN_RIGHT;
            }
          }
        }
      }
      return Items.Cast<ListItem>().Max(i => i.AutoWidth);
    }

    protected override void DefWndProc(ref Message m) {
      if (ReadOnly == false || ((m.Msg <= 0x0200 || m.Msg >= 0x020E)
                                && (m.Msg <= 0x0100 || m.Msg >= 0x0109)
                                && m.Msg != 0x2111
                                && m.Msg != 0x87)) {
        base.DefWndProc(ref m);
      }
    }

    protected void OnDrawItem(DrawItemEventArgs e, Rectangle cliprect) {
      // no need to draw nothing
      if (Items.Count == 0 || e.Index < 0) {
        return;
      }

      var item = Items[e.Index] as ListItem;
      var auth = item.Authenticator;

      // if the item is being dragged, we draw a blank placeholder
      if (item.Dragging) {
        if (cliprect.IntersectsWith(e.Bounds)) {
          using (var brush = new SolidBrush(SystemColors.ControlLightLight)) {
            using (var pen = new Pen(SystemColors.Control)) {
              e.Graphics.DrawRectangle(pen, e.Bounds);
              e.Graphics.FillRectangle(brush, e.Bounds);
            }
          }
        }

        return;
      }

      // draw the requested item
      using (var brush = new SolidBrush(e.ForeColor)) {
        var showCode = auth.AutoRefresh || item.DisplayUntil > DateTime.Now;

        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        var rect = new Rectangle(e.Bounds.X + MARGIN_LEFT, e.Bounds.Y + MARGIN_TOP, ICON_WIDTH, ICON_HEIGHT);
        if (cliprect.IntersectsWith(rect)) {
          using (var icon = auth.Icon) {
            if (icon != null) {
              e.Graphics.DrawImage(icon, e.Bounds.X + MARGIN_LEFT, e.Bounds.Y + MARGIN_TOP, ICON_WIDTH, ICON_HEIGHT);
            }
          }
        }

        using (var font = new Font(e.Font.FontFamily, FONT_SIZE, FontStyle.Regular)) {
          var label = auth.Name;
          var labelsize = e.Graphics.MeasureString(label, font);
          var labelMaxWidth = GetMaxAvailableLabelWidth(e.Bounds.Width);
          if (labelsize.Width > labelMaxWidth) {
            var newlabel = new StringBuilder(label + "...");
            while ((labelsize = e.Graphics.MeasureString(newlabel.ToString(), font)).Width > labelMaxWidth) {
              newlabel.Remove(newlabel.Length - 4, 1);
            }

            label = newlabel.ToString();
          }

          rect = new Rectangle(e.Bounds.X + 64, e.Bounds.Y + MARGIN_TOP, (int)labelsize.Height, (int)labelsize.Width);
          if (cliprect.IntersectsWith(rect)) {
            e.Graphics.DrawString(label, font, brush,
              new RectangleF(e.Bounds.X + MARGIN_LEFT + ICON_WIDTH + ICON_MARGIN_RIGHT, e.Bounds.Y + MARGIN_TOP,
                labelMaxWidth, labelsize.Height));
          }

          string code;
          if (showCode) {
            try {
              // we we aren't autorefresh we just keep the same code up for the 10 seconds so it doesn't change even crossing the 30s boundary
              if (auth.AutoRefresh == false) {
                if (item.LastCode == null) {
                  code = auth.CurrentCode;
                }
                else {
                  code = item.LastCode;
                }
              }
              else {
                code = auth.CurrentCode;
                if (code != item.LastCode && auth.CopyOnCode) {
                  // code has changed - copy to clipboard
                  auth.CopyCodeToClipboard(Parent as Form, code);
                }
              }

              item.LastCode = code;
              if (code != null && code.Length > 5) {
                code = code.Insert(code.Length / 2, " ");
              }
            }
            catch (EncryptedSecretDataException) {
              code = "- - - - - -";
            }
          }
          else {
            code = "- - - - - -";
          }

          var codesize = e.Graphics.MeasureString(code, e.Font);
          rect = new Rectangle(e.Bounds.X + MARGIN_LEFT + ICON_WIDTH + ICON_MARGIN_RIGHT,
            e.Bounds.Y + MARGIN_TOP + (int)labelsize.Height + LABEL_MARGIN_BOTTOM, (int)codesize.Width,
            (int)codesize.Height);
          if (cliprect.IntersectsWith(rect)) {
            e.Graphics.DrawString(code, e.Font, brush,
              new PointF(e.Bounds.X + MARGIN_LEFT + ICON_WIDTH + ICON_MARGIN_RIGHT,
                e.Bounds.Y + MARGIN_TOP + labelsize.Height + LABEL_MARGIN_BOTTOM));
          }
        }

        // draw the refresh image or pie
        rect = new Rectangle(e.Bounds.X + e.Bounds.Width - (MARGIN_RIGHT + ICON_WIDTH), e.Bounds.Y + MARGIN_TOP,
          ICON_WIDTH, ICON_HEIGHT);
        if (cliprect.IntersectsWith(rect)) {
          if (auth.AutoRefresh) {
            using (var piebrush = new SolidBrush(SystemColors.ActiveCaption)) {
              using (var piepen = new Pen(SystemColors.ActiveCaption)) {
                //int y = (this.TopIndex * this.ItemHeight) + e.Bounds.y
                //int tillUpdate = ((int)((auth.AuthenticatorData.ServerTime % 30000) / 1000L) + 1) * 12;
                var tillUpdate =
                  (int)Math.Round(
                    (decimal)(auth.AuthenticatorData.ServerTime % ((long)auth.AuthenticatorData.Period * 1000L) /
                               1000L) * (360M / (decimal)auth.AuthenticatorData.Period));
                e.Graphics.DrawPie(piepen, e.Bounds.X + e.Bounds.Width - (MARGIN_RIGHT + ICON_WIDTH),
                  e.Bounds.Y + MARGIN_TOP + PIE_MARGIN, PIE_WIDTH, PIE_HEIGHT, PIE_STARTANGLE, PIE_SWEEPANGLE);
                e.Graphics.FillPie(piebrush, e.Bounds.X + e.Bounds.Width - (MARGIN_RIGHT + ICON_WIDTH),
                  e.Bounds.Y + MARGIN_TOP + PIE_MARGIN, PIE_WIDTH, PIE_HEIGHT, PIE_STARTANGLE, tillUpdate);
              }
            }
          }
          else {
            if (showCode) {
              using (var piebrush = new SolidBrush(SystemColors.ActiveCaption)) {
                using (var piepen = new Pen(SystemColors.ActiveCaption)) {
                  var tillUpdate = (int)(item.DisplayUntil.Subtract(DateTime.Now).TotalSeconds * (double)360 /
                                          item.DisplayUntil.Subtract(item.LastUpdate).TotalSeconds);
                  e.Graphics.DrawPie(piepen, e.Bounds.X + e.Bounds.Width - (MARGIN_RIGHT + ICON_WIDTH),
                    e.Bounds.Y + MARGIN_TOP + PIE_MARGIN, PIE_WIDTH, PIE_HEIGHT, PIE_STARTANGLE, PIE_SWEEPANGLE);
                  e.Graphics.FillPie(piebrush, e.Bounds.X + e.Bounds.Width - (MARGIN_RIGHT + ICON_WIDTH),
                    e.Bounds.Y + MARGIN_TOP + PIE_MARGIN, PIE_WIDTH, PIE_HEIGHT, PIE_STARTANGLE, tillUpdate);
                }
              }
            }
            else if (auth.AuthenticatorData != null && auth.AuthenticatorData.RequiresPassword) {
              e.Graphics.DrawImage(Properties.Resources.RefreshIconWithLock, rect);
            }
            else {
              e.Graphics.DrawImage(Properties.Resources.RefreshIcon, rect);
            }
          }
        }

        // draw the separating line
        rect = new Rectangle(e.Bounds.X, e.Bounds.Y + ItemHeight - 1, 1, 1);
        if (cliprect.IntersectsWith(rect)) {
          using (var pen = new Pen(SystemColors.Control)) {
            e.Graphics.DrawLine(pen, e.Bounds.X, e.Bounds.Y + ItemHeight - 1, e.Bounds.X + e.Bounds.Width,
              e.Bounds.Y + ItemHeight - 1);
          }
        }
      }
    }

    protected override void OnPaint(PaintEventArgs e) {
      using (var brush = new SolidBrush(BackColor)) {
        var region = new Region(e.ClipRectangle);

        e.Graphics.FillRegion(brush, region);
        if (Items.Count > 0) {
          for (var i = 0; i < Items.Count; ++i) {
            var irect = GetItemRectangle(i);
            if (e.ClipRectangle.IntersectsWith(irect)) {
              if ((SelectionMode == SelectionMode.One && SelectedIndex == i)
                  || (SelectionMode == SelectionMode.MultiSimple && SelectedIndices.Contains(i))
                  || (SelectionMode == SelectionMode.MultiExtended && SelectedIndices.Contains(i))) {
                var diea = new DrawItemEventArgs(e.Graphics, Font,
                  irect, i,
                  DrawItemState.Selected, ForeColor,
                  BackColor);
                OnDrawItem(diea, e.ClipRectangle);
                base.OnDrawItem(diea);
              }
              else {
                var diea = new DrawItemEventArgs(e.Graphics, Font,
                  irect, i,
                  DrawItemState.Default, ForeColor,
                  BackColor);
                OnDrawItem(diea, e.ClipRectangle);
                base.OnDrawItem(diea);
              }

              region.Complement(irect);
            }
          }
        }
      }

      base.OnPaint(e);
    }

    #endregion
  }
}