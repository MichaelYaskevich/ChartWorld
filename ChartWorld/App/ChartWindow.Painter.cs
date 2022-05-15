using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ChartWorld.Chart;
using ChartWorld.Workspace;

namespace ChartWorld.App
{
    public static class Painter
    {
        private const double Radian = Math.PI / 180;
        private static readonly Size RectangleSize = new Size(30, 30);
        private static readonly Font DefaultFont = new Font("Arial", 10, FontStyle.Regular);
        private static readonly StringFormat Sf = new StringFormat();

        static Painter()
        {
            Sf.LineAlignment = StringAlignment.Center;
            Sf.Alignment = StringAlignment.Center;
        }

        public static void Paint(IWorkspaceEntity entity, Form form)
        {
            switch (entity.Entity)
            {
                case BarChart chart:
                    PaintBarChart(chart, form, entity.Size, entity.Location);
                    break;
                case PieChart chart:
                    PaintPieChart(chart, form, entity.Size, entity.Location);
                    break;
                default:
                    throw new ArgumentException($"Unexpected entity type: {entity.GetType()}", nameof(entity));
            }
        }

        private static void PaintBarChart(BarChart chart, Control form, Size size, Point location)
        {
            var rnd = new Random();
            // var height = WindowInfo.ScreenSize.Height;
            var (width, height) = (size.Width, size.Height);
            // var chartBottomRight = new Point(width - width / 20, height - height / 10);
            // var chartTopLeft = new Point(width / 20, height / 20);
            var chartTopLeft = location;
            var chartBottomRight = new Point(location.X + width, location.Y + height);
            // var chartStart = new Point(location.X + width / 20, location.Y + height - height / 10);
            var chartStart = new Point(location.X, location.Y + height);
            var pen = new Pen(Color.Black);
            pen.CustomEndCap = new AdjustableArrowCap(5, 5);
            pen.Width = 2;
            ChartWindow.ToPaint.Enqueue(g => g.DrawLine(pen, chartStart, chartBottomRight));
            ChartWindow.ToPaint.Enqueue(g => g.DrawLine(pen, chartStart, chartTopLeft));
            // Плейсхолдер
            ChartWindow.ToPaint.Enqueue(g => g.DrawString("nothing", new Font("Arial", 20, FontStyle.Regular),
                Brushes.Black,
                new Point(chartBottomRight.X - 100, chartBottomRight.Y + 15)));
            var items = chart.Data.GetOrderedItems().ToList();
            var chartHeight = chartStart.Y - chartTopLeft.Y;
            var chartWidth = chartBottomRight.X - chartStart.X;
            var barWidth = chartWidth / items.Count / 3;
            var max = items.Max(x => x.Item2);
            var min = items.Min(x => x.Item2);
            var shift = chartStart.X + 50;

            foreach (var (name, value) in items)
            {
                var shift1 = shift;
                ChartWindow.ToPaint.Enqueue(g => g.DrawRectangle(pen,
                    new Rectangle(shift1, chartTopLeft.Y + chartHeight - (int) (chartHeight * (value / max)), barWidth,
                        (int) (chartHeight * (value / max)))));
                var color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                ChartWindow.ToPaint.Enqueue(g => g.FillRectangle(new SolidBrush(color),
                    new Rectangle(shift1, chartTopLeft.Y + chartHeight - (int) (chartHeight * (value / max)), barWidth,
                        (int) (chartHeight * (value / max)))));
                ChartWindow.ToPaint.Enqueue(g =>
                {
                    var stringSize = g.MeasureString(name, DefaultFont);
                    g.DrawString(name, DefaultFont, new SolidBrush(Color.Black),
                        new PointF(shift1 + barWidth / 2, chartStart.Y + stringSize.Height * 2), Sf);
                });
                shift += barWidth + chartWidth / items.Count / 2;
            }

            form.Invalidate();
        }

        private static void PaintPieChart(PieChart chart, Control form, Size size, Point location)
        {
            var (width, height) = (size.Width, size.Height);
            var data = chart.Data.GetOrderedItems();
            var pen = new Pen(Color.Black);
            pen.Width = 2;
            var circleSize = new Size(height / 2, height / 2);
            var boundingRectangle = new Rectangle(location, circleSize);
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
                ChartWindow.ToPaint.Enqueue(g =>
                    g.DrawPie(pen, boundingRectangle, (float) currentAngle, (float) asAngle));
                var color = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                ChartWindow.ToPaint.Enqueue(g => g.FillPie(new SolidBrush(color),
                    boundingRectangle, (float) currentAngle, (float) asAngle));
                var shift1 = shift;
                ChartWindow.ToPaint.Enqueue(g => g.DrawRectangle(pen,
                    new Rectangle(WindowInfo.ScreenSize.Width - 300, 50 + shift1, RectangleSize.Width,
                        RectangleSize.Height)));
                ChartWindow.ToPaint.Enqueue(g => g.FillRectangle(new SolidBrush(color),
                    new Rectangle(WindowInfo.ScreenSize.Width - 300, 50 + shift1, RectangleSize.Width,
                        RectangleSize.Height)));
                ChartWindow.ToPaint.Enqueue(g =>
                    g.DrawString(
                        $"{str}:  {Math.Round(value, 3).ToString(CultureInfo.InvariantCulture).Replace(',', '.')}%",
                        DefaultFont, new SolidBrush(Color.Black),
                        WindowInfo.ScreenSize.Width - 290 + RectangleSize.Width,
                        44 + shift1 + RectangleSize.Height / 2));
                shift += 50;
            }
        }
    }
}