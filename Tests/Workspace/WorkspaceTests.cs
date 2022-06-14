using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChartWorld.App;
using ChartWorld.Domain.Workspace;
using ChartWorld.UI;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Workspace
{
    [TestFixture]
    public static class WorkspaceTests
    {
        private static ChartWorld.Domain.Workspace.Workspace Workspace { get; set; }
        private static ChartWindow Form { get; set; }

        [SetUp]
        public static void RefreshResources()
        {
            Workspace = new ChartWorld.Domain.Workspace.Workspace();
            Form = new ChartWindow(Workspace);
            Form.Controls.Clear();
        }

        [Test]
        public static void AddTest()
        {
            var emptyButton = new PictureBox();
            var buttons = new List<PictureBox>() {emptyButton};
            var entity = Workspace.Add(Form.Controls, "entity", Size.Empty, Point.Empty, buttons);

            emptyButton.Location.Should().Be(new Point(-40, 35));
            emptyButton.Size.Should().Be(new Size(30, 30));
            Form.Controls.Count.Should().Be(2);

            var closeButton = Form.Controls[0];
            closeButton.Location.Should().Be(new Point(-40, 0));
            closeButton.Size.Should().Be(new Size(30, 30));
            closeButton.Tag.Should().Be("CloseButton");

            var entities = Workspace.GetWorkspaceEntities().ToList();

            entities.Should().HaveCount(1);
            entities.Should().Contain(entity);

            RefreshResources();
        }

        [Test]
        public static void SelectTest()
        {
            var entity = Workspace.Add(Form.Controls, "entity", Size.Empty, Point.Empty);
            var types = new List<SelectionType>() {SelectionType.Move, SelectionType.Resize};
            foreach (var type in types)
            {
                Workspace.Select(entity, type);

                Workspace.SelectedEntity.Should().Be(entity);
                Workspace.SelectionType.Should().Be(type);
            }

            RefreshResources();
        }

        [Test]
        public static void MoveTest()
        {
            var entity1 = Workspace.Add(Form.Controls, "entity1", Size.Empty, Point.Empty);
            var entity2 = Workspace.Add(Form.Controls, "entity2", Size.Empty, Point.Empty);

            Workspace.Move(1, 0);

            entity1.Location.Should().Be(new Point(1, 0));
            entity2.Location.Should().Be(new Point(1, 0));

            Workspace.Move(0, -1);

            entity1.Location.Should().Be(new Point(1, -1));
            entity2.Location.Should().Be(new Point(1, -1));

            RefreshResources();
        }

        [Test]
        public static void ClearTest()
        {
            var workspace = new ChartWorld.Domain.Workspace.Workspace();
            var form = new ChartWindow(workspace);
            workspace.Add(form.Controls, "entity1", Size.Empty, Point.Empty);
            workspace.Add(form.Controls, "entity2", Size.Empty, Point.Empty);

            workspace.Clear();

            workspace.GetWorkspaceEntities().Should().BeEmpty();

            RefreshResources();
        }

        [Test]
        public static void ResizeTest()
        {
            var bigEntity = Workspace.Add(Form.Controls, "entity1", new Size(300, 300), Point.Empty);
            var smallEntity = Workspace.Add(Form.Controls, "entity2", new Size(200, 200), Point.Empty);

            Workspace.TryResize(2);
            bigEntity.Size.Should().Be(new Size(600, 600));
            smallEntity.Size.Should().Be(new Size(400, 400));

            Workspace.TryResize(2).Should().BeFalse();
            Workspace.TryResize(1.0 / 5).Should().BeFalse();

            RefreshResources();
        }
    }
}