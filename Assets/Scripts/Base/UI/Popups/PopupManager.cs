using System;
using Service;
using Registry;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace UI.Popup
{
    public class PopupManager : MonoBehaviour, PopupService
    {
        [SerializeField]
        private float fadeTime;

        [SerializeField]
        private Color overlayColor;

        [SerializeField]
        private RectTransform popupParent;

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

        private bool PopupsBlocked
        {
            get { return GlobalState.Instance.Services.Get<BlockadeService>().PopupsBlocked; }
        }

        public void Enqueue(PopupConfig config)
        {
            popupQueue.Enqueue(config, (int)config.Priority);
            ShowNextPopup();
        }

        public void EnqueueWithDelay(float delay, PopupConfig config)
        {
            StartCoroutine(DelayedEnqueue(delay, config));
        }

        protected void Start()
        {
            var service = GlobalState.EventService;
            service.Persistent.AddEventHandler<PopupDisplayedEvent>(OnPopupDisplayed);
            service.Persistent.AddEventHandler<PopupClosedEvent>(OnPopupClosed);
            service.Persistent.AddEventHandler<BlockadeEvent.PopupsUnblocked>(ShowNextPopup);

            GlobalState.Instance.Services.SetInstance<PopupService>(this);
        }

        private void ShowNextPopup()
        {
            if (PopupsEnabled && !PopupsBlocked && (popupQueue.Count > 0))
            {
                var config = popupQueue.Peek();

                if (config.IgnoreQueue || (currentPopups.Count == 0))
                {
                    popupQueue.Dequeue();

                    var popup = popupFactory.CreateByType(config.Type);
                    popup.gameObject.transform.SetParent(popupParent, false);

                    UpdatePopupOverlay();

                    var component = popup.gameObject.GetComponent<Popup>();
                    component.Setup(config);
                    component.Display();

                    var definition = popupFactory.GetDefinitionByType(config.Type);

                    if (definition.DisplaySound != null)
                    {
                        var soundSource = popup.gameObject.AddComponent<AudioSource>();
                        soundSource.clip = definition.DisplaySound;
                        soundSource.Play();
                    }
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
            var maxSiblingIndex = popupParent.childCount - 1;
            var overlayActive = (maxSiblingIndex > 0);
            var currentlyActive = popupOverlay.gameObject.activeInHierarchy;

            popupOverlay.gameObject.transform.SetSiblingIndex(Math.Max(0, maxSiblingIndex - 1));

            if (!currentlyActive && overlayActive)
            {
                popupOverlay.GetComponent<Image>().color = overlayColor;
                popupOverlay.gameObject.SetActive(true);
            }
            else if (currentlyActive && !overlayActive)
            {
                StartCoroutine(FadeOverlay());
            }
        }

        private IEnumerator DelayedEnqueue(float delay, PopupConfig config)
        {
            yield return new WaitForSeconds(delay);
            Enqueue(config);
        }

        private IEnumerator FadeOverlay()
        {
            var time = 0.0f;
            var overlayImage = popupOverlay.GetComponent<Image>();

            if (overlayImage != null)
            {
                while (time < fadeTime)
                {
                    time += Time.deltaTime;

                    var color = overlayImage.color;
                    color.a = Mathf.Lerp(color.a, 0, (time / fadeTime));
                    overlayImage.color = color;

                    yield return null;
                }
            }

            popupOverlay.gameObject.SetActive(false);
        }
    }
}