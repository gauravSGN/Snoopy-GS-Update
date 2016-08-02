using UnityEngine;
using System.Collections.Generic;
using Reaction;
using Service;
using Animation;
using Effects;
using System;

public class BubbleExplode : MonoBehaviour
{
    private static Dictionary<ScanType, Func<GameObject, RaycastHit2D[]>> scanMap =
        new Dictionary<ScanType, Func<GameObject, RaycastHit2D[]>>()
        {
            { ScanType.Circle, CircleScan },
            { ScanType.Diamond, DiamondScan },
        };

    public enum ScanType
    {
        Circle = 0,
        Diamond = 1,
    }

    [SerializeField]
    private ScanType scanType;

    [SerializeField]
    private AnimationType deathAnimationType;

    public void Start()
    {
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void Setup(ScanType type, AnimationType animation)
    {
        scanType = type;
        deathAnimationType = animation;
    }

    protected void OnDestroy()
    {
        GlobalState.Instance.Services.Get<EventService>().RemoveEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void OnSettling(GameEvent gameEvent)
    {
        var hits = scanMap[scanType](gameObject);
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

    private static RaycastHit2D[] CircleScan(GameObject baseBubble)
    {
        var baseSize = GlobalState.Instance.Config.bubbles.size * 0.9f;
        return Physics2D.CircleCastAll(baseBubble.transform.position, baseSize * 2, Vector2.up, 0.0f);
    }

    private static RaycastHit2D[] DiamondScan(GameObject baseBubble)
    {
        var bubbleSize = GlobalState.Instance.Config.bubbles.size;
        var basePosition = baseBubble.transform.position;
        var origin = new Vector2(basePosition.x - bubbleSize, basePosition.y - (0.5f * bubbleSize));
        var tripleBubbleSize = 3 * bubbleSize;
        var size = new Vector2(0.25f * bubbleSize, tripleBubbleSize);
        var theta = Mathf.PI / 3.0f;
        var direction = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));

        return Physics2D.BoxCastAll(origin, size, 30, direction, tripleBubbleSize);
    }
}
