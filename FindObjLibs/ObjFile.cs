using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FindObjLibs
{
    // see https://docs.microsoft.com/en-gb/windows/desktop/Debug/pe-format#coff-file-header-object-and-image for details of the PE format.
    internal class ObjFile
    {
        private readonly string file;

        public ObjFile(string file)
        {
            this.file = file;
            var bytes = File.ReadAllBytes(file);
            var sections = bytes[2] + (bytes[3] << 8);
            Console.WriteLine($"{file}");
            //Console.WriteLine($"file {file} has {sections} sections");

            for (int i = 0; i < sections; i++)
            {
                // the list of section headers starts at offset 20 decimal in the file - each item in the list is 40 bytes long.
                // Within a section header the name of the section is at offset zero and is a maximum of 8 characters long. Names
                // are padde with zeroes:
                var offset = i * 40 + 20;
                string name = "";
                for (int j = 0; j < 8; j++)
                {
                    var c = bytes[offset + j];
                    if (c == 0)
                    {
                        // a zero padding character, so this is the end of the section name:
                        break;
                    }
                    name += Convert.ToChar(bytes[offset + j]);
                }
                // Console.WriteLine($"{name}");
                if (name == ".drectve")
                {
                    // get length and address - 32 bit unsigned ints at offset 16 and 20 respectively:
                    var length = bytes[offset + 16] | (bytes[offset + 17] << 8) | (bytes[offset + 18] << 16) | (bytes[offset + 19] << 24);
                    var address = bytes[offset + 20] | (bytes[offset + 21] << 8) | (bytes[offset + 22] << 16) | (bytes[offset + 23] << 24);

                    // directives are space separated but sometimes there are a few spaces at the start and end:
                    var directiveSection = Encoding.UTF8.GetString(bytes, address, length).Trim();
                    var directives = directiveSection.Split(' ');

                    // print out all linker directives:
                    foreach (var directive in directives)
                    {
                        Console.WriteLine($"   {i + 1}>{directive}");
                    }
                }
            }
        }
    }
}