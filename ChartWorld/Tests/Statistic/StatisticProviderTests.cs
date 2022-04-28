using System.Collections.Generic;
using System.Linq;
using ChartWorld.Statistic;
using FluentAssertions;
using NUnit.Framework;

namespace ChartWorld.Tests.Statistic
{
    [TestFixture]
    public static class StatisticProviderTests
    {
        [Test]
        public static void GetItemsWithMinValueTest()
        {
            var chartData = new ChartData(
                new List<(string, double)>
                {
                    ("a", 1.0),
                    ("b", 1),
                    ("c", 2)
                });
            
            var actual = StatisticProvider.GetItemsWithMinValue(chartData).ToArray();

            var expected = new List<(string, double)>
            {
                ("a", 1),
                ("b", 1)
            };
            actual.Should().HaveCount(expected.Count);
            actual.Should().Equal(expected);
        }

        [Test]
        public static void GetItemsWithMaxValueTest()
        {
            var chartData = new ChartData(
                new List<(string, double)>
                {
                    ("a", 1.0),
                    ("b", 2),
                    ("c", 2)
                });
            var result = StatisticProvider.GetItemsWithMaxValue(chartData).ToArray();

            var res = new List<(string, double)>
            {
                ("b", 2),
                ("c", 2)
            };
            result.Should().HaveCount(res.Count);
            result.Should().Equal(res);
        }

        [Test]
        public static void GetItemsWithSameValueTest()
        {
            var chartData = new ChartData(
                new List<(string, double)>
                {
                    ("a", 1.0),
                    ("b", 2),
                    ("c", 2),
                    ("d", 3)
                });
            var res = new List<(string, double)>
            {
                ("b", 2),
                ("c", 2)
            };

            var result = StatisticProvider.GetItemsWithSameValue(chartData, res[0].Item2).ToArray();

            result.Should().HaveCount(res.Count);
            result.Should().Equal(res);
        }

        [Test]
        public static void TryGetMedianTest()
        {
            
        }
    }
}