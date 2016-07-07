using UnityEngine;
using System.Collections.Generic;
using Reaction;
using Service;
using Animation;
using Effects;

public class BubbleExplode : MonoBehaviour
{
    [SerializeField]
    private int sizeMultiplier = 2;

    [SerializeField]
    private AnimationType deathAnimationType;

    public void Start()
    {
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void Setup(int explosionSize, AnimationType animation)
    {
        sizeMultiplier = explosionSize;
        deathAnimationType = animation;
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

        var bubbleDeath = gameObject.GetComponent<BubbleDeath>();
        bubbleDeath.AddEffect(DeathAnimationEffect.Play(gameObject, deathAnimationType), BubbleDeathType.Pop);

        if (length > 0)
        {
            var bubbleList = new List<Bubble>();

            for (int index = 0; index < length; index++)
            {
                var bubble = hits[index].collider.gameObject;

                if (bubble.tag == StringConstants.Tags.BUBBLES)
                {
                    var bubbleAttachments = bubble.GetComponent<BubbleAttachments>();
                    bubbleList.Add(bubbleAttachments.Model);
                    BubbleReactionEvent.Dispatch(ReactionPriority.PowerUp, bubbleAttachments.Model);
                }
            }
        }
    }
}
