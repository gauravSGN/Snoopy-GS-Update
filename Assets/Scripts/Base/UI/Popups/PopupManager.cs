using UnityEngine;
using Core;

public class PopupManager : MonoBehaviour
{
    public enum PopupType
    {
        OneButton,
        TwoButton
    }

    public enum PopupPriority
    {
        Low = 10,
        Normal = 0,
        High = -10
    }

    [SerializeField]
    private OneButtonPopup oneButtonPopup;

    private PriorityQueue<Tuple<PopupType, PopupConfig>, int> popupQueue;

    private Popup currentPopup;

    void Start()
    {
        popupQueue = new PriorityQueue<Tuple<PopupType, PopupConfig>, int>(Util.MathUtil.CompareInts);
    }

    public void ShowOneButtonPopupImmediate(OneButtonPopupConfig config)
    {
        oneButtonPopup.Setup(config);
        ShowPopup(oneButtonPopup);
    }

    public void EnqueuePopup(PopupType type, PopupConfig config, PopupPriority priority)
    {
        popupQueue.Enqueue(new Tuple<PopupType, PopupConfig>(type, config), (int)priority);
    }

    public void EnqueueOneButtonPopup(OneButtonPopupConfig config, PopupPriority priority)
    {
        EnqueuePopup(PopupType.OneButton, config, priority);
    }

    public void HideCurrentPopup()
    {
        if (currentPopup != null)
        {
            currentPopup.gameObject.SetActive(false);
            currentPopup = null;
        }
    }

    public void ShowNextPopup()
    {
        if (popupQueue.Count > 0)
        {
            Tuple<PopupType, PopupConfig> info = popupQueue.Dequeue();
            if (info.Item1 == PopupType.OneButton)
            {
                ShowOneButtonPopupImmediate((OneButtonPopupConfig)info.Item2);
            }
        }
    }

    private void ShowPopup(Popup toShow)
    {
        HideCurrentPopup();
        currentPopup = toShow;
        currentPopup.gameObject.SetActive(true);
    }
}
