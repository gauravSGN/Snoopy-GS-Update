using UnityEngine;

public class PositionOnParent : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.transform.localPosition = Vector3.back;
    }
}
