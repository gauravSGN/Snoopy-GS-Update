using UnityEngine;

namespace Service
{
    public interface TopUIService : SharedService
    {
        void ShowLoading(AsyncOperation op);

        void ShowOneButtonPopup(OneButtonPopupConfig config);
    }
}
