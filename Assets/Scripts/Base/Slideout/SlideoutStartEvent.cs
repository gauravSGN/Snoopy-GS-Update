using UnityEngine;

namespace Slideout
{
    sealed public class SlideoutStartEvent : GameEvent
    {
        public GameObject instance;

        public SlideoutStartEvent(GameObject instance)
        {
            this.instance = instance;
        }
    }
}
