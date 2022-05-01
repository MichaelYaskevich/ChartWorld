using System;
using System.Collections.Generic;
using System.Linq;

namespace ChartWorld.Statistic
{
    public static class StatisticProvider
    {
        public static bool TryGetMedian(List<double> list, out double median)
        {
            median = 0;
            if (!list.Any())
                return false;

            median = GetMedian(list);
            return true;
        }

        private static double GetMedian(List<double> list)
        {
            return list.Count % 2 == 0
                ? (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2
                : list[list.Count / 2];
        }

        public static bool TryGetMean(List<double> list, out double mean)
        {
            mean = 0;
            if (!list.Any())
                return false;

            mean = list.Average();
            return true;
        }

        public static bool TryGetStd(List<double> list, out double std)
        {
            std = 0;
            if (!list.Any())
                return false;

            std = GetStd(list);

            return true;
        }

        private static double GetStd(List<double> list)
        {
            var mean = list.Average();
            return Math.Sqrt(list
                .Select(z => Math.Pow(z - mean, 2))
                .Sum() / (list.Count - 1));
        }

        public static double GetAutocorrelation(List<double> series, int lag = 1)
        {
            return GetCorrCoef(
                series.Take(series.Count - lag).ToList(),
                series.Skip(lag).ToList());
        }

        public static double GetCorrCoef(List<double> x, List<double> y)
        {
            var n = x.Count;
            var averageXY = 0.0;
            var varianceX = 0.0;
            var varianceY = 0.0;
            var averageX = x.Average();
            var averageY = y.Average();

            for (var i = 0; i < n; i++)
            {
                averageXY += x[i] * y[i];
                varianceX += Math.Pow(x[i], 2);
                varianceY += Math.Pow(y[i], 2);
            }

            averageXY /= n;
            varianceX /= n;
            varianceX -= averageX * averageX;
            varianceY /= n;
            varianceY -= averageY * averageY;

            return (averageXY - averageX * averageY) / Math.Sqrt(varianceX * varianceY);
        }

        public static IEnumerable<bool> GetBetweenStat(IEnumerable<double> list, double left, double right)
        {
            return list.Select(x => left < x + 0.000001 && x - 0.000001 < right);
        }

        public static IEnumerable<double> Clip(IEnumerable<double> list, double left, double right)
        {
            return list.Select(x => x > right
                ? right
                : x < left
                    ? left
                    : x);
        }

        public static bool TryClip(double[] list, double[] left, double[] right, out List<double> result)
        {
            result = new List<double>();
            if (list.Length != left.Length || list.Length != right.Length)
                return false;

            result = list.Select((x, i) => x > right[i]
                    ? right[i]
                    : x < left[i]
                        ? left[i]
                        : x)
                .ToList();

            return true;
        }

        public static bool TryGetExpectation(List<double> x, List<double> y, out double expectation)
        {
            expectation = 0;
            var one = y.Sum();
            if (one + 0.000001 < 1 || one - 0.000001 > 1)
                return false;

            expectation = x.Select((z, i) => z * y[i]).Sum();

            return true;
        }

        public static double GetExpectation(List<double> x)
        {
            var factor = 1.0 / x.Count;
            return x.Select((z, i) => z * factor).Sum();
        }

        public static double GetCovariance(List<double> x, List<double> y)
        {
            var n = x.Count;
            var eX = GetExpectation(x);
            var eY = GetExpectation(y);

            var sum = 0.0;
            for (int i = 0; i < n; i++)
                sum += (x[i] - eX) * (y[i] - eY);
            sum /= n - 1;
            
            return sum;
        }

        private static IEnumerable<double> GetCumulative(
            Func<double, double, double> findNewBest, IEnumerable<double> x, double currentBest)
        {
            foreach (var value in x)
            {
                currentBest = findNewBest(currentBest, value);
                yield return currentBest;
            }
        }

        public static IEnumerable<double> GetCummax(IEnumerable<double> x)
        {
            return GetCumulative(Math.Max, x, double.MinValue);
        }

        public static IEnumerable<double> GetCummin(IEnumerable<double> x)
        {
            return GetCumulative(Math.Min, x, double.MaxValue);
        }

        public static IEnumerable<double> GetCumprod(IEnumerable<double> x)
        {
            return GetCumulative((y, z) => y * z, x, 1);
        }
        
        public static IEnumerable<double> GetCumsum(IEnumerable<double> x)
        {
            return GetCumulative((y, z) => y + z, x, 0);
        }
    }
}