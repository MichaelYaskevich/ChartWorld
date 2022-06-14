using System.Drawing;
using System.Windows.Forms;

namespace ChartWorld.UI
{
    public static class SettingsLoader
    {
        public static void LoadDefaultSettings(Form form)
        {
            form.Size = new Size(Painter.ScreenSize.Width, Painter.ScreenSize.Height);
            form.WindowState = FormWindowState.Maximized;
            form.BackColor = Color.WhiteSmoke;
            form.Text = "Chart World";
        }
    }
}