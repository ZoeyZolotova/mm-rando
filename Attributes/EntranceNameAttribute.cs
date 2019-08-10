using System;

namespace MMRando.Attributes
{
    public class EntranceNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public EntranceNameAttribute(string name)
        {
            Name = name;
        }
    }
}
