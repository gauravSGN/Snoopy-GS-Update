using UnityEngine;
using System.Collections.Generic;

namespace Sequence
{
    sealed public class CompleteSequence : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GlobalState.EventService.Dispatch(new SequenceItemCompleteEvent(animator.gameObject));

            base.OnStateEnter(animator, stateInfo, layerIndex);
        }
    }
}
