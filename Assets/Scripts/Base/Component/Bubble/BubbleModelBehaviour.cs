using UnityEngine;

abstract public class BubbleModelBehaviour : MonoBehaviour
{
    public Bubble Model { get; private set; }

    public void SetModel(Bubble model)
    {
        Model = model;

        AddListeners();
    }

    virtual public void OnDestroy()
    {
        RemoveListeners();
    }

    virtual protected void AddListeners()
    {
        // Override to listen to model events
    }

    virtual protected void RemoveListeners()
    {
        // Override to remove model listeners
    }
}
