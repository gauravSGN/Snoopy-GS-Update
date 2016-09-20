using Registry;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sequence
{
    public class PowerUpBubbleDeathSequence : BubbleDeathSequence
    {
        private float timeToWait;

        public PowerUpBubbleDeathSequence(GameObject gameObject) : base(gameObject) { }
        public PowerUpBubbleDeathSequence(GameObject gameObject, Dictionary<BubbleDeathType, List<IEnumerator>> effects)
             : base(gameObject, effects) { }

        override public void Play(BubbleDeathType type)
        {
            timeToWait = GlobalState.Instance.Config.powerUp.popOrderDelay;
            base.Play(type);
        }

        override protected void Complete(SequenceItemCompleteEvent gameEvent)
        {
            GlobalState.Instance.RunCoroutine(ReduceWaitTime());
            GlobalState.EventService.AddEventHandler<BlockadeEvent.ReactionsUnblocked>(OnBlockingComplete);
        }

        private IEnumerator ReduceWaitTime()
        {
            while (timeToWait > 0)
            {
                yield return null;
                timeToWait = timeToWait - Time.deltaTime;
            }
        }

        private void OnBlockingComplete()
        {
            GlobalState.EventService.RemoveEventHandler<BlockadeEvent.ReactionsUnblocked>(OnBlockingComplete);
            GlobalState.Instance.RunCoroutine(WaitAndComplete(timeToWait));
            timeToWait = -1;
        }

        private IEnumerator WaitAndComplete(float delay)
        {
            yield return new WaitForSeconds(delay);
            Complete();
        }
    }
}