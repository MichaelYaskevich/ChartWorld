using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ChartWorld.Statistic;

namespace ChartWorld.Chart
{
    public class PieChart : IChart<PieChart>
    {
        public ChartData Data { get; }
        private ChartData ModifiedData { get; set; }

        public PieChart(ChartData data)
        {
            Data = data;
        }

        public PieChart BuildChart()
        {
            throw new NotImplementedException();

            // var sum = Data.GetSum();
            //
            // foreach (var (key, value) in Data.ChartValues)
            // {
            //     if (!ModifiedData.ChartValues.ContainsKey(key))
            //         ModifiedData.ChartValues.Add(key, value / sum);
            //     else
            //         ModifiedData.ChartValues[key] = value / sum;
            // }
            //
            // return this;
        }
    }
}