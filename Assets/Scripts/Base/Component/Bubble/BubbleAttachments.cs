using UnityEngine;

public class BubbleAttachments : MonoBehaviour
{
    public Bubble Model { get { return model; } }

    [SerializeField]
    private Bubble model;

    public void Attach(GameObject other)
    {
        Model.Connect(other.GetComponent<BubbleAttachments>().Model);
    }

    public void SetModel(Bubble model)
    {
        this.model = model;

        model.OnPopped += PoppedHandler;
        model.OnDisconnected += DisconnectedHandler;
    }

    private void RemoveHandlers()
    {
        Model.OnPopped -= PoppedHandler;
        Model.OnDisconnected -= DisconnectedHandler;
    }

    private void PoppedHandler(Bubble bubble)
    {
        RemoveHandlers();

        gameObject.layer = (int)Layers.IgnoreRayCast;
        var death = gameObject.GetComponent<BubbleDeath>();
        StartCoroutine(death.TriggerDeathEffects(BubbleDeathType.Pop));
    }

    private void DisconnectedHandler(Bubble bubble)
    {
        RemoveHandlers();

        var rigidBody = GetComponent<Rigidbody2D>();
        gameObject.layer = (int)Layers.FallingObjects;

        rigidBody.isKinematic = false;
        rigidBody.AddForce(new Vector3(Random.Range(GlobalState.Instance.Config.reactions.cullMinXForce, GlobalState.Instance.Config.reactions.cullMaxXForce),
                                       Random.Range(GlobalState.Instance.Config.reactions.cullMinYForce, GlobalState.Instance.Config.reactions.cullMaxYForce)));
    }
}
