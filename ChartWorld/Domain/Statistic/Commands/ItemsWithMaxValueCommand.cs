using System.Linq;
using ChartWorld.Domain.Chart.ChartData;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class ItemsWithMaxValueCommand : StatisticCommand
    {
        public ItemsWithMaxValueCommand() : base("Элементы с максимальным значением") {}

        public override WorkspaceEntity Execute(object[] args)
        {
            //TODO: красивл выводить или тоже сделать диаграмму
            return HelpMethods.MakeTextFromStatistic(args, 
                data => string.Join('\n', data
                    .GetItemsWithMaxValue()
                    .Select(x => x.ToString())));
        }
    }
}