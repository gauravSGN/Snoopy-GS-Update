using System.Collections.Generic;

namespace Util
{
    abstract public class BaseObjectPool<KeyType, BaseType>
    {
        public delegate BaseType Allocator(KeyType key);

        private readonly Dictionary<KeyType, Queue<BaseType>> items = new Dictionary<KeyType, Queue<BaseType>>();

        abstract public BaseType DefaultAllocator(KeyType key);
        abstract public void Release(BaseType item);

        public int Count(KeyType key)
        {
            return items.ContainsKey(key) ? items[key].Count : 0;
        }

        public void Allocate(KeyType key, int count)
        {
            Allocator allocator = DefaultAllocator;
            Allocate(key, count, allocator);
        }

        public void Allocate(KeyType key, int count, Allocator allocator)
        {
            for (var index = Count(key); index < count; index++)
            {
                Release(allocator(key));
            }
        }

        virtual public void Clear()
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
