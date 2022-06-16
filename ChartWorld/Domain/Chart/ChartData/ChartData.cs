using System;
using System.Collections.Generic;
using System.Linq;
using ChartWorld.Infrastructure;

namespace ChartWorld.Domain.Chart.ChartData
{
    public class ChartData : IChartData
    {
        private Dictionary<string, double> Dictionary { get; } = new();
        public List<string> Headers { get; }
        private List<string> Keys { get; } = new();

        public ChartData((List<string>, IEnumerable<(string, double)>) headersWithItems)
        {
            var (headers, items) = headersWithItems;
            Headers = headers;
            foreach (var (key, value) in items)
                TryAdd(key, value);
        }

        public double this[string key]
        {
            get
            {
                if (!Dictionary.ContainsKey(key))
                    throw new KeyNotFoundException("There is no such key in ChartData");
                return Dictionary[key];
            }
            set
            {
                if (!Dictionary.ContainsKey(key))
                    throw new KeyNotFoundException("There is no such key in ChartData");
                Dictionary[key] = value;
            }
        }

        public static ChartData Create(string path)
        {
            if (path.EndsWith(".xlsx"))
                return ParseAndCreate(new XlsxParser(), path);
            return path.EndsWith(".csv") ? ParseAndCreate(new CsvParser(), path) : null;
        }

        private static ChartData ParseAndCreate(IParser parser, string path)
        {
            try
            {
                var data = parser.Parse(path);
                return new ChartData(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

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
            return new ChartData((
                Headers,
                values.Select((v, i) => (Keys[i], v))
            ));
        }
    }
}