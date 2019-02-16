using System;
using System.Collections.Generic;
using System.IO;

namespace FindObjLibs
{
    internal class Finder
    {
        private readonly string pattern;
        private readonly string startdir;
        private readonly List<string> files = new List<string>();

        public Finder(string pattern, string startdir = "")
        {
            this.pattern = pattern;
            this.startdir = startdir;
            FindFiles(pattern, string.IsNullOrEmpty(startdir) ? Directory.GetCurrentDirectory() : startdir);
        }

        private void FindFiles(string pattern, string startdir)
        {
            var files = Directory.GetFiles(startdir, pattern);
            this.files.AddRange(files);
            var dirs = Directory.GetDirectories(startdir);
            foreach (var dir in dirs)
            {
                FindFiles(pattern, dir);
            }
        }

        public List<string> Files => files;
    }
}