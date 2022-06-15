using System.Collections.Generic;
using System.Drawing;
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
            Workspace = new ChartWorld.Domain.Workspace.Workspace(null);
            Form = new ChartWindow(Workspace);
            Form.Controls.Clear();
        }

        [Test]
        public static void SelectTest()
        {
            var entity = new WorkspaceEntity(Workspace, "entity1", Size.Empty, Point.Empty);
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
            var entity1 = new WorkspaceEntity(Workspace, "entity1", Size.Empty, Point.Empty);
            var entity2 = new WorkspaceEntity(Workspace, "entity2", Size.Empty, Point.Empty);

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
            var workspace = new ChartWorld.Domain.Workspace.Workspace(null);
            var form = new ChartWindow(workspace);
            
            new WorkspaceEntity(workspace, "entity1", Size.Empty, Point.Empty);
            new WorkspaceEntity(workspace, "entity2", Size.Empty, Point.Empty);
            workspace.Clear();

            workspace.GetWorkspaceEntities().Should().BeEmpty();

            RefreshResources();
        }

        [Test]
        public static void ResizeTest()
        {
            var bigEntity = new WorkspaceEntity(Workspace, "entity1", new Size(300, 300), Point.Empty);
            var smallEntity = new WorkspaceEntity(Workspace, "entity1", new Size(200, 200), Point.Empty);

            Workspace.TryResize(2);
            bigEntity.Size.Should().Be(new Size(600, 600));
            smallEntity.Size.Should().Be(new Size(400, 400));

            Workspace.TryResize(2).Should().BeFalse();
            Workspace.TryResize(1.0 / 5).Should().BeFalse();

            RefreshResources();
        }
    }
}