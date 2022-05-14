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
    }
}