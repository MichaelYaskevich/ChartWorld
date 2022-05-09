using System;
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
                    PaintBarChart(chart);
                    break;
                case PieChart:
                    PaintPieChart(chart);
                    break;
                default:
                    throw new ArgumentException($"Unexpected chart type: {chart.GetType()}", nameof(chart));
            }
        }

        private static void PaintBarChart(IChart chart)
        {
            var size = WindowInfo.Screen.Size;
            using (var pen = new Pen(Color.Black))
            {
                Graphics.DrawLine(pen, new Point(size.Width / 10, size.Height - size.Height / 10), new Point(size.Width - size.Width / 10, size.Height - size.Height / 10));
                pen.CustomEndCap = new AdjustableArrowCap(5, 5);
                pen.Width = 2;
                var width = WindowInfo.Screen.Size.Width;
                var height = WindowInfo.Screen.Size.Height;
                var chartBottomRight = new Point(width - width / 20, height - height / 10);
                var chartTopLeft = new Point(width / 20,  height / 20);
                var chartStart = new Point(width / 20, height - height / 10);
                Graphics.DrawLine(pen, chartStart, chartBottomRight);
                Graphics.DrawLine(pen, chartStart, chartTopLeft);
                Graphics.DrawString("nothing", new Font("Arial", 20, FontStyle.Regular), Brushes.Black, chartBottomRight);
            }

            var items = chart.Data.GetOrderedItems();
        }
        
        private static void PaintPieChart(IChart chart)
        {
            throw new NotImplementedException();
        }
    }
}