using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChartWorld.Domain.Statistic;
using ChartWorld.Infrastructure;

namespace ChartWorld.Domain.Workspace
{
    public class Workspace : ICanMakeAction
    {
        private List<WorkspaceEntity> WorkspaceEntities { get; } = new();
        public ICommandsExecutor Executor { get; }
        public ICanMakeAction SelectedEntity { get; set; }
        public SelectionType SelectionType { get; private set; }
        public bool WasModified { get; set; }

        public Workspace(ICommandsExecutor executor)
        {
            Executor = executor;
        }

        public IEnumerable<WorkspaceEntity> GetWorkspaceEntities()
        {
            return WorkspaceEntities;
        }

        public void Select(ICanMakeAction entity, SelectionType type)
        {
            SelectedEntity = entity;
            SelectionType = type;
        }

        public void Move(int shiftX, int shiftY)
        {
            foreach (var entity in WorkspaceEntities)
                entity.Move(shiftX, shiftY);
            WasModified = true;
        }

        public bool TryResize(double factor)
        {
            foreach (var entity in WorkspaceEntities)
                if (!entity.CanResize(factor))
                    return false;
            foreach (var entity in WorkspaceEntities)
                entity.TryResize(factor);
            WasModified = true;
            return true;
        }

        public void Clear()
        {
            WorkspaceEntities.Clear();
        }

        public void Add(WorkspaceEntity entity)
        {
            if (!WorkspaceEntities.Contains(entity))
                WorkspaceEntities.Add(entity);
        }

        public void Delete(WorkspaceEntity entity)
        {
            WorkspaceEntities.Remove(entity);
            WasModified = true;
        }

        // public WorkspaceEntity Add(Control.ControlCollection controls, object entity, Size size, Point location,
        //     List<PictureBox> interactionButtons = null)
        // {
        //     var buttons = InitialiseButtons(controls, interactionButtons, location);
        //     var workspaceEntity = new WorkspaceEntity(entity, size, location, buttons);
        //     WorkspaceEntities.Add(workspaceEntity);
        //     buttons[0].Click += (_, _) =>
        //     {
        //         WorkspaceEntities.Remove(workspaceEntity);
        //         foreach (var button in workspaceEntity.InteractionButtons)
        //             controls.Remove(button);
        //         WasModified = true;
        //     };
        //     return workspaceEntity;
        // }

        private List<PictureBox> InitialiseButtons(Control.ControlCollection controls,
            List<PictureBox> interactionButtons, Point location)
        {
            var buttons = interactionButtons ?? new List<PictureBox>();
            var result = new List<PictureBox> {MakeCloseButton()};
            result.AddRange(buttons);
            var buttonsCount = 1;

            foreach (var button in result)
            {
                button.Location = new Point(location.X - 40, location.Y + 35 * (buttonsCount - 1));
                button.Size = new Size(30, 30);
                controls.Add(button);
                buttonsCount++;
            }

            return result;
        }

        //TODO: кнопки не тут
        private static PictureBox MakeCloseButton()
        {
            var button = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = "CloseButton",
                Image = new Bitmap(ResourceExplorer.PathToImages + "close_button.png"),
                Visible = true
            };
            return button;
        }

        public void Save()
        {
            // TODO: сохранение текущего состояния воркспейса в БД
        }

        public static void Retrieve()
        {
            // TODO: достать из БД
        }
    }
}