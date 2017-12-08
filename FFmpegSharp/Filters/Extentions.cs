using System;
using System.ComponentModel;

namespace FFmpegSharp.Filters
{
    public static class Extentions
    {
        public static string GetDescription(this X264Preset value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);

            if (name == null) return null;

            var field = type.GetField(name);

            if (field == null) return null;

            var attr =
                Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attr != null ? attr.Description : null;
        } 
    }
}