using UnityEngine;
using Util;

public class ActiveGameArea : MonoBehaviour
{
    protected void OnEnable()
    {
        var bounds = gameObject.GetComponent<Collider2D>().bounds;
        var bubbles =  CastingUtil.BoundsBoxCast(bounds, 1 << (int)Layers.GameObjects);

        foreach (var bubble in bubbles)
        {
            ActivateBubble(bubble.transform.gameObject, true);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        ActivateBubble(collision.gameObject, true);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        ActivateBubble(collision.gameObject, false);
    }

    private void ActivateBubble(GameObject bubble, bool active)
    {
        if (bubble.tag == StringConstants.Tags.BUBBLES)
        {
            bubble.GetComponent<BubbleAttachments>().Model.active = active;
        }
    }
}
