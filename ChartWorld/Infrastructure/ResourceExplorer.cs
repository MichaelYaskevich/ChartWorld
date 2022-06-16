using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChartWorld.Infrastructure
{
    public static class ResourceExplorer
    {
        public static readonly string PathToProject = GetRootDirectory().FullName;
        public static readonly string PathToResources = PathToProject + "\\Infrastructure\\Resources\\";
        public static readonly string PathToImages = PathToResources + "Images\\";

        private static DirectoryInfo GetRootDirectory()
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (!dir.ToString().EndsWith("ChartWorld") && !dir.ToString().EndsWith("Tests"))
                dir = dir.Parent;
            return dir;
        }
        
        public static IEnumerable<string> GetAllInputFileNames()
        {
            var workingDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory
                .GetParent(workingDirectory)?.Parent?.Parent;
            if (projectDirectory is null)
                return Array.Empty<string>();
            var resourcesDirectory = projectDirectory
                .GetDirectories()
                .FirstOrDefault(d => d.Name == "Infrastructure")?
                .GetDirectories()
                .FirstOrDefault(d => d.Name == "Resources");
            return resourcesDirectory is null
                ? Array.Empty<string>()
                : resourcesDirectory
                    .GetFiles()
                    .Select(file => file.Name);
        }
    }
}