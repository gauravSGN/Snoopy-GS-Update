using UnityEngine;
using System.Collections.Generic;
using Reaction;
using Service;
using Animation;
using Effects;
using Model.Scan;
using Util;

public class BubbleExplode : MonoBehaviour
{
    [SerializeField]
    private AnimationType deathAnimationType;

    [SerializeField]
    private ScanDefinition scanDefinition;

    public void Start()
    {
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void Setup(ScanDefinition definition, AnimationType animation)
    {
        scanDefinition = definition;
        deathAnimationType = animation;
    }

    protected void OnDestroy()
    {
        GlobalState.Instance.Services.Get<EventService>().RemoveEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void OnSettling(GameEvent gameEvent)
    {
        var hits = CastingUtil.RelativeBubbleCast(gameObject, scanDefinition);
        var length = hits.Length;

        var bubbleDeath = gameObject.GetComponent<BubbleDeath>();
        bubbleDeath.AddEffect(AnimationEffect.Play(gameObject, deathAnimationType), BubbleDeathType.Pop);

        if (length > 0)
        {
            var bubbleList = new List<Bubble>();

            for (int index = 0; index < length; index++)
            {
                var bubble = hits[index].collider.gameObject;
                var bubbleAttachments = bubble.GetComponent<BubbleAttachments>();

                if ((bubble.tag == StringConstants.Tags.BUBBLES) &&
                    (bubbleAttachments.Model.Active || bubble == gameObject))
                {
                    bubbleList.Add(bubbleAttachments.Model);
                    BubbleReactionEvent.Dispatch(ReactionPriority.PowerUp, bubbleAttachments.Model);
                }
            }
        }
    }
}
