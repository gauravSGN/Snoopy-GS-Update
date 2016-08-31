using System;
using System.Collections.Generic;

namespace Util
{
    sealed public class ObjectPool<T>
    {
        private readonly Dictionary<Type, Queue<T>> items = new Dictionary<Type, Queue<T>>();

        private static U DefaultAllocator<U>(Type type) where U : T
        {
            return (U)Activator.CreateInstance(type);
        }

        public void Clear()
        {
            items.Clear();
        }

        public T Get()
        {
            return Get(typeof(T));
        }

        public T Get(Type type)
        {
            Func<Type, T> allocator = DefaultAllocator<T>;
            return Get(type, allocator);
        }

        public T Get(Type type, Func<Type, T> allocator)
        {
            Queue<T> instances;

            if (items.TryGetValue(type, out instances) && (instances.Count > 0))
            {
                return instances.Dequeue();
            }

            return allocator(type);
        }

        public U Get<U>() where U : T
        {
            return Get<U>(typeof(U));
        }

        public U Get<U>(Type type) where U : T
        {
            Func<Type, U> allocator = DefaultAllocator<U>;
            return Get<U>(type, allocator);
        }

        public U Get<U>(Type type, Func<Type, U> allocator) where U : T
        {
            return (U)Get(type, t => (T)allocator(t));
        }

        public void Release(T item)
        {
            var itemType = item.GetType();

            if (!items.ContainsKey(itemType))
            {
                items.Add(itemType, new Queue<T>());
            }

            items[itemType].Enqueue(item);
        }
    }
}
