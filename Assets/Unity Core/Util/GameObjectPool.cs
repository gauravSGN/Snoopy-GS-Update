﻿using UnityEngine;
using System.Collections.Generic;

namespace Util
{
    sealed public class GameObjectPool : BaseObjectPool<GameObject, GameObject>
    {
        private readonly Dictionary<GameObject, GameObject> allocated = new Dictionary<GameObject, GameObject>();

        override public void Release(GameObject item)
        {
            GameObject prefab;

            if (!allocated.TryGetValue(item, out prefab))
            {
                throw new System.ArgumentException("Trying to release an item not allocated by this pool.");
            }

            item.transform.SetParent(null, false);
            item.SetActive(false);

            allocated.Remove(item);
            ReturnToPool(prefab, item);
        }

        override protected GameObject DefaultAllocator(GameObject key)
        {
            var instance = GameObject.Instantiate(key);

            allocated.Add(instance, key);

            return instance;
        }
    }
}
