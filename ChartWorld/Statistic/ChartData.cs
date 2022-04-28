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

        public IEnumerable<(string, double)> GetItems() 
            => Keys.Select(key => (key, Dictionary[key]));

        public IEnumerable<string> GetKeys() 
            => Keys;

        public IEnumerable<double> GetValues() 
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
    }
}