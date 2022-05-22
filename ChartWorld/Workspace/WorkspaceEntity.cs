using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChartWorld.App;

namespace ChartWorld.Workspace
{
    public class WorkspaceEntity : IWorkspaceEntity
    {
        public double SizeMultiplier { get; set; }
        public Point Location { get; set; }
        public object Entity { get; }
        public List<PictureBox> InteractionButtons { get; }

        public WorkspaceEntity(object entity, double sizeMultiplier, Point location, List<PictureBox> interactionButtons = null)
        {
            SizeMultiplier = sizeMultiplier;
            Entity = entity;
            Location = location;
            InteractionButtons = interactionButtons;
        }

        public Size GetSize()
        {
            return new Size((int)(WindowInfo.ScreenSize.Height * SizeMultiplier),
                (int)(WindowInfo.ScreenSize.Width * SizeMultiplier));
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