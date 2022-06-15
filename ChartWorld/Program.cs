using System;
using System.Windows.Forms;
using ChartWorld.Domain.Statistic;
using ChartWorld.Domain.Statistic.Commands;
using ChartWorld.Domain.Workspace;
using ChartWorld.UI;
using Ninject;

namespace ChartWorld
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //TODO: добавить DI-container 
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var workspace = new Workspace(CreateExecutor());
            Application.Run(new ChartWindow(workspace));
        }

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