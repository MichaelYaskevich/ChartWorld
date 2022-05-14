using System;
using System.Collections.Generic;
using System.Drawing;
using ChartWorld.Chart;

namespace ChartWorld.Workspace
{
    public class Workspace
    {
        private List<WorkspaceEntity> WorkspaceEntities { get; } = new();

        public Workspace()
        {
            // TODO: Retrieve()
        }

        public void Add(WorkspaceEntity entity)
        {
            WorkspaceEntities.Add(entity);
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