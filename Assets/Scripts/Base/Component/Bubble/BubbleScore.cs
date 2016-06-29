using UnityEngine;
using Service;

public class BubbleScore : MonoBehaviour
{
    private Bubble model;

    public void SetModel(Bubble bubbleModel)
    {
        model = bubbleModel;
    }

    public void OnDestroy()
    {
        if ((model != null) && (GlobalState.Instance != null))
        {
            GlobalState.Instance.Services.Get<EventService>().Dispatch(new BubbleDestroyedEvent(model.definition.Score, gameObject));
        }
    }
}
