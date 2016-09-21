using UnityEngine;
using Animation;
using Sequence;
using System.Collections.Generic;

public class BubbleDeath : MonoBehaviour
{
    // It is specifically 2 so death effects have a chance to get going
    // 1 frame for animation effect delay + 1 to setup and start rendering
    private const int DEACTIVATION_DELAY = 2;

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

        GetComponent<Rigidbody2D>().isKinematic = true;
        Util.FrameUtil.AfterFrames(DEACTIVATION_DELAY, DeactivateObjects);

        var sounds = GetComponent<BubbleModelBehaviour>().Model.definition.Sounds;
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

    public void AddPowerUpEffect(GameObject parent, AnimationType type, BubbleDeathType deathType)
    {
        if (!(DeathSequence is PowerUpBubbleDeathSequence))
        {
            DeathSequence = new PowerUpBubbleDeathSequence(gameObject, DeathSequence.EffectDictionary);
        }

        AddBlockingEffect(parent, type, deathType);
    }

    public void DeactivateObjectOnDeath(GameObject gameObject)
    {
        deactivateOnDeath.Add(gameObject);
    }

    protected void Awake()
    {
        DeathSequence = new BubbleDeathSequence(gameObject);
    }

    private void DeactivateObjects()
    {
        GameObjectUtil.SetActive(deactivateOnDeath, false);
    }
}
