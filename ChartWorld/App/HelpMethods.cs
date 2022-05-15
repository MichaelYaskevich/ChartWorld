using System.IO;

namespace ChartWorld.App
{
    public static class HelpMethods
    {
        public static string PathToImages = GetGameDirectoryRoot().FullName + "\\Resources\\Images\\";
        public static DirectoryInfo GetGameDirectoryRoot() {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (!dir.ToString().EndsWith("ChartWorld")) {
                dir = dir.Parent;
            }
            return dir;
        }
    }
}