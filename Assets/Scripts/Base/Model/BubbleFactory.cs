using System.Collections.Generic;
using UnityEngine;
using Util;
using Model;
using Effects;
using Animation;
using Modifiers;

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
        var instance = Instantiate(definition.Prefab);

        var model = new Bubble
        {
            type = type,
            definition = definition,
            IsRoot = definition.ActsAsRoot,
        };

        SetupBasePopEffects(instance, definition);
        instance.SendMessage("SetModel", model);

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

    private void SetupBasePopEffects(GameObject instance, BubbleDefinition definition)
    {
        var bubbleDeath = instance.GetComponent<BubbleDeath>();

        if (bubbleDeath)
        {
            foreach (var pair in definition.AnimationMap)
            {
                foreach (var animationType in pair.Value)
                {
                    bubbleDeath.AddEffect(AnimationEffect.Play(instance, animationType), pair.Key);
                }
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
