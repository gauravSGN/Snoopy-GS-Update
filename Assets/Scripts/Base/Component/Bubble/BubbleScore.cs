using UnityEngine;
using Service;

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
        GlobalState.Instance.Services.Get<EventService>().Dispatch(new BubbleDestroyedEvent(bubble.definition.Score, gameObject));
    }
}
