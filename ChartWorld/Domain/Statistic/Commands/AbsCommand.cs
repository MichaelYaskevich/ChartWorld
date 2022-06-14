using ChartWorld.Domain.Chart.ChartData;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class AbsCommand : StatisticCommand
    {
        public AbsCommand() : base("Абсолютные значения") {}
        
        public override void Execute(object[] args) => 
            HelpMethods.MakeChartFromStatistic(args, GetNewData);
        private ChartData GetNewData(ChartData data) => data.Abs();
    }
}