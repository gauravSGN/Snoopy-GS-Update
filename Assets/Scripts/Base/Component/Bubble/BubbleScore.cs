using Effects;
using Service;
using Animation;
using UnityEngine;

public class BubbleScore : MonoBehaviour
{
    public void SetModel(Bubble bubbleModel)
    {
        bubbleModel.OnPopped += OnImpendingDestruction;
        bubbleModel.OnDisconnected += OnImpendingDestruction;
    }

    private void RemoveHandlers(Bubble model)
    {
        model.OnPopped -= OnImpendingDestruction;
        model.OnDisconnected -= OnImpendingDestruction;
    }

    private void OnImpendingDestruction(Bubble bubble)
    {
        RemoveHandlers(bubble);
        var bubbleDestroyedEvent = new BubbleDestroyedEvent(bubble.definition.Score, gameObject);
        GlobalState.Instance.Services.Get<EventService>().Dispatch(bubbleDestroyedEvent);

        if (bubble.definition.Score > 0)
        {
            var effectController = gameObject.GetComponent<BubbleEffectController>();
            effectController.AddEffect(DeathAnimationEffect.Play(gameObject, AnimationType.ScoreText));
        }
    }
}
