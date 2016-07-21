using Util;
using UnityEngine;

namespace UI.Popup
{
    public class PopupFactory : ScriptableFactory<PopupType, PopupDefinition>
    {
        override public GameObject CreateByType(PopupType type)
        {
            var definition = GetDefinitionByType(type);
            return Instantiate(definition.Prefab);
        }
    }
}
