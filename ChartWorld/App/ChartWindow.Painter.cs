using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ChartWorld.Chart;
using ChartWorld.Workspace;

namespace ChartWorld.App
{
    public static class Painter
    {
        private const double Radian = Math.PI / 180;
        private static readonly Size RectangleSize = new Size(30, 30);

        public static void Paint(IWorkspaceEntity entity, Form form)
        {
            switch (entity)
            {
                case BarChart chart:
                    PaintBarChart(chart.BuildChart(), form);
                    break;
                case PieChart chart:
                    PaintPieChart(chart.BuildChart(), form);
                    break;
                default:
                    throw new ArgumentException($"Unexpected chart type: {entity.GetType()}", nameof(entity));
            }
        }

        private static void PaintBarChart(BarChart chart, Control form)
        {
            var width = WindowInfo.ScreenSize.Width;
            var height = WindowInfo.ScreenSize.Height;
            var chartBottomRight = new Point(width - width / 20, height - height / 10);
            var chartTopLeft = new Point(width / 20, height / 20);
            var chartStart = new Point(width / 20, height - height / 10);
            var pen = new Pen(Color.Black);
            pen.CustomEndCap = new AdjustableArrowCap(5, 5);
            pen.Width = 2;
            ChartWindow.ToPaint.Enqueue(g => g.DrawLine(pen, chartStart, chartBottomRight));
            ChartWindow.ToPaint.Enqueue(g => g.DrawLine(pen, chartStart, chartTopLeft));
            ChartWindow.ToPaint.Enqueue(g => g.DrawString("nothing", new Font("Arial", 20, FontStyle.Regular),
                Brushes.Black,
                chartBottomRight));
            var items = chart.Data.GetOrderedItems();
            form.Invalidate();
        }

        private static void PaintPieChart(PieChart chart, Control form)
        {
            var width = WindowInfo.ScreenSize.Width;
            var height = WindowInfo.ScreenSize.Height;
            var data = chart.PercentageData.GetOrderedItems();
            var pen = new Pen(Color.Black);
            pen.Width = 2;
            var circleSize = new Size(height / 2, height / 2);
            var boundingRectangle = new Rectangle(new Point((width - circleSize.Width) / 2, (height - circleSize.Height) / 2), circleSize);
            EnqueuePieDrawing(pen, data, boundingRectangle);
            form.Invalidate();
        }

        private static void EnqueuePieDrawing(Pen pen, IEnumerable<(string, double)> data, Rectangle boundingRectangle)
        {
            var rnd = new Random();
            double angle = rnd.Next(0, 360);
            ChartWindow.ToPaint.Enqueue(g =>
                g.DrawEllipse(pen, boundingRectangle));
            var angleSum = angle;
            var shift = 0;
            foreach (var (str, value) in data)
            {
                var asAngle = value / 100 * 360;
                var currentAngle = angleSum;
                angleSum += asAngle;
                // Нужен ли контур?
                ChartWindow.ToPaint.Enqueue(g =>
                    g.DrawPie(pen, boundingRectangle, (float) currentAngle, (float) asAngle));
                var color = Color.FromArgb(rnd.Next(5, 250), rnd.Next(5, 250), rnd.Next(5, 250));
                ChartWindow.ToPaint.Enqueue(g => g.FillPie(new SolidBrush(color),
                    boundingRectangle, (float) currentAngle, (float) asAngle));
                var shift1 = shift;
                ChartWindow.ToPaint.Enqueue(g => g.DrawRectangle(pen, new Rectangle(WindowInfo.ScreenSize.Width - 300, 50 + shift1, RectangleSize.Width, RectangleSize.Height)));
                ChartWindow.ToPaint.Enqueue(g => g.FillRectangle(new SolidBrush(color), new Rectangle(WindowInfo.ScreenSize.Width - 300, 50 + shift1, RectangleSize.Width, RectangleSize.Height)));
                ChartWindow.ToPaint.Enqueue(g => g.DrawString($"{str}: {Math.Round(value, 3)}%", new Font("Arial", 10, FontStyle.Regular), new SolidBrush(color), WindowInfo.ScreenSize.Width - 290 + RectangleSize.Width, 44 + shift1 + RectangleSize.Height / 2));
                shift += 50;
            }
        }
    }
}