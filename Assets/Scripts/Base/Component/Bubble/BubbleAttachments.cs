using UnityEngine;
using System.Collections.Generic;

public class BubbleAttachments : MonoBehaviour
{
    private List<GameObject> bubbles = new List<GameObject>();
    private List<Joint2D> joints = new List<Joint2D>();

    public void Attach(GameObject other)
    {
        var otherAttachments = other.GetComponent<BubbleAttachments>();
        var joint = otherAttachments.GetAttachmentFor(gameObject);

        if (joint == null)
        {
            joint = gameObject.AddComponent<RelativeJoint2D>();

            joint.connectedBody = other.GetComponent<Rigidbody2D>();

            Attach(other, joint);
            otherAttachments.Attach(gameObject, joint);
        }
    }

    public void Attach(GameObject other, Joint2D joint)
    {
        if (!bubbles.Contains(other))
        {
            bubbles.Add(other);
            joints.Add(joint);
        }
    }

    public void Detach(GameObject other)
    {
        if (bubbles.Contains(other))
        {
            var index = bubbles.IndexOf(other);
            bubbles.RemoveAt(index);
            joints.RemoveAt(index);

            other.GetComponent<BubbleAttachments>().Detach(gameObject);
        }
    }

    public Joint2D GetAttachmentFor(GameObject other)
    {
        return bubbles.Contains(other) ? joints[bubbles.IndexOf(other)] : null;
    }
}
