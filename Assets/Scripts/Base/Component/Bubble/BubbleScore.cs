using Event;
using Effects;
using Animation;
using UnityEngine;

public class BubbleScore : BubbleModelBehaviour
{
    private bool playedEffect;

    public int Score { get; private set; }

    public void Start()
    {
        GlobalState.EventService.AddEventHandler<BubbleScoreEvent>(OnBubbleScore);
    }

    override public void OnDestroy()
    {
        base.OnDestroy();

        GlobalState.EventService.RemoveEventHandler<BubbleScoreEvent>(OnBubbleScore);
    }

    private void OnBubbleScore(BubbleScoreEvent gameEvent)
    {
        if (gameEvent.bubble == Model)
        {
            Score += gameEvent.score;

            if (!playedEffect)
            {
                playedEffect = true;

                var bubbleDestroyedEvent = new BubbleDestroyedEvent(Model.definition.Score, gameObject);
                GlobalState.EventService.Dispatch(bubbleDestroyedEvent);

                var effectController = gameObject.GetComponent<BubbleEffectController>();
                effectController.AddEffect(AnimationEffect.Play(gameObject, AnimationType.ScoreText));
            }
        }
    }
}
