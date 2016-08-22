using Service;
using UnityEngine;
using System.Collections.Generic;

namespace Slideout
{
    sealed public class SlideoutController : MonoBehaviour
    {
        private readonly Queue<GameObject> queue = new Queue<GameObject>();
        private EventService eventService;

        private bool Blocked { get { return GlobalState.Instance.Services.Get<BlockadeService>().PopupsBlocked; } }

        public void Start()
        {
            eventService = GlobalState.EventService;

            eventService.AddEventHandler<ShowSlideoutEvent>(OnShowSlideout);
        }

        private void ShowNextSlideout()
        {
            if ((queue.Count > 0) && !Blocked)
            {
                Debug.Log(string.Format("Showing slideout ({0} in queue)", queue.Count));

                var prefab = queue.Peek();
                var instance = Instantiate(prefab);
                instance.transform.SetParent(transform, false);

                var slideout = instance.GetComponent<SlideoutInstance>();
                slideout.OnComplete += OnSlideoutComplete;

                eventService.Dispatch(new SlideoutStartEvent(instance));
            }
        }

        private void OnShowSlideout(ShowSlideoutEvent gameEvent)
        {
            queue.Enqueue(gameEvent.prefab);

            if (queue.Count == 1)
            {
                ShowNextSlideout();
            }
        }

        private void OnSlideoutComplete(SlideoutInstance slideout)
        {
            Debug.Log("Slideout completed");

            slideout.OnComplete -= OnSlideoutComplete;
            queue.Dequeue();

            eventService.Dispatch(new SlideoutCompleteEvent(slideout.gameObject));

            ShowNextSlideout();
        }
    }
}
