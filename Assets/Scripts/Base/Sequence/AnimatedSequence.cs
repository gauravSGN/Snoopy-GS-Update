using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sequence
{
    sealed public class AnimatedSequence : MonoBehaviour
    {
        [SerializeField]
        private AnimatedSequence[] startOnComplete;

        private readonly List<GameObject> pending = new List<GameObject>();

        public void Start()
        {
            foreach (Transform child in transform)
            {
                pending.Add(child.gameObject);
            }

            GlobalState.EventService.AddEventHandler<SequenceItemCompleteEvent>(OnItemComplete);
        }

        public void OnItemComplete(SequenceItemCompleteEvent gameEvent)
        {
            if (pending.Remove(gameEvent.item) && (pending.Count == 0))
            {
                GlobalState.EventService.RemoveEventHandler<SequenceItemCompleteEvent>(OnItemComplete);

                var parent = transform.parent;

                foreach (var sequence in startOnComplete)
                {
                    var instance = Instantiate(sequence);
                    instance.transform.SetParent(parent, false);
                }

                Destroy(gameObject);
            }
        }
    }
}
