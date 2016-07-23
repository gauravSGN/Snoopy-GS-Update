using Service;
using UnityEngine;

namespace UI.Popup
{
    public class Popup : MonoBehaviour
    {
        virtual public void Setup(PopupConfig config)
        {
        }

        virtual public void Display()
        {
            gameObject.SetActive(true);
            gameObject.GetComponent<TranslateTween>().PlayFrom();
            GlobalState.Instance.Services.Get<EventService>().Dispatch(new PopupDisplayedEvent(this));
        }

        virtual public void Close()
        {
            gameObject.GetComponent<TranslateTween>().PlayToBack(OnCloseTweenComplete);
        }

        virtual protected void OnCloseTweenComplete(AbstractGoTween tween)
        {
            gameObject.transform.SetParent(null);
            GlobalState.Instance.Services.Get<EventService>().Dispatch(new PopupClosedEvent(this));
            Destroy(gameObject);
        }

        protected void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}