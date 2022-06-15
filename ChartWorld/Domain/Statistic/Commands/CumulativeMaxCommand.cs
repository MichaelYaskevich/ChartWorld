using ChartWorld.Domain.Chart.ChartData;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class CumulativeMaxCommand : StatisticCommand
    {
        public CumulativeMaxCommand() : base("Кумулятивный максимум") {}
        
        public override void Execute(object[] args) => 
            HelpMethods.MakeChartFromStatistic(args, GetNewData);
        private ChartData GetNewData(ChartData data) => data.GetCumulativeMax();
    }
}