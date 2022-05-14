using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ChartWorld.Chart;
using ChartWorld.Statistic;

namespace ChartWorld.App
{
    public sealed partial class ChartWindow : Form
    {
        public ChartWindow()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SettingsLoader.LoadDefaultSettings(this);
            ChartSettings.InitializeChartTypeSelection(this);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        // Как-то криво, не?
        protected override void OnPaint(PaintEventArgs e)
        {
            Painter.Graphics = e.Graphics;
            var width = WindowInfo.Screen.Size.Width;
            var height = WindowInfo.Screen.Size.Height;
            using (var pen = new Pen(Color.Black))
            {
                pen.Width = 2;
                var circleSize = new Size(height / 2, height / 2);
                var circleCenter = new Point(width / 3 + height / 4, height / 2);
                var radius = circleSize.Height / 2;
                e.Graphics.DrawEllipse(pen, new Rectangle(new Point(width / 3, height / 4), circleSize));
                // Работает
                e.Graphics.DrawLine(pen, circleCenter,
                    new Point(circleCenter.X - circleSize.Width / 2, circleCenter.Y));
                e.Graphics.DrawLine(pen, circleCenter,
                    new PointF((float) (circleCenter.X + radius * Math.Cos(30 * (Math.PI / 180))),
                        (float) (circleCenter.Y + radius * Math.Sin(30 * (Math.PI / 180)))));
            }
        }
    }
}