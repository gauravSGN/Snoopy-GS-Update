using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Util;

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

        [SerializeField]
        private GameObject floatPrefab;

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
                IEnumerable<GameObject> fields;

                if (property.PropertyType.IsArray)
                {
                    fields = CreateArrayFields(property, target);
                }
                else
                {
                    fields = CreateField(property);
                }

                int index = 0;
                var attributes = property.GetCustomAttributes(typeof(PropertyDisplayAttribute), false);
                var display = (attributes.Length > 0) ? (PropertyDisplayAttribute)attributes[0] : null;

                foreach (var field in fields)
                {
                    field.SendMessage("InitializeField", new FieldPropertyInfo(target, property, index, display));
                    index++;
                }
            }
        }

        private IEnumerable<GameObject> CreateField(PropertyInfo propertyInfo)
        {
            var instance = CreateField(propertyInfo.PropertyType);

            if (instance != null)
            {
                instance.name = propertyInfo.Name;
                yield return instance;
            }
        }

        private IEnumerable<GameObject> CreateArrayFields(PropertyInfo propertyInfo, object target)
        {
            var elementType = propertyInfo.PropertyType.GetElementType();
            var test = propertyInfo.GetValue(target, null);
            var array = (Array)test;

            for (var index = 0; index < array.Length; index++)
            {
                var instance = CreateField(elementType);

                if (instance != null)
                {
                    instance.name = string.Format("{0} {1}", propertyInfo.Name, index + 1);
                    yield return instance;
                }
            }
        }

        private GameObject CreateField(Type fieldType)
        {
            GameObject prefab;
            GameObject instance = null;

            if (prefabMap.TryGetValue(fieldType, out prefab))
            {
                instance = Instantiate(prefab);
                instance.transform.SetParent(itemContainer, false);
            }

            return instance;
        }

        private void CreatePrefabMap()
        {
            prefabMap[typeof(int)] = integerPrefab;
            prefabMap[typeof(float)] = floatPrefab;
        }
    }
}
