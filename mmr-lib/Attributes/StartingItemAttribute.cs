using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMRando.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class StartingItemAttribute : Attribute
    {
        public int Address { get; private set; }
        public byte Value { get; private set; }
        public bool IsAdditional { get; private set; }

        public StartingItemAttribute(int address, byte value, bool isAdditional = false)
        {
            Address = address;
            Value = value;
            IsAdditional = isAdditional;
        }
    }
}
