using System.Collections.Generic;
using System.Linq;
using ChartWorld.Statistic;
using FluentAssertions;
using NUnit.Framework;


namespace ChartWorld.Tests.Statistic
{
    [TestFixture]
    public static class ChartDataExtensionsTests
    {
        private static ChartData ChartData { get; set; }

        [SetUp]
        public static void SetUp()
        {
            ChartData = new ChartData(
                new List<(string, double)>
                {
                    ("a", -1.0),
                    ("b", -1),
                    ("c", 2),
                    ("d", 2),
                    ("e", 3),
                    ("f", 3),
                    ("g", 3)
                });
        }

        [Test]
        public static void GetItemsWithMinValueTest()
        {
            var actual = ChartData.GetItemsWithMinValue().ToArray();

            var expected = new[]
            {
                ("a", -1.0),
                ("b", -1)
            };
            actual.Should().HaveCount(expected.Length);
            actual.Should().Equal(expected);
        }

        [Test]
        public static void GetItemsWithMaxValueTest()
        {
            var actual = ChartData.GetItemsWithMaxValue().ToArray();

            var expected = new[]
            {
                ("e", 3.0),
                ("f", 3),
                ("g", 3)
            };
            actual.Should().HaveCount(expected.Length);
            actual.Should().Equal(expected);
        }

        [Test]
        public static void GetItemsWithSameValueTest()
        {
            var expected = new[]
            {
                ("c", 2.0),
                ("d", 2)
            };

            var actual = ChartData.GetItemsWithSameValue(expected[0].Item2).ToArray();

            actual.Should().HaveCount(expected.Length);
            actual.Should().Equal(expected);
        }

        [Test]
        public static void AbsTest()
        {
            var actual = ChartData.Abs();

            actual.GetOrderedValues().Should().Equal(1.0, 1, 2, 2, 3, 3, 3);
        }

        [Test]
        public static void ClipTest()
        {
            var actual = ChartData.Clip(0, 3);

            actual.GetOrderedValues().Should().Equal(0, 0, 2, 2, 3, 3, 3);
        }

        [Test]
        public static void TryClipTest()
        {
            ChartData.TryClip(
                    new[] {-1.0, 0, 0, 0, 0, 0, 0},
                    new[] {3.0, 3, 3, 3, 3, 3, 2},
                    out var actual
                ).Should()
                .BeTrue();

            actual.GetOrderedValues().Should().Equal(-1.0, 0, 2, 2, 3, 3, 2);
        }
    }
}