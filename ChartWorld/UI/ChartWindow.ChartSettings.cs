using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChartWorld.App;
using ChartWorld.Domain.Chart;
using ChartWorld.Domain.Chart.ChartData;
using ChartWorld.Domain.Workspace;
using ChartWorld.Infrastructure;

namespace ChartWorld.UI
{
    public static class ChartSettings
    {
        private static readonly List<PictureBox> HomeControlButtons = new();

        private static readonly StringFormat Sf = new()
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };

        private const string SelectFromDrivePrompt = "... Select .csv from drive";

        private static ComboBox _chartTypeDdl;
        private static ComboBox _chartDataDdl;
        private static ChartWindow _form;
        private static Workspace _workspace;
        private static ChartData _selectedData;

        public static void InitializeStartButtons(ChartWindow form, Workspace workspace)
        {
            _form = form;
            _workspace = workspace;

            var screenMiddle = Painter.ScreenSize.Width / 2;
            HomeControlButtons.Add(ButtonsFactory.CreateClearButton(form, workspace));
            HomeControlButtons.Add(ButtonsFactory.CreateMoveButton(
                workspace, new Point(screenMiddle - 55, 10)));
            HomeControlButtons.Add(ButtonsFactory.CreateResizingButton(
                workspace, new Point(screenMiddle + 5, 10)));
            var initializingActions = new List<Action>
            {
                InitializeChartDataSelection,
                InitializeChartTypeSelection,
            };
            HomeControlButtons.Add(ButtonsFactory.CreateOpenButton(
                form, HomeControlButtons, initializingActions));
            foreach (var button in HomeControlButtons)
                form.Controls.Add(button);
        }

        private static void InitializeChartDataSelection()
        {
            _chartDataDdl = new ComboBox { Anchor = AnchorStyles.Top | AnchorStyles.Left };
            _chartDataDdl.DropDownStyle = ComboBoxStyle.DropDownList;
            _chartDataDdl.Text = "Choose the stats you want to visualize";
            _chartDataDdl.Location = new Point(0, 0);
            _chartDataDdl.Size = new Size(
                Painter.ScreenSize.Width / 6,
                Painter.ScreenSize.Height);
            _chartDataDdl.Items.AddRange(ResourceExplorer.GetAllCsvFileNames()
                .Select(name => name)
                .Cast<object>()
                .ToArray());
            _chartDataDdl.Items.Add(SelectFromDrivePrompt);
            _chartDataDdl.SelectedValueChanged += ChartDataDdlSelectedItemChanged;
            _form.Controls.Add(_chartDataDdl);
        }

        private static void GoHome()
        {
            _form.Controls.Clear();
            foreach (var button in HomeControlButtons)
                _form.Controls.Add(button);
        }

        private static void ChartDataDdlSelectedItemChanged(object sender, EventArgs e)
        {
            if (_chartDataDdl.SelectedItem.ToString() == SelectFromDrivePrompt)
            {
                var location = new Point(Painter.ScreenSize.Width / 2, 10);
                var incorrectFileButton = ButtonsFactory.CreateIncorrectFileButton(location);
                location.X -= incorrectFileButton.Size.Width / 2;
                incorrectFileButton.Location = location;
                var okButton = ButtonsFactory.CreateOkButton(
                    new Point(location.X, location.Y + incorrectFileButton.Size.Height + 10));
                
                _form.Controls.Add(incorrectFileButton);
                _form.Controls.Add(okButton);

                okButton.Click += (_, _) => { GoHome(); };
            }
            else if (_selectedChartType is not null)
            {
                var openFileDialog = new OpenFileDialog();
                var result = openFileDialog.ShowDialog();
                var entity = new WorkspaceChart(_workspace, chart,
                    new Size(500, 500), new Point(100, 100));
                EntityHandler.AddButtons(entity);
                
        
                _selectedData = new ChartData(path);
            }
            else
                _selectedData = new ChartData(ResourceExplorer.PathToResources + _chartDataDdl.SelectedItem);

            _form.Update();
        }

        private static void InitializeChartTypeSelection()
        {
            _chartTypeDdl = new ComboBox { Anchor = AnchorStyles.Top | AnchorStyles.Right };
            _chartTypeDdl.DropDownStyle = ComboBoxStyle.DropDownList;
            _chartTypeDdl.Text = "Choose the type of chart";
            _chartTypeDdl.Size = new Size(
                Painter.ScreenSize.Width / 15,
                Painter.ScreenSize.Height / 15);
            _chartTypeDdl.Location = new Point(
                Painter.ScreenSize.Width - _chartTypeDdl.Size.Width,
                0);
            _chartTypeDdl.Items.AddRange(
                HelpMethods.GetAllInterfaceImplementationsNames(typeof(IChart)).Cast<object>().ToArray());
            _chartTypeDdl.SelectedValueChanged += ChartTypeDdlSelectedItemChanged;
            _form.Controls.Add(_chartTypeDdl);
        }

        private static void ChartTypeDdlSelectedItemChanged(object sender, EventArgs e)
        {
            var chart = (IChart) Activator.CreateInstance(
                GetSelectedChartType(), _selectedData);
            if (IsNullSelectedData())
                return;
            if (chart is null)
                throw new ArgumentNullException(nameof(chart));

            var entity = new WorkspaceChart(_workspace, chart,
                new Size(500, 500), new Point(100, 100));
            EntityHandler.AddButtons(entity);

            foreach (var button in HomeControlButtons)
                _form.Controls.Add(button);
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
                const string errorMessage = "Error: Data for chart is not selected";
                var textSize = g.MeasureString(errorMessage, font);
                g.DrawString(errorMessage, font,
                    new SolidBrush(Color.Black),
                    new PointF(_chartTypeDdl.Location.X - textSize.Width / 2,
                        _chartTypeDdl.Location.Y + _chartTypeDdl.Size.Height / 2), Sf);
            });
            _form.Invalidate();
            return true;
        }

        private static Type GetSelectedChartType()
        {
            var name = _chartTypeDdl.SelectedItem
                .ToString()?.Replace(" ", string.Empty);
            return HelpMethods.GetImplementations(typeof(IChart))
                .FirstOrDefault(t => t.Name == name);
        }
    }
}