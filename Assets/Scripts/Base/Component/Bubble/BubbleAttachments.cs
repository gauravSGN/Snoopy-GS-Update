using UnityEngine;

public class BubbleAttachments : MonoBehaviour
{
    public const int UPDATES_BEFORE_DESTRUCTION = 2;
    public const int DO_NOT_DESTRUCT = -1;

    public Bubble Model { get; private set; }

    private int updatesTilDestruction = -1;

    public void Attach(GameObject other)
    {
        Model.AddConnection(other.GetComponent<BubbleAttachments>().Model);
    }

    public void SetModel(Bubble model)
    {
        Model = model;

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

    private void PoppedHandler()
    {
        RemoveHandlers();

        transform.position = new Vector3(-1000.0f, -1000.0f);
        MarkForDestruction();
    }

    private void DisconnectedHandler()
    {
        RemoveHandlers();

        var rigidBody = GetComponent<Rigidbody2D>();
        gameObject.layer = (int)Layers.FallingObjects;

        rigidBody.isKinematic = false;
        rigidBody.AddForce(new Vector2(Random.Range(-3.0f, 3.0f), 0.0f));
    }
}
