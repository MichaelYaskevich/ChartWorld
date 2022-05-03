using ChartWorld.Statistic;
using FluentAssertions;
using NUnit.Framework;

namespace ChartWorld.Tests.Statistic
{
    [TestFixture]
    public static class ChartDataTests
    {
        [TestCase("ChartWorld.Tests.Resources.ChartDataWithOneValueForName.csv",
            new[]
            {
                "January", "February", "March", "April",
                "May", "June", "July", "August",
                "September", "October", "November", "December"
            },
            new[] {5.0, 15, 5, 10, 15, 25, 30, 40, 60, 70, 50, 30},
            TestName = "ChartDataWithOneValueForName")]
        [TestCase("ChartWorld.Tests.Resources.ChartDataWithManyValuesForName.csv",
            new[] {"Winter#1", "Winter#2", "Winter#3", 
                "Spring#1", "Spring#2", "Spring#3", 
                "Summer#1", "Summer#2", "Summer#3", 
                "Autumn#1", "Autumn#2", "Autumn#3"},
            new[] {30.0, 5, 15, 5, 10, 15, 25, 30, 40, 60, 70, 50},
            TestName = "ChartDataWithManyValuesForName")]
        public static void ChartDataCtorParseCsvRight(string csvPath, string[] keys, double[] values)
        {
            var chartData = new ChartData(csvPath);

            chartData.GetOrderedItems().Should().HaveCount(values.Length);
            chartData.GetOrderedKeys().Should().Equal(keys);
            chartData.GetOrderedValues().Should().Equal(values);
        }
    }
}