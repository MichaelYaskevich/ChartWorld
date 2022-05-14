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

namespace ChartWorld.App
{
    public static class WorkspaceEntityFactory
    {
        public static PictureBox MakeMoveButton()
        {
            return new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = "MoveButton",
                Image = new Bitmap(PathToImages + "move_cursor.png"),
                Visible = true
            };
        }
        
        public static PictureBox MakeStatisticButton()
        {
            return new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = "StatisticButton",
                Image = new Bitmap(PathToImages + "statistic_button.png"),
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
            return GetStatisticMethods()
                .Where(x => x.GetParameters().Length == 1)
                .FirstOrDefault(x => x.Name == name)?
                .Invoke(data, Array.Empty<object>());
        }

        public static Label MakeAnswer(object obj, Point location)
        {
            return new Label()
            {
                Text = obj.ToString(), Location = location
            };
        }

        public static ComboBox MakeDropDownList(Point location, string[] statisticMethods)
        {
            var list = new ComboBox();
            list.DropDownStyle = ComboBoxStyle.Simple;
            list.Size = new Size(50, 50);
            list.Location = location;
            list.Items.AddRange(statisticMethods);
            return list;
        }
        
        public static WorkspaceEntity CreateWorkspaceEntity(IChart chart, Form form)
        {
            var moveButton = MakeMoveButton();
            var statisticButton = MakeStatisticButton();
            var statisticMethods = GetStatisticMethods()
                .Select(x => x.Name)
                .ToArray();;
            
            var location = new Point(0, 0);
            var chartData = chart.Data;
            statisticButton.Click += (o, args) =>
            {
                var list = MakeDropDownList(location, statisticMethods);
                form.Controls.Add(list);
                list.SelectedValueChanged += (sender, eventArgs) =>
                {
                    var value = list.SelectedItem?.ToString();
                    var obj = InvokeStatisticMethod(value, chartData);
                    form.Controls.Add(MakeAnswer(obj, location));
                };

            };
            form.Controls.Add(moveButton);
            var chartAsWorkspaceEntity = new WorkspaceEntity(
                chart, 
                new Size(100, 100), 
                location, 
                Color.Black, 
                new List<PictureBox>()
                {
                    moveButton, statisticButton
                });
            return chartAsWorkspaceEntity;
        }
        
        private static string PathToImages = GetGameDirectoryRoot().FullName + "\\Resources\\Images\\";
        private static DirectoryInfo GetGameDirectoryRoot() {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (!dir.ToString().EndsWith("ChartWorld")) {
                dir = dir.Parent;
            }
            return dir;
        }
    }
}