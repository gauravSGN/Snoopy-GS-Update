using Reaction;
using Animation;
using UnityEngine;
using System.Collections.Generic;
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
        var hitGroups = scanFunction();
        var delay = GlobalState.Instance.Config.powerUp.popOrderDelay;
        AnimationReactionEvent.Dispatch(ReactionPriority.PreReactionAnimation, explosionAnimationType, gameObject);

        foreach (var hits in hitGroups)
        {
            var bubbles = new List<Bubble>();

            foreach (var hit in hits)
            {
                var bubble = hit.collider.gameObject;

                if (bubble.tag == StringConstants.Tags.BUBBLES)
                {
                    var model = bubble.GetComponent<BubbleModelBehaviour>().Model;

                    if (model.Active || (bubble == gameObject))
                    {
                        bubbles.Add(model);
                        AddDeathAnimation(bubble, model);
                    }
                }
            }

            if (bubbles.Count > 0)
            {
                BubbleGroupReactionEvent.Dispatch(ReactionPriority.PowerUp, bubbles, delay);
            }
        }
    }

    protected void Start()
    {
        GlobalState.EventService.AddEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    protected void OnDestroy()
    {
        GlobalState.EventService.RemoveEventHandler<BubbleSettlingEvent>(OnSettling);
    }

    private void AddDeathAnimation(GameObject bubble, Bubble model)
    {
        if (deathAnimationType != AnimationType.None)
        {
            var bubbleDeath = bubble.GetComponent<BubbleDeath>();
            bubbleDeath.AddBlockingEffect(bubble, deathAnimationType, BubbleDeathType.Pop);
        }
    }
}
