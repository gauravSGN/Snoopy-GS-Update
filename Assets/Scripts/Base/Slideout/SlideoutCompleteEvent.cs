using UnityEngine;

namespace Slideout
{
    sealed public class SlideoutCompleteEvent : GameEvent
    {
        public GameObject instance;

        public SlideoutCompleteEvent(GameObject instance)
        {
            this.instance = instance;
        }
    }
}
