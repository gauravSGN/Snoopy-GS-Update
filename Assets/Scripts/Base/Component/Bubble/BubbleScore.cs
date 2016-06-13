using UnityEngine;

public class BubbleScore : MonoBehaviour
{
    public Bubble Model { get; private set; }

    public void SetModel(Bubble model)
    {
        Model = model;
    }

    public void OnDestroy()
    {
        if (Model != null)
        {
            EventDispatcher.Instance.Dispatch(new BubbleDestroyedEvent(Model.definition.score, gameObject));
        }
    }
}
