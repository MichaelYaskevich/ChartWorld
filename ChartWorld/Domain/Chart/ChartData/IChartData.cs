using System.Collections.Generic;

namespace ChartWorld.Domain.Chart.ChartData
{
    public interface IChartData
    {
        IEnumerable<(string, double)> GetOrderedItems();
        string[] Headers { get; }
        IEnumerable<string> GetOrderedKeys();
        IEnumerable<double> GetOrderedValues();
        bool TryGetValue(string key, out double result);
        bool TryAdd(string key, double value);
        ChartData CreateChartDataWithValues(List<double> values);
    }
}