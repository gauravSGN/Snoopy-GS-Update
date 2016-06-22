namespace LevelEditor.Properties
{
    public class IntegerFieldInspector : TextFieldInspector
    {
        override protected void WritePropertyValue(string value)
        {
            Property.SetValue(Target, int.Parse(value), null);
        }
    }
}
