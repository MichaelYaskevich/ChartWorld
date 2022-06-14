using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChartWorld.Domain.Workspace;
using ChartWorld.Infrastructure;

namespace ChartWorld.UI
{
    public static class ButtonsFactory
    {
        public static PictureBox CreateOpenButton(
            ChartWindow form, List<PictureBox> controlButtons, List<Action> initializingActions)
        {
            var openButton = GetSimpleButton(
                "OpenButton", "open_button.png", new Point(0, 0));
            openButton.Click += (_, _) =>
                ButtonsActions.OpenAction(form, controlButtons, initializingActions);

            return openButton;
        }

        public static PictureBox CreateClearButton(ChartWindow form, Workspace workspace)
        {
            var clearButton = GetSimpleButton(
                "ClearButton", "clear_button.png", new Point(60, 0));
            clearButton.Click += (_, _) =>
                ButtonsActions.ClearAction(clearButton, form, workspace);

            return clearButton;
        }

        public static PictureBox CreateMoveButton(Workspace workspace, Point location)
        {
            var moveButton = GetSimpleButton(
                "MoveButton", "move_button.png", location);
            moveButton.Click += (_, _) =>
                workspace.Select(workspace, SelectionType.Move);

            return moveButton;
        }

        public static PictureBox CreateResizingButton(Workspace workspace, Point location)
        {
            var resizeButton = GetSimpleButton(
                "ResizingButton", "resizing_button.png", location);
            resizeButton.Click += (_, _) =>
                workspace.Select(workspace, SelectionType.Resize);

            return resizeButton;
        }

        public static PictureBox GetSimpleButton(string tag, string name, Point location)
        {
            return new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = tag,
                Image = new Bitmap(ResourceExplorer.PathToImages + name),
                Location = location,
                Size = new Size(50, 50),
                BackColor = Color.Transparent
            };
        }
    }
}