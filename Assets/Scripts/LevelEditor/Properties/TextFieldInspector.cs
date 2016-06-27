﻿using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.Properties
{
    abstract public class TextFieldInspector : MonoBehaviour
    {
        [SerializeField]
        protected Text label;

        [SerializeField]
        protected InputField inputField;

        private FieldPropertyInfo fieldInfo;

        public object Target { get { return fieldInfo.Target; } }
        public PropertyInfo Property { get { return fieldInfo.Property; } }

        abstract protected void WritePropertyValue(string value);

        virtual public void InitializeField(FieldPropertyInfo info)
        {
            fieldInfo = info;

            label.text = Regex.Replace(Property.Name, "([a-z])([A-Z0-9])", "$1 $2");
            ReadPropertyValue();

            if (Target is Observable)
            {
                (Target as Observable).AddListener(OnTargetChanged);
            }

            inputField.onEndEdit.AddListener(WritePropertyValue);
        }

        virtual protected void ReadPropertyValue()
        {
            inputField.text = Property.GetValue(Target, null).ToString();
        }

        private void OnTargetChanged(Observable target)
        {
            ReadPropertyValue();
        }
    }
}