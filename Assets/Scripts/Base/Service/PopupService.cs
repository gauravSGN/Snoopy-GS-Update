using UI.Popup;

namespace Service
{
    public interface PopupService : SharedService
    {
        void Enqueue(PopupConfig config);
        void EnqueueWithDelay(float delay, PopupConfig config);
    }
}
