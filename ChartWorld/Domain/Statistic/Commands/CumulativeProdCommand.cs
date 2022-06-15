using ChartWorld.Domain.Chart.ChartData;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class CumulativeProdCommand : StatisticCommand
    {
        public CumulativeProdCommand() : base("Кумулятивное произведение") {}
        
        public override WorkspaceChart Execute(object[] args) => 
            HelpMethods.MakeChartFromStatistic(args, GetNewData);
        private ChartData GetNewData(ChartData data) => data.GetCumulativeProd();
    }
}