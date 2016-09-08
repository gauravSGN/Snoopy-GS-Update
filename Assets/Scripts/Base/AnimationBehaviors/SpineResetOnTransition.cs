using UnityEngine;
using Spine;
using Spine.Unity;

public class SpineResetOnTransition : SelectableTransitionBehaviour
{
    private Skeleton skeleton;

    override protected void OnSetTransition(Animator animator)
    {
        if (skeleton == null)
        {
            skeleton = animator.gameObject.GetComponent<SkeletonAnimator>().Skeleton;
        }

        skeleton.SetToSetupPose();
    }
}
