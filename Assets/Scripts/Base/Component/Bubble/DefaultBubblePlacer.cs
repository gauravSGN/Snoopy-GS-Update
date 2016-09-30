using Model;
using UnityEngine;

sealed public class DefaultBubblePlacer : BubblePlacer
{
    [SerializeField]
    private Transform bubbleContainer;

    override public GameObject PlaceBubble(BubbleData data)
    {
        var instance = CreateBubble(data);

        if (bubbleContainer != null)
        {
            instance.transform.SetParent(bubbleContainer, false);
        }

        instance.transform.localPosition = data.WorldPosition;

        return instance;
    }
}
