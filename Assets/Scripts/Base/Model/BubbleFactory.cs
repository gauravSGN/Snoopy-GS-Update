using System.Collections.Generic;
using UnityEngine;
using Util;
using Model;
using Effects;
using Animation;

public class BubbleFactory : ScriptableFactory<BubbleType, BubbleDefinition>
{
    public IEnumerable<BubbleDefinition> Bubbles { get { return definitions; } }

    [SerializeField]
    private List<BubbleModifierDefinition> modifiers;

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

    private void SetupBasePopEffects(GameObject instance, BubbleDefinition definition)
    {
        var bubbleDeath = instance.GetComponent<BubbleDeath>();

        if (bubbleDeath)
        {
            foreach (var pair in definition.AnimationMap)
            {
                foreach (var animationType in pair.Value)
                {
                    bubbleDeath.AddEffect(DeathAnimationEffect.Play(instance, animationType), pair.Key);
                }
            }
        }
    }
}
