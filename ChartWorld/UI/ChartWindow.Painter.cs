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
        private static readonly Font DefaultFont = new("Arial", 10, FontStyle.Regular);
        private static readonly Font DefaultFontForNumbers = new("Arial", 16, FontStyle.Regular);
        private static readonly Brush BlackSolidBrush = new SolidBrush(Color.Black);
        private static readonly Pen MarkPen = new(Color.Black, 2);
        private static readonly Pen DottedPen = new(new SolidBrush(Color.FromArgb(128, Color.Black)), 2);
        private static readonly Pen DefaultPen = new(Color.Black, 2);
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
            var width = WindowInfo.ScreenSize.Width;
            var height = WindowInfo.ScreenSize.Height;
            var fullscreenChartSize = new Size(
                width - width / 10,
                height - 3 * height / 20
            );
            var chartTopLeft = new Point(location.X, location.Y);
            var chartStart = new Point(location.X, location.Y + size.Height);
            var chartBottomRight = new Point(location.X + size.Width, location.Y + size.Height);
            var items = chart.Data.GetOrderedItems().ToList();
            var chartSize = new Size(chartBottomRight.X - chartStart.X, chartStart.Y - chartTopLeft.Y);
            var barWidth = chartSize.Width / items.Count / 3;
            var max = items.Max(x => x.Item2);
            var fontSizeMultiplier = chartSize.Width / (float) fullscreenChartSize.Width;
            EnqueueAxisDrawing(chart.Data.Headers, size.Width / 100, chartSize.Height / MarkCount,
                (int) max / MarkCount,
                chartStart, chartBottomRight, chartTopLeft, fontSizeMultiplier);
            EnqueueBarsDrawing(items, SetColors(items.Count, ref _barColors), chartStart.X + chartSize.Width / 20, max,
                barWidth, chartStart, chartSize, fontSizeMultiplier);
            form.Invalidate();
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
            for (var i = 1; i < MarkCount; i++)
            {
                var iValue = i;
                var value = markValue;
                ChartWindow.ToPaint.Enqueue(g =>
                {
                    var valAsStr = value.ToString(CultureInfo.InvariantCulture);
                    var textSize = g.MeasureString(valAsStr, font);
                    g.DrawString(valAsStr, font, BlackSolidBrush,
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
            int barWidth, Point chartStart, Size chartSize, float fontSizeMultiplier)
        {
            var font = new Font("Arial", DefaultFont.Size * fontSizeMultiplier * 2);
            var numberFont = new Font("Arial", DefaultFontForNumbers.Size * fontSizeMultiplier * 2);
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
                    var textSize = g.MeasureString(name, font);
                    g.DrawString(name, font, BlackSolidBrush,
                        new PointF(shiftValue + barWidth / 2, chartStart.Y + textSize.Height * 2), Sf);
                    var valAsStr = Math.Round(value, 2).ToString(CultureInfo.InvariantCulture);
                    textSize = g.MeasureString(valAsStr, numberFont);
                    g.DrawString(valAsStr, numberFont, BlackSolidBrush,
                        new PointF(shiftValue + barWidth / 2,
                            chartStart.Y - textSize.Height - (float) (chartSize.Height * (value / max))), Sf);
                });
                shift += barWidth + chartSize.Width / items.Count / 2;
            }
        }

        private static void PaintPieChart(PieChart chart, Control form, Size size, Point location)
        {
            var fullscreenChartSize = new Size(WindowInfo.ScreenSize.Width / 2, WindowInfo.ScreenSize.Height / 2);
            var width = size.Width;
            var height = size.Height;
            var multiplier = width / (float) fullscreenChartSize.Width;
            var data = chart.PercentageData.GetOrderedItems();
            var circleSize = new Size(width / 2, height / 2);
            var boundingRectangle = new Rectangle(location, circleSize);
            EnqueuePieChartDrawing(data, boundingRectangle, size, location, multiplier);
            form.Invalidate();
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
            var font = new Font("Arial", DefaultFont.Size * multiplier * 2);
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
                    g.DrawPie(DefaultPen, boundingRectangle, (float) currentAngle, (float) asAngle);
                    g.FillPie(new SolidBrush(colors[iValue]), boundingRectangle, (float) currentAngle, (float) asAngle);
                    g.DrawRectangle(DefaultPen,
                        new Rectangle(location.X + size.Width, location.Y + shiftValue, rectangleSize.Width,
                            rectangleSize.Height));
                    g.FillRectangle(new SolidBrush(colors[iValue]),
                        new Rectangle(location.X + size.Width, location.Y + shiftValue, rectangleSize.Width,
                            rectangleSize.Height));
                    var info =
                        $"{name}:  {Math.Round(value, 3).ToString(CultureInfo.InvariantCulture).Replace(',', '.')}%";
                    var textSize = g.MeasureString(info, font);
                    g.DrawString(
                        info,
                        font, BlackSolidBrush,
                        location.X + size.Width + rectangleSize.Width * 3 / 2 + textSize.Width / 2,
                        location.Y + shiftValue + rectangleSize.Height / 2, Sf);
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