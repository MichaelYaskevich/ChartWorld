using System.Drawing;
using System.Windows.Forms;

namespace ChartWorld.Application
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