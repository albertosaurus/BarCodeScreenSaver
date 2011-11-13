/*
Copyright (c) 2011 Arthur Shagall, Mindflight, Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace BarCodeScreenSaver {
    public partial class ScreenSaverWindow : Form {
        private Point bp;
        private readonly Font tf = new Font("arial", 80);
        private readonly Code128Painter c128 = new Code128Painter();
        private readonly string format;
        private readonly int screenIndex;

        public ScreenSaverWindow() {
            InitializeComponent();
            drawTimer.Tick+=delegate { this.Refresh(); };
            Load += ScreenSaverWindow_Load;
            Shown += ScreenSaverWindow_Shown;
            c128.Height = 250;
            c128.Width = 4;
            format = global::BarCodeScreenSaver.Properties.Settings.Default.Format;
        }

        void ScreenSaverWindow_Shown(object sender, EventArgs e) {
            bp = MousePosition;
            Cursor.Hide();
            drawTimer.Start();
            Bounds = Screen.AllScreens[screenIndex].Bounds;
        }

        void ScreenSaverWindow_Load(object sender, EventArgs e) {
            MouseMove += Exit;
            MouseDown += delegate { Exit(null, null); };
            this.MouseWheel += delegate { Exit(null, null); };
            KeyDown += delegate { Exit(null, null); };
        }

        public ScreenSaverWindow(int i) : this() {
            Screen s = Screen.AllScreens[i];
            this.screenIndex = i;
            Bounds = s.Bounds;
            TopMost = true;
        }

        protected override void OnPaint(PaintEventArgs e) {
            Rectangle r = new Rectangle(0,0,Bounds.Width, Bounds.Height);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.FillRectangle(Brushes.Black, r);
            DateTime dt = DateTime.Now;
            string str = dt.ToString(format);
            Size ts = TextRenderer.MeasureText(g, str, tf);
            int hPos = r.Left + (r.Width - ts.Width)/2;
            int vPos = r.Top + (r.Height - ts.Height)/2+100;
            g.DrawString(str, tf, Brushes.White, hPos, vPos);
            int vPos2 = vPos - 260;
            c128.Y = vPos2;
            c128.X = r.Left + r.Width/2 - 268;
            c128.DrawBarCode(str, g);
        }

        private void Exit(object o, MouseEventArgs e) {
            if (e != null) {
                int delta = CalculatePositionDelta(bp, e.Location);
                if (delta < 100) {
                    return;
                }
            }
            Application.Exit();
        }

        private static int CalculatePositionDelta(Point p1, Point p2) {
            int dx = Math.Abs(p1.X - p2.X);
            int dy = Math.Abs(p1.Y - p2.Y);
            return (int)Math.Sqrt((dx*dx) + (dy*dy));
        }
    }
}