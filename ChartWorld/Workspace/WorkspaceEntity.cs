using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ChartWorld.Workspace
{
    public class WorkspaceEntity : IWorkspaceEntity
    {
        public Size Size { get; set; }
        public Point Location { get; set; }
        public Color Color { get; set; }
        public object Entity { get; }
        public List<PictureBox> InteractionButtons { get; }

        public WorkspaceEntity(object entity, Size size, Point location, Color color = default, List<PictureBox> interactionButtons = null)
        {
            Entity = entity;
            Size = size;
            Location = location;
            Color = color;
            InteractionButtons = interactionButtons ?? new List<PictureBox>();
            var buttonsCount = 0;
            foreach (var button in InteractionButtons)
            {
                button.Location = new Point(Location.X, Location.Y + 25 * buttonsCount);
                button.Size = new Size(20, 20);
                buttonsCount++;
            }
        }
    }
}