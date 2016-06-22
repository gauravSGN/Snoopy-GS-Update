using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Util
{
    abstract public class AttributeDrivenFactory<BaseType, AttributeType, KeyType>
        where BaseType : class
        where AttributeType : Attribute
    {
        private Dictionary<KeyType, Type> validTypes;

        abstract protected KeyType GetKeyFromAttribute(AttributeType attribute);

        public BaseType Create(KeyType key)
        {
            if (validTypes == null)
            {
                GenerateActionTypeMap();
            }

            BaseType result = null;

            if (validTypes.ContainsKey(key))
            {
                result = Activator.CreateInstance(validTypes[key]) as BaseType;
            }

            return result;
        }

        private void GenerateActionTypeMap()
        {
            validTypes = new Dictionary<KeyType, Type>();

            foreach (var type in GetValidTypes())
            {
                var attributes = type.GetCustomAttributes(typeof(AttributeType), false);

                if (attributes.Length > 0)
                {
                    var key = GetKeyFromAttribute(attributes[0] as AttributeType);
                    validTypes[key] = type;
                }
            }
        }

        private static IEnumerable<Type> GetValidTypes()
        {
            var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
            var actionTypes = assemblyTypes.Where(t => typeof(BaseType).IsAssignableFrom(t));
            return actionTypes.Where(t => !t.IsAbstract && !t.IsInterface);
        }
    }
}
