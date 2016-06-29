using System.Reflection;

namespace LevelEditor.Properties
{
    sealed public class FieldPropertyInfo
    {
        public object Target { get; private set; }
        public PropertyInfo Property { get; private set; }
        public int Index { get; private set; }

        public FieldPropertyInfo(object target, PropertyInfo info, int index)
        {
            Target = target;
            Property = info;
            Index = index;
        }
    }
}
