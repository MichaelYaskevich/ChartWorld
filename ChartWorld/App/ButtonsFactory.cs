using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChartWorld.Workspace;

namespace ChartWorld.App
{
    public static class ButtonsFactory
    {
        public static PictureBox CreateOpenButton(
            ChartWindow form, List<PictureBox> controlButtons, List<Action> initializingActions)
        {
            var openButton = GetSimplePictureBox(
                "OpenButton", "open_button.png", new Point(0, 0));
            openButton.Click += (_, _) => 
                OpenAction(form, controlButtons, initializingActions);

            return openButton;
        }

        private static void OpenAction(ChartWindow form, 
            List<PictureBox> controlButtons, List<Action> initializingActions)
        {
            foreach (var button in controlButtons
                         .Where(button => form.Controls.Contains(button)))
                form.Controls.Remove(button);

            foreach (var action in initializingActions)
                action();
                
            form.Update();
        }

        public static PictureBox CreateClearButton(ChartWindow form, Workspace.Workspace workspace)
        {
            var clearButton = GetSimplePictureBox(
                "ClearButton", "clear_button.png", new Point(60, 0));
            clearButton.Click += (_, _) => 
                ClearAction(clearButton, form, workspace);

            return clearButton;
        }

        private static void ClearAction(
            Control clearButton, ChartWindow form, Workspace.Workspace workspace)
        {
            PictureBox openButton = null;
            foreach (var control in form.Controls)
                if (control is PictureBox pictureBox 
                    && pictureBox.Tag.ToString() == "OpenButton")
                    openButton = pictureBox;
            form.Controls.Clear();
            form.Controls.Add(clearButton);
            if (openButton != null)
                form.Controls.Add(openButton);
            workspace.Clear();
            form.Invalidate();
        }

        public static PictureBox CreateMoveButton(Workspace.Workspace workspace, Point location)
        {
            var moveButton = GetSimplePictureBox(
                "MoveButton", "move_button.png", location);
            moveButton.Click += (_, _) => 
                workspace.Select(workspace, SelectionType.Move);

            return moveButton;
        }
        
        public static PictureBox CreateResizingButton(Workspace.Workspace workspace, Point location)
        {
            var resizeButton = GetSimplePictureBox(
                "ResizeButton", "resizing_button.png", location);
            resizeButton.Click += (_, _) => 
                workspace.Select(workspace, SelectionType.Resize);

            return resizeButton;
        }

        public static PictureBox GetSimplePictureBox(string tag, string pictureName, Point location)
        {
            return new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = tag,
                Image = new Bitmap(HelpMethods.PathToImages + pictureName),
                Location = location,
                Size = new Size(50, 50)
            };
        }
    }
}