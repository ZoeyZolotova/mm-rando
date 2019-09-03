using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMRando.Extensions
{
    public static class AttributeExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }

        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>();
        }

        public static bool HasAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
            {
                return false;
            }
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .Any();
        }
    }
}
