using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class LightTest : Form
    {
        public LightTest()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            e.Graphics.FillEllipse(Brushes.DarkRed, 40, 40, this.ClientSize.Width - 80, this.ClientSize.Height - 80);

            System.Drawing.Drawing2D.GraphicsPath gPath = new System.Drawing.Drawing2D.GraphicsPath();
            gPath.AddEllipse(40, 40, this.ClientSize.Width - 80, this.ClientSize.Height - 80);

            using (System.Drawing.Drawing2D.PathGradientBrush p = new System.Drawing.Drawing2D.PathGradientBrush(gPath))
            {
                p.CenterColor = Color.FromArgb(127, 255, 255, 255);
                p.SurroundColors = new Color[] { Color.FromArgb(27, 255, 255, 255) };
                e.Graphics.FillEllipse(p, 40, 40, this.ClientSize.Width - 80, this.ClientSize.Height - 80);
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            this.Invalidate();
        }
    }
}
