using UnityEngine;
using Spine;
using Spine.Unity;

public class SpineSetXOnComplete : StateMachineBehaviour
{
    [SerializeField]
    private TransitionType transitionType;

    [SerializeField]
    private bool flipX;

    public enum TransitionType
    {
        Enter,
        Exit,
    }

    private Skeleton skeleton;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetX(animator, TransitionType.Enter);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetX(animator, TransitionType.Exit);
    }

    private void SetX(Animator animator, TransitionType transition)
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
