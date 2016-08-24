using UnityEngine;
using Spine;
using Spine.Unity;

public class SpineResetOnEnter : StateMachineBehaviour
{
    private Skeleton skeleton;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (skeleton == null)
        {
            skeleton = animator.gameObject.GetComponent<SkeletonAnimator>().Skeleton;
        }

        skeleton.SetToSetupPose();
    }
}
