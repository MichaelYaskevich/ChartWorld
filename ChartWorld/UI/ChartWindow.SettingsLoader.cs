using System.Drawing;
using System.Windows.Forms;

namespace ChartWorld.UI
{
    public static class SettingsLoader
    {
        public static void LoadDefaultSettings(Form form)
        {
            SetScreenPosition(form);
            SetScreenSize(form);
            SetWindowState(form);
            SetBackgroundColor(form);
            SetWindowName(form);
        }

        private static void SetScreenPosition(Form form)
        {
        }

        private static void SetScreenSize(Form form)
        {
            form.Size = new Size(WindowInfo.ScreenSize.Width, WindowInfo.ScreenSize.Height);
        }

        private static void SetWindowState(Form form)
        {
            form.WindowState = FormWindowState.Maximized;
        }

        private static void SetBackgroundColor(Form form)
        {
            form.BackColor = Color.WhiteSmoke;
        }

        private static void SetWindowName(Form form)
        {
            form.Text = "ChartWorld";
        }
    }
}