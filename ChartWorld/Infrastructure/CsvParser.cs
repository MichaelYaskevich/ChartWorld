using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using CsvHelper;

namespace ChartWorld.Infrastructure
{
    public static class CsvParser
    {
        public static (string[], IEnumerable<(string, double)>) Parse(string csvPath)
        {
            using var csvReader = new StreamReader(csvPath);
            using var csv = new CsvReader(csvReader, CultureInfo.InvariantCulture, true);
            csv.Read();
            csv.ReadHeader();
            var headers = csv.HeaderRecord;

            var result = (headers, ParseFields(csvPath));
            return result;
        }

        private static IEnumerable<(string, double)> ParseFields(string csvPath)
        {
            using var csvReader = new StreamReader(csvPath);
            using var csv = new CsvReader(csvReader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();
            var columnCount = csv.HeaderRecord.Length;
            while (csv.Read())
            {
                for (var i = 1; i < columnCount; i++)
                {
                    var keySuffix = columnCount > 2 ? $"#{i}" : "";
                    yield return ($"{csv.GetField(csv.HeaderRecord[0])}{keySuffix}",
                        Convert.ToDouble(csv.GetField(csv.HeaderRecord[i])));
                }
            }
        }

        // private static Stream GetStream(string path)
        // {
        //     return Assembly
        //         .GetExecutingAssembly()
        //         .GetManifestResourceStream(path);
        // }
    }
}