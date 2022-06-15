using ChartWorld.Domain.Chart.ChartData;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class CumulativeSumCommand : StatisticCommand
    {
        public CumulativeSumCommand() : base("Кумулятивная сумма") {}
        
        public override void Execute(object[] args) => 
            HelpMethods.MakeChartFromStatistic(args, GetNewData);
        private ChartData GetNewData(ChartData data) => data.GetCumulativeSum();
    }
}