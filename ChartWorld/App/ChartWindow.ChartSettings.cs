using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ChartWorld.Chart;
using ChartWorld.Statistic;
using ChartWorld.Workspace;

namespace ChartWorld.App
{
    public static class ChartSettings
    {
        private static ComboBox _chartTypeDdl;
        private static ComboBox _chartDataDdl;
        private static Form _form;
        private static Workspace.Workspace _workspace;
        private static ChartData _selectedData;

        public static void InitializeChartDataSelection(Form form, Workspace.Workspace workspace)
        {
            _chartDataDdl = new ComboBox();
            _chartDataDdl.DropDownStyle = ComboBoxStyle.DropDownList;
            _chartDataDdl.Name = "Choose the stats you want to visualize";
            _chartDataDdl.Location = new Point(0, 0);
            var fileNames = GetAllCsvFileNames();
            _chartDataDdl.Size = new Size(WindowInfo.ScreenSize.Width / 6, WindowInfo.ScreenSize.Height);
            _chartDataDdl.Items.AddRange(fileNames.Select(name => "ChartWorld.Resources." + name).Cast<object>().ToArray());
            _chartDataDdl.SelectedValueChanged += ChartDataDdlSelectedItemChanged;
            form.Controls.Add(_chartDataDdl);
        }

        private static void ChartDataDdlSelectedItemChanged(object sender, EventArgs e)
        {
            _selectedData = new ChartData(_chartDataDdl.SelectedItem.ToString());
            _form.Invalidate();
        }
        
        private static IEnumerable<string> GetAllCsvFileNames()
        {
            var workingDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent;
            if (projectDirectory is null)
                return Array.Empty<string>();
            var resourcesDirectory = projectDirectory
                .GetDirectories()
                .FirstOrDefault(d => d.Name == "Resources");
            return resourcesDirectory is null
                ? Array.Empty<string>()
                : resourcesDirectory.GetFiles().Select(file => file.Name);
        }

        public static void InitializeChartTypeSelection(Form form, Workspace.Workspace workspace)
        {
            _form = form;
            _chartTypeDdl = new ComboBox();
            _workspace = workspace;
            _chartTypeDdl.DropDownStyle = ComboBoxStyle.DropDownList;
            _chartTypeDdl.Name = "Choose the way you want your stats to be visualized";
            _chartTypeDdl.Size = new Size(WindowInfo.ScreenSize.Width / 15, WindowInfo.ScreenSize.Height / 15);
            _chartTypeDdl.Location = new Point(WindowInfo.ScreenSize.Width - _chartTypeDdl.Size.Width, 0);
            _chartTypeDdl.Items.AddRange(GetAllChartNames().Cast<object>().ToArray());
            _chartTypeDdl.SelectedValueChanged += ChartTypeDdlSelectedItemChanged;
            form.Controls.Add(_chartTypeDdl);
        }

        private static void ChartTypeDdlSelectedItemChanged(object sender, EventArgs e)
        {
            var value = _chartTypeDdl.SelectedItem?.ToString()?.Replace(" ", string.Empty);
            if (IsNullSelectedData())
                return;
            var chart = Activator.CreateInstance(GetChartByName(value), _selectedData);
            if (chart is null)
                throw new ArgumentNullException(nameof(chart));

            var chartAsWorkspaceEntity = new WorkspaceEntity(
                chart,
                new Size(100, 100),
                new Point(0, 0),
                Color.Black,
                new List<PictureBox>());
            _workspace.Add(chartAsWorkspaceEntity);
            Painter.Paint(chartAsWorkspaceEntity, _form);
        }

        private static bool IsNullSelectedData()
        {
            if (_selectedData is not null)
                return false;
            ChartWindow.ToPaint.Enqueue(g =>
            {
                var font = new Font("Arial", 10, FontStyle.Regular);
                var errorMessage = "Error: Data for chart is not selected";
                var textSize = g.MeasureString(errorMessage, font);
                g.DrawString(errorMessage, font,
                    new SolidBrush(Color.Black), new PointF(_chartTypeDdl.Location.X - textSize.Width - 5, _chartTypeDdl.Location.Y + 4));
            });
            _form.Invalidate();
            return true;
        }

        private static string[] SplitStringByCapitalLetters(string str)
        {
            return Regex.Split(str, @"(?<!^)(?=[A-Z])");
        }

        private static IEnumerable<Type> GetImplementations(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(t => t.GetInterfaces().Contains(type));
        }

        private static Type GetChartByName(string name)
        {
            return GetImplementations(typeof(IChart)).FirstOrDefault(t => t.Name == name);
        }

        private static string[] GetAllChartNames()
        {
            return GetImplementations(typeof(IChart))
                .Select(t => t.Name)
                .Select(SplitStringByCapitalLetters)
                .Select(split => string.Join(" ", split))
                .ToArray();
        }
    }
}