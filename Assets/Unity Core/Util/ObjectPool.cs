using System;

namespace Util
{
    sealed public class ObjectPool<T> : BaseObjectPool<Type, T>
    {
        public int Count<DerivedType>() where DerivedType : T
        {
            return Count(typeof(DerivedType));
        }

        public void Allocate<DerivedType>(int count) where DerivedType : T
        {
            Allocate(typeof(DerivedType), count);
        }

        public void Allocate<DerivedType>(int count, Allocator allocator) where DerivedType : T
        {
            Allocate(typeof(DerivedType), count, allocator);
        }

        public T Get()
        {
            return Get(typeof(T));
        }

        public U Get<U>() where U : T
        {
            return Get<U>(typeof(U));
        }

        public DerivedType Get<DerivedType>(Type key) where DerivedType : T
        {
            Func<Type, DerivedType> allocator = DefaultAllocator<DerivedType>;
            return Get<DerivedType>(key, allocator);
        }

        public DerivedType Get<DerivedType>(Type key, System.Func<Type, DerivedType> allocator) where DerivedType : T
        {
            Allocator wrapper = (k) => (T)allocator(k);
            return (DerivedType)Get(key, wrapper);
        }

        override public void Release(T item)
        {
            ReturnToPool(item.GetType(), item);
        }

        override protected T DefaultAllocator(Type key)
        {
            return (T)Activator.CreateInstance(key);
        }

        private DerivedType DefaultAllocator<DerivedType>(Type key)
        {
            return (DerivedType)Activator.CreateInstance(key);
        }
    }
}
