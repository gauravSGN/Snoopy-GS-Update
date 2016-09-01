using System.Collections.Generic;

namespace Util
{
    abstract public class BaseObjectPool<KeyType, BaseType>
    {
        public delegate BaseType Allocator(KeyType key);

        private readonly Dictionary<KeyType, Queue<BaseType>> items = new Dictionary<KeyType, Queue<BaseType>>();

        abstract public void Release(BaseType item);
        abstract protected BaseType DefaultAllocator(KeyType key);

        public void Clear()
        {
            items.Clear();
        }

        public BaseType Get(KeyType key)
        {
            Allocator allocator = DefaultAllocator;
            return Get(key, allocator);
        }

        public BaseType Get(KeyType key, Allocator allocator)
        {
            Queue<BaseType> instances;

            if (items.TryGetValue(key, out instances) && (instances.Count > 0))
            {
                return instances.Dequeue();
            }

            return allocator(key);
        }

        protected void ReturnToPool(KeyType key, BaseType item)
        {
            if (!items.ContainsKey(key))
            {
                items.Add(key, new Queue<BaseType>());
            }

            items[key].Enqueue(item);
        }
    }
}
