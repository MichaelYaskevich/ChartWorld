using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ChartWorld.Chart;
using ChartWorld.Statistic;

namespace ChartWorld.App
{
    public static class ChartSettings
    {
        private static ComboBox _chartTypeDdl;
        private static ComboBox _chartDataDdl;
        private static ChartWindow _form;
        private static Workspace.Workspace _workspace;
        private static ChartData _selectedData;
        private static PictureBox _openButton;
        private static PictureBox _clearButton;

        public static void InitializeStartButton(ChartWindow form, Workspace.Workspace workspace)
        {
            _form = form;
            _workspace = workspace;
            InitializeOpenButton();
            InitializeClearButton();
        }
        
        public static void InitializeOpenButton()
        {
            _openButton = new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = "OpenButton",
                Image = new Bitmap(HelpMethods.PathToImages + "open_button.png"),
                Visible = true,
                Location = new Point(0, 0),
                Size = new Size(50, 50)
            };
            _openButton.Click += (sender, args) =>
            {
                _form.Controls.Remove(_openButton);
                if (_form.Controls.Contains(_clearButton))
                    _form.Controls.Remove(_clearButton);
                InitializeChartDataSelection();
                InitializeChartTypeSelection();
                _form.Update();
            };
            _form.Controls.Add(_openButton);
        }

        public static void InitializeClearButton()
        {
            _clearButton = new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = "ClearButton",
                Image = new Bitmap(HelpMethods.PathToImages + "clear_button.png"),
                Visible = true,
                Location = new Point(60, 0),
                Size = new Size(50, 50)
            };
            _clearButton.Click += (sender, args) =>
            {
                _form.Controls.Clear();
                _form.Controls.Add(_openButton);
                if (_clearButton != null)
                    _form.Controls.Add(_clearButton);
                _workspace.Clear();
                _form.Invalidate();
            };
            _form.Controls.Add(_clearButton);
        }

        public static void InitializeChartDataSelection()
        {
            _chartDataDdl = new ComboBox();
            _chartDataDdl.DropDownStyle = ComboBoxStyle.DropDownList;
            _chartDataDdl.Name = "Choose the stats you want to visualize";
            _chartDataDdl.Location = new Point(0, 0);
            var fileNames = GetAllCsvFileNames();
            _chartDataDdl.Size = new Size(WindowInfo.ScreenSize.Width / 6, WindowInfo.ScreenSize.Height);
            _chartDataDdl.Items.AddRange(fileNames.Select(name => "ChartWorld.Resources." + name).Cast<object>().ToArray());
            _chartDataDdl.SelectedValueChanged += ChartDataDdlSelectedItemChanged;
            _form.Controls.Add(_chartDataDdl);
        }

        private static void ChartDataDdlSelectedItemChanged(object sender, EventArgs e)
        {
            _selectedData = new ChartData(_chartDataDdl.SelectedItem.ToString());
            _form.Update();
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

        public static void InitializeChartTypeSelection()
        {
            _chartTypeDdl = new ComboBox();
            _chartTypeDdl.DropDownStyle = ComboBoxStyle.DropDownList;
            _chartTypeDdl.Name = "Choose the way you want your stats to be visualized";
            _chartTypeDdl.Size = new Size(WindowInfo.ScreenSize.Width / 15, WindowInfo.ScreenSize.Height / 15);
            _chartTypeDdl.Location = new Point(WindowInfo.ScreenSize.Width - _chartTypeDdl.Size.Width, 0);
            _chartTypeDdl.Items.AddRange(GetAllChartNames().Cast<object>().ToArray());
            _chartTypeDdl.SelectedValueChanged += ChartTypeDdlSelectedItemChanged;
            _form.Controls.Add(_chartTypeDdl);
        }

        private static void ChartTypeDdlSelectedItemChanged(object sender, EventArgs e)
        {
            var value = _chartTypeDdl.SelectedItem?.ToString()?.Replace(" ", string.Empty);
            if (IsNullSelectedData())
                return;
            var chart = Activator.CreateInstance(GetChartByName(value), _selectedData);
            if (chart is null)
                throw new ArgumentNullException(nameof(chart));
          
            WorkspaceEntityFactory.CreateWorkspaceEntity(
                (IChart)chart, _form, _workspace, _chartTypeDdl);
            _form.Controls.Add(_openButton);
            if (_clearButton != null)
                _form.Controls.Add(_clearButton);
            _form.Controls.Remove(_chartDataDdl);
            _form.Controls.Remove(_chartTypeDdl);
            _form.Update();
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