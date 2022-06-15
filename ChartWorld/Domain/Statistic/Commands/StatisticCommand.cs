namespace ChartWorld.Domain.Statistic.Commands
{
    public abstract class StatisticCommand
    {
        protected StatisticCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public abstract void Execute(object[] args);

    }
}