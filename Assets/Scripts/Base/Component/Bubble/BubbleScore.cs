using UnityEngine;

public class BubbleScore : MonoBehaviour
{
    private Bubble model;

    public void SetModel(Bubble bubbleModel)
    {
        model = bubbleModel;
    }

    public void OnDestroy()
    {
        if (model != null)
        {
            GlobalState.Instance.EventDispatcher.Dispatch(new BubbleDestroyedEvent(model.definition.Score, gameObject));
        }
    }
}
