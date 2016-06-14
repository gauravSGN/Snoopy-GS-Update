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
            EventDispatcher.Instance.Dispatch(new BubbleDestroyedEvent(model.definition.score, gameObject));
        }
    }
}
