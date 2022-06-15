using System.Globalization;
using ChartWorld.Domain.Chart.ChartData;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class ExpectationCommand : StatisticCommand
    {
        public ExpectationCommand() : base("Математическое ожидание") {}

        public override WorkspaceEntity Execute(object[] args)
        {
            return HelpMethods.MakeTextFromStatistic(args, 
                data => data.GetExpectation().ToString(CultureInfo.InvariantCulture));
        }
    }
}