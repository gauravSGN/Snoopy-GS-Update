using UnityEngine;
using System.Collections.Generic;

public class BubbleAttachments : MonoBehaviour
{
    public Bubble Model { get; private set; }

    private List<GameObject> bubbles = new List<GameObject>();

    public void Attach(GameObject other)
    {
        bubbles.Add(other);
        Model.AddConnection(other.GetComponent<BubbleAttachments>().Model);
    }

    public void Attach(GameObject other, Joint2D joint)
    {
        if (!bubbles.Contains(other))
        {
            bubbles.Add(other);

            Model.AddConnection(other.GetComponent<BubbleAttachments>().Model);
        }
    }

    public void Detach(GameObject other)
    {
        if (bubbles.Contains(other))
        {
            bubbles.Remove(other);

            Model.RemoveConnection(other.GetComponent<BubbleAttachments>().Model);

            other.GetComponent<BubbleAttachments>().Detach(gameObject);
        }
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

        while (bubbles.Count > 0)
        {
            Detach(bubbles[0]);
        }

        Destroy(gameObject);
    }

    private void DisconnectedHandler()
    {
        RemoveHandlers();

        while (bubbles.Count > 0)
        {
            Detach(bubbles[0]);
        }

        var rigidBody = GetComponent<Rigidbody2D>();

        rigidBody.isKinematic = false;
        rigidBody.AddForce(new Vector2(Random.Range(-1.0f, 1.0f), 0.0f));
    }
}
