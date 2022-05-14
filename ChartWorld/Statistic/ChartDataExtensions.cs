using System;
using System.Collections.Generic;
using System.Linq;

namespace ChartWorld.Statistic
{
    public static class ChartDataExtensions
    {
        public static IEnumerable<(string, double)> GetItemsWithMaxValue(this IChartData data) 
            => data.GetOrderedValues().Any()
                ? GetItemsWithSameValue(data, data.GetOrderedValues().Max())
                : Enumerable.Empty<(string, double)>();

        public static IEnumerable<(string, double)> GetItemsWithMinValue(this IChartData data) 
            => data.GetOrderedValues().Any()
                ? GetItemsWithSameValue(data, data.GetOrderedValues().Min())
                : Enumerable.Empty<(string, double)>();

        public static IEnumerable<(string, double)> GetItemsWithSameValue(this IChartData data, double value) 
            => data.GetOrderedItems()
                .Where(item => Math.Abs(value - item.Item2) < 0.000001);

        public static bool TryGetMedian(this IChartData data, out double median) 
            => StatisticProvider.TryGetMedian(data.GetOrderedValues().ToList(), out median);

        public static bool TryGetMean(this IChartData data, out double mean) 
            => StatisticProvider.TryGetMean(data.GetOrderedValues().ToList(), out mean);

        public static bool TryGetStd(this IChartData data, out double std) 
            => StatisticProvider.TryGetStd(data.GetOrderedValues().ToList(), out std);

        public static IChartData Abs(this IChartData data) 
            => new ChartData(data.GetOrderedItems()
                .Select(item => (item.Item1, Math.Abs(item.Item2))));

        public static double Autocorrelation(this ChartData chartData, int lag = 1) 
            => StatisticProvider.GetAutocorrelation(chartData.GetOrderedValues().ToList(), lag);

        public static ChartData Clip(this IChartData chartData, double left, double right) 
            => chartData.CreateChartDataWithValues(
                StatisticProvider.Clip(chartData.GetOrderedValues(), left, right)
                    .ToList());

        public static bool TryClip(this IChartData chartData, double[] left, double[] right, out ChartData newChartData)
        {
            newChartData = null;
            if (!StatisticProvider.TryClip(
                    chartData.GetOrderedValues().ToArray(),
                    left,
                    right,
                    out var newValues))
                return false;

            newChartData = chartData.CreateChartDataWithValues(newValues);
            
            return true;
        }
        
        public static double GetExpectation(this IChartData chartData) 
            => StatisticProvider.GetExpectation(chartData.GetOrderedValues().ToList());

        public static ChartData GetCumulativeMax(this IChartData chartData) 
            => GetCumulative(chartData, StatisticProvider.GetCumulativeMax);
        
        public static ChartData GetCumulativeMin(this IChartData chartData) 
            => GetCumulative(chartData, StatisticProvider.GetCumulativeMin);
        
        public static ChartData GetCumulativeProd(this IChartData chartData) 
            => GetCumulative(chartData, StatisticProvider.GetCumulativeProd);
        
        public static ChartData GetCumulativeSum(this IChartData chartData) 
            => GetCumulative(chartData, StatisticProvider.GetCumulativeSum);

        private static ChartData GetCumulative(this IChartData chartData, Func<IEnumerable<double>, IEnumerable<double>> cumFunc) 
            => chartData.CreateChartDataWithValues(
                cumFunc(chartData.GetOrderedValues())
                    .ToList());
    }
}