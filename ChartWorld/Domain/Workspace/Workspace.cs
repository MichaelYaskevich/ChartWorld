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
    }
}