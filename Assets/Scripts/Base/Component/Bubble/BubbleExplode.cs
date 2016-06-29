using UnityEngine;
using System.Collections.Generic;
using Reaction;
using Service;

public class BubbleExplode : MonoBehaviour
{
    [SerializeField]
    private int sizeMultiplier = 2;

    [SerializeField]
    private AnimatorOverrideController deathAnimation;

    public void Start()
    {
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void Setup(int explosionSize, AnimatorOverrideController animation)
    {
        sizeMultiplier = explosionSize;
        deathAnimation = animation;
    }

    protected void OnDestroy()
    {
        GlobalState.Instance.Services.Get<EventService>().RemoveEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void OnSettling(GameEvent gameEvent)
    {
        var baseSize = GlobalState.Instance.Config.bubbles.size * 0.9f;
        var hits = Physics2D.CircleCastAll(transform.position, baseSize * sizeMultiplier, Vector2.up, 0.0f);
        var length = hits.Length;

        if (length > 0)
        {
            var bubbleList = new List<Bubble>();

            for (int index = 0; index < length; index++)
            {
                if (hits[index].collider.gameObject.tag == StringConstants.Tags.BUBBLES)
                {
                    var bubbleAttachments = hits[index].collider.gameObject.GetComponent<BubbleAttachments>();
                    bubbleAttachments.animationController = deathAnimation;
                    bubbleList.Add(bubbleAttachments.Model);
                    BubbleReactionEvent.Dispatch(ReactionPriority.PowerUp, bubbleAttachments.Model);
                }
            }
        }
    }
}
