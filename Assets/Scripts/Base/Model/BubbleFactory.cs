using UnityEngine;
using Util;

public class BubbleFactory : ScriptableFactory<BubbleType, BubbleDefinition>
{
    public override GameObject CreateByType(BubbleType type)
    {
        var definition = GetDefinitionByType(type);
        var instance = Instantiate(definition.Prefab);

        var model = new Bubble
        {
            type = type,
            definition = definition,
            IsRoot = definition.ActsAsRoot,
        };

        instance.SendMessage("SetModel", model);

        return instance;
    }
}
