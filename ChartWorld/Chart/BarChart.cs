using System.Collections.Generic;
using ChartWorld.Statistic;

namespace ChartWorld.Chart
{
    public class BarChart : IChart
    {
        public ChartData Data { get; }

        public BarChart(ChartData data)
        {
            Data = data;
        }

        public BarChart BuildChart()
        {
            // Что тут будет?
            return this;
        }

        public BarChart ChangeBar(string key, double newValue)
        {
            throw new System.NotImplementedException();
        }
    }
}