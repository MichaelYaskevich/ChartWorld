using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChartWorld.App
{
    public static class HelpMethods
    {
        private static string[] SplitStringByCapitalLetters(string str)
        {
            return Regex.Split(str, @"(?<!^)(?=[A-Z])");
        }

        public static IEnumerable<Type> GetImplementations(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => t.GetInterfaces().Contains(type));
        }

        public static string[] GetAllInterfaceImplementationsNames(Type type)
        {
            return GetImplementations(type)
                .Select(t => SplitStringByCapitalLetters(t.Name))
                .Select(split => string.Join(" ", split))
                .ToArray();
        }
    }
}