using UnityEngine;

public class BubbleAttachments : BubbleModelBehaviour
{
    public void Attach(GameObject other)
    {
        Model.Connect(other.GetComponent<BubbleModelBehaviour>().Model);
    }

    protected override void AddListeners()
    {
        Model.OnPopped += PoppedHandler;
        Model.OnDisconnected += DisconnectedHandler;
    }

    protected override void RemoveListeners()
    {
        Model.OnPopped -= PoppedHandler;
        Model.OnDisconnected -= DisconnectedHandler;

        GlobalState.EventService.RemoveEventHandler<CullAllBubblesEvent>(OnCullAllBubbles);
    }

    protected void Start()
    {
        if (Model.type != BubbleType.Ceiling)
        {
            GlobalState.EventService.AddEventHandler<CullAllBubblesEvent>(OnCullAllBubbles);
        }
    }

    private void PoppedHandler(Bubble bubble)
    {
        RemoveListeners();

        gameObject.layer = (int)Layers.IgnoreRayCast;
        BubbleDeath.KillBubble(gameObject, BubbleDeathType.Pop);
    }

    private void DisconnectedHandler(Bubble bubble)
    {
        RemoveListeners();
    }

    private void OnCullAllBubbles(CullAllBubblesEvent gameEvent)
    {
        if (gameObject.layer == (int)Layers.GameObjects)
        {
            Model.RemoveFromGraph();
        }
    }
}
