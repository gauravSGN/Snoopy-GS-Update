using System;

namespace Util
{
    public static class AttributeUtil
    {
        public static bool HasAttribute<T>(Type type) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), false).Length > 0;
        }
    }
}
