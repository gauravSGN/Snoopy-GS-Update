using UnityEngine;
using System.Collections;

public class BubbleScore : MonoBehaviour
{
    public Bubble Model { get; private set; }

    public void SetModel(Bubble model)
    {
        Model = model;
    }

    public void OnDestroy()
    {
        EventDispatcher.Instance.Dispatch(new BubbleDestroyedEvent(Model.definition.score));
    }
}
