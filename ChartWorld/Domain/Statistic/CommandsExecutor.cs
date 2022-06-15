using System;
using System.Linq;
using ChartWorld.Domain.Statistic.Commands;

namespace ChartWorld.Domain.Statistic
{
    public class CommandsExecutor : ICommandsExecutor
    {
        private readonly StatisticCommand[] _commands;
        
        public CommandsExecutor(StatisticCommand[] commands)
        {
            _commands = commands;
        }
        
        public string[] GetAvailableCommandNames()
        {
            return _commands.Select(c => c.Name).ToArray();
        }

        public StatisticCommand FindCommandByName(string name)
        {
            return _commands.FirstOrDefault(c =>
                string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public void Execute(string commandName, object[] args)
        {
            var cmd = FindCommandByName(commandName);
            if (cmd != null)
                cmd.Execute(args);
        }
    }
}