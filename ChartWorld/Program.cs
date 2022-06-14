using System;
using System.Windows.Forms;
using ChartWorld.Domain.Workspace;
using ChartWorld.UI;

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
            var workspace = new Workspace();
            Application.Run(new ChartWindow(workspace));
        }
    }
}