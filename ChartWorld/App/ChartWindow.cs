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
            var pen = new Pen(Color.Black);
            pen.CustomEndCap = new AdjustableArrowCap(5, 5);
            pen.Width = 2;
            var width = WindowInfo.Screen.Size.Width;
            var height = WindowInfo.Screen.Size.Height;
            var chartBottomRight = new Point(width - width / 20, height - height / 10);
            var chartTopLeft = new Point(width / 20,  height / 20);
            var chartStart = new Point(width / 20, height - height / 10);
            e.Graphics.DrawLine(pen, chartStart, chartBottomRight);
            e.Graphics.DrawLine(pen, chartStart, chartTopLeft);
            e.Graphics.DrawString("nothing", new Font("Arial", 20, FontStyle.Regular), Brushes.Black, chartBottomRight);
        }
    }
}