using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ChartWorld.Domain;
using ChartWorld.Domain.Chart;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.UI
{
    public static class Painter
    {
        public static readonly Size ScreenSize = Screen.PrimaryScreen.WorkingArea.Size;
        private static readonly Font DefaultFontForLetters = new("Arial", 10, FontStyle.Regular);
        private static readonly Font DefaultFontForNumbers = new("Arial", 16, FontStyle.Regular);
        private static readonly Brush BlackSolidBrush = new SolidBrush(Color.Black);
        private static readonly Brush BlackTransparentBrush = new SolidBrush(Color.FromArgb(128, Color.Black));
        private static readonly Pen DottedPen = new(BlackTransparentBrush, 2) {DashStyle = DashStyle.Dash};
        private static readonly Pen DefaultPen = new(Color.Black, 2);

        private static readonly StringFormat StringFormat = new()
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };

        private static Color[] _pieColors;
        private static Color[] _barColors;
        private const int BarChartMarkCount = 6;

        public static void Paint(IWorkspaceEntity entity, Form form)
        {
            switch (entity.Entity)
            {
                case BarChart chart:
                    PaintBarChart(chart, entity.Size, entity.Location);
                    break;
                case PieChart chart:
                    PaintPieChart(chart, entity.Size, entity.Location);
                    break;
                case string text:
                    PaintString(text, entity.Size, entity.Location);
                    break;
                default:
                    throw new ArgumentException($"Unexpected entity type: {entity.GetType()}", nameof(entity));
            }

            form.Invalidate();
        }

        private static void PaintString(string text, Size size, Point location)
        {
            ChartWindow.ToPaint.Enqueue(g =>
            {
                g.DrawString(text, DefaultFontForLetters, BlackSolidBrush,
                    location.X + 80, location.Y + 10, StringFormat);
            });
        }

        private static void PaintBarChart(BarChart chart, Size size, Point location)
        {
            var fullscreenChartSize = new Size(
                ScreenSize.Width - ScreenSize.Width / 10,
                ScreenSize.Height - 3 * ScreenSize.Height / 20
            );
            var chartTopLeft = new Point(location.X, location.Y);
            var chartStartPoint = new Point(location.X, location.Y + size.Height);
            var chartBottomRight = new Point(location.X + size.Width, location.Y + size.Height);
            var items = chart.Data.GetOrderedItems().ToList();
            var chartSize = new Size(chartBottomRight.X - chartStartPoint.X, chartStartPoint.Y - chartTopLeft.Y);
            var barWidth = chartSize.Width / items.Count / 3;
            var max = items.Max(x => x.Item2);
            var fontSizeMultiplier = chartSize.Width / (float) fullscreenChartSize.Width;
            EnqueueAxisDrawing(chart.Data.Headers, size.Width / 100, chartSize.Height / BarChartMarkCount,
                (int) max / BarChartMarkCount,
                chartStartPoint, chartBottomRight, chartTopLeft, fontSizeMultiplier);
            EnqueueBarsDrawing(items, SetColors(items.Count, ref _barColors), chartStartPoint.X + chartSize.Width / 20,
                max,
                barWidth, chartStartPoint, chartSize, fontSizeMultiplier);
        }

        private static void EnqueueAxisDrawing(string[] headers, int markWidth, int markShift, int markShiftValue,
            Point chartStart, Point chartBottomRight, Point chartTopLeft, float fontSizeMultiplier)
        {
            var font = new Font("Arial", DefaultFontForNumbers.Size * fontSizeMultiplier * 2);
            ChartWindow.ToPaint.Enqueue(g => g.DrawLine(DefaultPen, chartStart, chartBottomRight));
            ChartWindow.ToPaint.Enqueue(g => g.DrawLine(DefaultPen, chartStart, chartTopLeft));
            ChartWindow.ToPaint.Enqueue(g =>
            {
                var horizontalHeader = headers[0];
                var textSize = g.MeasureString(horizontalHeader, font);
                g.DrawString(horizontalHeader, font,
                    Brushes.Black,
                    new Point(chartBottomRight.X - (int) textSize.Width,
                        chartBottomRight.Y + (int) textSize.Height / 2));
            });
            var markValue = markShiftValue;
            for (var i = 1; i < BarChartMarkCount; i++)
            {
                var iValue = i;
                var valAsStr = markValue.ToString(CultureInfo.InvariantCulture);
                ChartWindow.ToPaint.Enqueue(g =>
                {
                    var textSize = g.MeasureString(valAsStr, font);
                    var yLocation = chartStart.Y - markShift * iValue;
                    g.DrawString(valAsStr, font, BlackSolidBrush,
                        new PointF(chartStart.X - markWidth - textSize.Width / 2, yLocation),
                        StringFormat);
                    g.DrawLine(DefaultPen,
                        new Point(chartStart.X - markWidth / 2, yLocation),
                        new Point(chartStart.X + markWidth / 2, yLocation));
                    var startPoint = new Point(chartStart.X + markWidth / 2 + 2, yLocation);
                    if (iValue == BarChartMarkCount)
                        startPoint.X = chartStart.X;
                    g.DrawLine(DottedPen, startPoint, new Point(chartBottomRight.X, yLocation));
                });
                markValue += markShiftValue;
            }
        }

        private static void EnqueueBarsDrawing(List<(string, double)> items, Color[] colors, int shift, double max,
            int barWidth, Point chartStart, Size chartSize, float fontSizeMultiplier)
        {
            var font = new Font("Arial", DefaultFontForLetters.Size * fontSizeMultiplier * 2);
            var numberFont = new Font("Arial", DefaultFontForNumbers.Size * fontSizeMultiplier * 2);
            for (var i = 0; i < items.Count; i++)
            {
                var (name, value) = items[i];
                var shiftValue = shift;
                var iValue = i;
                ChartWindow.ToPaint.Enqueue(g =>
                {
                    var barHeight = chartSize.Height * (value / max);
                    var bar = new Rectangle(shiftValue, chartStart.Y - (int) barHeight, barWidth, (int) barHeight);
                    g.DrawRectangle(DefaultPen, bar);
                    g.FillRectangle(new SolidBrush(colors[iValue]), bar);
                    var textSize = g.MeasureString(name, font);
                    g.DrawString(name, font, BlackSolidBrush,
                        new PointF(shiftValue + barWidth / 2, chartStart.Y + textSize.Height * 2), StringFormat);
                    var valAsStr = Math.Round(value, 2).ToString(CultureInfo.InvariantCulture);
                    textSize = g.MeasureString(valAsStr, numberFont);
                    g.DrawString(valAsStr, numberFont, BlackSolidBrush,
                        new PointF(shiftValue + barWidth / 2, chartStart.Y - textSize.Height - (float) barHeight),
                        StringFormat);
                });
                shift += barWidth + chartSize.Width / items.Count / 2;
            }
        }

        private static void PaintPieChart(PieChart chart, Size size, Point location)
        {
            var multiplier = ScreenSize.Width / (float) ScreenSize.Width / 2;
            var data = chart.PercentageData.GetOrderedItems();
            var circleSize = new Size(ScreenSize.Width / 2, ScreenSize.Height / 2);
            var boundingRectangle = new Rectangle(location, circleSize);
            EnqueuePieChartDrawing(data, boundingRectangle, size, location, multiplier);
        }

        private static void EnqueuePieChartDrawing(IEnumerable<(string, double)> data,
            Rectangle boundingRectangle, Size size, Point location, float multiplier)
        {
            ChartWindow.ToPaint.Enqueue(g =>
                g.DrawEllipse(DefaultPen, boundingRectangle));
            var angleSum = 0.0;
            var shift = 0;
            var dataAsArray = data.ToArray();
            var sum = dataAsArray.Sum(x => x.Item2);
            var colors = SetColors(dataAsArray.Length, ref _pieColors);
            var rectangleSize = new Size(size.Width / 20, size.Height / 20);
            var font = new Font("Arial", DefaultFontForLetters.Size * multiplier * 2);
            for (var i = 0; i < dataAsArray.Length; i++)
            {
                var (name, value) = dataAsArray[i];
                var iValue = i;
                var shiftValue = shift;
                var asAngle = value / sum * 360;
                var currentAngle = angleSum;
                angleSum += asAngle;
                ChartWindow.ToPaint.Enqueue(g =>
                {
                    var rectangle = new Rectangle(location.X + size.Width, location.Y + shiftValue, rectangleSize.Width,
                        rectangleSize.Height);
                    g.DrawPie(DefaultPen, boundingRectangle, (float) currentAngle, (float) asAngle);
                    g.FillPie(new SolidBrush(colors[iValue]), boundingRectangle, (float) currentAngle, (float) asAngle);
                    g.DrawRectangle(DefaultPen, rectangle);
                    g.FillRectangle(new SolidBrush(colors[iValue]), rectangle);
                    var info =
                        $"{name}: {Math.Round(value, 3).ToString(CultureInfo.InvariantCulture).Replace(',', '.')}%";
                    var textSize = g.MeasureString(info, font);
                    g.DrawString(info, font, BlackSolidBrush,
                        location.X + size.Width + rectangleSize.Width * 3 / 2 + textSize.Width / 2,
                        location.Y + shiftValue + rectangleSize.Height / 2, StringFormat);
                });
                shift += rectangleSize.Height / 2 * 3;
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