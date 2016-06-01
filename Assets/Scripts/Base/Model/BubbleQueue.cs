using UnityEngine;

public class BubbleQueue
{
    public BubbleType GetNext()
    {
        return (BubbleType)Random.Range(0, 4);
    }
}
