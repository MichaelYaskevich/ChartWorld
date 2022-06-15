using System.Drawing;
using ChartWorld.Domain.Chart;

namespace ChartWorld.Domain.Workspace
{
    public class WorkspaceChart : WorkspaceEntity
    {
        public IChart Chart { get; }
        
        public WorkspaceChart(Workspace workspace, IChart chart, Size size, Point location)
            : base(workspace, chart, size, location)
        {
            Chart = chart;
        }

        public string[] GetStatisticMethods()
        {
            return Workspace.Executor.GetAvailableCommandNames();
        }

        public WorkspaceEntity ExecuteStatisticMethod(string chosenMethod)
        {
            return Workspace.Executor.Execute(chosenMethod,
                new object[] {this, Chart.Data, Workspace});
        }
    }
}