using UnityEngine;

public class BubbleAttachments : MonoBehaviour
{
    private const int UPDATES_BEFORE_DESTRUCTION = 2;
    private const int DO_NOT_DESTRUCT = -1;

    public Bubble Model { get { return model; } }

    [SerializeField]
    private Bubble model;

    private int updatesTilDestruction = -1;

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

    public void MarkForDestruction()
    {
        updatesTilDestruction = UPDATES_BEFORE_DESTRUCTION;
    }

    protected void FixedUpdate()
    {
        if (updatesTilDestruction != DO_NOT_DESTRUCT)
        {
            if (updatesTilDestruction > 0)
            {
                updatesTilDestruction--;
            }
            else
            {
                RemoveHandlers();
                Destroy(gameObject);
            }
        }
    }

    private void RemoveHandlers()
    {
        Model.OnPopped -= PoppedHandler;
        Model.OnDisconnected -= DisconnectedHandler;
    }

    private void PoppedHandler(Bubble bubble)
    {
        RemoveHandlers();

        transform.position = new Vector3(-1000.0f, -1000.0f);
        MarkForDestruction();
    }

    private void DisconnectedHandler(Bubble bubble)
    {
        RemoveHandlers();

        var rigidBody = GetComponent<Rigidbody2D>();
        gameObject.layer = (int)Layers.FallingObjects;

        rigidBody.isKinematic = false;
        rigidBody.AddForce(new Vector2(Random.Range(-3.0f, 3.0f), 0.0f));
    }
}
