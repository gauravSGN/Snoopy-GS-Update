using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LevelEditor.Properties
{
    abstract public class FieldInspector : MonoBehaviour
    {
        public FieldPropertyInfo FieldInfo { get; private set; }
        public object Target { get { return FieldInfo.Target; } }
        public PropertyInfo Property { get { return FieldInfo.Property; } }
        public int Index { get { return FieldInfo.Index; } }

        abstract protected void ReadPropertyValue();

        virtual public void InitializeField(FieldPropertyInfo info)
        {
            FieldInfo = info;

            if (Target is Observable)
            {
                (Target as Observable).AddListener(OnTargetChanged);
            }

            ReadPropertyValue();
        }

        protected void SetLabelText(Text label)
        {
            string labelText = null;

            if ((FieldInfo.Display != null) && (Index < FieldInfo.Display.Names.Length))
            {
                labelText = FieldInfo.Display.Names[Index];
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

        private void OnTargetChanged(Observable target)
        {
            ReadPropertyValue();
        }
    }
}
