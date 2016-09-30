using Util;
using Model;

namespace Modifiers
{
    public class BubbleModifierList : ScriptableList<Model.BubbleModifierDefinition>
    {
        public BubbleModifierDefinition GetDefinitionFromTypeAndData(BubbleModifierType type, string data)
        {
            foreach (var modifier in items)
            {
                if ((type == modifier.Type) && (data == modifier.Data))
                {
                    return modifier;
                }
            }

            return null;
        }
    }
}
