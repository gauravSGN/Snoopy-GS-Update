using UnityEngine;
using System.Collections.Generic;

public class BubbleFactory : ScriptableObject
{
    [SerializeField]
    private List<BubbleDefinition> bubbles;

    private Dictionary<BubbleType, BubbleDefinition> lookup;

    public BubbleDefinition GetBubbleDefinitionByType(BubbleType type)
    {
        if (lookup == null)
        {
            CreateLookupTable();
        }

        return lookup[type];
    }

    public GameObject CreateBubbleByType(BubbleType type)
    {
        var definition = GetBubbleDefinitionByType(type);
        var instance = Instantiate(definition.prefab);

        var model = new Bubble
        {
            type = type,
            definition = definition,
        };

        instance.SendMessage("SetModel", model);

        return instance;
    }

    private void CreateLookupTable()
    {
        lookup = new Dictionary<BubbleType, BubbleDefinition>();

        foreach (var info in bubbles)
        {
            lookup.Add(info.type, info);
        }
    }
}
