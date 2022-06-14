using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChartWorld.Domain.Workspace
{
    public class WorkspaceEntity : IWorkspaceEntity, ICanMakeAction
    {
        public Size Size { get; set; }
        public Point Location { get; set; }
        public object Entity { get; }
        public List<PictureBox> InteractionButtons { get; }

        public WorkspaceEntity(object entity, Size size, Point location, List<PictureBox> interactionButtons = null)
        {
            Entity = entity;
            Size = size;
            Location = location;
            InteractionButtons = interactionButtons;
        }

        public void Move(int shiftX, int shiftY)
        {
            Location = new Point(
                Location.X + shiftX,
                Location.Y + shiftY);
            foreach (var button in InteractionButtons)
            {
                button.Location = new Point(
                    button.Location.X + shiftX,
                    button.Location.Y + shiftY);
            }
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