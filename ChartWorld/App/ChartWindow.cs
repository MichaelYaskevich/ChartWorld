using System.Windows.Forms;

namespace ChartWorld.App
{
    public partial class ChartWindow : Form
    {
        public ChartWindow()
        {
            InitializeComponent();
            SetScreenSize();
            SetScreenPosition();
            SetWindowState();
        }
    }
}