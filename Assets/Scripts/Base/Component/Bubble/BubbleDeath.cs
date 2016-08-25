using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        for (int i = 0, length = effectDictionary[type].Count; i < length; i++)
        {
            effectController.AddEffect(effectDictionary[type][i]);
        }

        GameObjectUtil.SetActive((type == BubbleDeathType.Pop) ? activateOnPop : activateOnCull, true);
        GameObjectUtil.SetActive(deactivateOnDeath, false);

        var player = GetComponent<BubbleSoundPlayer>();

        if (player != null)
        {
            var sounds = GetComponent<BubbleAttachments>().Model.definition.Sounds;
            player.Play((type == BubbleDeathType.Pop) ? sounds.match : sounds.cull);
        }

        Destroy(destroyOnFinish, deathDelay);
    }

    public void AddEffect(IEnumerator effect, BubbleDeathType type)
    {
        effectDictionary[type].Add(effect);
    }

    public void DeactivateObjectOnDeath(GameObject gameObject)
    {
        deactivateOnDeath.Add(gameObject);
    }

    protected void Awake()
    {
        effectDictionary = new Dictionary<BubbleDeathType, List<IEnumerator>>();
        effectDictionary[BubbleDeathType.Pop] = new List<IEnumerator>();
        effectDictionary[BubbleDeathType.Cull] = new List<IEnumerator>();
    }
}
