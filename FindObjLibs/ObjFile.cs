using System;
using System.IO;
using System.Linq;

namespace FindObjLibs
{
    // see https://docs.microsoft.com/en-gb/windows/desktop/Debug/pe-format#coff-file-header-object-and-image for details
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
                    var length = bytes[offset + 16] + (bytes[offset + 17] << 8) + (bytes[offset + 18] << 16) + (bytes[offset + 19] << 24);
                    var address = bytes[offset + 20] + (bytes[offset + 21] << 8) + (bytes[offset + 22] << 16) + (bytes[offset + 23] << 24);
                    foreach (var b in bytes.Skip(address).Take(length))
                    {
                        if (b == 32)
                        {
                            Console.WriteLine();
                        }
                        else if (b > 31)
                        {
                            Console.Write($"{Convert.ToChar(b)}");
                        }
                        else
                        {
                            Console.Write("$");
                        }
                    }

                    // write section to test file:
                    File.WriteAllBytes(@"c:\temp\ams\direct1.bin", bytes.Skip(address).Take(length).ToArray());
                }
            }
        }
    }
}