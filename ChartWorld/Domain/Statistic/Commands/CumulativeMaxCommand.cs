using ChartWorld.Domain.Chart.ChartData;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class CumulativeMaxCommand : StatisticCommand
    {
        public CumulativeMaxCommand() : base("Кумулятивный максимум") {}
        
        public override WorkspaceChart Execute(object[] args) => 
            HelpMethods.MakeChartFromStatistic(args, GetNewData);
        private ChartData GetNewData(ChartData data) => data.GetCumulativeMax();
    }
}