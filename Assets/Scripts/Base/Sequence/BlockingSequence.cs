using UnityEngine;
using System.Collections.Generic;

namespace Sequence
{
    abstract public class BlockingSequence
    {
        protected readonly List<GameObject> pending = new List<GameObject>();

        public bool Blocking { get { return pending.Count > 0; } }

        abstract protected void Complete(SequenceItemCompleteEvent gameEvent);

        public BlockingSequence()
        {
            GlobalState.EventService.AddEventHandler<SequenceItemCompleteEvent>(OnItemComplete);
        }

        virtual protected void OnItemComplete(SequenceItemCompleteEvent gameEvent)
        {
            if (pending.Remove(gameEvent.item) && (pending.Count == 0))
            {
                GlobalState.EventService.RemoveEventHandler<SequenceItemCompleteEvent>(OnItemComplete);
                Complete(gameEvent);
            }
        }
    }
}
