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
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var workspace = new Workspace(Configurator.CreateExecutor());
            Application.Run(new ChartWindow(workspace));
        }
    }
}