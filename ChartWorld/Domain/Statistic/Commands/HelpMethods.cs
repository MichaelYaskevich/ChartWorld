using System;
using System.Drawing;
using ChartWorld.Domain.Chart;
using ChartWorld.Domain.Chart.ChartData;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic.Commands
{
    public static class HelpMethods
    {
        public static WorkspaceChart MakeChartFromStatistic(WorkspaceEntity entity,
            ChartData data, Workspace.Workspace workspace)
        {
            var chart = new BarChart(data);
            return new WorkspaceChart(
                workspace, chart, new Size(500, 500), 
                new Point(entity.Location.X + entity.Size.Width + 50, entity.Location.Y));
        }
        
        public static WorkspaceChart MakeChartFromStatistic(object[] args, Func<ChartData, ChartData> getNewData)
        {
            var parsedArgs = ParseArgs(args);
            if (parsedArgs is null) return null;
            var (entity, data, workspace) = parsedArgs.Value;
            return MakeChartFromStatistic(entity, getNewData(data), workspace);
        }

        public static WorkspaceEntity MakeTextFromStatistic(object[] args, Func<ChartData, string> convertToString)
        {
            var parsedArgs = ParseArgs(args);
            if (parsedArgs is null) return null;
            var (entity, data, workspace) = parsedArgs.Value;
            return new WorkspaceEntity(workspace, convertToString(data), new Size(300, 300),
                new Point(entity.Location.X + entity.Size.Width + 50, entity.Location.Y));
        }

        private static (WorkspaceEntity, ChartData, Workspace.Workspace)? ParseArgs(object[] args)
        {
            if (args[0] is WorkspaceEntity entity
                && args[1] is ChartData data
                && args[2] is Workspace.Workspace workspace)
            {
                return (entity, data, workspace);
            }

            return null;
        }
    }
}