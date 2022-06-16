using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Exceptions;


namespace ChartWorld.Infrastructure
{
    public class XlsxParser : IParser
    {
        public XlsxParser()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public (List<string>, IEnumerable<(string, double)>) Parse(string xlsxPath)
        {
            var first = true;
            var headers = new List<string>();
            var result = GetRows(xlsxPath)
                .Select(fields =>
                {
                    var flattenLines = new List<(string, string)>();
                    if (first)
                    {
                        first = false;
                        headers.AddRange(fields);
                        return flattenLines;
                    }
                    for (var i = 1; i < headers.Count; i++)
                    {
                        var keySuffix = headers.Count > 2 ? $"-{headers[i]}" : "";
                        flattenLines.Add(($"{fields[0]}{keySuffix}", fields[i]));
                    }
                    return flattenLines;
                }).SelectMany(x => x)
                .Select(x => (x.Item1, Convert.ToDouble(x.Item2)));
            
            return (headers, result);
        }
        
        private IEnumerable<string[]> GetRows(string xlsxPath)
        {
            using ExcelPackage xlPackage = new ExcelPackage(new FileInfo(xlsxPath));
            
            var myWorksheet = xlPackage.Workbook.Worksheets.First();
            var totalRows = myWorksheet.Dimension.End.Row;
            var totalColumns = myWorksheet.Dimension.End.Column;
            
            for (var rowNum = 1; rowNum <= totalRows; rowNum++)
            {
                var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns]
                    .Select(c => c.Value == null ? string.Empty : c.Value.ToString()).ToArray();
                if (row.Contains("")) throw new ExcelErrorValueException(eErrorType.Value);
                yield return row;
            }
        }
    }
}