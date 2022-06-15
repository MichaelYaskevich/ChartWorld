using ChartWorld.Domain.Statistic.Commands;

namespace ChartWorld.Domain.Statistic
{
    public interface ICommandsExecutor
    {
        public string[] GetAvailableCommandNames();

        public StatisticCommand FindCommandByName(string name);

        public void Execute(string commandName, object[] args);
    }
}