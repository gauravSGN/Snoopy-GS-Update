using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Reaction;
using Service;
using Animation;
using Effects;
using Model.Scan;

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
        var hits = Scan(gameObject, scanDefinition);
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

    private static RaycastHit2D[] Scan(GameObject baseBubble, ScanDefinition scanLocations)
    {
        var bubbleSize = GlobalState.Instance.Config.bubbles.size;
        var baseSize = bubbleSize * 0.2f;
        var basePosition = baseBubble.transform.position;

        var scans = new List<RaycastHit2D[]>();

        foreach (var location in scanLocations)
        {
            var origin = new Vector2(basePosition.x + (location.x * bubbleSize),
                                     basePosition.y + (location.y * bubbleSize));

            scans.Add(Physics2D.CircleCastAll(origin, baseSize, Vector2.zero, 0.0f));
        }

        return scans.Aggregate(new RaycastHit2D[0], (acc, scan) => acc.Concat(scan).ToArray()).ToArray();
    }
}
