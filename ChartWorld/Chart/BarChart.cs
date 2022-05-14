using System.Collections.Generic;
using ChartWorld.Statistic;

namespace ChartWorld.Chart
{
    public class BarChart : IChart<BarChart>
    {
        public ChartData Data { get; }

        public BarChart(ChartData data)
        {
            Data = data;
        }

        public BarChart BuildChart()
        {
            throw new System.NotImplementedException();
        }

        public BarChart ChangeBar(string key, double newValue)
        {
            throw new System.NotImplementedException();
        }
    }
}