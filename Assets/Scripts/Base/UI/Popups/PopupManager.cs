using System;
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
            popupQueue.Enqueue(config, (int)config.Priority);
            ShowNextPopup();
        }

        protected void Start()
        {
            var eventService = GlobalState.Instance.Services.Get<EventService>();

            eventService.AddEventHandler<PopupDisplayedEvent>(OnPopupDisplayed, Event.HandlerDictType.Persistent);
            eventService.AddEventHandler<PopupClosedEvent>(OnPopupClosed, Event.HandlerDictType.Persistent);

            GlobalState.Instance.Services.SetInstance<PopupService>(this);
        }

        private void ShowNextPopup()
        {
            if (PopupsEnabled && (popupQueue.Count > 0))
            {
                var config = popupQueue.Peek();

                if (config.IgnoreQueue || (currentPopups.Count == 0))
                {
                    popupQueue.Dequeue();

                    var popup = popupFactory.CreateByType(config.Type);
                    popup.gameObject.transform.SetParent(parentCanvas.transform, false);

                    UpdatePopupOverlay();

                    var component = popup.gameObject.GetComponent<Popup>();
                    component.Setup(config);
                    component.Display();
                }
            }
        }

        private void OnPopupDisplayed(PopupDisplayedEvent gameEvent)
        {
            if (!currentPopups.Contains(gameEvent.Popup))
            {
                currentPopups.Add(gameEvent.Popup);
            }
        }

        private void OnPopupClosed(PopupClosedEvent gameEvent)
        {
            if (currentPopups.Contains(gameEvent.Popup))
            {
                currentPopups.Remove(gameEvent.Popup);
            }

            ShowNextPopup();
            UpdatePopupOverlay();
        }

        private void UpdatePopupOverlay()
        {
            var maxSiblingIndex = parentCanvas.transform.childCount - 1;

            popupOverlay.gameObject.SetActive(maxSiblingIndex > 0);
            popupOverlay.gameObject.transform.SetSiblingIndex(Math.Max(0, maxSiblingIndex - 1));
        }
    }
}