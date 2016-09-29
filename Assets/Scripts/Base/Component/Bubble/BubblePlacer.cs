using Util;
using Model;
using UnityEngine;

abstract public class BubblePlacer : MonoBehaviour
{
    [SerializeField]
    private BubbleFactory bubbleFactory;

    abstract public GameObject PlaceBubble(BubbleData data);

    protected GameObject CreateBubble(BubbleData data)
    {
        return bubbleFactory.Create(data);
    }
}
