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

                [SerializeField]
                public bool instantiate;
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
            var instantiateList = new List<Action>();

            foreach (var entry in registry.registry)
            {
                var interfaceType = Type.GetType(entry.interfaceName);
                var serviceType = Type.GetType(entry.className);

                serviceTypes[interfaceType] = serviceType;

                if (entry.instantiate)
                {
                    instantiateList.Add(() => { GetOrCreateService(interfaceType); });
                }
            }

            foreach (var action in instantiateList)
            {
                action.Invoke();
            }
        }

        public T Get<T>() where T : SharedService
        {
            return (T)GetOrCreateService(typeof(T));
        }

        public void SetInstance<T>(T instance) where T : SharedService
        {
            instances[typeof(T)] = instance;
        }

        private SharedService GetOrCreateService(Type interfaceType)
        {
            SharedService service = null;
            Type type = null;

            if (!instances.TryGetValue(interfaceType, out service) && serviceTypes.TryGetValue(interfaceType, out type))
            {
                service = (SharedService)Activator.CreateInstance(type);
                instances.Add(interfaceType, service);
            }

            return service;
        }
    }
}
