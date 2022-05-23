using System.Drawing;

namespace ChartWorld.Workspace
{
    public interface ICanMakeAction
    {
        public void Move(int shiftX, int shiftY);
        public bool TryResize(double factor);
    }
}