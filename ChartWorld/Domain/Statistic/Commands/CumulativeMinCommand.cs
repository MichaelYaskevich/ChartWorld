using ChartWorld.Domain.Chart.ChartData;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class CumulativeMinCommand : StatisticCommand
    {
        public CumulativeMinCommand() : base("Кумулятивный минимум") {}
        
        public override WorkspaceChart Execute(object[] args) => 
            HelpMethods.MakeChartFromStatistic(args, GetNewData);
        private ChartData GetNewData(ChartData data) => data.GetCumulativeMin();
    }
}