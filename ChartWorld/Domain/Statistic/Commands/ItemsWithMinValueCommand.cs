using System.Linq;
using ChartWorld.Domain.Chart.ChartData;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class ItemsWithMinValueCommand : StatisticCommand
    {
        public ItemsWithMinValueCommand() : base("Элементы с минимальным значением") {}

        public override void Execute(object[] args)
        {
            //TODO: красивл выводить или тоже сделать диаграмму
            HelpMethods.MakeTextFromStatistic(args, 
                data => string.Join('\n', data
                    .GetItemsWithMinValue()
                    .Select(x => x.ToString())));
        }
    }
}