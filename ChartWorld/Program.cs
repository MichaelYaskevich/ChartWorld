using System;
using System.Windows.Forms;
using ChartWorld.App;

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
            var workspace = new Workspace.Workspace();
            Application.Run(new ChartWindow(workspace));
        }
    }
}