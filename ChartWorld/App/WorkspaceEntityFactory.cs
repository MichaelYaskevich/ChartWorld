using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ChartWorld.Chart;
using ChartWorld.Statistic;
using ChartWorld.Workspace;
using FluentAssertions;

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

        public static object InvokeStatisticMethod(string name, ChartData data)
        {
            switch (name)
            {
                case "GetItemsWithMaxValue":
                    return data.GetItemsWithMaxValue();
                case "GetItemsWithMinValue":
                    return data.GetItemsWithMinValue();
                case "Abs":
                    return data.Abs().GetOrderedItems();
                case "GetExpectation":
                    return data.GetExpectation();
                case "GetCumulativeMax":
                    return data.GetCumulativeMax();
                case "GetCumulativeMin":
                    return data.GetCumulativeMin();
                case "GetCumulativeProd":
                    return data.GetCumulativeProd();
                case "GetCumulativeSum":
                    return data.GetCumulativeSum();
            }

            return null;
        }

        public static Control MakeAnswer(object obj, Point location)
        {
            return new TextBox()
            {
                Text = obj.ToString(),
                // Size = new Size(300, 200),
                // Multiline = true,
                
                // AcceptsReturn = true,
                // AcceptsTab = true,
                Location = location
            };
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
        
        public static WorkspaceEntity CreateWorkspaceEntity(IChart chart, Form form, Workspace.Workspace workspace, ComboBox _comboBox)
        {
            var moveButton = MakeMoveButton();
            var statisticButton = MakeStatisticButton();
            var statisticMethods = GetStatisticMethods()
                .Where(x => x.GetParameters().Length == 1)
                .Select(x => x.Name)
                .ToArray();;
            
            var location = new Point(100, 100);
            var chartData = chart.Data;
            statisticButton.Click += (o, args) =>
            {
                var list = MakeDropDownList(location, statisticMethods);
                form.Controls.Add(list);
                list.SelectedValueChanged += (sender, eventArgs) =>
                {
                    var chodenMethod = list.SelectedItem?.ToString();
                    var obj = InvokeStatisticMethod(chodenMethod, chartData);
                    var answer = MakeAnswer(obj, location);
                    form.Controls.Add(answer);
                    form.Controls.Remove(list);
                    form.Invalidate();
                    foreach (var entity in workspace.GetWorkspaceEntities())
                    {
                        Painter.Paint(entity, form);
                    }
                };

            };
            
            var chartAsWorkspaceEntity = workspace.Add(
                form.Controls,
                chart,
                new Size(500, 500),
                location,
                new List<PictureBox>()
                {
                    moveButton, statisticButton
                });
            
            moveButton.Click += (sender, args) =>
            {
                workspace.Choose(chartAsWorkspaceEntity);
                //подумать
                form.Controls.Remove(_comboBox);
            };
            return chartAsWorkspaceEntity;
        }
    }
}