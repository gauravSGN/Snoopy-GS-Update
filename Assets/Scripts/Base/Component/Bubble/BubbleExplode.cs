using UnityEngine;
using Reaction;
using Service;
using Animation;
using Effects;
using ScanFunction = Util.CastingUtil.ScanFunction;

public class BubbleExplode : MonoBehaviour
{
    private ScanFunction scanFunction;

    [SerializeField]
    private AnimationType deathAnimationType;

    [SerializeField]
    private AnimationType explosionAnimationType;

    public void Setup(ScanFunction callback, AnimationType deathAnimation, AnimationType explosionAnimation)
    {
        scanFunction = callback;
        deathAnimationType = deathAnimation;
        explosionAnimationType = explosionAnimation;
    }

    public void OnSettling(GameEvent gameEvent)
    {
        var hits = scanFunction();
        var length = hits.Length;

        AnimationReactionEvent.Dispatch(ReactionPriority.PreReactionAnimation, explosionAnimationType, gameObject);

        if (length > 0)
        {
            for (int index = 0; index < length; index++)
            {
                var bubble = hits[index].collider.gameObject;
                var bubbleAttachments = bubble.GetComponent<BubbleAttachments>();

                if ((bubble.tag == StringConstants.Tags.BUBBLES) &&
                    (bubbleAttachments.Model.Active || bubble == gameObject))
                {
                    AddReaction(bubble, bubbleAttachments.Model);
                }
            }
        }
    }

    protected void Start()
    {
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    protected void OnDestroy()
    {
        GlobalState.Instance.Services.Get<EventService>().RemoveEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    private void AddReaction(GameObject bubble, Bubble model)
    {
        if (deathAnimationType != AnimationType.None)
        {
            var bubbleDeath = bubble.GetComponent<BubbleDeath>();
            bubbleDeath.AddEffect(AnimationEffect.Play(bubble, deathAnimationType), BubbleDeathType.Pop);
        }

        BubbleReactionEvent.Dispatch(ReactionPriority.PowerUp, model);
    }
}
