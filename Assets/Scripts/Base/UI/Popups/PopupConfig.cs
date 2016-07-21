namespace UI.Popup
{
    public class PopupConfig
    {
        virtual public PopupPriority priority { get; set; }
        virtual public PopupType type { get { return PopupType.Generic; } }
    }
}