using System;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.Properties
{
    abstract public class TextFieldInspector : FieldInspector
    {
        [SerializeField]
        protected Text label;

        [SerializeField]
        protected InputField inputField;

        [SerializeField]
        protected Image background;

        abstract protected void WritePropertyValue(string value);

        override public void InitializeField(FieldPropertyInfo info)
        {
            base.InitializeField(info);

            SetLabelText(label);
            SetFieldColor();

            inputField.onEndEdit.AddListener(WritePropertyValue);
        }

        override protected void ReadPropertyValue()
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

        private void SetFieldColor()
        {
            if ((FieldInfo.Display != null) &&
                (FieldInfo.Display.Colors != null) &&
                (Index < FieldInfo.Display.Colors.Length))
            {
                var hexColor = FieldInfo.Display.Colors[Index];
                var red = Convert.ToInt32(hexColor.Substring(0, 2), 16) / 255.0f;
                var green = Convert.ToInt32(hexColor.Substring(2, 2), 16) / 255.0f;
                var blue = Convert.ToInt32(hexColor.Substring(4, 2), 16) / 255.0f;
                background.color = new Color(red, green, blue);
            }
        }
    }
}
