using Service;
using UnityEngine;

namespace Namespace
{
    sealed public class PlayGoalVFX : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var transformService = GlobalState.Instance.Services.Get<TransformService>();
            var goalIcon = transformService.Get(Registry.TransformUsage.RescueGoalIcon);
            var iconAnimator = goalIcon.parent.GetComponent<Animator>();

            if (iconAnimator != null)
            {
                iconAnimator.SetTrigger("PlayVFX");
            }
        }
    }
}
