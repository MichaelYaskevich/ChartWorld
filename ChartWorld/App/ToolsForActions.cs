using System.Windows.Forms;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.App
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

        private static void MakeMoveAction(Keys keyCode, ICanMakeAction entity)
        {
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
                default:
                    //TODO: показывать информацию о том какие кнопки нажать
                    break;
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