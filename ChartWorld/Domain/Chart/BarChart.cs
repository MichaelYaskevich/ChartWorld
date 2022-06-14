namespace ChartWorld.Domain.Chart
{
    public class BarChart : IChart
    {
        public ChartData.ChartData Data { get; }

        public BarChart(ChartData.ChartData data)
        {
            Data = data;
        }
    }
}