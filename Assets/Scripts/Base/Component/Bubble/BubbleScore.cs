using Event;
using Effects;
using Service;
using Animation;
using UnityEngine;

public class BubbleScore : MonoBehaviour
{
    private Bubble model;
    private bool playedEffect;

    public int Score { get; private set; }

    public void Start()
    {
        GlobalState.EventService.AddEventHandler<BubbleScoreEvent>(OnBubbleScore);
    }

    public void OnDestroy()
    {
        GlobalState.EventService.RemoveEventHandler<BubbleScoreEvent>(OnBubbleScore);
    }

    public void SetModel(Bubble bubbleModel)
    {
        model = bubbleModel;
    }

    private void OnBubbleScore(BubbleScoreEvent gameEvent)
    {
        if (gameEvent.bubble == model)
        {
            Score += gameEvent.score;

            if (!playedEffect)
            {
                playedEffect = true;

                var bubbleDestroyedEvent = new BubbleDestroyedEvent(model.definition.Score, gameObject);
                GlobalState.Instance.Services.Get<EventService>().Dispatch(bubbleDestroyedEvent);

                var effectController = gameObject.GetComponent<BubbleEffectController>();
                effectController.AddEffect(AnimationEffect.Play(gameObject, AnimationType.ScoreText));
            }
        }
    }
}
