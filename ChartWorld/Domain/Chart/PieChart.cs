using System.Linq;

namespace ChartWorld.Domain.Chart
{
    public class PieChart : IChart
    {
        public ChartData.ChartData Data { get; }
        public ChartData.ChartData PercentageData { get; }

        public PieChart(ChartData.ChartData data)
        {
            Data = data;
            PercentageData = ToPercentageData(new ChartData.ChartData((data.Headers, data.GetOrderedItems())));
        }

        private ChartData.ChartData ToPercentageData(ChartData.ChartData data)
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