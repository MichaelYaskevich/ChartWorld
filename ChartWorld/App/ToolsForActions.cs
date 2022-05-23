using System.Windows.Forms;
using ChartWorld.Workspace;

namespace ChartWorld.App
{
    public static class ToolsForActions
    {
        public static void MakeEntityAction(Keys keyCode, WorkspaceEntity entity, SelectionType type)
        {
            switch (type)
            {
                case SelectionType.Move:
                    switch (keyCode)
                    {
                        case Keys.Up:
                            entity.Move(0, -10);
                            break;
                        case Keys.Down:
                            entity.Move(0, 10);
                            break;
                        case Keys.Left:
                            entity.Move(-10, 0);
                            break;
                        case Keys.Right:
                            entity.Move(10, 0);
                            break;
                    }
                    break;
                case SelectionType.Resize:
                    switch (keyCode)
                    {
                        case Keys.Up:
                            entity.Move(0, -10);
                            break;
                        case Keys.Down:
                            entity.Move(0, 10);
                            break;
                        case Keys.Left:
                            entity.Move(-10, 0);
                            break;
                        case Keys.Right:
                            entity.Move(10, 0);
                            break;
                    }
                    break;
            }
        }

        public static void MakeWorkspaceAction(Keys keyCode, Workspace.Workspace workspace)
        {
            switch (keyCode)
            {
                case Keys.Up:
                    workspace.Move(0, -10);
                    break;
                case Keys.Down:
                    workspace.Move(0, 10);
                    break;
                case Keys.Left:
                    workspace.Move(-10, 0);
                    break;
                case Keys.Right:
                    workspace.Move(10, 0);
                    break;
            }
        }
    }
}