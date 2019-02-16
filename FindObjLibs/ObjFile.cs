using System;
using System.IO;

namespace FindObjLibs
{
    internal class ObjFile
    {
        private readonly string file;

        public ObjFile(string file)
        {
            this.file = file;
            var bytes = File.ReadAllBytes(file);
            var sections = bytes[2] + (bytes[3] << 8);
            Console.WriteLine($"file {file} has {sections} sections");

            for (int i = 0; i < sections; i++)
            {
                var offset = i * 40 + 20;
                string name = "";
                for (int j = 0; j < 8; j++)
                {
                    var c = bytes[offset + j];
                    if (c == 0)
                    {
                        break;
                    }
                    name += Convert.ToChar(bytes[offset + j]);
                }
                Console.WriteLine($"{name}");
                if (name == ".drectve")
                {
                    var address = bytes[offset + 20] + (bytes[offset + 21] << 8) + (bytes[offset + 22] << 16) + (bytes[offset + 23] << 24);
                    Console.WriteLine();
                }
            }
        }
    }
}