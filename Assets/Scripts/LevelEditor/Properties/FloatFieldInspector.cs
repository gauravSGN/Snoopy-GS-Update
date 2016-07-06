using System;

namespace LevelEditor.Properties
{
    public class FloatFieldInspector : TextFieldInspector
    {
        override protected void WritePropertyValue(string value)
        {
            if (Property.PropertyType.IsArray)
            {
                var array = (Array)Property.GetValue(Target, null);
                array.SetValue(float.Parse(value), Index);
            }
            else
            {
                Property.SetValue(Target, float.Parse(value), null);
            }
        }
    }
}
