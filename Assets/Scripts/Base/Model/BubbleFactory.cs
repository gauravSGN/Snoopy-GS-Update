using System.Collections.Generic;
using UnityEngine;
using Util;

public class BubbleFactory : ScriptableFactory<BubbleType, BubbleDefinition>
{
    public IEnumerable<BubbleDefinition> Bubbles { get { return definitions; } }

    override public GameObject CreateByType(BubbleType type)
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
