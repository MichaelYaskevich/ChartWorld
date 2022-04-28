using System;
using System.Collections.Generic;
using System.Linq;

namespace ChartWorld.Statistic
{
    public static class StatisticProvider
    {
        public static IEnumerable<(string, double)> GetItemsWithMaxValue(IChartData data)
        {
            return data.GetValues().Any()
                ? GetItemsWithSameValue(data, data.GetValues().Max())
                : Enumerable.Empty<(string, double)>();
        }
        
        public static IEnumerable<(string, double)> GetItemsWithMinValue(IChartData data)
        {
            return data.GetValues().Any()
                ? GetItemsWithSameValue(data, data.GetValues().Min())
                : Enumerable.Empty<(string, double)>();
        }
        
        public static IEnumerable<(string, double)> GetItemsWithSameValue(IChartData data, double value)
        {
            return data.GetItems()
                .Where(item => Math.Abs(value - item.Item2) < 0.000001);
        }
        
        public static bool TryGetMedian(IChartData data, out double median)
        {
            median = 0;
            if (!data.GetValues().Any()) 
                return false;

            median = GetMedian(data);
            return true;
        }
        
        private static double GetMedian(IChartData data)
        {
            var list = data.GetValues().OrderBy(z => z).ToList();
            return list.Count % 2 == 0 
                ? (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2 
                : list[list.Count / 2];
        }
        
        public static bool TryGetMean(IChartData data, out double mean)
        {
            mean = 0;
            if (!data.GetValues().Any())
                return false;

            mean = data.GetValues().Average();
            return true;
        }
        
        public static bool TryGetStd(IChartData data, out double std)
        {
            std = 0;
            if (!data.GetValues().Any())
                return false;

            std = GetStd(data);
            return true;
        }

        private static double GetStd(IChartData data)
        {
            var list = data.GetValues().ToList();
            var mean = list.Average();

            return Math.Sqrt(list
                .Select(z => Math.Pow(z - mean, 2))
                .Sum() / (list.Count - 1));
        }

        public static IChartData Abs(IChartData data)
        {
            return new ChartData(data.GetItems()
                    .Select(item => (item.Item1, Math.Abs(item.Item2))));
        }

        public static double Autocorrelation(List<double> series, int lag = 1)
        {
            var shiftedSeries = series.Take(series.Count-lag).ToList();
            shiftedSeries.AddRange(series.Skip(lag));
            
            return GetPearsonCoeff(series, shiftedSeries);
        }
        
        private static double GetPearsonCoeff(List<double> x, List<double> y)
        {
            var n = x.Count;
            var num = 0.0;
            var den1 = 0.0;
            var den2 = 0.0;
            var meanX = x.Average();
            var meanY = y.Average();
            
            for (var i = 0; i < n; i++)
            {
                num += (x[i] - meanX) * (y[i] - meanY);
                den1 += Math.Pow(x[i] - meanX, 2);
                den2 += Math.Pow(y[i] - meanY, 2);
            }
            
            return num / Math.Sqrt(den1 * den2);
        }
    }
}