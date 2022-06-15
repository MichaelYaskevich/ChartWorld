using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic.Commands
{
    public abstract class StatisticCommand
    {
        protected StatisticCommand(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public abstract WorkspaceEntity Execute(object[] args);

    }
}