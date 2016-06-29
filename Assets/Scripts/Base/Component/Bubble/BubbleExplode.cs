using UnityEngine;
using System.Collections.Generic;
using Reaction;
using Service;

public class BubbleExplode : MonoBehaviour
{
    [SerializeField]
    private int sizeMultiplier = 2;

    [SerializeField]
    private GameObject deathAnimation;

    public void Start()
    {
        GlobalState.Instance.Services.Get<EventService>().AddEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    public void Setup(int explosionSize, GameObject animation)
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
                var bubble = hits[index].collider.gameObject;

                if (bubble.tag == StringConstants.Tags.BUBBLES)
                {
                    var bubbleDeath = bubble.GetComponent<BubbleDeath>();
                    var animationObject = (GameObject)Instantiate(deathAnimation, bubble.transform.position, Quaternion.identity);
                    bubbleDeath.AddPopEffect(animationObject);

                    var bubbleAttachments = bubble.GetComponent<BubbleAttachments>();
                    bubbleList.Add(bubbleAttachments.Model);
                    BubbleReactionEvent.Dispatch(ReactionPriority.PowerUp, bubbleAttachments.Model);
                }
            }
        }
    }
}
