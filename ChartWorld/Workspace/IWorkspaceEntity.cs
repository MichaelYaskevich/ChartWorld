using System.Drawing;


namespace ChartWorld.Workspace
{
    public interface IWorkspaceEntity
    {
        Size Size { get; }
        Point Location { get; }
        Color Color { get; }
        object Entity { get; }
    }
}