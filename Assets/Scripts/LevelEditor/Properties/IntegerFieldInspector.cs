namespace LevelEditor.Properties
{
    public class IntegerFieldInspector : TextFieldInspector
    {
        protected override void WritePropertyValue(string value)
        {
            Property.SetValue(Target, int.Parse(value), null);
        }
    }
}
