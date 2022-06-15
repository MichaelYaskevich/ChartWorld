using System.Drawing;

namespace ChartWorld.Domain.Workspace
{
    public class WorkspaceEntity : IWorkspaceEntity, ICanMakeAction
    {
        public Size Size { get; set; }
        public Point Location { get; set; }
        public object Entity { get; }
        public Workspace Workspace { get; }

        public WorkspaceEntity(Workspace workspace, object entity, Size size, Point location)
        {
            Workspace = workspace;
            Workspace.Add(this);
            Entity = entity;
            Size = size;
            Location = location;
        }

        public void BecomeSelected(SelectionType type)
        {
            Workspace.Select(this, type);
        }

        public void Move(int shiftX, int shiftY)
        {
            Location = new Point(
                Location.X + shiftX,
                Location.Y + shiftY);
        }

        public bool CanResize(Size size)
        {
            return size.Width is >= 100 and <= 1000
                   && size.Height is >= 100 and <= 1000;
        }

        public bool TryResize(Size size)
        {
            if (!CanResize(size))
                return false;
            Size = size;
            return true;
        }

        public bool CanResize(double factor)
        {
            return (int) (Size.Width * factor) is >= 100 and <= 1000
                   && (int) (Size.Height * factor) is >= 100 and <= 1000;
        }

        public bool TryResize(double factor)
        {
            var size = new Size(
                (int) (Size.Width * factor),
                (int) (Size.Height * factor));
            if (!CanResize(size))
                return false;
            Size = size;
            return true;
        }
    }
}