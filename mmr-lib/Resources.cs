using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MMRando
{
    static class Resources
    {
        public static string GetTextFile(string name)
        {
            using (var stream = File.OpenText($"Resources/{name}"))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
