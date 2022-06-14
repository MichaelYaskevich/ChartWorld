using System.Globalization;
using ChartWorld.Domain.Chart.ChartData;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class ExpectationCommand : StatisticCommand
    {
        public ExpectationCommand() : base("Математическое ожидание") {}

        public override void Execute(object[] args)
        {
            HelpMethods.MakeTextFromStatistic(args, 
                data => data.GetExpectation().ToString(CultureInfo.InvariantCulture));
        }
    }
}