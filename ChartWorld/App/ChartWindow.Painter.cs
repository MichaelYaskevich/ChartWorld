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
        private static readonly Size RectangleSize = new(30, 30);
        private static readonly Font DefaultFont = new("Arial", 10, FontStyle.Regular);
        private static readonly Font DefaultFontForNumbers = new("Arial", 16, FontStyle.Regular);
        private static readonly Brush BlackSolidBrush = new SolidBrush(Color.Black);
        private static readonly Pen MarkPen = new(Color.Black, 2);
        private static readonly Pen DottedPen = new(new SolidBrush(Color.FromArgb(128, Color.Black)), 2);
        private static readonly Pen DefaultPen = new(Color.Black, 2);
        private static readonly ColorGenerator ColorGenerator = new();
        private static readonly StringFormat Sf = new();
        private static Color[] _pieColors;
        private static Color[] _barColors;
        private const int MarkCount = 6;

        static Painter()
        {
            Sf.LineAlignment = StringAlignment.Center;
            Sf.Alignment = StringAlignment.Center;
            DottedPen.DashStyle = DashStyle.Dash;
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

        private static void PaintBarChart(BarChart chart, Form form, Size size, Point location)
        {
            var width = size.Width;
            var height = size.Height;
            var widthRatio = width / (double) WindowInfo.ScreenSize.Width;
            var heightRatio = height / (double) WindowInfo.ScreenSize.Height;
            var chartBottomRight = new Point(width - width / 20, height - height / 10);
            var chartTopLeft = new Point(width / 20, height / 20);
            var chartStart = new Point(width / 20, height - height / 10);
            var items = chart.Data.GetOrderedItems().ToList();
            var chartSize = new Size(chartBottomRight.X - chartStart.X, chartStart.Y - chartTopLeft.Y);
            var barWidth = chartSize.Width / items.Count / 3;
            var max = items.Max(x => x.Item2);
            EnqueueAxisDrawing(size.Width / 100, chartSize.Height / MarkCount, (int) max / MarkCount,
                chartStart, chartBottomRight, chartTopLeft, size);
            EnqueueBarsDrawing(items, SetColors(items.Count, ref _barColors), chartStart.X + chartSize.Width / 20, max,
                barWidth, chartStart, chartSize);
            form.Invalidate();
        }

        private static void EnqueueAxisDrawing(int markWidth, int markShift, int markShiftValue,
            Point chartStart, Point chartBottomRight, Point chartTopLeft, Size size)
        {
            ChartWindow.ToPaint.Enqueue(g => g.DrawLine(DefaultPen, chartStart, chartBottomRight));
            ChartWindow.ToPaint.Enqueue(g => g.DrawLine(DefaultPen, chartStart, chartTopLeft));
            ChartWindow.ToPaint.Enqueue(g =>
            {
                // Плейсхолдер
                var str = "nothing";
                var font = new Font("Arial", 20, FontStyle.Regular);
                var strSize = g.MeasureString(str, font);
                g.DrawString(str, font,
                    Brushes.Black,
                    new Point(chartBottomRight.X - 100, chartBottomRight.Y + (int) strSize.Height));
            });
            var markValue = markShiftValue;
            for (var i = 1; i < MarkCount; i++)
            {
                var iValue = i;
                var value = markValue;
                ChartWindow.ToPaint.Enqueue(g =>
                {
                    var valAsStr = value.ToString(CultureInfo.InvariantCulture);
                    var textSize = g.MeasureString(valAsStr, DefaultFontForNumbers);
                    g.DrawString(valAsStr, DefaultFontForNumbers, BlackSolidBrush,
                        new PointF(chartStart.X - markWidth - textSize.Width / 2, chartStart.Y - markShift * iValue),
                        Sf);
                    g.DrawLine(MarkPen,
                        new Point(chartStart.X - markWidth / 2, chartStart.Y - markShift * iValue),
                        new Point(chartStart.X + markWidth / 2, chartStart.Y - markShift * iValue));
                    var startPoint = new Point(chartStart.X + markWidth / 2 + 2, chartStart.Y - markShift * iValue);
                    if (iValue == MarkCount)
                        startPoint.X = chartStart.X;
                    g.DrawLine(DottedPen, startPoint, new Point(chartBottomRight.X, chartStart.Y - markShift * iValue));
                });
                markValue += markShiftValue;
            }
        }

        private static void EnqueueBarsDrawing(List<(string, double)> items, Color[] colors, int shift, double max,
            int barWidth, Point chartStart, Size chartSize)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var (name, value) = items[i];
                var shiftValue = shift;
                var iValue = i;
                ChartWindow.ToPaint.Enqueue(g =>
                {
                    g.DrawRectangle(DefaultPen,
                        new Rectangle(shiftValue, chartStart.Y - (int) (chartSize.Height * (value / max)), barWidth,
                            (int) (chartSize.Height * (value / max))));
                    g.FillRectangle(new SolidBrush(colors[iValue]),
                        new Rectangle(shiftValue, chartStart.Y - (int) (chartSize.Height * (value / max)), barWidth,
                            (int) (chartSize.Height * (value / max))));
                    var textSize = g.MeasureString(name, DefaultFont);
                    g.DrawString(name, DefaultFont, BlackSolidBrush,
                        new PointF(shiftValue + barWidth / 2, chartStart.Y + textSize.Height * 2), Sf);
                    var valAsStr = Math.Round(value, 2).ToString(CultureInfo.InvariantCulture);
                    textSize = g.MeasureString(valAsStr, DefaultFontForNumbers);
                    g.DrawString(valAsStr, DefaultFontForNumbers, BlackSolidBrush,
                        new PointF(shiftValue + barWidth / 2,
                            chartStart.Y - textSize.Height - (float) (chartSize.Height * (value / max))), Sf);
                });
                shift += barWidth + chartSize.Width / items.Count / 2;
            }
        }

        private static void PaintPieChart(PieChart chart, Control form, Size size, Point location)
        {
            var width = size.Width;
            var height = size.Height;
            var data = chart.PercentageData.GetOrderedItems();
            var circleSize = new Size(height / 2, height / 2);
            var boundingRectangle = new Rectangle(location, circleSize);
            EnqueuePieChartDrawing(data, boundingRectangle, size);
            form.Invalidate();
        }

        private static void EnqueuePieChartDrawing(IEnumerable<(string, double)> data,
            Rectangle boundingRectangle, Size size)
        {
            ChartWindow.ToPaint.Enqueue(g =>
                g.DrawEllipse(DefaultPen, boundingRectangle));
            var angleSum = 0.0;
            var shift = 0;
            var dataAsArray = data.ToArray();
            var colors = SetColors(dataAsArray.Length, ref _pieColors);
            for (var i = 0; i < dataAsArray.Length; i++)
            {
                var (name, value) = dataAsArray[i];
                var iValue = i;
                var shiftValue = shift;
                var asAngle = value / 100 * 360;
                var currentAngle = angleSum;
                angleSum += asAngle;
                ChartWindow.ToPaint.Enqueue(g =>
                {
                    g.DrawPie(DefaultPen, boundingRectangle, (float) currentAngle, (float) asAngle);
                    g.FillPie(new SolidBrush(colors[iValue]), boundingRectangle, (float) currentAngle, (float) asAngle);
                    g.DrawRectangle(DefaultPen,
                        new Rectangle(size.Width - 300, 50 + shiftValue, RectangleSize.Width,
                            RectangleSize.Height));
                    g.FillRectangle(new SolidBrush(colors[iValue]),
                        new Rectangle(size.Width - 300, 50 + shiftValue, RectangleSize.Width,
                            RectangleSize.Height));
                    g.DrawString(
                        $"{name}:  {Math.Round(value, 3).ToString(CultureInfo.InvariantCulture).Replace(',', '.')}%",
                        DefaultFont, BlackSolidBrush,
                        size.Width - 290 + RectangleSize.Width,
                        44 + shiftValue + RectangleSize.Height / 2);
                });
                shift += 50;
            }
        }

        private static Color[] SetColors(int count, ref Color[] savedColors)
        {
            if (savedColors is null || savedColors.Length != count)
            {
                savedColors = ColorGenerator.GetColors(count);
            }

            return savedColors;
        }
    }
}