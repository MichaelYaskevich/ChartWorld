using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ChartWorld.Chart;
using ChartWorld.Statistic;
using ChartWorld.Workspace;

namespace ChartWorld.App
{
    public static class WorkspaceEntityFactory
    {
        public static PictureBox MakeMoveButton()
        {
            var button = new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = "MoveButton",
                Image = new Bitmap(HelpMethods.PathToImages + "move_cursor.png"),
                Visible = true
            };
            return button;
        }

        public static PictureBox MakeStatisticButton()
        {
            return new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = "StatisticButton",
                Image = new Bitmap(HelpMethods.PathToImages + "statistic_button.png"),
                Visible = true
            };
        }

        public static IEnumerable<MethodInfo> GetStatisticMethods()
        {
            var objectMethods = typeof(object)
                .GetMethods()
                .Select(x => x.Name)
                .ToHashSet();
            return typeof(ChartDataExtensions)
                .GetMethods()
                .Where(x => !objectMethods.Contains(x.Name));
        }

        public static object InvokeStatisticMethod(string name, WorkspaceEntity entity,
            ChartData data, Form form, Workspace.Workspace workspace, ComboBox comboBox)
        {
            switch (name)
            {
                case "GetItemsWithMaxValue":
                    return string.Join('\n', data
                        .GetItemsWithMaxValue()
                        .Select(x => x.ToString()));
                case "GetItemsWithMinValue":
                    return string.Join('\n', data
                        .GetItemsWithMinValue()
                        .Select(x => x.ToString()));
                case "Abs":
                    MakeChartFromStatistic(entity, data.Abs(), form, workspace, comboBox);
                    return "";
                case "GetExpectation":
                    return data.GetExpectation();
                case "GetCumulativeMax":
                    MakeChartFromStatistic(entity, data.GetCumulativeMax(), form, workspace, comboBox);
                    return "";
                case "GetCumulativeMin":
                    MakeChartFromStatistic(entity, data.GetCumulativeMin(), form, workspace, comboBox);
                    return "";
                case "GetCumulativeProd":
                    MakeChartFromStatistic(entity, data.GetCumulativeProd(), form, workspace, comboBox);
                    return "";
                case "GetCumulativeSum":
                    MakeChartFromStatistic(entity, data.GetCumulativeSum(), form, workspace, comboBox);
                    return "";
            }

            return null;
        }

        public static void MakeChartFromStatistic(WorkspaceEntity entity,
            ChartData data, Form form, Workspace.Workspace workspace, ComboBox comboBox)
        {
            var chart = new BarChart(data);
            CreateWorkspaceEntity(chart, form, workspace, comboBox,
                new Point(entity.Location.X + entity.GetSize().Width + 50, entity.Location.Y));
            form.Update();
        }

        public static void DrawAnswer(object obj, WorkspaceEntity entity)
        {
            ChartWindow.ToPaint.Enqueue(g =>
                g.DrawString(
                    obj.ToString(),
                    new Font("Arial", 8, FontStyle.Regular),
                    new SolidBrush(Color.Black),
                    entity.Location.X + entity.GetSize().Width + 10,
                    entity.Location.Y));
        }

        public static ComboBox MakeDropDownList(Point location, object[] statisticMethods)
        {
            var list = new ComboBox();
            list.DropDownStyle = ComboBoxStyle.DropDownList;
            list.Size = new Size(200, 200);
            list.Location = location;
            list.Items.AddRange(statisticMethods);
            return list;
        }

        public static void CreateWorkspaceEntity(IChart chart, Form form, Workspace.Workspace workspace,
            ComboBox comboBox, Point location = default)
        {
            var moveButton = MakeMoveButton();
            var statisticButton = MakeStatisticButton();
            var statisticMethods = GetStatisticMethods()
                .Where(x => x.GetParameters().Length == 1)
                .Select(x => x.Name)
                .ToArray();

            if (location == default)
                location = new Point(100, 100);
            var chartData = chart.Data;

            var chartAsWorkspaceEntity = workspace.Add(
                form.Controls,
                chart,
                0.4,
                location,
                new List<PictureBox>() {moveButton, statisticButton});

            moveButton.Click += (sender, args) => { workspace.Choose(chartAsWorkspaceEntity); };

            statisticButton.Click += (o, args) =>
            {
                var list = MakeDropDownList(chartAsWorkspaceEntity.Location, statisticMethods);
                form.Controls.Add(list);
                list.SelectedValueChanged += (sender, eventArgs) =>
                {
                    var chosenMethod = list.SelectedItem?.ToString();
                    var obj = InvokeStatisticMethod(chosenMethod, chartAsWorkspaceEntity, chartData, form, workspace,
                        comboBox);
                    DrawAnswer(obj, chartAsWorkspaceEntity);
                    form.Controls.Remove(list);
                    form.Invalidate();
                    foreach (var entity in workspace.GetWorkspaceEntities())
                    {
                        Painter.Paint(entity, form);
                    }
                };
            };
        }
    }
}