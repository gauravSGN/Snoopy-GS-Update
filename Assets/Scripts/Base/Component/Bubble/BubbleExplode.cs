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
            ProcessHitGroup(hits, delay);
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

    private void AddDeathAnimation(GameObject bubble)
    {
        if (deathAnimationType != AnimationType.None)
        {
            var bubbleDeath = bubble.GetComponent<BubbleDeath>();
            bubbleDeath.AddPowerUpEffect(bubble, deathAnimationType, BubbleDeathType.Pop);
        }
    }

    private void ProcessHitGroup(RaycastHit2D[] hits, float delay)
    {
        List<Bubble> bubbles = null;

        foreach (var hit in hits)
        {
            var bubble = hit.collider.gameObject;

            if (bubble.tag == StringConstants.Tags.BUBBLES)
            {
                var model = bubble.GetComponent<BubbleModelBehaviour>().Model;

                // Check against gameObject is to ensure power ups can explode even when they exit the active area
                if (model.Active || (bubble == gameObject))
                {
                    bubbles = bubbles ?? new List<Bubble>();
                    bubbles.Add(model);
                    AddDeathAnimation(bubble);
                }
            }
        }

        if (bubbles != null)
        {
            BubbleGroupReactionEvent.Dispatch(ReactionPriority.PowerUp, bubbles, delay);
        }
    }
}
