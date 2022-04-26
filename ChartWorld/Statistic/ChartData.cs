using System.Collections.Generic;
using System.Linq;

namespace ChartWorld.Statistic
{
    public class ChartData
    {
        private Dictionary<string, double> Dictionary { get; } = new();
        private List<string> Keys { get; } = new();

        public IEnumerable<(string, double)> GetItems()
        {
            return Keys.Select(key => (key, Dictionary[key]));
        }

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