using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LevelEditor.Properties
{
    public class LevelPropertyInspector : MonoBehaviour
    {
        [SerializeField]
        private LevelManipulator manipulator;

        [SerializeField]
        private Transform itemContainer;

        [SerializeField]
        private string propertyName;

        [SerializeField]
        private GameObject integerPrefab;

        private readonly Dictionary<Type, GameObject> prefabMap = new Dictionary<Type, GameObject>();

        protected void Start()
        {
            CreatePrefabMap();
            CreateFields();
        }

        private void CreateFields()
        {
            var propertyInfo = manipulator.GetType().GetProperty(propertyName);
            var target = propertyInfo.GetValue(manipulator, null);

            foreach (var property in propertyInfo.PropertyType.GetProperties())
            {
                var field = CreateField(property);

                if (field != null)
                {
                    field.SendMessage("InitializeField", new FieldPropertyInfo(target, property));
                }
            }
        }

        private GameObject CreateField(PropertyInfo propertyInfo)
        {
            GameObject prefab;
            GameObject instance = null;

            if (prefabMap.TryGetValue(propertyInfo.PropertyType, out prefab))
            {
                instance = Instantiate(prefab);
                instance.name = propertyInfo.Name;
                instance.transform.SetParent(itemContainer, false);
            }

            return instance;
        }

        private void CreatePrefabMap()
        {
            prefabMap[typeof(int)] = integerPrefab;
        }
    }
}
