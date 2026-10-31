using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace XTools
{
    public class TabCloseRequestedEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public TabPage Page { get; private set; }

        public TabCloseRequestedEventArgs(int index, TabPage page)
        {
            Index = index;
            Page = page;
        }
    }

    public class ModernTabControl : TabControl
    {
        private const int BaseTabHeight = 44;
        private const int BaseCloseSize = 18;

        private int hoverIndex = -1;
        private bool hoverClose;

        private float DpiScale
        {
            get
            {
                float scale = DeviceDpi / 96f;
                return scale <= 0f ? 1f : scale;
            }
        }

        private int Dpi(int value)
        {
            return (int)Math.Round(value * DpiScale);
        }

        public event EventHandler<TabCloseRequestedEventArgs> TabCloseRequested;

        public ModernTabControl()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);
            DrawMode = TabDrawMode.OwnerDrawFixed;
            SizeMode = TabSizeMode.Fixed;
            Appearance = TabAppearance.Normal;
            ItemSize = new Size(Dpi(160), Dpi(BaseTabHeight));
            Font = UITheme.TabFont;
            BackColor = UITheme.Background;
            ForeColor = UITheme.Text;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try
            {
                // 关闭系统主题
                SetWindowTheme(Handle, string.Empty, string.Empty);
            }
            catch
            {
                // 非 Windows 或不支持 uxtheme 时忽略
            }
            UpdateItemSize();
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            UpdateItemSize();
            Invalidate();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            TabPage page = e.Control as TabPage;
            if (page != null)
            {
                page.BackColor = Color.FromArgb(20, 21, 27);
                page.ForeColor = UITheme.Text;
                page.Padding = new Padding(0);
                page.UseVisualStyleBackColor = false;
                page.TextChanged += PageTextChanged;
            }

            UpdateItemSize();
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            TabPage page = e.Control as TabPage;
            if (page != null)
            {
                page.TextChanged -= PageTextChanged;
            }

            base.OnControlRemoved(e);
            UpdateItemSize();
        }

        private void PageTextChanged(object sender, EventArgs e)
        {
            UpdateItemSize();
            Invalidate();
        }

        private void UpdateItemSize()
        {
            int width = Dpi(174);
            foreach (TabPage page in TabPages)
            {
                int textWidth = TextRenderer.MeasureText(page.Text ?? string.Empty, UITheme.TabSelectedFont).Width;
                int extra = Dpi(TabPages.IndexOf(page) == 0 ? 52 : 88);
                width = Math.Max(width, textWidth + extra);
            }

            width = Math.Min(Dpi(340), width);
            Size size = new Size(width, Dpi(BaseTabHeight));
            if (ItemSize != size)
            {
                ItemSize = size;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(UITheme.Background);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int headerHeight = Math.Max(0, Math.Min(Height, DisplayRectangle.Top));
            using (SolidBrush header = new SolidBrush(UITheme.Background))
            {
                graphics.FillRectangle(header, 0, 0, Width, headerHeight);
            }

            for (int i = 0; i < TabCount; i++)
            {
                DrawTab(graphics, i, GetTabRect(i));
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= TabPages.Count)
            {
                return;
            }

            DrawTab(e.Graphics, e.Index, e.Bounds);
        }

        private void DrawTab(Graphics graphics, int index, Rectangle bounds)
        {
            TabPage page = TabPages[index];
            bool selected = index == SelectedIndex;
            bool hovered = index == hoverIndex;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int insetX = Dpi(2);
            int insetTop = Dpi(4);
            int insetBottom = Dpi(5);
            Rectangle tab = new Rectangle(
                bounds.X + insetX,
                bounds.Y + insetTop,
                Math.Max(1, bounds.Width - insetX * 2),
                Math.Max(1, bounds.Height - insetTop - insetBottom));
            Color fill = selected
                ? Color.FromArgb(40, 44, 58)
                : hovered ? Color.FromArgb(31, 34, 44) : Color.FromArgb(24, 26, 34);

            using (GraphicsPath path = UITheme.CreateRoundedRectangle(tab, Dpi(9)))
            using (SolidBrush brush = new SolidBrush(fill))
            {
                graphics.FillPath(brush, path);
            }

            if (selected)
            {
                using (Pen accent = new Pen(UITheme.Accent, 2.4f * DpiScale))
                {
                    accent.StartCap = LineCap.Round;
                    accent.EndCap = LineCap.Round;
                    float lineY = tab.Top + 1.5f * DpiScale;
                    graphics.DrawLine(accent, tab.Left + Dpi(12), lineY, tab.Right - Dpi(12), lineY);
                }
            }
            else
            {
                using (Pen border = new Pen(Color.FromArgb(38, 42, 54), DpiScale))
                using (GraphicsPath path = UITheme.CreateRoundedRectangle(tab, Dpi(9)))
                {
                    graphics.DrawPath(border, path);
                }
            }

            int reservedRight = Dpi(index == 0 ? 14 : 32);
            Rectangle textBounds = new Rectangle(
                tab.Left + Dpi(16),
                tab.Top,
                Math.Max(1, tab.Width - Dpi(16) - reservedRight),
                tab.Height);
            Font font = selected ? UITheme.TabSelectedFont : UITheme.TabFont;
            Color textColor = selected ? UITheme.Text : hovered ? Color.FromArgb(210, 215, 226) : UITheme.TextMuted;

            TextRenderer.DrawText(
                graphics,
                page.Text ?? string.Empty,
                font,
                textBounds,
                textColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

            if (index > 0)
            {
                DrawCloseButton(graphics, index);
            }
        }

        private void DrawCloseButton(Graphics graphics, int index)
        {
            Rectangle closeBounds = GetCloseBounds(index);
            bool active = index == hoverIndex && hoverClose;

            if (active)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(92, UITheme.Danger)))
                {
                    graphics.FillEllipse(brush, closeBounds);
                }
            }

            float centerX = closeBounds.Left + closeBounds.Width / 2f;
            float centerY = closeBounds.Top + closeBounds.Height / 2f;
            float radius = Dpi(3);
            using (Pen pen = new Pen(active ? Color.White : Color.FromArgb(126, 132, 148), 1.5f * DpiScale))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                graphics.DrawLine(pen, centerX - radius, centerY - radius, centerX + radius, centerY + radius);
                graphics.DrawLine(pen, centerX - radius, centerY + radius, centerX + radius, centerY - radius);
            }
        }

        private Rectangle GetCloseBounds(int index)
        {
            Rectangle tab = GetTabRect(index);
            int size = Dpi(BaseCloseSize);
            return new Rectangle(
                tab.Right - Dpi(27),
                tab.Top + (tab.Height - size) / 2,
                size,
                size);
        }

        private int HitTest(Point location)
        {
            for (int i = 0; i < TabCount; i++)
            {
                if (GetTabRect(i).Contains(location))
                {
                    return i;
                }
            }

            return -1;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int index = HitTest(e.Location);
            bool close = index > 0 && GetCloseBounds(index).Contains(e.Location);
            if (index != hoverIndex || close != hoverClose)
            {
                hoverIndex = index;
                hoverClose = close;
                Cursor = close ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (hoverIndex != -1 || hoverClose)
            {
                hoverIndex = -1;
                hoverClose = false;
                Cursor = Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int index = HitTest(e.Location);
            bool closeHit = index > 0 && GetCloseBounds(index).Contains(e.Location);

            if ((e.Button == MouseButtons.Left && closeHit) || (e.Button == MouseButtons.Middle && index > 0))
            {
                OnTabCloseRequested(index);
                return;
            }

            base.OnMouseDown(e);
        }

        private void OnTabCloseRequested(int index)
        {
            EventHandler<TabCloseRequestedEventArgs> handler = TabCloseRequested;
            if (handler != null && index >= 0 && index < TabPages.Count)
            {
                handler(this, new TabCloseRequestedEventArgs(index, TabPages[index]));
            }
        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
    }
}
