using System.Drawing;


namespace ChartWorld.Workspace
{
    public interface IWorkspaceEntity
    {
        Size Size { get; }
        Point Location { get; }
        object Entity { get; }
    }
}