using Service;
using Registry;
using UnityEngine;
using System.Collections.Generic;

namespace Sequence
{
    abstract public class BlockingSequence : Blockade
    {
        protected readonly List<GameObject> pending = new List<GameObject>();
        private static BlockadeService blockadeService;

        public bool Blocking { get { return pending.Count > 0; } }

        abstract public BlockadeType BlockadeType { get; }

        abstract protected void Complete(SequenceItemCompleteEvent gameEvent);

        public BlockingSequence()
        {
            GlobalState.EventService.AddEventHandler<SequenceItemCompleteEvent>(OnItemComplete);

            if (blockadeService == null)
            {
                blockadeService = GlobalState.Instance.Services.Get<BlockadeService>();
            }
        }

        public void Play()
        {
            blockadeService.Add(this);
        }

        protected void OnItemComplete(SequenceItemCompleteEvent gameEvent)
        {
            if (pending.Remove(gameEvent.item) && (pending.Count == 0))
            {
                GlobalState.EventService.RemoveEventHandler<SequenceItemCompleteEvent>(OnItemComplete);
                blockadeService.Remove(this);
                Complete(gameEvent);
            }
        }
    }
}
