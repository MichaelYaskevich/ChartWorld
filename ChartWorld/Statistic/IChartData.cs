using System.Collections.Generic;

namespace ChartWorld.Statistic
{
    public interface IChartData
    {
        IEnumerable<(string, double)> GetItems();
        IEnumerable<string> GetKeys();
        IEnumerable<double> GetValues();
        bool TryGetValue(string key, out double result);
        bool TryAdd(string key, double value);
    }
}