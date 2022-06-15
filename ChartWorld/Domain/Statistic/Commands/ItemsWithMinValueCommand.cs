using System.Linq;
using ChartWorld.Domain.Chart.ChartData;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class ItemsWithMinValueCommand : StatisticCommand
    {
        public ItemsWithMinValueCommand() : base("Элементы с минимальным значением") {}

        public override WorkspaceEntity Execute(object[] args)
        {
            //TODO: красивл выводить или тоже сделать диаграмму
            return HelpMethods.MakeTextFromStatistic(args, 
                data => string.Join('\n', data
                    .GetItemsWithMinValue()
                    .Select(x => x.ToString())));
        }
    }
}