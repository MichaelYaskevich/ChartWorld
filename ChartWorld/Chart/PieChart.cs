using System.Linq;
using ChartWorld.Statistic;

namespace ChartWorld.Chart
{
    public class PieChart : IChart
    {
        public ChartData Data { get; }
        public ChartData PercentageData { get; }

        public PieChart(ChartData data)
        {
            Data = data;
            PercentageData = new ChartData((data.Headers, data.GetOrderedItems()));
        }

        private ChartData ToPercentageData(ChartData data)
        {
            var orderedItems = data.GetOrderedItems();
            var valueTuples = orderedItems.ToList();
            var sum = valueTuples
                .Select(tuple => tuple.Item2)
                .Sum();
            foreach (var (name, value) in valueTuples)
            {
                data[name] = value / sum * 100;
            }

            return data;
        }
    }
}