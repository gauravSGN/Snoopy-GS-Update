using System;
using System.Reflection;
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

        [SerializeField]
        protected Image background;

        private FieldPropertyInfo fieldInfo;

        public object Target { get { return fieldInfo.Target; } }
        public PropertyInfo Property { get { return fieldInfo.Property; } }
        public int Index { get { return fieldInfo.Index; } }

        abstract protected void WritePropertyValue(string value);

        virtual public void InitializeField(FieldPropertyInfo info)
        {
            fieldInfo = info;

            SetLabelText();
            SetFieldColor();
            ReadPropertyValue();

            if (Target is Observable)
            {
                (Target as Observable).AddListener(OnTargetChanged);
            }

            inputField.onEndEdit.AddListener(WritePropertyValue);
        }

        virtual protected void ReadPropertyValue()
        {
            if (Property.PropertyType.IsArray)
            {
                var array = (Array)Property.GetValue(Target, null);
                inputField.text = array.GetValue(Index).ToString();
            }
            else
            {
                inputField.text = Property.GetValue(Target, null).ToString();
            }
        }

        private void SetLabelText()
        {
            string labelText = null;

            if ((fieldInfo.Display != null) && (Index < fieldInfo.Display.Names.Length))
            {
                labelText = fieldInfo.Display.Names[Index];
            }

            if (string.IsNullOrEmpty(labelText))
            {
                labelText = Regex.Replace(Property.Name, "([a-z])([A-Z0-9])", "$1 $2");

                if (Property.PropertyType.IsArray)
                {
                    labelText = labelText + string.Format(" {0}", Index + 1);
                }
            }

            label.text = labelText;
        }

        private void SetFieldColor()
        {
            if ((fieldInfo.Display != null) &&
                (fieldInfo.Display.Colors != null) &&
                (Index < fieldInfo.Display.Colors.Length))
            {
                var hexColor = fieldInfo.Display.Colors[Index];
                var red = Convert.ToInt32(hexColor.Substring(0, 2), 16) / 255.0f;
                var green = Convert.ToInt32(hexColor.Substring(2, 2), 16) / 255.0f;
                var blue = Convert.ToInt32(hexColor.Substring(4, 2), 16) / 255.0f;
                background.color = new Color(red, green, blue);
            }
        }

        private void OnTargetChanged(Observable target)
        {
            ReadPropertyValue();
        }
    }
}
