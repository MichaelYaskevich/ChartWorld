using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ChartWorld.Statistic;

namespace ChartWorld.Chart
{
    public class PieChart : IChart
    {
        public IChartData Data { get; set; }
        private IChartData ModifiedData { get; set; }

        public IChart BuildChart()
        {
            var sum = Data.GetSum();

            foreach (var (key, value) in Data.ChartValues)
            {
                if (!ModifiedData.ChartValues.ContainsKey(key))
                    ModifiedData.ChartValues.Add(key, value / sum);
                else
                    ModifiedData.ChartValues[key] = value / sum;
            }

            return this;
        }
    }
}