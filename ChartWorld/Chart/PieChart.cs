using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ChartWorld.Statistic;

namespace ChartWorld.Chart
{
    public class PieChart : IChart
    {
        public ChartData Data { get; }
        public ChartData PercentageData { get; }

        public PieChart(ChartData data)
        {
            Data = data;
            // Костыль?
            PercentageData = data;
        }

        public IChart BuildChart()
        {
            var items = Data.GetOrderedItems();
            var valueTuples = items.ToList();
            var sum = valueTuples.Select(tuple => tuple.Item2).Sum();
            foreach (var (key, value) in valueTuples)
            {
                PercentageData[key] = value / sum * 100;
            }

            return this;
        }
    }
}