using System;
using System.Collections.Generic;
using System.Drawing;
using ChartWorld.Chart;

namespace ChartWorld.Workspace
{
    public class Workspace
    {
        public Dictionary<IWorkspaceEntity, Point> EntityLocations { get; set; }

        public List<IChart> Charts;
    }
}