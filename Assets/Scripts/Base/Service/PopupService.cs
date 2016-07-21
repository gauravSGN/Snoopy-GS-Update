using UI.Popup;

namespace Service
{
    public interface PopupService : SharedService
    {
        void Enqueue(PopupType type, PopupConfig config, PopupPriority priority = PopupPriority.Normal);
    }
}
