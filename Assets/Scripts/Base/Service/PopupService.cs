using UI.Popup;

namespace Service
{
    public interface PopupService : SharedService
    {
        void Enqueue(PopupConfig config);
    }
}
