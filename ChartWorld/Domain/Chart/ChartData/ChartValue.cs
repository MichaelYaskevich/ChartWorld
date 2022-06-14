namespace ChartWorld.Domain.Chart.ChartData
{
    public class ChartValue
    {
        public ChartValue(string name, double value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public double Value { get; }
    }
}