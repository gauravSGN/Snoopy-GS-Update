using UnityEngine;

public class EnableInputOnComplete : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GlobalState.EventService.Dispatch(new InputToggleEvent(true));
    }
}
