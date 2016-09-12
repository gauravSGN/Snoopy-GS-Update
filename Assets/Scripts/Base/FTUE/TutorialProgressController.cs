using System;
using UnityEngine;
using System.Collections.Generic;

namespace FTUE
{
    sealed public class TutorialProgressController : MonoBehaviour
    {
        [Serializable]
        public struct EventTriggerMapping
        {
            public string eventName;
            public TutorialTrigger trigger;
        }

        [SerializeField]
        private List<EventTriggerMapping> mappings;

        public void Start()
        {
            foreach (var mapping in mappings)
            {
                RegisterMapping(mapping.eventName, mapping.trigger);
            }
        }

        private void RegisterMapping(string eventName, TutorialTrigger trigger)
        {
            var eventType = Type.GetType(eventName);

            GlobalState.EventService.Persistent.AddEventHandler(eventType, () => { DispatchTrigger(trigger); });
        }

        private void DispatchTrigger(TutorialTrigger trigger)
        {
            GlobalState.EventService.Dispatch(new TutorialProgressEvent(trigger));
        }
    }
}
