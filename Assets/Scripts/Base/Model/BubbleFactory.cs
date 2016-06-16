using UnityEngine;
using System.Collections.Generic;
using BubbleContent;

public class BubbleFactory : ScriptableObject
{
    public List<BubbleDefinition> bubbles;
    public List<BubbleContentDefinition> contents;

    private Dictionary<BubbleType, BubbleDefinition> bubbleLookup;
    private Dictionary<BubbleContentType, BubbleContentDefinition> contentLookup;

    public BubbleDefinition GetBubbleDefinitionByType(BubbleType type)
    {
        bubbleLookup = bubbleLookup ?? CreateLookupTable<BubbleType, BubbleDefinition>(bubbles);

        return bubbleLookup[type];
    }

    public BubbleContentDefinition GetContentDefinitionByType(BubbleContentType type)
    {
        contentLookup = contentLookup ?? CreateLookupTable<BubbleContentType, BubbleContentDefinition>(contents);

        return contentLookup[type];
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

    public GameObject CreateContentByType(BubbleContentType type)
    {
        return Instantiate(GetContentDefinitionByType(type).prefab);
    }

    private Dictionary<K, V> CreateLookupTable<K, V>(List<V> items) where V : GameObjectDefinition<K>
    {
        var lookup = new Dictionary<K, V>();

        foreach (var info in items)
        {
            lookup.Add(info.Type, info);
        }

        return lookup;
    }
}
