using System;
using MMRando.GameObjects;

namespace MMRando.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class GossipRestrictAttribute : Attribute
    {
        public Item Item { get; private set; }
        public RestrictionType Type { get; private set; }
        public bool ForceClear { get; private set; }

        public GossipRestrictAttribute(RestrictionType type, Item item, bool forceClear = false)
        {
            Type = type;
            Item = item;
            ForceClear = forceClear;
        }

        public enum RestrictionType
        {
            Item,
            Location
        }
    }
}
