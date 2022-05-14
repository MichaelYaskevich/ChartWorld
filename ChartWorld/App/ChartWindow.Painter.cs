using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;
using ChartWorld.Chart;
using ChartWorld.Statistic;

namespace ChartWorld.App
{
    public static class Painter
    {
        public static Graphics Graphics { get; set; }

        public static void PaintChart(IChart chart)
        {
            switch (chart)
            {
                case BarChart:
                    PaintBarChart((BarChart) chart.BuildChart());
                    break;
                case PieChart:
                    PaintPieChart((PieChart) chart.BuildChart());
                    break;
                default:
                    throw new ArgumentException($"Unexpected chart type: {chart.GetType()}", nameof(chart));
            }
        }

        private static void PaintBarChart(BarChart chart)
        {
            var width = WindowInfo.Screen.Size.Width;
            var height = WindowInfo.Screen.Size.Height;
            var chartBottomRight = new Point(width - width / 20, height - height / 10);
            var chartTopLeft = new Point(width / 20, height / 20);
            var chartStart = new Point(width / 20, height - height / 10);
            using (var pen = new Pen(Color.Black))
            {
                pen.CustomEndCap = new AdjustableArrowCap(5, 5);
                pen.Width = 2;
                Graphics.DrawLine(pen, chartStart, chartBottomRight);
                Graphics.DrawLine(pen, chartStart, chartTopLeft);
                Graphics.DrawString("nothing", new Font("Arial", 20, FontStyle.Regular), Brushes.Black,
                    chartBottomRight);
            }

            var items = chart.Data.GetOrderedItems();
        }

        private static void PaintPieChart(PieChart chart)
        {
            var width = WindowInfo.Screen.Size.Width;
            var height = WindowInfo.Screen.Size.Height;
            var data = chart.PercentageData.GetOrderedItems();
            using (var pen = new Pen(Color.Black))
            {
                pen.Width = 2;
                var circleSize = new Size(height / 2, height / 2);
                Graphics.DrawEllipse(pen, new Rectangle(new Point(width / 3, height / 4), circleSize));
                var circleCenter = new Point(width / 3 + height / 4, height / 2);
                // 2 * Math.PI * circleSize.Height / 2
                var circumference = Math.PI * circleSize.Height;
                foreach (var pair in data)
                {
                    Graphics.DrawLine(pen, circleCenter,
                        new Point(circleCenter.X - circleSize.Width / 2, circleCenter.Y));
                }
            }
        }
    }
}