using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private static readonly List<PictureBox> ControlButtons = new();

        private static readonly StringFormat Sf = new()
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };

        private static ComboBox _chartTypeDdl;
        private static ComboBox _chartDataDdl;
        private static ChartWindow _form;
        private static Workspace _workspace;
        private static ChartData _selectedData;
        private static Type _selectedChartType;

        public static void InitializeStartButtons(ChartWindow form, Workspace workspace)
        {
            _form = form;
            _workspace = workspace;

            var screenMiddle = Painter.ScreenSize.Width / 2;
            ControlButtons.Add(ButtonsFactory.CreateClearButton(form, workspace));
            ControlButtons.Add(ButtonsFactory.CreateMoveButton(
                workspace, new Point(screenMiddle - 55, 10)));
            ControlButtons.Add(ButtonsFactory.CreateResizingButton(
                workspace, new Point(screenMiddle + 5, 10)));
            var initializingActions = new List<Action>
            {
                InitializeChartDataSelection,
                InitializeChartTypeSelection,
                // InitializeChartDataSelectionFromDrive
            };
            ControlButtons.Add(ButtonsFactory.CreateOpenButton(
                form, ControlButtons, initializingActions));
            foreach (var button in ControlButtons)
                form.Controls.Add(button);
        }

        // private static void InitializeChartDataSelectionFromDrive()
        // {
        //     var chartDataSelectionButton = new Button();
        //     chartDataSelectionButton.Text = $"Select .csv from drive";
        //     chartDataSelectionButton.Location = new Point(_chartDataDdl.Location.X,
        //         _chartDataDdl.Location.Y + _chartDataDdl.Size.Height);
        //     chartDataSelectionButton.Size = _chartDataDdl.Size;
        //     chartDataSelectionButton.Click += ChartDataSelectionFromDriveButtonOnClick;
        //     _form.Controls.Add(chartDataSelectionButton);
        // }
        
        // private static void ChartDataSelectionFromDriveButtonOnClick(object sender, EventArgs e)
        // {
        //     var openFileDialog = new OpenFileDialog();
        //     var result = openFileDialog.ShowDialog(); // Show the dialog.
        //     if (result != DialogResult.OK) 
        //         return;
        //     
        //     var path = openFileDialog.FileName;
        //
        //     _selectedData = new ChartData(path + _chartDataDdl.SelectedItem);
        //
        //     if (_selectedChartType is not null)
        //     {
        //         var chart = (IChart) Activator.CreateInstance(
        //             GetSelectedChartType(), _selectedData);
        //         WorkspaceEntityFactory.CreateWorkspaceEntity(chart, _form, _workspace, _chartTypeDdl);
        //         var entity = _workspace.Add(_form.Controls, chart, new Size(500, 500), new Point(100, 100));
        //         Painter.Paint(entity, _form);
        //     }
        //
        //     _form.Update();
        // }

        private static void InitializeChartDataSelection()
        {
            _chartDataDdl = new ComboBox();
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
            _chartDataDdl.SelectedValueChanged += ChartDataDdlSelectedItemChanged;
            _form.Controls.Add(_chartDataDdl);
        }

        private static void ChartDataDdlSelectedItemChanged(object sender, EventArgs e)
        {
            _selectedData = new ChartData(ResourceExplorer.PathToResources + _chartDataDdl.SelectedItem);

            if (_selectedChartType is not null)
            {
                var chart = (IChart) Activator.CreateInstance(
                    GetSelectedChartType(), _selectedData);
                WorkspaceEntityFactory.CreateWorkspaceEntity(chart, _form, _workspace, _chartTypeDdl);
                var entity = _workspace.Add(_form.Controls, chart, new Size(500, 500), new Point(100, 100));
                Painter.Paint(entity, _form);
            }

            _form.Update();
        }

        private static void InitializeChartTypeSelection()
        {
            _chartTypeDdl = new ComboBox();
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
            _selectedChartType = null;
            var chart = (IChart) Activator.CreateInstance(
                GetSelectedChartType(), _selectedData);
            if (IsNullSelectedData())
                return;
            if (chart is null)
                throw new ArgumentNullException(nameof(chart));

            WorkspaceEntityFactory.CreateWorkspaceEntity(chart, _form, _workspace, _chartTypeDdl);

            foreach (var button in ControlButtons)
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
                var errorMessage = "Error: Data for chart is not selected";
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
            if (_selectedChartType is not null)
                return _selectedChartType;
            var name = _chartTypeDdl.SelectedItem
                .ToString()?.Replace(" ", string.Empty);
            _selectedChartType = HelpMethods.GetImplementations(typeof(IChart))
                .FirstOrDefault(t => t.Name == name);
            return _selectedChartType;
        }
    }
}