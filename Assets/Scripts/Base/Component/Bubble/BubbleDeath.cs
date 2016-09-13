using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Animation;
using Effects;
using Sequence;

public class BubbleDeath : MonoBehaviour
{
    public bool dying = false;

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
    private List<GameObject> blockingList = new List<GameObject>();

    public static void KillBubble(GameObject bubble, BubbleDeathType type)
    {
        var death = bubble.GetComponent<BubbleDeath>();

        if (death == null)
        {
            Destroy(bubble);
        }
        else if (!death.dying)
        {
            bubble.GetComponent<Rigidbody2D>().isKinematic = true;
            death.TriggerDeathEffects(type);
            death.dying = true;
        }
    }

    public void TriggerDeathEffects(BubbleDeathType type)
    {
        var effectController = gameObject.GetComponent<BubbleEffectController>();
        var effects = effectDictionary.ContainsKey(type) ? effectDictionary[type] : GetDefaultEffects(type);

        GlobalState.EventService.AddEventHandler<SequenceItemCompleteEvent>(OnItemComplete);

        foreach (var effect in effects)
        {
            effectController.AddEffect(effect);
        }

        GameObjectUtil.SetActive((type == BubbleDeathType.Pop) ? activateOnPop : activateOnCull, true);
        GameObjectUtil.SetActive(deactivateOnDeath, false);

        var sounds = GetComponent<BubbleModelBehaviour>().Model.definition.Sounds;
        Sound.PlaySoundEvent.Dispatch((type == BubbleDeathType.Pop) ? sounds.match : sounds.cull);
    }



    public void AddEffect(GameObject parent, AnimationType type, BubbleDeathType deathType)
    {
        if (!effectDictionary.ContainsKey(deathType))
        {
            effectDictionary.Add(deathType, new List<IEnumerator>());
        }

        effectDictionary[deathType].Add(AnimationEffect.PlayAndRegister(parent, type, RegisterBlockers));
    }

    public void ReplaceEffect(GameObject parent, AnimationType type, BubbleDeathType deathType)
    {
        if (effectDictionary.ContainsKey(deathType))
        {
            effectDictionary[deathType].Clear();
        }

        AddEffect(parent, type, deathType);
    }

    public void DeactivateObjectOnDeath(GameObject gameObject)
    {
        deactivateOnDeath.Add(gameObject);
    }

    public void RegisterBlockers(GameObject blocker)
    {
        blockingList.Add(blocker);
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
                effects.Add(AnimationEffect.PlayAndRegister(gameObject, animationType, RegisterBlockers));
            }
        }

        return effects;
    }

    private void OnItemComplete(SequenceItemCompleteEvent gameEvent)
    {
        if (blockingList.Remove(gameEvent.item) && (blockingList.Count == 0))
        {
            if (gameObject.GetComponent<BubbleScore>().Score > 0)
            {
                var effectController = gameObject.GetComponent<BubbleEffectController>();
                effectController.AddEffect(AnimationEffect.Play(gameObject, AnimationType.ScoreText));
            }

            GlobalState.EventService.RemoveEventHandler<SequenceItemCompleteEvent>(OnItemComplete);
            Destroy(destroyOnFinish, deathDelay);
        }
    }
}
