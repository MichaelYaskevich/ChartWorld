using System.Collections.Generic;
using ChartWorld.Statistic;
using ChartWorld.Workspace;

namespace ChartWorld.Chart
{
    public interface IChart
    {
        public ChartData Data { get; }

        public IChart BuildChart();
    }
}