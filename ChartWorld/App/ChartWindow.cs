using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using ChartWorld.Chart;
using ChartWorld.Statistic;

namespace ChartWorld.App
{
    public sealed partial class ChartWindow : Form
    {
        public static Queue<Action<Graphics>> ToPaint { get; } = new();

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
            var graphics = e.Graphics;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            while (ToPaint.Count > 0)
            {
                var action = ToPaint.Dequeue();
                action(graphics);
            }
        }
    }
}