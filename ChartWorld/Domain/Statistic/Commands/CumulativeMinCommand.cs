using ChartWorld.Domain.Chart.ChartData;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class CumulativeMinCommand : StatisticCommand
    {
        public CumulativeMinCommand() : base("Кумулятивный минимум") {}
        
        public override void Execute(object[] args) => 
            HelpMethods.MakeChartFromStatistic(args, GetNewData);
        private ChartData GetNewData(ChartData data) => data.GetCumulativeMin();
    }
}