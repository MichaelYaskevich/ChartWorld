using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChartWorld.App;
using ChartWorld.UI;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.App
{
    [TestFixture]
    public class ButtonsFactoryTests
    {
        //TODO: поправить иерархию папок в тест
        private static ChartWindow Form { get; set; }
        private static ChartWorld.Domain.Workspace.Workspace Workspace { get; set; }
        private static List<PictureBox> Buttons { get; set; }

        [SetUp]
        public static void SetUp()
        {
            RefreshResources();
            Buttons = new List<PictureBox>()
            {
                ButtonsFactory.CreateClearButton(Form, Workspace),
                ButtonsFactory.CreateMoveButton(Workspace, Point.Empty),
                ButtonsFactory.CreateResizingButton(Workspace, Point.Empty)
            };
            var log = "";
            Buttons.Add(ButtonsFactory.CreateOpenButton(Form, Buttons,
                new List<Action>() {() => log += "1", () => log += "2"}));
        }

        private static void RefreshResources()
        {
            Workspace = new ChartWorld.Domain.Workspace.Workspace(null);
            Form = new ChartWindow(Workspace);
            Form.Controls.Clear();
        }

        [Test]
        public static void OpenButtonTest()
        {
            foreach (var button in Buttons)
                Form.Controls.Add(button);

            var log = "";
            ButtonsActions.OpenAction(Form, Buttons,
                new List<Action>() {() => log += "1", () => log += "2"});

            Form.Controls.Count.Should().Be(0);
            log.Should().Be("12");

            RefreshResources();
        }

        [Test]
        public static void SimpleButtonWithCorrectNameTest()
        {
            var button = ButtonsFactory.GetSimpleButton("tag", "clear_button.png", Point.Empty);

            button.SizeMode.Should().Be(PictureBoxSizeMode.StretchImage);
            button.Tag.Should().Be("tag");
            button.Location.Should().Be(Point.Empty);
            button.Size.Should().Be(new Size(50, 50));
        }

        [Test]
        public static void SimpleButtonWithWrongNameTest()
        {
            try
            {
                ButtonsFactory.GetSimpleButton("tag", "wrongName", Point.Empty);
                Assert.True(false);
            }
            catch (ArgumentException _)
            {
                Assert.True(true);
            }
            catch (Exception _)
            {
                Assert.True(false);
            }
        }

        [Test]
        public static void ButtonsShouldBeInRightFormatTest()
        {
            var tags = new[] {"ClearButton", "MoveButton", "OpenButton", "ResizingButton"};
            var locations = new[] {new Point(60, 0), Point.Empty, Point.Empty, Point.Empty};
            var count = 0;
            foreach (var button in Buttons.OrderBy(x => x.Tag))
            {
                button.SizeMode.Should().Be(PictureBoxSizeMode.StretchImage);
                button.Tag.Should().Be(tags[count]);
                button.Location.Should().Be(locations[count]);
                button.Size.Should().Be(new Size(50, 50));
                count++;
            }
        }
    }
}