using System;

namespace MMRando.Attributes
{
    public class GossipLocationHintAttribute : Attribute
    {
        public string[] Values { get; private set; }

        public GossipLocationHintAttribute(params string[] values)
        {
            Values = values;
        }
    }
}
