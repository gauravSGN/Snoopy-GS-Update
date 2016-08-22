using UnityEngine;

public class BubbleParty : MonoBehaviour
{
    Rigidbody2D rb;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<BubbleAttachments>().Model.Active = true;
    }

    protected void Update()
    {
        if ((rb.velocity.y < 0) && (gameObject.layer != (int)Layers.FallingObjects))
        {
            gameObject.layer = (int)Layers.FallingObjects;
        }
    }
}