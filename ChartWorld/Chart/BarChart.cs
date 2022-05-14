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

        public IChart BuildChart()
        {
            throw new System.NotImplementedException();
        }

        public BarChart ChangeBar(string key, double newValue)
        {
            throw new System.NotImplementedException();
        }
    }
}