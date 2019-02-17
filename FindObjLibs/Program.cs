using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindObjLibs
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine($"Scanning for object files in {Directory.GetCurrentDirectory()}");
                    var finder = new Finder("*.obj");
                    foreach (var file in finder.Files)
                    {
                        var objfile = new ObjFile(file);
                    }
                }
                else
                {
                    foreach (var filename in args)
                    {
                        new ObjFile(filename);
                    }
                }
            }
            catch (Exception ex)
            {
                var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
                var progname = Path.GetFileNameWithoutExtension(fullname);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
