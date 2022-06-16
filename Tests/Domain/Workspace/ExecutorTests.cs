using System.Collections.Generic;
using ChartWorld;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Domain.Workspace
{
    [TestFixture]
    public class ExecutorTests
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
            var executor = Configurator.CreateExecutor();
            
            var allNamesActual = executor.GetAvailableCommandNames();

            allNamesActual.Should().NotBeEmpty();
            allNamesActual.Should().Equal(allNamesExpected);
        }
    }
}