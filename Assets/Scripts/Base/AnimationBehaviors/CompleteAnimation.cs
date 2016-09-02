using UnityEngine;
using Animation;

public class CompleteAnimation : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimationCompleteEvent.Dispatch(animator.gameObject);
    }
}
