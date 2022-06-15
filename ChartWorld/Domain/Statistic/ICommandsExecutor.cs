using ChartWorld.Domain.Statistic.Commands;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic
{
    public interface ICommandsExecutor
    {
        public string[] GetAvailableCommandNames();

        public StatisticCommand FindCommandByName(string name);

        public WorkspaceEntity Execute(string commandName, object[] args);
    }
}