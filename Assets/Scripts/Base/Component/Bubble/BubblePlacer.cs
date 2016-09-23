using Util;
using Model;
using UnityEngine;

abstract public class BubblePlacer : MonoBehaviour
{
    [SerializeField]
    private BubbleFactory bubbleFactory;

    private float rowDistance;
    private float topEdge;

    abstract public GameObject PlaceBubble(BubbleData data);

    virtual public void Start()
    {
        rowDistance = GlobalState.Instance.Config.bubbles.size * MathUtil.COS_30_DEGREES;
        topEdge = Camera.main.orthographicSize - (0.5f * rowDistance);
    }

    protected GameObject CreateBubble(BubbleData data)
    {
        return bubbleFactory.Create(data);
    }

    protected Vector3 GetBubbleLocation(int x, int y)
    {
        var config = GlobalState.Instance.Config;
        var offset = (y & 1) * config.bubbles.size / 2.0f;
        var leftEdge = -(config.bubbles.numPerRow - 1) * config.bubbles.size / 2.0f;
        return new Vector3(leftEdge + x * config.bubbles.size + offset, topEdge - y * rowDistance);
    }
}
