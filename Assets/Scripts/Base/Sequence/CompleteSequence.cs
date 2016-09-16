using UnityEngine;

namespace Sequence
{
    sealed public class CompleteSequence : SelectableTransitionBehaviour
    {
        override protected void OnSetTransition(Animator animator)
        {
            GlobalState.EventService.Dispatch(new SequenceItemCompleteEvent(animator.gameObject));
        }
    }
}
