using System.Collections.Generic;
using System.Linq;
using ChartWorld;
using ChartWorld.App;
using ChartWorld.Domain.Workspace;
using ChartWorld.UI;
using FluentAssertions;
using NUnit.Framework;


namespace Tests.App
{
    [TestFixture]
    public class WorkspaceEntityFactoryTests
    {
        [Test]
        public static void GetAvailableCommandNameTest()
        {
            var allNamesExpected = new List<string>()
            {
                "Абсолютные значения", "Кумулятивный максимум", "Кумулятивный минимум",
                "Кумулятивное произведение","Кумулятивная сумма", "Математическое ожидание",
                "Элементы с максимальным значением", "Элементы с минимальным значением"
            };
            var executor = Program.CreateExecutor();
            
            var allNamesActual = executor.GetAvailableCommandNames();

            allNamesActual.Should().NotBeEmpty();
            allNamesActual.Should().Equal(allNamesExpected);
        }
    }
}