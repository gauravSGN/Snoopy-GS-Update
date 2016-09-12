using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Animation;
using Effects;

public class BubbleDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyOnFinish;

    [SerializeField]
    private float deathDelay;

    [SerializeField]
    private List<GameObject> activateOnPop;

    [SerializeField]
    private List<GameObject> activateOnCull;

    [SerializeField]
    private List<GameObject> deactivateOnDeath;

    private Dictionary<BubbleDeathType, List<IEnumerator>> effectDictionary;

    public static void KillBubble(GameObject bubble, BubbleDeathType type)
    {
        if(bubble.GetComponent<BubbleScore>().Score > 0)
        {
            var effectController = bubble.GetComponent<BubbleEffectController>();
            effectController.AddEffect(AnimationEffect.Play(bubble, AnimationType.ScoreText));
        }

        var death = bubble.GetComponent<BubbleDeath>();

        if (death != null)
        {
            bubble.GetComponent<Rigidbody2D>().isKinematic = true;
            death.TriggerDeathEffects(type);
        }
        else
        {
            Destroy(bubble);
        }
    }

    public void TriggerDeathEffects(BubbleDeathType type)
    {
        var effectController = gameObject.GetComponent<BubbleEffectController>();
        var effects = effectDictionary.ContainsKey(type) ? effectDictionary[type] : GetDefaultEffects(type);

        foreach (var effect in effects)
        {
            effectController.AddEffect(effect);
        }

        GameObjectUtil.SetActive((type == BubbleDeathType.Pop) ? activateOnPop : activateOnCull, true);
        GameObjectUtil.SetActive(deactivateOnDeath, false);

        var sounds = GetComponent<BubbleModelBehaviour>().Model.definition.Sounds;
        Sound.PlaySoundEvent.Dispatch((type == BubbleDeathType.Pop) ? sounds.match : sounds.cull);

        Destroy(destroyOnFinish, deathDelay);
    }

    public void AddEffect(IEnumerator effect, BubbleDeathType type)
    {
        if (!effectDictionary.ContainsKey(type))
        {
            effectDictionary.Add(type, new List<IEnumerator>());
        }

        effectDictionary[type].Add(effect);
    }

    public void ReplaceEffect(IEnumerator effect, BubbleDeathType type)
    {
        if (effectDictionary.ContainsKey(type))
        {
            effectDictionary[type].Clear();
        }

        AddEffect(effect, type);
    }

    public void DeactivateObjectOnDeath(GameObject gameObject)
    {
        deactivateOnDeath.Add(gameObject);
    }

    protected void Awake()
    {
        effectDictionary = new Dictionary<BubbleDeathType, List<IEnumerator>>();
    }

    private List<IEnumerator> GetDefaultEffects(BubbleDeathType type)
    {
        var effects = new List<IEnumerator>();
        var animationMap = GetComponent<BubbleModelBehaviour>().Model.definition.AnimationMap;

        if (animationMap.ContainsKey(type))
        {
            foreach (var animationType in animationMap[type])
            {
                effects.Add(AnimationEffect.Play(gameObject, animationType));
            }
        }

        return effects;
    }
}
