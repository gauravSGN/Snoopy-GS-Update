using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.Properties
{
    public abstract class TextFieldInspector : MonoBehaviour
    {
        [SerializeField]
        protected Text label;

        [SerializeField]
        protected InputField inputField;

        private FieldPropertyInfo fieldInfo;

        public object Target { get { return fieldInfo.Target; } }
        public PropertyInfo Property { get { return fieldInfo.Property; } }

        protected abstract void WritePropertyValue(string value);

        public virtual void InitializeField(FieldPropertyInfo info)
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

        protected virtual void ReadPropertyValue()
        {
            inputField.text = Property.GetValue(Target, null).ToString();
        }

        private void OnTargetChanged(Observable target)
        {
            ReadPropertyValue();
        }
    }
}
