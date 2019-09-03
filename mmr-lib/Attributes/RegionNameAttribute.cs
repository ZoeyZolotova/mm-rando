using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMRando.Attributes
{
    public class RegionNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public RegionNameAttribute(string name)
        {
            Name = name;
        }
    }
}
