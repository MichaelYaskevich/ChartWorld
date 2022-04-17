using System.Collections.Generic;
using ChartWorld.Statistic;
using ChartWorld.Workspace;

namespace ChartWorld.Chart
{
    public interface IChart : IWorkspaceEntity
    {
        public IChartData Data { get; set; }

        public IChart BuildChart();
    }
}