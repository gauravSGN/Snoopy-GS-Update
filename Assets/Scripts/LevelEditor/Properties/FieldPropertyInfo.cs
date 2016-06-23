using System.Reflection;

namespace LevelEditor.Properties
{
    sealed public class FieldPropertyInfo
    {
        public object Target { get; private set; }
        public PropertyInfo Property { get; private set; }

        public FieldPropertyInfo(object target, PropertyInfo info)
        {
            Target = target;
            Property = info;
        }
    }
}
