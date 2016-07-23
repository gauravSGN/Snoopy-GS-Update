using UI.Popup;

public class BasePopupEvent : GameEvent
{
    public Popup Popup { get; private set; }

    public BasePopupEvent(Popup popup)
    {
        Popup = popup;
    }
}