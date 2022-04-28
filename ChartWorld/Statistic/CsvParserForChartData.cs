using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using CsvHelper;

namespace ChartWorld.Statistic
{
    public static class CsvParserForChartData
    {
        public static IEnumerable<(string, double)> Parse(string csvPath)
        {
            using var csvReader = new StreamReader(GetStream(csvPath));
            using var csv = new CsvReader(csvReader, CultureInfo.InvariantCulture);
            
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                var columnCount = csv.HeaderRecord.Length;
                for (var i = 1; i < columnCount; i++)
                {
                    var keySuffix = columnCount > 2 ? $"#{i}" : "";
                    yield return ($"{csv.GetField(csv.HeaderRecord[0])}{keySuffix}",
                        Convert.ToDouble(csv.GetField(csv.HeaderRecord[i])));
                }
            }
        }

        private static Stream GetStream(string path)
        {
            return Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream(path);
        }
    }
}