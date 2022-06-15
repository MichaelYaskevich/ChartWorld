using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChartWorld.Domain.Chart;
using ChartWorld.UI;

namespace ChartWorld.Domain.Workspace
{
    public static class WorkspaceEntityFactory
    {
        private static ComboBox MakeDropDownList(Point location, object[] statisticMethods)
        {
            var list = new ComboBox();
            list.DropDownStyle = ComboBoxStyle.DropDownList;
            list.Size = new Size(200, 200);
            list.Location = location;
            list.Items.AddRange(statisticMethods);
            return list;
        }

        public static void CreateWorkspaceEntity(IChart chart, Form form, Workspace workspace,
            ComboBox comboBox, Point location = default)
        {
            var moveButton = ButtonsFactory.CreateMoveButton();
            var statisticButton = ButtonsFactory.CreateStatisticButton();
            var resizingButton = ButtonsFactory.CreateResizingButton();
            var statisticMethods = ChartWindow.Executor.GetAvailableCommandNames();

            if (location == default)
                location = new Point(100, 100);
            var chartData = chart.Data;

            var chartAsWorkspaceEntity = workspace.Add(
                form.Controls,
                chart,
                new Size(500, 500),
                location,
                new List<PictureBox> {moveButton, statisticButton, resizingButton});

            moveButton.Click += (_, _) => { workspace.Select(chartAsWorkspaceEntity, SelectionType.Move); };

            statisticButton.Click += (_, _) =>
            {
                var list = MakeDropDownList(chartAsWorkspaceEntity.Location, statisticMethods);
                form.Controls.Add(list);
                list.SelectedValueChanged += (_, _) =>
                {
                    var chosenMethod = list.SelectedItem?.ToString();
                    ChartWindow.Executor.Execute(chosenMethod,
                        new object[] {chartAsWorkspaceEntity, chartData, form, workspace, comboBox});
                    form.Controls.Remove(list);
                    form.Invalidate();
                    foreach (var entity in workspace.GetWorkspaceEntities())
                        Painter.Paint(entity, form);
                };
            };
            resizingButton.Click += (_, _) =>
            {
                workspace.Select(chartAsWorkspaceEntity, SelectionType.Resize);
            };
        }
    }
}