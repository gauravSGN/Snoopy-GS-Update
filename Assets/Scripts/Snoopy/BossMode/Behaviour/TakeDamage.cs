using UnityEngine;
using System.Collections.Generic;

namespace Snoopy.BossMode.Behaviour
{
    sealed public class TakeDamage : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.transform.rotation = Quaternion.identity;
            animator.SetInteger("Health", animator.GetInteger("Health") - 1);

            GlobalState.EventService.Dispatch(new DamageBossEvent());
        }
    }
}
