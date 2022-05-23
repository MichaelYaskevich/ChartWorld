using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChartWorld.App;

namespace ChartWorld.Workspace
{
    public class Workspace
    {
        private List<WorkspaceEntity> WorkspaceEntities { get; } = new();
        public WorkspaceEntity SelectedEntity { get; set; }
        public SelectionType SelectionType { get; set; }
        public bool WasModified { get; set; } = false;

        public IEnumerable<WorkspaceEntity> GetWorkspaceEntities()
        {
            return WorkspaceEntities;
        }

        public void Select(WorkspaceEntity entity, SelectionType type)
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

        public Workspace()
        {
            // TODO: Retrieve()
        }

        public void Clear()
        {
            WorkspaceEntities.Clear();
        }

        public WorkspaceEntity Add(Control.ControlCollection controls, object entity, Size size, Point location, 
            List<PictureBox> interactionButtons = null)
        {
            var buttons = InitialiseButtons(controls, interactionButtons, location);
            var workspaceEntity = new WorkspaceEntity(entity, size, location, buttons);
            WorkspaceEntities.Add(workspaceEntity);
            buttons[0].Click += (sender, args) =>
            {
                WorkspaceEntities.Remove(workspaceEntity);
                foreach (var button in workspaceEntity.InteractionButtons)
                    controls.Remove(button);
                WasModified = true;
            };
            return workspaceEntity;
        }
        
        private List<PictureBox> InitialiseButtons(Control.ControlCollection controls, List<PictureBox> interactionButtons, Point location)
        {
            var buttons = interactionButtons ?? new List<PictureBox>();
            var result = new List<PictureBox>() { MakeCloseButton() };
            result.AddRange(buttons);
            var buttonsCount = 1;
            
            foreach (var button in result)
            {
                button.Location = new Point(location.X - 40, location.Y + 35 * buttonsCount);
                button.Size = new Size(30, 30);
                controls.Add(button);
                buttonsCount++;
            }

            return result;
        }
        
        public static PictureBox MakeCloseButton()
        {
            var button = new PictureBox()
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Tag = "CloseButton",
                Image = new Bitmap(HelpMethods.PathToImages + "close_button.png"),
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