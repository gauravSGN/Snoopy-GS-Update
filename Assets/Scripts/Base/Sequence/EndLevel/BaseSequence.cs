using System;
using UnityEngine;
using System.Collections;

namespace Sequence
{
    abstract public class BaseSequence<T> : MonoBehaviour
    {
        abstract public void Begin(T parameters);

        protected void TransitionToReturnScene()
        {
            GlobalState.EventService.Dispatch(new TransitionToReturnSceneEvent());
        }

        protected IEnumerator RunActionAfterDelay(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}
