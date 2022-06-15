using System.Linq;
using ChartWorld.Domain.Chart.ChartData;

namespace ChartWorld.Domain.Statistic.Commands
{
    public class ItemsWithMaxValueCommand : StatisticCommand
    {
        public ItemsWithMaxValueCommand() : base("Элементы с максимальным значением") {}

        public override void Execute(object[] args)
        {
            //TODO: красивл выводить или тоже сделать диаграмму
            HelpMethods.MakeTextFromStatistic(args, 
                data => string.Join('\n', data
                    .GetItemsWithMaxValue()
                    .Select(x => x.ToString())));
        }
    }
}