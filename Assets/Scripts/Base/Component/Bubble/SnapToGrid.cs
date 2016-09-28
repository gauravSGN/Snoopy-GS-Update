using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SnapToGrid : MonoBehaviour
{
    public void AdjustToGrid()
    {
        var myPosition = (Vector2)transform.position;
        var nearbyBubbles = NearbyBubbles(transform.position).Select(b => b.gameObject).ToArray();
        var attachPoints = GetAttachmentPoints(nearbyBubbles).OrderBy(p => (p - myPosition).sqrMagnitude).ToArray();

        foreach (var attachPoint in attachPoints)
        {
            if (CanPlaceAtLocation(attachPoint))
            {
                transform.position = attachPoint;
                return;
            }
        }
    }

    protected IEnumerable<Collider2D> NearbyBubbles(Vector2 location)
    {
        return NearbyBubbles(location, GlobalState.Instance.Config.bubbles.size);
    }

    protected IEnumerable<Collider2D> NearbyBubbles(Vector2 location, float radius)
    {
        foreach (var hit in Physics2D.CircleCastAll(location, radius, Vector2.up, 0.0f))
        {
            if ((hit.collider.gameObject != gameObject) &&
                (hit.collider.gameObject.tag == StringConstants.Tags.BUBBLES))
            {
                yield return hit.collider;
            }
        }
    }

    private bool CanPlaceAtLocation(Vector2 location)
    {
        var halfSize = GlobalState.Instance.Config.bubbles.size / 2.0f;

        foreach (var hit in Physics2D.CircleCastAll(location, halfSize * 0.9f, Vector2.up, 0.0f,
                                                    (1 << (int)Layers.GameObjects | 1 << (int)Layers.Walls)))
        {
            if (hit.collider.gameObject != gameObject)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerable<Vector2> GetAttachmentPoints(IEnumerable<GameObject> bubbles)
    {
        var theta = Mathf.PI / 3.0f;
        var bubbleSize = GlobalState.Instance.Config.bubbles.size;

        foreach (var bubble in bubbles)
        {
            var bubblePosition = (Vector2)bubble.transform.position;

            for (var index = 0; index < 6; index++)
            {
                yield return new Vector2(
                    bubblePosition.x + Mathf.Cos(index * theta) * bubbleSize,
                    bubblePosition.y + Mathf.Sin(index * theta) * bubbleSize
                );
            }
        }
    }
}