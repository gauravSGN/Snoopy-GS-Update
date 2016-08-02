using System;
using System.Collections.Generic;

namespace Model
{
    sealed public class RandomBag<T>
    {
        private readonly List<T> items = new List<T>();
        private bool needsShuffle = false;

        public int Count { get { return items.Count; } }
        public bool Empty { get { return items.Count == 0; } }

        public RandomBag() { }

        public RandomBag(IEnumerable<T> items)
        {
            this.items.AddRange(items);
            needsShuffle = true;
        }

        public void Add(T item)
        {
            items.Add(item);
            needsShuffle = true;
        }

        public void Add(T item, int quantity)
        {
            for (var index = 0; index < quantity; index++)
            {
                items.Add(item);
            }

            needsShuffle = true;
        }

        public void Clear()
        {
            items.Clear();
        }

        public T Next()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("No items remaining in random bag");
            }

            if (needsShuffle)
            {
                Shuffle();
            }

            var item = items[0];
            items.RemoveAt(0);

            return item;
        }

        private void Shuffle()
        {
            var rng = new Random();
            int target;
            T temp;

            for (var index = items.Count - 1; index > 0; index--)
            {
                target = rng.Next(index);

                temp = items[index];
                items[index] = items[target];
                items[target] = temp;
            }

            needsShuffle = false;
        }
    }
}
