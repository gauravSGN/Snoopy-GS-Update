using UnityEngine;
using Reaction;
using Animation;
using Effects;
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
        var hits = scanFunction();
        var length = hits.Length;

        AnimationReactionEvent.Dispatch(ReactionPriority.PreReactionAnimation, explosionAnimationType, gameObject);

        for (int index = 0; index < length; index++)
        {
            var bubble = hits[index].collider.gameObject;

            if (bubble.tag == StringConstants.Tags.BUBBLES)
            {
                var model = bubble.GetComponent<BubbleModelBehaviour>().Model;

                if (model.Active || (bubble == gameObject))
                {
                    AddReaction(bubble, model);
                }
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

    private void AddReaction(GameObject bubble, Bubble model)
    {
        if (deathAnimationType != AnimationType.None)
        {
            var bubbleDeath = bubble.GetComponent<BubbleDeath>();
            bubbleDeath.ReplaceEffect(AnimationEffect.Play(bubble, deathAnimationType), BubbleDeathType.Pop);
        }

        BubbleReactionEvent.Dispatch(ReactionPriority.PowerUp, model);
    }
}
