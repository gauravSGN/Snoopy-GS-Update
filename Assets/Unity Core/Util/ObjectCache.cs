using System.Collections.Generic;

namespace Util
{
    sealed public class ObjectCache<KeyType, ValueType>
    {
        public delegate ValueType CreateCallback(KeyType key);

        private readonly Dictionary<KeyType, ValueType> items = new Dictionary<KeyType, ValueType>();

        public CreateCallback OnMissingKey { get; set; }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(KeyType key)
        {
            return items.ContainsKey(key);
        }

        public void Add(KeyType key, ValueType value)
        {
            items.Add(key, value);
        }

        public ValueType Get(KeyType key)
        {
            if (!Contains(key))
            {
                if (OnMissingKey != null)
                {
                    Add(key, OnMissingKey(key));
                }
                else
                {
                    throw new KeyNotFoundException("Cache does not contain key " + key);
                }
            }

            return items[key];
        }
    }
}
