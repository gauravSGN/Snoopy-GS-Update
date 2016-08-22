using UnityEngine;

namespace Slideout
{
    sealed public class ShowSlideoutEvent : GameEvent
    {
        public GameObject prefab;

        public ShowSlideoutEvent(GameObject prefab)
        {
            this.prefab = prefab;
        }
    }
}
