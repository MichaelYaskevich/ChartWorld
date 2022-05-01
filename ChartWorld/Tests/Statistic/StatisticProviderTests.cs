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
        private static List<double> ValuesSample1 { get; set; }
        private static List<double> ValuesSample2 { get; set; }

        [SetUp]
        public static void SetUp()
        {
            ValuesSample1 = new List<double>
            {
                -1.0, -1, 2, 2, 3, 3, 3
            };
            ValuesSample2 = new List<double>()
            {
                2.0, 3, 7, 1, 4, 8
            };
        }

        [Test]
        public static void TryGetMedianTest()
        {
            var ok = StatisticProvider.TryGetMedian(ValuesSample1, out var actual);

            ok.Should().BeTrue();
            actual.Should().Be(2);
        }

        [Test]
        public static void TryGetMeanTest()
        {
            var expected = 11 / 7.0;

            var ok = StatisticProvider.TryGetMean(ValuesSample1, out var actual);

            ok.Should().BeTrue();
            actual.Should().BeInRange(expected - 0.000001, expected + 0.000001);
        }

        [Test]
        public static void TryGetStdTest()
        {
            var expected = 1.812653;

            var ok = StatisticProvider.TryGetStd(ValuesSample1, out var actual);

            ok.Should().BeTrue();
            actual.Should().BeInRange(expected - 0.000001, expected + 0.000001);
        }

        [Test]
        public static void AutocorrelationTest()
        {
            var actual = StatisticProvider.GetAutocorrelation(ValuesSample2);
            var expected = -0.309084;

            actual.Should().BeInRange(expected - 0.000001, expected + 0.000001);
        }

        [Test]
        public static void BetweenStatTest()
        {
            var actual = StatisticProvider.GetBetweenStat(ValuesSample2, 3, 7);

            actual.Should().Equal(false, true, true, false, true, false);
        }

        [Test]
        public static void ClipTest()
        {
            var actual = StatisticProvider.Clip(ValuesSample2, 3, 7);

            actual.Should().Equal(3, 3, 7, 3, 4, 7);
        }

        [Test]
        public static void TryClipTest()
        {
            StatisticProvider.TryClip(
                    ValuesSample2.ToArray(),
                    new[] {1.0, 1, 1, 2, 1, 1},
                    new[] {8.0, 8, 6, 8, 8, 8},
                    out var actual
                ).Should()
                .BeTrue();

            actual.Should().Equal(2, 3, 6, 2, 4, 8);
        }

        [Test]
        public static void TryGetExpectationTest()
        {
            var y = new List<double>() {1 / 6.0, 1 / 6.0, 1 / 6.0, 1 / 6.0, 1 / 6.0, 1 / 6.0};
            var z = new List<double>() {1.0, 1, 1, 1, 1, 1};
            var expected = 4.16666;

            var okY = StatisticProvider.TryGetExpectation(ValuesSample2, y, out var actual);
            var okZ = StatisticProvider.TryGetExpectation(ValuesSample2, z, out _);

            okY.Should().BeTrue();
            okZ.Should().BeFalse();
            actual.Should().BeInRange(expected - 0.00001, expected + 0.00001);
        }

        [Test]
        public static void GetExpectationTest()
        {
            var expected = 4.16666;

            var actual = StatisticProvider.GetExpectation(ValuesSample2);

            actual.Should().BeInRange(expected - 0.00001, expected + 0.00001);
        }

        [Test]
        public static void GetCovarianceTest()
        {
            var y = new List<double>() {3.0, 3, 2, 4, 8, 1};
            var expected = -2.9;

            var actual = StatisticProvider.GetCovariance(ValuesSample2, y);

            actual.Should().BeInRange(expected - 0.00001, expected + 0.00001);
        }

        [Test]
        public static void GetCummaxTest()
        {
            var actual = StatisticProvider.GetCummax(ValuesSample2);

            actual.Should().Equal(2.0, 3, 7, 7, 7, 8);
        }

        [Test]
        public static void GetCumminTest()
        {
            var x = new List<double>() {8.0, 4, 1, 7, 3, 2};

            var actual = StatisticProvider.GetCummin(x);

            actual.Should().Equal(8.0, 4, 1, 1, 1, 1);
        }
        
        [Test]
        public static void GetCumprodTest()
        {
            var actual = StatisticProvider.GetCumprod(ValuesSample2);

            actual.Should().Equal(2.0, 6, 42, 42, 168, 1344);
        }
        
        [Test]
        public static void GetCumsumTest()
        {
            var actual = StatisticProvider.GetCumsum(ValuesSample2);

            actual.Should().Equal(2.0, 5, 12, 13, 17, 25);
        }
    }
}