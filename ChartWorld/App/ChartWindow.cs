using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ChartWorld.App
{
    public sealed partial class ChartWindow : Form
    {
        public static Queue<Action<Graphics>> ToPaint { get; } = new();

        public ChartWindow(Workspace.Workspace workspace)
        {
            InitializeComponent();
            DoubleBuffered = true;
            SettingsLoader.LoadDefaultSettings(this);
            ChartSettings.InitializeChartTypeSelection(this, workspace);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

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