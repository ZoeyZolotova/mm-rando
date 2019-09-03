using System;

namespace MMRando.Attributes
{
    public class GossipItemHintAttribute : Attribute
    {
        public string[] Values { get; private set; }

        public GossipItemHintAttribute(params string[] values)
        {
            Values = values;
        }
    }
}
