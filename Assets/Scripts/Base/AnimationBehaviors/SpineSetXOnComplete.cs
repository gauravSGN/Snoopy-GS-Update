using UnityEngine;
using Spine;
using Spine.Unity;

public class SpineSetXOnComplete : StateMachineBehaviour
{
    [SerializeField]
    private StateTransitionType transitionType;

    [SerializeField]
    private bool flipX;

    private Skeleton skeleton;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetX(animator, StateTransitionType.Enter);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetX(animator, StateTransitionType.Exit);
    }

    private void SetX(Animator animator, StateTransitionType transition)
    {
        if (transitionType == transition)
        {
            if (skeleton == null)
            {
                skeleton = animator.gameObject.GetComponent<SkeletonAnimator>().Skeleton;
            }

            skeleton.FlipX = flipX;
        }
    }
}
