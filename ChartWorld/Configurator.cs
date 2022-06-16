using ChartWorld.Domain.Statistic;
using ChartWorld.Domain.Statistic.Commands;
using Ninject;

namespace ChartWorld
{
    public static class Configurator
    {
        public static ICommandsExecutor CreateExecutor()
        {
            var container = new StandardKernel();
            
            container.Bind<StatisticCommand>().To<AbsCommand>();
            container.Bind<StatisticCommand>().To<CumulativeMaxCommand>();
            container.Bind<StatisticCommand>().To<CumulativeMinCommand>();
            container.Bind<StatisticCommand>().To<CumulativeProdCommand>();
            container.Bind<StatisticCommand>().To<CumulativeSumCommand>();
            container.Bind<StatisticCommand>().To<ExpectationCommand>();
            container.Bind<StatisticCommand>().To<ItemsWithMaxValueCommand>();
            container.Bind<StatisticCommand>().To<ItemsWithMinValueCommand>();

            container.Bind<ICommandsExecutor>().To<CommandsExecutor>();
            return container.Get<ICommandsExecutor>();
        }
    }
}