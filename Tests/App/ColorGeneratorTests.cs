using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using ChartWorld.App;
using ChartWorld.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.App
{
    [TestFixture]
    public class ColorGeneratorTests
    {
        [Test]
        public static void GeneratingColorsFailsWhenGettingMoreThanMethodCanProvideAtOnce()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ColorGenerator.GetColors(int.MaxValue));
        }

        [Test]
        public static void GeneratingColorsFailsWhenGettingMoreColorsThanMethodCanProvide()
        {
            Assert.DoesNotThrow(() => ColorGenerator.GetColors(ColorGenerator.MaxColorsCount));
            Assert.Throws<ArgumentOutOfRangeException>(() => ColorGenerator.GetColors(1));
        }

        [Test]
        public static void GeneratingColorsDoesNotReturnSameColors()
        {
            var colors = ColorGenerator.GetColors(ColorGenerator.MaxColorsCount);
            var distinctColors = colors.Distinct().ToArray();
            Assert.AreEqual(colors.Length, distinctColors.Length);
        }
    }
}