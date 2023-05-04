using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Authenticator {
  public partial class Notification : Form {
    public enum AnimationMethod {
      Roll = 0x0,
      Center = 0x10,
      Slide = 0x40000,
      Fade = 0x80000
    }

    [Flags]
    public enum AnimationDirection {
      Right = 0x1,
      Left = 0x2,
      Down = 0x4,
      Up = 0x8
    }

    [DllImport("user32")]
    internal static extern IntPtr GetForegroundWindow();

    [DllImport("user32")]
    internal static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32")]
    internal extern static bool AnimateWindow(IntPtr hWnd, int dwTime, int dwFlags);

    private class FormAnimator {
      #region Constants

      private const int AW_HIDE = 0x10000;

      private const int AW_ACTIVATE = 0x20000;

      private const int DEFAULT_DURATION = 250;

      #endregion // Constants

      #region Variables

      private readonly Form form;
      private AnimationMethod method;

      #endregion // Variables

      #region Properties

      public AnimationDirection Direction { get; set; }

      public int Duration { get; set; }

      #endregion // Properties

      #region Constructors

      public FormAnimator(Form form) {
        this.form = form;

        this.form.Load += Form_Load;
        this.form.VisibleChanged += Form_VisibleChanged;
        this.form.Closing += Form_Closing;

        Duration = DEFAULT_DURATION;
      }

      public FormAnimator(Form form, AnimationMethod method, int duration) : this(form) {
        this.method = method;
        Duration = duration;
      }

      public FormAnimator(Form form, AnimationMethod method, AnimationDirection direction, int duration) : this(form,
        method, duration) {
        Direction = direction;
      }

      #endregion // Constructors

      #region Event Handlers

      private void Form_Load(object sender, EventArgs e) {
        // MDI child forms do not support transparency so do not try to use the Fade method
        if (form.MdiParent == null || method != AnimationMethod.Fade) {
          AnimateWindow(form.Handle, Duration, AW_ACTIVATE | (int) method | (int) Direction);
        }
      }

      private void Form_VisibleChanged(object sender, EventArgs e) {
        // Do not attempt to animate MDI child forms while showing or hiding as they do not behave as expected
        if (form.MdiParent == null) {
          var flags = (int) method | (int) Direction;

          if (form.Visible) {
            flags = flags | AW_ACTIVATE;
          }
          else {
            flags = flags | AW_HIDE;
          }

          AnimateWindow(form.Handle, Duration, flags);
        }
      }

      private void Form_Closing(object sender, CancelEventArgs e) {
        if (e.Cancel) return;
        // MDI child forms do not support transparency so do not try to use the Fade method.
        if (form.MdiParent == null || method != AnimationMethod.Fade) {
          AnimateWindow(form.Handle, Duration, AW_HIDE | (int) method | (int) Direction);
        }
      }

      #endregion // Event Handlers
    }

    private static List<Notification> openNotifications = new List<Notification>();

    private bool allowFocus;
    private readonly FormAnimator animator;
    private IntPtr currentForegroundWindow;
    private readonly List<NotificationButton> buttons;

    public class NotificationButton {
      public string Text;
      public object Tag;
      public Action<Form> OnPressed;
    }

    public class NotificationAnimation {
      public AnimationMethod Method = AnimationMethod.Slide;
      public AnimationDirection Direction = AnimationDirection.Up;
    }

    public EventHandler OnNotificationClicked;

    public Notification(string title, string body, int hideInMs = 0, List<NotificationButton> buttons = null,
      NotificationAnimation animation = null) {
      InitializeComponent();

      if (animation == null) {
        animation = new NotificationAnimation();
      }

      lifeTimer.Interval = (hideInMs > 0 ? hideInMs : int.MaxValue);

      // set the title
      labelTitle.Text = title;

      // show or hide the buttons
      this.buttons = buttons;
      if (this.buttons == null || this.buttons.Count == 0) {
        buttonPanel.Visible = false;
      }
      else {
        buttonPanel.Visible = true;
        Height += buttonPanel.Height;
        labelBody.Height -= buttonPanel.Height;
        htmlBody.Height -= buttonPanel.Height;

        if (this.buttons.Count >= 1) {
          button1.Text = this.buttons[0].Text;
          button1.Tag = this.buttons[0];
        }

        if (this.buttons.Count >= 2) {
          button2.Text = this.buttons[1].Text;
          button2.Tag = this.buttons[1];
          button2.Visible = true;
        }

        if (this.buttons.Count >= 3) {
          button3.Text = this.buttons[2].Text;
          button3.Tag = this.buttons[2];
          button3.Visible = true;
        }
      }

      // set the body text or html
      if (body.IndexOf("<") == -1) {
        labelBody.Text = body;
        labelBody.Visible = true;
      }
      else {
        // inject browser
        var browser = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
        browser.Dock = DockStyle.Fill;
        browser.Location = new Point(0, 0);
        browser.Size = new Size(htmlBody.Width, htmlBody.Height);
        htmlBody.Controls.Add(browser);

        browser.Text =
          "<!doctype html><html><head><meta charset=\"UTF-8\"><style>html,body{width:100%;height:100%;margin:0;padding:0;}body{margin:0 5px;font-size:14px;border:1px;}h1{font-size:16px;font-weight:normal;margin:0;padding:0 0 4px 0;}</style></head><body>" +
          body + "</body></html>";
        browser.AutoScroll = false;
        browser.IsSelectionEnabled = false;

        browser.Click += Notification_Click;

        htmlBody.Visible = true;
      }

      if (OnNotificationClicked != null) {
        labelBody.Cursor = Cursors.Hand;
        htmlBody.Cursor = Cursors.Hand;
      }

      animator = new FormAnimator(this, animation.Method, animation.Direction, 500);

      //Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width - 5, Height - 5, 5, 5));
    }

    #region Methods

    public new void Show() {
      // Determine the current foreground window so it can be reactivated each time this form tries to get the focus
      currentForegroundWindow = GetForegroundWindow();
      base.Show();
    }

    #endregion // Methods

    #region Event Handlers

    private void Notification_Load(object sender, EventArgs e) {
      // Display the form just above the system tray.
      Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width,
        Screen.PrimaryScreen.WorkingArea.Height - Height);

      // Move each open form upwards to make room for this one
      foreach (var openForm in openNotifications) {
        openForm.Top -= Height;
      }

      openNotifications.Add(this);
      lifeTimer.Start();
    }

    private void Notification_Activated(object sender, EventArgs e) {
      // Prevent the form taking focus when it is initially shown
      if (!allowFocus) {
        // Activate the window that previously had focus
        SetForegroundWindow(currentForegroundWindow);
      }
    }

    private void Notification_Shown(object sender, EventArgs e) {
      // Once the animation has completed the form can receive focus
      allowFocus = true;

      // Close the form by sliding down.
      animator.Duration = 0;
      animator.Direction = AnimationDirection.Down;
    }

    private void Notification_FormClosed(object sender, FormClosedEventArgs e) {
      // Move down any open forms above this one
      foreach (var openForm in openNotifications) {
        if (openForm == this) {
          // Remaining forms are below this one
          break;
        }

        openForm.Top += Height;
      }

      openNotifications.Remove(this);
    }

    private void lifeTimer_Tick(object sender, EventArgs e) {
      Close();
    }

    private void Notification_Click(object sender, EventArgs e) {
      Close();

      if (OnNotificationClicked != null) {
        OnNotificationClicked(this, e);
      }
    }

    #endregion // Event Handlers

    private void button1_Click(object sender, EventArgs e) {
      var button = button1.Tag as NotificationButton;
      if (button != null && button.OnPressed != null) {
        button.OnPressed(this);
      }
    }
    
    private void closeLink_Click(object sender, EventArgs e) {
      Close();
    }
  }
}