using Util;
using Model;
using Modifiers;
using UnityEngine;
using System.Collections.Generic;

public class BubbleFactory : ScriptableFactory<BubbleType, BubbleDefinition>
{
    private class ModifierFactory : AttributeDrivenFactory<BubbleModifier, BubbleModifierAttribute, BubbleModifierType>
    {
        override protected BubbleModifierType GetKeyFromAttribute(BubbleModifierAttribute attribute)
        {
            return attribute.ModifierType;
        }
    }

    public IEnumerable<BubbleDefinition> Bubbles { get { return definitions; } }

    public LevelConfiguration Configuration
    {
        get { return configuration; }
        set
        {
            configuration = value;

            PopulateModifiers();

            foreach (var modifier in bubbleModifiers)
            {
                modifier.Configuration = value;
            }
        }
    }

    private List<BubbleModifier> bubbleModifiers;
    private LevelConfiguration configuration;

    public void ResetModifiers()
    {
        bubbleModifiers = null;
    }

    public GameObject Create(BubbleData data)
    {
        PopulateModifiers();

        if (data.modifiers != null)
        {
            foreach (var modifier in bubbleModifiers)
            {
                modifier.ApplyDataModifications(data);
            }
        }

        var gameObject = CreateByType(data.Type);

        if (data.modifiers != null)
        {
            foreach (var modifier in bubbleModifiers)
            {
                modifier.ApplyGameObjectModifications(data, gameObject);
            }
        }

        return gameObject;
    }

    override public GameObject CreateByType(BubbleType type)
    {
        var definition = GetDefinitionByType(type);
        GameObject instance = null;

        if (!definition.EditorOnly)
        {
            instance = Instantiate(definition.Prefab);

            var model = new Bubble
            {
                type = type,
                definition = definition,
                IsRoot = definition.ActsAsRoot,
            };

            instance.SendMessage("SetModel", model, SendMessageOptions.DontRequireReceiver);
        }

        return instance;
    }

    public void ApplyEditorModifiers(GameObject instance, BubbleData data)
    {
        PopulateModifiers();

        if (data.modifiers != null)
        {
            foreach (var modifier in bubbleModifiers)
            {
                modifier.ApplyEditorModifications(data, instance);
            }
        }
    }

    private void PopulateModifiers()
    {
        if (bubbleModifiers == null)
        {
            var factory = new ModifierFactory();
            bubbleModifiers = new List<BubbleModifier>(factory.CreateAll());
        }
    }
}
