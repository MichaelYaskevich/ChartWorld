using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChartWorld.Workspace
{
    public class WorkspaceEntity : IWorkspaceEntity
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
    }
}