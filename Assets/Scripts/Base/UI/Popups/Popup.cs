using Service;
using UnityEngine;

namespace UI.Popup
{
    abstract public class Popup : MonoBehaviour
    {
        private bool affirmativeClick;
        private PopupConfig genericConfig;

        virtual public void Setup(PopupConfig config)
        {
            genericConfig = config;
        }

        virtual public void Display()
        {
            gameObject.SetActive(true);
            GetComponent<TranslateTween>().PlayFrom();
            GlobalState.Instance.Services.Get<EventService>().Dispatch(new PopupDisplayedEvent(this));
        }

        virtual public void Affirmative()
        {
            affirmativeClick = true;
            Close();
        }

        virtual public void Close()
        {
            GetComponent<TranslateTween>().PlayToBack(OnCloseTweenComplete);
        }

        virtual protected void OnCloseTweenComplete(AbstractGoTween tween)
        {
            transform.SetParent(null);

            var actionsToProcess = (affirmativeClick ? genericConfig.affirmativeActions : genericConfig.closeActions);

            if (actionsToProcess != null)
            {
                foreach (var action in actionsToProcess)
                {
                    action.Invoke();
                }
            }

            GlobalState.Instance.Services.Get<EventService>().Dispatch(new PopupClosedEvent(this));
            Destroy(gameObject);
        }

        protected void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}