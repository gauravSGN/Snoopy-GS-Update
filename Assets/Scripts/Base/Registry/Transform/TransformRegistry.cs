using System;
using Service;
using UnityEngine;
using System.Collections.Generic;

namespace Registry
{
    public class TransformRegistry : TransformService
    {
        private readonly Dictionary<TransformUsage, Transform> elements = new Dictionary<TransformUsage, Transform>();

        public void Register(Transform transform, TransformUsage usage)
        {
            if (elements.ContainsKey(usage))
            {
                throw new ArgumentException(string.Format("TransformUsage type {0} already registered.", usage));
            }

            elements[usage] = transform;
        }

        public void Unregister(Transform transform, TransformUsage usage)
        {
            Transform current;

            if (elements.TryGetValue(usage, out current) && (current == transform))
            {
                elements.Remove(usage);
            }
        }

        public Transform Get(TransformUsage usage)
        {
            Transform transform = null;

            elements.TryGetValue(usage, out transform);

            return transform;
        }

        public T Get<T>(TransformUsage usage) where T : Transform
        {
            Transform transform = null;

            elements.TryGetValue(usage, out transform);

            return transform as T;
        }
    }
}
