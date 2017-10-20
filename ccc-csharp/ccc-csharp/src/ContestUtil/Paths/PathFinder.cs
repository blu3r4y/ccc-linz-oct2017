using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CCC.ContestUtil.Paths
{
    public static class PathFinder
    {
        /// <summary>
        /// searches for the java executable. prefers enironment variables over
        /// registry entries over hard directories. if multiple java versions
        /// are present within an x64 architecture, jre and the latest version
        /// is prefered.
        /// </summary>
        public static string Java => _java ?? (_java = SearchForJava());
        private static string _java;

        private static string SearchForJava()
        {
            // try environment variables
            string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrEmpty(environmentPath))
            {
                return Path.Combine(environmentPath, @"bin\java.exe");
            }

            // try registry keys
            const string javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaKey))
            {
                if (rk != null)
                {
                    string currentVersion = rk.GetValue("CurrentVersion").ToString();
                    using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                    {
                        if (key != null) return Path.Combine(key.GetValue("JavaHome").ToString(), @"bin\java.exe");
                    }
                }
            }

            // try search in folders
            string x86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string x64 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            var digitReplace = new Regex(@"[^\d]");

            // prefer x64 version
            string javaFolder = Directory.EnumerateDirectories(x64).FirstOrDefault(e => e.Contains("Java"))
                ?? Directory.EnumerateDirectories(x86).FirstOrDefault(e => e.Contains("Java"));
            if (javaFolder != null)
            {
                // prefer jre
                List<string> jvmFolders = Directory.EnumerateDirectories(javaFolder).Where(e => e.Contains("jre")).ToList();
                if (!jvmFolders.Any()) jvmFolders = Directory.EnumerateDirectories(javaFolder).Where(e => e.Contains("jdk")).ToList();

                if (jvmFolders.Any())
                {
                    // prefer latest version
                    jvmFolders.Sort((s1, s2) => int.Parse(digitReplace.Replace(s1, "")) - int.Parse(digitReplace.Replace(s2, "")));
                    return Path.Combine(jvmFolders.Last(), @"bin\java.exe");
                } 
            }

            return null;
        }
    }
}
