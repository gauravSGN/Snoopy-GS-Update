using UnityEngine;

public class FireAnimationEventOnComplete : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GlobalState.EventService.Dispatch(new FiringAnimationCompleteEvent());
    }
}

