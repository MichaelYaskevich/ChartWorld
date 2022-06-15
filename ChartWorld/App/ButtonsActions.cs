using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.App
{
    public static class ButtonsActions
    {
        public static void OpenAction(Form form,
            List<PictureBox> controlButtons, List<Action> initializingActions)
        {
            foreach (var button in controlButtons
                .Where(button => form.Controls.Contains(button)))
                form.Controls.Remove(button);

            foreach (var action in initializingActions)
                action();

            form.Update();
        }

        public static void ClearAction(
            Control clearButton, Form form, Workspace workspace)
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
    }
}