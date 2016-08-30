using System.Collections.Generic;
using System;
using UnityEngine;

namespace Service
{
    public sealed class ServiceRepository
    {
        [Serializable]
        private class SerializedRegistry
        {
            [Serializable]
            public class RegistryEntry
            {
                [SerializeField]
                public string interfaceName;

                [SerializeField]
                public string className;
            }

            [SerializeField]
            public List<RegistryEntry> registry;
        }

        private readonly Dictionary<Type, Type> serviceTypes = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, SharedService> instances = new Dictionary<Type, SharedService>();

        public void Register<T>(Type type) where T : SharedService
        {
            serviceTypes[typeof(T)] = type;
        }

        public void RegisterFromJson(string json)
        {
            var registry = JsonUtility.FromJson<SerializedRegistry>(json);

            foreach (var entry in registry.registry)
            {
                var interfaceType = Type.GetType(entry.interfaceName);
                var serviceType = Type.GetType(entry.className);

                serviceTypes[interfaceType] = serviceType;
            }
        }

        public T Get<T>() where T : SharedService
        {
            var serviceType = typeof(T);
            SharedService service = null;
            Type type = null;

            if (!instances.TryGetValue(serviceType, out service) && serviceTypes.TryGetValue(serviceType, out type))
            {
                service = (SharedService)Activator.CreateInstance(type);
                instances.Add(serviceType, service);
            }

            return (T)service;
        }

        public void SetInstance<T>(T instance) where T : SharedService
        {
            instances[typeof(T)] = instance;
        }
    }
}
