using System.Drawing;
using System.Windows.Forms;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.UI
{
    public static class ToolsForActions
    {
        public static void MakeEntityAction(Keys keyCode, ICanMakeAction entity, SelectionType type)
        {
            switch (type)
            {
                case SelectionType.Move:
                    MakeMoveAction(keyCode, entity);
                    break;
                case SelectionType.Resize:
                    MakeResizeAction(keyCode, entity);
                    break;
            }
        }

        private static void MoveButtons(ICanMakeAction entity, int shiftX, int shiftY)
        {
            if (entity is Workspace workspace)
            {
                foreach (var wsEntity in workspace.GetWorkspaceEntities())
                    MoveButtons(wsEntity, shiftX, shiftY);
            }
            else if (entity is WorkspaceEntity wsEntity)
            {
                var buttons = EntityHandler.Buttons[wsEntity];
                foreach (var button in buttons)
                {
                    button.Location = new Point(
                        button.Location.X + shiftX,
                        button.Location.Y + shiftY);
                }
            }
        }

        private static void MakeMoveAction(Keys keyCode, ICanMakeAction entity)
        {
            var (shiftX, shiftY) = (0, 0);
            var shouldMove = true;
            switch (keyCode)
            {
                case Keys.Up:
                    (shiftX, shiftY) = (0, -10);
                    break;
                case Keys.Down:
                    (shiftX, shiftY) = (0, 10);
                    break;
                case Keys.Left:
                    (shiftX, shiftY) = (-10, 0);
                    break;
                case Keys.Right:
                    (shiftX, shiftY) = (10, 0);
                    break;
                default:
                    shouldMove = false;
                    //TODO: показывать информацию о том какие кнопки нажать
                    break;
            }

            if (shouldMove)
            {
                entity.Move(shiftX, shiftY);
                MoveButtons(entity, shiftX, shiftY);
            }
        }

        private static void MakeResizeAction(Keys keyCode, ICanMakeAction entity)
        {
            switch (keyCode)
            {
                case Keys.Up:
                    entity.TryResize(1.1);
                    break;
                case Keys.Down:
                    entity.TryResize(0.90909);
                    break;
            }
        }
    }
}