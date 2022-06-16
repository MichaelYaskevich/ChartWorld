using System;
using System.Collections.Generic;

namespace ChartWorld.Infrastructure
{
    public interface IParser
    {
        public (List<string>, IEnumerable<(string, double)>) Parse(string path);
    }
}