using System.Drawing;

namespace ChartWorld.Workspace
{
    public class Line
    {
        public double k { get; }
        public double b { get; }
        public Line(Point first, Point second)
        {
            var denominator = first.X - second.X;
            if (denominator == 0)
                k = 100000;
            else
                k = (first.Y - second.Y) / denominator;
            b = first.Y - k * first.X;
        }
    }
}