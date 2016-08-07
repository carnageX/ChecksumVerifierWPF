using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChecksumVerifier
{
    public static class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                            Attribute.GetCustomAttribute(field,
                                typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static HashOption GetHashOptionValue(this string value)
        {
            //return (HashOption)Enum.Parse(typeof(HashOption), value, true);

            var eVals = Enum.GetValues(typeof(HashOption));
            foreach(HashOption e in eVals)
            {
                if (value.Equals(e.Description())) { return e; }
            }
            throw new ArgumentException("No matching hash option for {0}", value);
        }
    }
}
