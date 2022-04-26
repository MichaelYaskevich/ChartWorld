using System.Drawing;
using System.Windows.Forms;

namespace ChartWorld.Application
{
    partial class ChartWindow
    {
        private readonly Rectangle _screen = Screen.PrimaryScreen.WorkingArea;

        private void SetScreenPosition()
        {
            //Location = new Point((screen.Width - w) / 2, (screen.Height - h) / 2);
        }

        private void SetScreenSize()
        {
            Size = new Size(_screen.Width, _screen.Height);
        }

        private void SetWindowState()
        {
            WindowState = FormWindowState.Maximized;
        }
    }
}