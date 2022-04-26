using ChartWorld.Statistic;
using NUnit.Framework;

namespace ChartWorld.Tests.Statistic
{
    [TestFixture]
    public static class ChartDataTests
    {
        [Test]
        public static void Test1()
        {
            var chartData = new ChartData(
                "D:\\The_p\\prog\\c#\\ChartWorld\\ChartWorld\\Tests\\Resources\\ChartDataWithOneValueForName.csv");
            
        }
    }
}