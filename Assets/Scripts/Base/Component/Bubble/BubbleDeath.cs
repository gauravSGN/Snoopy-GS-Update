using UnityEngine;
using Animation;
using Sequence;
using System.Collections.Generic;

public class BubbleDeath : MonoBehaviour
{
    public bool dying = false;

    [SerializeField]
    private GameObject destroyOnFinish;

    [SerializeField]
    private float deathDelay;

    [SerializeField]
    private List<GameObject> deactivateOnDeath;

    public BubbleDeathSequence DeathSequence { get; private set;}

    public static void KillBubble(GameObject bubble, BubbleDeathType type)
    {
        var death = bubble.GetComponent<BubbleDeath>();

        if (death == null)
        {
            Destroy(bubble);
        }
        else if (!death.dying)
        {
            death.Die(type);
        }
    }

    public void Die(BubbleDeathType type)
    {
        dying = true;

        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        GameObjectUtil.SetActive(deactivateOnDeath, false);

        var sounds = gameObject.GetComponent<BubbleModelBehaviour>().Model.definition.Sounds;
        Sound.PlaySoundEvent.Dispatch((type == BubbleDeathType.Pop) ? sounds.match : sounds.cull);

        DeathSequence.Play(type);
    }

    public void AddEffect(GameObject parent, AnimationType type, BubbleDeathType deathType)
    {
        DeathSequence.AddEffect(parent, type, deathType, false);
    }

    public void AddBlockingEffect(GameObject parent, AnimationType type, BubbleDeathType deathType)
    {
        DeathSequence.AddEffect(parent, type, deathType, true);
    }

    public void DeactivateObjectOnDeath(GameObject gameObject)
    {
        deactivateOnDeath.Add(gameObject);
    }

    protected void Awake()
    {
        DeathSequence = new BubbleDeathSequence(gameObject);
    }
}
