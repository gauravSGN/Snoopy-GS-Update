using UnityEngine;
using System.Collections.Generic;
using Reaction;
using Service;

public class BubbleExplode : MonoBehaviour
{
    [SerializeField]
    private int sizeMultiplier = 2;

    public void Start()
    {
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void Setup(int explosionSize)
    {
        sizeMultiplier = explosionSize;
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
                    var model = hits[index].collider.gameObject.GetComponent<BubbleAttachments>().Model;
                    bubbleList.Add(model);
                    BubbleReactionEvent.Dispatch(ReactionPriority.PowerUp, model);
                }
            }
        }
    }
}
