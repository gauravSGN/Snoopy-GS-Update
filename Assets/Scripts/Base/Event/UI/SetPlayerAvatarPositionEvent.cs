using UnityEngine;

public class SetPlayerAvatarPositionEvent : GameEvent
{
    public RectTransform origin;
    public RectTransform destination;

    public SetPlayerAvatarPositionEvent(RectTransform origin, RectTransform destination)
    {
        this.origin = origin;
        this.destination = destination;
    }
}
