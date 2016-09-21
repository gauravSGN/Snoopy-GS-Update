using UnityEngine;

namespace Sequence
{
    sealed public class SequenceItemCompleteEvent : GameEvent
    {
        public GameObject item;

        public SequenceItemCompleteEvent(GameObject item)
        {
            this.item = item;
        }
    }
}
