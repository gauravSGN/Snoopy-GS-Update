using System;

namespace LevelEditor.Properties
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PropertyDisplayAttribute : Attribute
    {
        public string[] Names { get; private set; }
        public string[] Colors { get; private set; }

        public PropertyDisplayAttribute(string[] names) : this(names, null)
        {
        }

        public PropertyDisplayAttribute(string[] names, string[] colors)
        {
            Names = names;
            Colors = colors;
        }
    }
}
