using UnityEngine;
using Spine.Unity;

namespace Snoopy.BossMode.Behaviour
{
    sealed public class TakeDamage : StateMachineBehaviour
    {
        [SerializeField]
        private string[] damageSkins;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var damageLevel = UpdateHealthAndDamageLevel(animator);

            UpdateSkin(animator, damageLevel);
            UpdatePlaneRig(animator, damageLevel);

            GlobalState.EventService.Dispatch(new DamageBossEvent());
        }

        private int UpdateHealthAndDamageLevel(Animator animator)
        {
            var newHealth = animator.GetInteger("Health") - 1;
            var maxHealth = animator.GetInteger("MaxHealth");
            var maxDamage = damageSkins.Length;
            var damageLevel = Mathf.RoundToInt((1.0f - (float)newHealth / (float)maxHealth) * (maxDamage - 1.0f));

            animator.SetInteger("Health", newHealth);
            animator.SetInteger("DamageLevel", damageLevel);

            return damageLevel;
        }

        private void UpdateSkin(Animator animator, int damageLevel)
        {
            var skeleton = animator.GetComponentInChildren<SkeletonAnimator>().skeleton;
            skeleton.SetSkin(damageSkins[damageLevel]);
        }

        private void UpdatePlaneRig(Animator animator, int damageLevel)
        {
            var planeRig = animator.transform.GetChild(0).GetComponent<Animator>();

            planeRig.SetInteger("DamageLevel", damageLevel);
            planeRig.SetTrigger("Hit");
        }
    }
}
