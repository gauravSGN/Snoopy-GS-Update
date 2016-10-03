using UnityEngine;

namespace Snoopy.BossMode.Behaviour
{
    sealed public class MoveToTrackStart : StateMachineBehaviour
    {
        private Vector3 target;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            target = animator.GetComponent<BossController>().Path.Advance(1.0f);
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var transform = animator.transform;
            var position = transform.localPosition;

            if ((target - position).sqrMagnitude > Mathf.Epsilon)
            {
                var config = GlobalState.Instance.Config;
                var speed = config.boss.flightSpeed * config.bubbles.size;
                var next = Vector3.MoveTowards(position, target, Time.deltaTime * speed);

                transform.localPosition = next;

                if ((next - target).sqrMagnitude <= Mathf.Epsilon)
                {
                    animator.SetTrigger("ReachedStart");
                }
            }
        }
    }
}
