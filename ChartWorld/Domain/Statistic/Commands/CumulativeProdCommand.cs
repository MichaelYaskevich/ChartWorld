using ChartWorld.Domain.Chart.ChartData;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class CumulativeProdCommand : StatisticCommand
    {
        public CumulativeProdCommand() : base("Кумулятивное произведение") {}
        
        public override void Execute(object[] args) => 
            HelpMethods.MakeChartFromStatistic(args, GetNewData);
        private ChartData GetNewData(ChartData data) => data.GetCumulativeProd();
    }
}