using ChartWorld.Domain.Chart.ChartData;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class CumulativeSumCommand : StatisticCommand
    {
        public CumulativeSumCommand() : base("Кумулятивная сумма") {}
        
        public override WorkspaceChart Execute(object[] args) => 
            HelpMethods.MakeChartFromStatistic(args, GetNewData);
        private ChartData GetNewData(ChartData data) => data.GetCumulativeSum();
    }
}