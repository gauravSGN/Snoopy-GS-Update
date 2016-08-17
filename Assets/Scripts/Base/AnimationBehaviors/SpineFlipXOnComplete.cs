using UnityEngine;
using Spine;
using Spine.Unity;

public class SpineFlipXOnComplete : StateMachineBehaviour
{
    private Skeleton skeleton;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (skeleton == null)
        {
            skeleton = animator.gameObject.GetComponent<SkeletonAnimator>().Skeleton;
        }

        skeleton.FlipX = false;
    }
}
