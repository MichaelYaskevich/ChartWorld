using ChartWorld.Domain.Chart.ChartData;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class AbsCommand : StatisticCommand
    {
        public AbsCommand() : base("Абсолютные значения") {}
        
        public override WorkspaceChart Execute(object[] args) => 
            HelpMethods.MakeChartFromStatistic(args, GetNewData);
        private ChartData GetNewData(ChartData data) => data.Abs();
    }
}