using System.Collections.Generic;
using System.Linq;
using ChartWorld.App;
using FluentAssertions;
using NUnit.Framework;


namespace Tests.App
{
    [TestFixture]
    public class WorkspaceEntityFactoryTests
    {
        [Test]
        public static void GetStatisticMethodsTest()
        {
            var allMethodsExpected = new List<string>()
            {
                "GetItemsWithMaxValue", "GetItemsWithMinValue", "GetItemsWithSameValue", "TryGetMedian", "TryGetMean",
                "TryGetStd", "Abs", "Autocorrelation", "Clip", "TryClip", "GetExpectation", "GetCumulativeMax", "GetCumulativeMin",
                "GetCumulativeProd", "GetCumulativeSum"
            };
            
            var withoutParametersExpected = new List<string>()
            {
                "GetItemsWithMaxValue", "GetItemsWithMinValue", "Abs", "GetExpectation", 
                "GetCumulativeMax", "GetCumulativeMin", "GetCumulativeProd", "GetCumulativeSum"
            };
            
            var allMethodsActual = WorkspaceEntityFactory
                .GetStatisticMethods()
                .Select(x => x.Name)
                .ToArray();
            var withoutParametersActual = WorkspaceEntityFactory
                .GetStatisticMethods()
                .Where(x => x.GetParameters().Length == 1)
                .Select(x => x.Name)
                .ToArray();
            
            allMethodsActual.Should().NotBeEmpty();
            allMethodsActual.Should().Equal(allMethodsExpected);

            withoutParametersActual.Should().NotBeEmpty();
            withoutParametersActual.Should().Equal(withoutParametersExpected);
        }
    }
}