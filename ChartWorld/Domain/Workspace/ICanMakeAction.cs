namespace ChartWorld.Domain.Workspace
{
    public interface ICanMakeAction
    {
        public void Move(int shiftX, int shiftY);
        public bool TryResize(double factor);
    }
}