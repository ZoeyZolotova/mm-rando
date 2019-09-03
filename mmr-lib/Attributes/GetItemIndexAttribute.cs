using System;

namespace MMRando.Attributes
{
    public class GetItemIndexAttribute : Attribute
    {
        public int Index { get; private set; }

        public GetItemIndexAttribute(int index)
        {
            Index = index;
        }
    }
}
