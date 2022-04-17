using System.Collections.Generic;

namespace ChartWorld.Statistic
{
    public interface IChartData
    {
        public SortedDictionary<string, double> ChartValues { get; }
        
        public void ModifyChartValues(string key, double newValue);
        
        public double GetSum();

        public double GetMedian();

        public double GetMax();

        public double GetMin();
    }
}