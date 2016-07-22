namespace UI.Popup
{
    public class PopupConfig
    {
        virtual public PopupPriority Priority { get; set; }
        virtual public PopupType Type { get { return PopupType.Generic; } }
        virtual public bool IgnoreQueue { get { return false; } }
    }
}