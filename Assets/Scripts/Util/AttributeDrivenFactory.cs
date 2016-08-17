using System;
using System.Collections.Generic;

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

        public IEnumerable<BaseType> CreateAll()
        {
            if (validTypes == null)
            {
                GenerateActionTypeMap();
            }

            foreach (var pair in validTypes)
            {
                yield return Create(pair.Key);
            }
        }

        private void GenerateActionTypeMap()
        {
            validTypes = new Dictionary<KeyType, Type>();

            foreach (var type in ReflectionUtil.GetDerivedTypes<BaseType>())
            {
                var attributes = type.GetCustomAttributes(typeof(AttributeType), false);

                foreach (AttributeType attribute in attributes)
                {
                    validTypes[GetKeyFromAttribute(attribute)] = type;
                }
            }
        }
    }
}
