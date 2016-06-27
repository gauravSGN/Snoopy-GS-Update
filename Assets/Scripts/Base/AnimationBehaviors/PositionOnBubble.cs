using UnityEngine;

public class PositionOnBubble : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.transform.localPosition = new Vector3(0,0,-1);
    }
}
