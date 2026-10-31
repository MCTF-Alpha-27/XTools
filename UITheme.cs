using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace XTools
{
    internal static class UITheme
    {
        public static readonly Color Background = Color.FromArgb(18, 19, 24);
        public static readonly Color Surface = Color.FromArgb(26, 28, 36);
        public static readonly Color SurfaceAlt = Color.FromArgb(32, 35, 45);
        public static readonly Color SurfaceHover = Color.FromArgb(43, 47, 61);
        public static readonly Color Border = Color.FromArgb(52, 56, 70);
        public static readonly Color Text = Color.FromArgb(232, 234, 240);
        public static readonly Color TextMuted = Color.FromArgb(148, 154, 170);
        public static readonly Color Accent = Color.FromArgb(104, 132, 255);
        public static readonly Color Danger = Color.FromArgb(255, 107, 107);

        public static readonly Font BodyFont = CreateFont(9.5f, FontStyle.Regular);
        public static readonly Font MenuFont = CreateFont(10f, FontStyle.Regular);
        public static readonly Font TabFont = CreateFont(9.5f, FontStyle.Regular);
        public static readonly Font TabSelectedFont = CreateFont(9.5f, FontStyle.Bold);

        private static readonly HashSet<ToolStripItem> StyledItems = new HashSet<ToolStripItem>();

        private static Font CreateFont(float size, FontStyle style)
        {
            Font font = new Font("Microsoft YaHei UI", size, style, GraphicsUnit.Point);
            if (string.Equals(font.Name, "Microsoft YaHei UI", StringComparison.OrdinalIgnoreCase))
            {
                return font;
            }

            font.Dispose();
            return new Font("Segoe UI", size, style, GraphicsUnit.Point);
        }

        public static void ApplyMenuTheme(MenuStrip menu)
        {
            menu.Renderer = new ModernMenuRenderer();
            menu.BackColor = Surface;
            menu.ForeColor = Text;
            menu.Font = MenuFont;
            menu.GripStyle = ToolStripGripStyle.Hidden;

            ApplyMenuMetrics(menu);

            foreach (ToolStripItem item in menu.Items)
            {
                StyleMenuItem(item);
            }

            menu.ItemAdded += (sender, e) => StyleMenuItem(e.Item);
            menu.HandleCreated += (sender, e) => ApplyMenuMetrics(menu);
            menu.DpiChangedAfterParent += (sender, e) => ApplyMenuMetrics(menu);
        }

        private static void ApplyMenuMetrics(MenuStrip menu)
        {
            float scale = GetScale(menu);
            menu.Padding = ScalePadding(new Padding(12, 6, 0, 6), scale);
            foreach (ToolStripItem item in menu.Items)
            {
                ApplyItemMetrics(item, scale);
            }
        }

        private static void ApplyItemMetrics(ToolStripItem item, float scale)
        {
            bool topLevel = item.Owner is MenuStrip;
            item.Padding = ScalePadding(topLevel ? new Padding(12, 7, 12, 7) : new Padding(10, 5, 10, 5), scale);

            ToolStripMenuItem menuItem = item as ToolStripMenuItem;
            if (menuItem == null)
            {
                return;
            }

            menuItem.DropDown.Padding = ScalePadding(new Padding(4, 6, 4, 6), scale);
            foreach (ToolStripItem child in menuItem.DropDownItems)
            {
                ApplyItemMetrics(child, scale);
            }
        }

        public static float GetScale(Control control)
        {
            if (control == null || control.DeviceDpi <= 0)
            {
                return 1f;
            }
            return control.DeviceDpi / 96f;
        }

        public static int Dpi(Control control, int value)
        {
            return (int)Math.Round(value * GetScale(control));
        }

        private static Padding ScalePadding(Padding padding, float scale)
        {
            return new Padding(
                (int)Math.Round(padding.Left * scale),
                (int)Math.Round(padding.Top * scale),
                (int)Math.Round(padding.Right * scale),
                (int)Math.Round(padding.Bottom * scale));
        }

        public static void StyleMenuItem(ToolStripItem item)
        {
            if (item == null || !StyledItems.Add(item))
            {
                return;
            }

            bool topLevel = item.Owner is MenuStrip;
            float scale = GetScale(item.Owner as Control);
            item.ForeColor = Text;
            item.Font = topLevel ? MenuFont : BodyFont;
            item.Padding = ScalePadding(topLevel ? new Padding(12, 7, 12, 7) : new Padding(10, 5, 10, 5), scale);

            ToolStripMenuItem menuItem = item as ToolStripMenuItem;
            if (menuItem == null)
            {
                return;
            }

            ToolStripDropDown dropDown = menuItem.DropDown;
            dropDown.BackColor = SurfaceAlt;
            dropDown.ForeColor = Text;
            dropDown.Padding = ScalePadding(new Padding(4, 6, 4, 6), scale);
            dropDown.DropShadowEnabled = true;

            dropDown.ItemAdded += (sender, e) => StyleMenuItem(e.Item);
            foreach (ToolStripItem child in menuItem.DropDownItems)
            {
                StyleMenuItem(child);
            }
        }

        public static void EnableDarkTitleBar(Form form)
        {
            if (form == null)
            {
                return;
            }

            if (form.IsHandleCreated)
            {
                ApplyDarkTitleBar(form.Handle);
                return;
            }

            form.HandleCreated += (sender, e) => ApplyDarkTitleBar(form.Handle);
        }

        private static void ApplyDarkTitleBar(IntPtr handle)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                return;
            }

            int enabled = 1;
            // 20 = DWMWA_USE_IMMERSIVE_DARK_MODE (Win10 2004+ / Win11)，19 = 旧版 Win10
            if (DwmSetWindowAttribute(handle, 20, ref enabled, sizeof(int)) != 0)
            {
                DwmSetWindowAttribute(handle, 19, ref enabled, sizeof(int));
            }
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        public static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = Math.Max(1, radius * 2);
            if (diameter > bounds.Width)
            {
                diameter = Math.Max(1, bounds.Width);
            }
            if (diameter > bounds.Height)
            {
                diameter = Math.Max(1, bounds.Height);
            }

            Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));
            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }

    internal class DarkColorTable : ProfessionalColorTable
    {
        public override Color MenuStripGradientBegin { get { return UITheme.Surface; } }
        public override Color MenuStripGradientEnd { get { return UITheme.Surface; } }
        public override Color ToolStripDropDownBackground { get { return UITheme.SurfaceAlt; } }
        public override Color ImageMarginGradientBegin { get { return UITheme.SurfaceAlt; } }
        public override Color ImageMarginGradientMiddle { get { return UITheme.SurfaceAlt; } }
        public override Color ImageMarginGradientEnd { get { return UITheme.SurfaceAlt; } }
        public override Color MenuBorder { get { return UITheme.Border; } }
        public override Color MenuItemBorder { get { return UITheme.Accent; } }
        public override Color MenuItemSelected { get { return UITheme.SurfaceHover; } }
        public override Color MenuItemSelectedGradientBegin { get { return UITheme.SurfaceHover; } }
        public override Color MenuItemSelectedGradientEnd { get { return UITheme.SurfaceHover; } }
        public override Color MenuItemPressedGradientBegin { get { return UITheme.SurfaceHover; } }
        public override Color MenuItemPressedGradientEnd { get { return UITheme.SurfaceHover; } }
        public override Color SeparatorDark { get { return UITheme.Border; } }
        public override Color SeparatorLight { get { return UITheme.Border; } }
        public override Color CheckBackground { get { return UITheme.Accent; } }
        public override Color CheckSelectedBackground { get { return UITheme.Accent; } }
        public override Color CheckPressedBackground { get { return UITheme.Accent; } }
        public override Color ButtonSelectedHighlight { get { return UITheme.SurfaceHover; } }
        public override Color ButtonSelectedBorder { get { return UITheme.Accent; } }
        public override Color ButtonPressedGradientBegin { get { return UITheme.SurfaceHover; } }
        public override Color ButtonPressedGradientMiddle { get { return UITheme.SurfaceHover; } }
        public override Color ButtonPressedGradientEnd { get { return UITheme.SurfaceHover; } }
        public override Color ToolStripGradientBegin { get { return UITheme.SurfaceAlt; } }
        public override Color ToolStripGradientMiddle { get { return UITheme.SurfaceAlt; } }
        public override Color ToolStripGradientEnd { get { return UITheme.SurfaceAlt; } }
        public override Color OverflowButtonGradientBegin { get { return UITheme.SurfaceAlt; } }
        public override Color OverflowButtonGradientMiddle { get { return UITheme.SurfaceAlt; } }
        public override Color OverflowButtonGradientEnd { get { return UITheme.SurfaceAlt; } }
        public override Color StatusStripGradientBegin { get { return UITheme.Surface; } }
        public override Color StatusStripGradientEnd { get { return UITheme.Surface; } }
    }

    internal class ModernMenuRenderer : ToolStripProfessionalRenderer
    {
        public ModernMenuRenderer()
            : base(new DarkColorTable())
        {
            RoundedEdges = false;
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(e.ToolStrip is MenuStrip ? UITheme.Surface : UITheme.SurfaceAlt))
            {
                e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is MenuStrip)
            {
                using (Pen pen = new Pen(UITheme.Border))
                {
                    e.Graphics.DrawLine(pen, 0, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
                }
                return;
            }

            Rectangle border = new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
            using (Pen pen = new Pen(UITheme.Border))
            {
                e.Graphics.DrawRectangle(pen, border);
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            bool highlighted = e.Item.Selected || e.Item.Pressed;
            if (!highlighted)
            {
                return;
            }

            Rectangle bounds = e.Item.IsOnDropDown
                ? new Rectangle(
                    UITheme.Dpi(e.ToolStrip, 3),
                    UITheme.Dpi(e.ToolStrip, 2),
                    e.Item.Width - UITheme.Dpi(e.ToolStrip, 6),
                    e.Item.Height - UITheme.Dpi(e.ToolStrip, 4))
                : new Rectangle(
                    UITheme.Dpi(e.ToolStrip, 4),
                    UITheme.Dpi(e.ToolStrip, 3),
                    e.Item.Width - UITheme.Dpi(e.ToolStrip, 8),
                    e.Item.Height - UITheme.Dpi(e.ToolStrip, 6));

            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                return;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = UITheme.CreateRoundedRectangle(bounds, UITheme.Dpi(e.ToolStrip, 6)))
            using (SolidBrush brush = new SolidBrush(e.Item.Pressed ? UITheme.SurfaceHover : Color.FromArgb(44, 48, 62)))
            {
                e.Graphics.FillPath(brush, path);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (!e.Item.Enabled)
            {
                e.TextColor = UITheme.TextMuted;
            }
            else if (e.Item.Selected || e.Item.Pressed)
            {
                e.TextColor = Color.White;
            }
            else
            {
                e.TextColor = UITheme.Text;
            }

            base.OnRenderItemText(e);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            ToolStrip owner = e.Item.Owner as ToolStrip;
            Rectangle arrow = e.ArrowRectangle;
            if (owner == null || arrow.Width < 6 || arrow.Height < 6)
            {
                base.OnRenderArrow(e);
                return;
            }

            float midX = arrow.Left + arrow.Width / 2f;
            float midY = arrow.Top + arrow.Height / 2f;
            int offset = UITheme.Dpi(owner, 3);
            PointF[] points;

            switch (e.Direction)
            {
                case ArrowDirection.Left:
                    points = new[]
                    {
                        new PointF(arrow.Right - offset, midY - offset),
                        new PointF(arrow.Left + offset, midY),
                        new PointF(arrow.Right - offset, midY + offset)
                    };
                    break;
                case ArrowDirection.Right:
                    points = new[]
                    {
                        new PointF(arrow.Left + offset, midY - offset),
                        new PointF(arrow.Right - offset, midY),
                        new PointF(arrow.Left + offset, midY + offset)
                    };
                    break;
                case ArrowDirection.Up:
                    points = new[]
                    {
                        new PointF(midX - offset, arrow.Bottom - offset),
                        new PointF(midX, arrow.Top + offset),
                        new PointF(midX + offset, arrow.Bottom - offset)
                    };
                    break;
                default:
                    points = new[]
                    {
                        new PointF(midX - offset, arrow.Top + offset),
                        new PointF(midX, arrow.Bottom - offset),
                        new PointF(midX + offset, arrow.Top + offset)
                    };
                    break;
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(e.Item.Selected ? Color.White : UITheme.TextMuted, 1.4f * UITheme.GetScale(owner)))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                e.Graphics.DrawLines(pen, points);
            }
        }
    }
}
