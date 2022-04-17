using System.Collections.Generic;
using System.Drawing;

namespace ChartWorld.Workspace
{
    public interface IWorkspaceTool
    {
        public IWorkspaceEntity CreateEntity(Point point);
    }
}