using System;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.Properties
{
    public class BooleanFieldInspector : FieldInspector
    {
        [SerializeField]
        private Text label;

        [SerializeField]
        private Toggle toggle;

        override public void InitializeField(FieldPropertyInfo info)
        {
            base.InitializeField(info);

            SetLabelText(label);

            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        override protected void ReadPropertyValue()
        {
            if (Property.PropertyType.IsArray)
            {
                var array = (Array)Property.GetValue(Target, null);
                toggle.isOn = (bool)array.GetValue(Index);
            }
            else
            {
                toggle.isOn = (bool)Property.GetValue(Target, null);
            }
        }

        private void OnValueChanged(bool newValue)
        {
            Property.SetValue(Target, newValue, null);
        }
    }
}
