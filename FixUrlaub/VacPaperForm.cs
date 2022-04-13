
using FixUrlaub.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FixUrlaub
{
    internal abstract partial class VacPaperForm : Form
    {
        public readonly ComponentResourceManager resources = new ComponentResourceManager(typeof(VacPaperForm));
        public ColorTheme AppliedTheme;

        public FontFamily FrutigerFam, FrutigerBoldFam;

        public VacPaperForm(string t)
        {
            // Reads the two Fonts as pure Memorystream from the Resource-File and adds them to two Collections (adding to a singular one doesnt seem to work)

            using (PrivateFontCollection collection = new PrivateFontCollection(), collection2 = new PrivateFontCollection())
            {
                unsafe
                {
                    fixed (byte* pFontData = (byte[])resources.GetObject("Frutiger"))
                    {
                        collection.AddMemoryFont((System.IntPtr)pFontData, ((byte[])resources.GetObject("Frutiger")).Length);
                    }
                    fixed (byte* pFontBData = (byte[])resources.GetObject("Frutiger_bold"))
                    {
                        collection2.AddMemoryFont((System.IntPtr)pFontBData, ((byte[])resources.GetObject("Frutiger_bold")).Length);
                    }
                }

                FrutigerFam = collection.Families[0];
                FrutigerBoldFam = collection2.Families[0];
            }


            FormBorderStyle = FormBorderStyle.None;
            AppliedTheme = ColorTheme.Default;
            BackColor = AppliedTheme.Primary;
            ForeColor = AppliedTheme.Secondary;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (WindowState == FormWindowState.Normal)
                DrawBorder(e.Graphics);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            Graphics g = CreateGraphics();
            g.Clear(BackColor);
            if (WindowState == FormWindowState.Normal)
                DrawBorder(g);

            Invalidate();
        }
        #region Registers the Default-Windows-Process to move a window for the MouseDownEvent of this Element
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                WindowState = (this.WindowState == FormWindowState.Maximized) ? FormWindowState.Normal : this.WindowState;

                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);       // Sends the order to the operating system, to grab the Window
            }
        }
        #endregion
        #region FormWindow-SmoothAroundBoxShadow
        private bool m_aeroEnabled;
        private const int CS_DROPSHADOW = 0x20000;                          // Variable for box shadow
        private const int WM_NCPAINT = 0x85;                                // Windows Message for the Window-Paint event

        public struct MARGINS                                               // Margins for the shadow
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        /// <summary>
        /// Checks, if the newest visual Interface "Aero" is activated, aka, if Windows is graphically able to properly display it
        /// </summary>
        /// <returns></returns>
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return enabled == 1;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:                        // Window Paint Message
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(Handle, 2, ref v, 4);
                        MARGINS margins =
                            new MARGINS()
                            {
                                bottomHeight = 1,
                                leftWidth = 1,
                                rightWidth = 1,
                                topHeight = 1
                            };
                        DwmExtendFrameIntoClientArea(Handle, ref margins);

                    }
                    break;

                default:
                    break;
            }
            base.WndProc(ref m);

        }
        #endregion

        private void DrawBorder(Graphics g)
        {
            g.DrawLine(new Pen(AppliedTheme.Secondary, 1), 0, 0, 0, Height - 1);
            g.DrawLine(new Pen(AppliedTheme.Secondary, 1), 0, Height - 1, Width - 1, Height - 1);
            g.DrawLine(new Pen(AppliedTheme.Secondary, 1), Width - 1, 0, Width - 1, Height - 1);
            g.DrawLine(new Pen(AppliedTheme.Secondary, 1), 0, 0, Width - 1, 0);
        }


        #region dll-Imports
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,      // x-coordinate of upper-left corner
            int nTopRect,       // y-coordinate of upper-left corner
            int nRightRect,     // x-coordinate of lower-right corner
            int nBottomRect,    // y-coordinate of lower-right corner
            int nWidthEllipse,  // height of ellipse
            int nHeightEllipse  // width of ellipse
        );
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        #endregion
    }
}
