using UnityEngine;
using System.Collections.Generic;

public class BubbleAttachments : MonoBehaviour
{
    public Bubble Model { get; private set; }

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

    private void RemoveHandlers()
    {
        Model.OnPopped -= PoppedHandler;
        Model.OnDisconnected -= DisconnectedHandler;
    }

    private void PoppedHandler()
    {
        RemoveHandlers();

        transform.position = new Vector3(-1000.0f, -1000.0f);
        Destroy(gameObject, 0.1f);
    }

    private void DisconnectedHandler()
    {
        RemoveHandlers();

        var rigidBody = GetComponent<Rigidbody2D>();
        gameObject.layer = 8;

        rigidBody.isKinematic = false;
        rigidBody.AddForce(new Vector2(Random.Range(-3.0f, 3.0f), 0.0f));
    }
}
