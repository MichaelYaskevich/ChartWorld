using System.Collections.Generic;
using System.Linq;

namespace ChartWorld.Statistic
{
    public class ChartData : IChartData
    {
        private Dictionary<string, double> Dictionary { get; } = new();
        private List<string> Keys { get; } = new();

        public ChartData(IEnumerable<(string, double)> items)
        {
            foreach (var (key, value) in items)
                TryAdd(key, value);
        }

        public ChartData(string csvPath) : this(CsvParserForChartData.Parse(csvPath)) { }

        public IEnumerable<(string, double)> GetOrderedItems() 
            => Keys.Select(key => (key, Dictionary[key]));

        public IEnumerable<string> GetOrderedKeys() 
            => Keys;

        public IEnumerable<double> GetOrderedValues() 
            => Keys.Select(key => Dictionary[key]);

        public bool TryGetValue(string key, out double result)
        {
            if (Dictionary.ContainsKey(key))
            {
                result = Dictionary[key];
                return true;
            }

            result = default;
            return false;
        }

        public bool TryAdd(string key, double value)
        {
            if (Dictionary.ContainsKey(key))
                return false;
            Keys.Add(key);
            Dictionary.Add(key, value);
            return true;
        }

        public ChartData CreateChartDataWithValues(List<double> values)
        {
            return new ChartData(values
                .Select((v, i) => (Keys[i], v)));
        }
    }
}