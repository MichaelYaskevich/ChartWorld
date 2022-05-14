﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ChartWorld.Chart;
using ChartWorld.Statistic;

namespace ChartWorld.App
{
    public static class ChartSettings
    {
        private static ComboBox _dropDownList;

        public static void InitializeChartTypeSelection(Form form)
        {
            _dropDownList = new ComboBox();
            _dropDownList.DropDownStyle = ComboBoxStyle.DropDownList;
            _dropDownList.Name = "Choose the stats you want to visualize";
            _dropDownList.Size = new Size(WindowInfo.Screen.Size.Width / 15, WindowInfo.Screen.Size.Height / 15);
            _dropDownList.Location = new Point(WindowInfo.Screen.Size.Width - _dropDownList.Size.Width, 0);
            // Связать класс диаграммы с тем методом, которые ее рисует или оставить так?
            _dropDownList.Items.AddRange(GetAllChartNames().Cast<object>().ToArray());
            _dropDownList.SelectedValueChanged += DropDownListSelectedItemChanged;
            form.Controls.Add(_dropDownList);
        }

        private static void DropDownListSelectedItemChanged(object sender, EventArgs e)
        {
            var value = _dropDownList.SelectedItem?.ToString()?.Replace(" ", string.Empty);
            // Временный костыль
            var chartData = new ChartData("ChartWorld.Tests.Resources.ChartDataWithManyValuesForName.csv");
            ////////////////////
            var chart = Activator.CreateInstance(GetChartByName(value), chartData);
            if (chart is null)
                throw new ArgumentNullException(nameof(chart));
            Painter.PaintChart((IChart) chart);
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