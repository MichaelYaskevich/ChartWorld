using System.Drawing;


namespace ChartWorld.Workspace
{
    public interface IWorkspaceEntity
    {
        Size GetSize();
        double SizeMultiplier { get; set; }
        Point Location { get; }
        object Entity { get; }
    }
}