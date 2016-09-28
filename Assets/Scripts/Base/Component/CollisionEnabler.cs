using UnityEngine;
using System.Collections.Generic;

public class CollisionEnabler : MonoBehaviour
{
    [SerializeField]
    private Layers layer;

    [SerializeField]
    private List<Renderer> disableOnCollision;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Layers)collision.gameObject.layer == layer)
        {
            EnableBehaviours(false);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if ((Layers)collision.gameObject.layer == layer)
        {
            EnableBehaviours(true);
        }
    }

    private void EnableBehaviours(bool enable)
    {
        foreach (var behaviour in disableOnCollision)
        {
            behaviour.enabled = enable;
        }
    }
}