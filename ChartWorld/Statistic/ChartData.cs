using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace ChartWorld.Statistic
{
    public class ChartData
    {
        private Dictionary<string, double> Dictionary { get; } = new();
        private List<string> Keys { get; } = new();
        
        public ChartData(Stream csvStream)
        {
            using (var csvReader = new StreamReader(csvStream))
            using (var csv = new CsvReader(csvReader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var columnCount = csv.HeaderRecord.Length;
                    for (var i = 1; i < columnCount; i++)
                    {
                        var keySuffix = columnCount > 2 ? $"#{i}" : "";
                        TryAdd(
                            $"{csv.GetField(csv.HeaderRecord[0])}{keySuffix}", 
                            Convert.ToDouble(csv.GetField(csv.HeaderRecord[i])));
                    }
                }
            }
        }

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