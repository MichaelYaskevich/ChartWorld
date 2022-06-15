using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ChartWorld.App;
using ChartWorld.Domain.Workspace;

namespace ChartWorld.UI
{
    public static class EntityHandler
    {
        public static Form Form { get; set; }
        public static Dictionary<WorkspaceEntity, List<PictureBox>> Buttons { get; } = new();
        public static void AddButtons(WorkspaceEntity entity)
        {
            var buttons = new List<PictureBox>();
            AddStandartButtons(entity, buttons);
            if (entity is WorkspaceChart chart)
                AddStatisticButton(chart, buttons);

            for (var i = 0; i < buttons.Count; i++)
            {
                buttons[i].Location = new Point(
                    entity.Location.X - 40,
                    entity.Location.Y + 35 * i);
                buttons[i].Size = new Size(30, 30);
                Form.Controls.Add(buttons[i]);
            }
            Buttons.Add(entity, buttons);
        }

        private static void AddStandartButtons(WorkspaceEntity entity, List<PictureBox> buttons)
        {
            var closeButton = ButtonsFactory.CreateCloseButton();
            var moveButton = ButtonsFactory.CreateMoveButton();
            var resizingButton = ButtonsFactory.CreateResizingButton();

            closeButton.Click += (_, _) =>
            {
                entity.Workspace.Delete(entity);
                foreach (var button in buttons)
                    Form.Controls.Remove(button);
                Buttons.Remove(entity);
            };
            moveButton.Click += (_, _) => { entity.BecomeSelected(SelectionType.Move); };
            resizingButton.Click += (_, _) => { entity.BecomeSelected(SelectionType.Resize); };
            
            buttons.Add(closeButton);
            buttons.Add(moveButton);
            buttons.Add(resizingButton);
        }

        private static void AddStatisticButton(WorkspaceChart entity, List<PictureBox> buttons)
        {
            var statisticButton = ButtonsFactory.CreateStatisticButton();
            var statisticMethods = entity.GetStatisticMethods()
                .Select(x => (object)x)
                .ToArray();
            
            statisticButton.Click += (_, _) =>
            {
                var list = MakeDropDownList(entity.Location, statisticMethods);
                Form.Controls.Add(list);
                list.SelectedValueChanged += (_, _) =>
                {
                    var chosenMethod = list.SelectedItem?.ToString();
                    AddButtons(entity.ExecuteStatisticMethod(chosenMethod));
                    Form.Controls.Remove(list);
                    Form.Invalidate();
                    foreach (var entity in entity.Workspace.GetWorkspaceEntities())
                        Painter.Paint(entity, Form);
                };
            };
            buttons.Add(statisticButton);
        }
        
        private static ComboBox MakeDropDownList(Point location, object[] statisticMethods)
        {
            var list = new ComboBox();
            list.DropDownStyle = ComboBoxStyle.DropDownList;
            list.Size = new Size(200, 200);
            list.Location = location;
            list.Items.AddRange(statisticMethods);
            return list;
        }
    }
}