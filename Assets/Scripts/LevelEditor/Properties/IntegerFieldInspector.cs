using System;

namespace LevelEditor.Properties
{
    public class IntegerFieldInspector : TextFieldInspector
    {
        override protected void WritePropertyValue(string value)
        {
            if (Property.PropertyType.IsArray)
            {
                var array = (Array)Property.GetValue(Target, null);
                array.SetValue(int.Parse(value), Index);
            }
            else
            {
                Property.SetValue(Target, int.Parse(value), null);
            }
        }
    }
}
