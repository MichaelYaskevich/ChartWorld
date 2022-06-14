using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChartWorld.Domain.Chart;
using ChartWorld.Domain.Chart.ChartData;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.Domain.Statistic.Commands
{
    public static class HelpMethods
    {
        public static void MakeChartFromStatistic(WorkspaceEntity entity,
            ChartData data, Form form, Workspace.Workspace workspace, ComboBox comboBox)
        {
            var chart = new BarChart(data);
            WorkspaceEntityFactory.CreateWorkspaceEntity(chart, form, workspace, comboBox,
                new Point(entity.Location.X + entity.Size.Width + 50, entity.Location.Y));
            form.Update();
        }
        
        public static void MakeChartFromStatistic(object[] args, Func<ChartData, ChartData> getNewData)
        {
            var parsedArgs = ParseArgs(args);
            if (parsedArgs is null) return;
            var (entity, data, form, workspace, box) = parsedArgs.Value;
            MakeChartFromStatistic(entity, getNewData(data), form, workspace, box);
        }

        public static void MakeTextFromStatistic(object[] args, Func<ChartData, string> convertToString)
        {
            var parsedArgs = ParseArgs(args);
            if (parsedArgs is null) return;
            var (entity, data, form, workspace, box) = parsedArgs.Value;
            workspace.Add(
                form.Controls,
                convertToString(data),
                new Size(300, 100),
                new Point(entity.Location.X + entity.Size.Width + 50, entity.Location.Y),
                new List<PictureBox> {});
            form.Update();
        }

        public static (WorkspaceEntity, ChartData, Form, Workspace.Workspace, ComboBox)? ParseArgs(object[] args)
        {
            if (args[0] is WorkspaceEntity entity
                && args[1] is ChartData data
                && args[2] is Form form
                && args[3] is Workspace.Workspace workspace
                && args[4] is ComboBox box)
            {
                return (entity, data, form, workspace, box);
            }

            return null;
        }
    }
}