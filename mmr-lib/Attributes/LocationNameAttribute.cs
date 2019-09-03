using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMRando.Attributes
{
    public class LocationNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public LocationNameAttribute(string name)
        {
            Name = name;
        }
    }
}
