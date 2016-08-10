using UnityEngine;

public class SnapMapToLocationEvent : GameEvent
{
    public RectTransform target;

    public SnapMapToLocationEvent(RectTransform target)
    {
        this.target = target;
    }
}
