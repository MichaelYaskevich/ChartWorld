using System.Drawing;

namespace ChartWorld.Workspace
{
    public class WorkspaceEntity : IWorkspaceEntity
    {
        public Size Size { get; set; }
        public Point Location { get; set; }
        public Color Color { get; set; }
        public object Entity { get; }

        public WorkspaceEntity(object entity, Size size, Point location, Color color = default)
        {
            Entity = entity;
            Size = size;
            Location = location;
            Color = color;
        }
    }
}