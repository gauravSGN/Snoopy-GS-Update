using UnityEngine;
using System.Collections.Generic;
using Graph;
using Reaction;

public class BubbleExplode : MonoBehaviour
{
    private const float BUBBLE_SPACING = 0.225f;

    [SerializeField]
    private int sizeMultiplier = 2;

    public void Start()
    {
        GlobalState.Instance.EventDispatcher.AddEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void Setup(int explosionSize)
    {
        sizeMultiplier = explosionSize;
    }

    protected void OnDestroy()
    {
        GlobalState.Instance.EventDispatcher.RemoveEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void OnSettling(GameEvent gameEvent)
    {
        var hits = Physics2D.CircleCastAll(transform.position, BUBBLE_SPACING * sizeMultiplier, Vector2.up, 0.0f);
        var length = hits.Length;

        if (length > 0)
        {
            var bubbleList = new List<Bubble>();

            for (int index = 0; index < length; index++)
            {
                if (hits[index].collider.gameObject.tag == StringConstants.Tags.BUBBLES)
                {
                    var model = hits[index].collider.gameObject.GetComponent<BubbleAttachments>().Model;
                    bubbleList.Add(model);
                    BubbleReactionEvent.Dispatch(ReactionPriority.PowerUp, model);
                }
            }
        }
    }
}
