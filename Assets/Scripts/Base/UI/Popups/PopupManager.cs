using Core;
using Service;
using UnityEngine;
using System.Collections.Generic;

namespace UI.Popup
{
    public class PopupManager : MonoBehaviour, PopupService
    {
        [SerializeField]
        private Canvas parentCanvas;

        [SerializeField]
        private PopupFactory popupFactory;

        [SerializeField]
        private GameObject popupOverlay;

        private bool popupsEnabled = true;
        private readonly List<Popup> currentPopups = new List<Popup>();
        private readonly PriorityQueue<PopupConfig, int> popupQueue =
            new PriorityQueue<PopupConfig, int>(Util.MathUtil.CompareInts);

        private bool PopupsEnabled
        {
            get { return popupsEnabled; }
            set
            {
                popupsEnabled = value;

                if (PopupsEnabled)
                {
                    ShowNextPopup();
                }
            }
        }

        public void Enqueue(PopupConfig config)
        {
            popupQueue.Enqueue(config, (int)config.priority);
            ShowNextPopup();
        }

        protected void Start()
        {
            var eventService = GlobalState.Instance.Services.Get<EventService>();

            eventService.AddEventHandler<PopupDisplayedEvent>(OnPopupDisplayed);
            eventService.AddEventHandler<PopupClosedEvent>(OnPopupClosed);

            GlobalState.Instance.Services.SetInstance<PopupService>(this);
        }

        private void ShowNextPopup()
        {
            if (PopupsEnabled && (popupQueue.Count > 0))
            {
                var config = popupQueue.Dequeue();

                popupOverlay.gameObject.SetActive(true);

                var popup = popupFactory.CreateByType(config.type);
                popup.gameObject.transform.SetParent(parentCanvas.transform, false);

                var component = popup.gameObject.GetComponent<Popup>();
                component.Setup(config);
                component.Display();
            }
        }

        private void OnPopupDisplayed(PopupDisplayedEvent gameEvent)
        {
            if (!currentPopups.Contains(gameEvent.Popup))
            {
                currentPopups.Add(gameEvent.Popup);
            }

            popupOverlay.gameObject.SetActive(true);
        }

        private void OnPopupClosed(PopupClosedEvent gameEvent)
        {
            if (currentPopups.Contains(gameEvent.Popup))
            {
                currentPopups.Remove(gameEvent.Popup);
            }

            ShowNextPopup();

            if (currentPopups.Count == 0)
            {
                popupOverlay.gameObject.SetActive(false);
            }
        }
    }
}