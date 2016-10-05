using UnityEngine;

namespace Snoopy.BossMode.Behaviour
{
    sealed public class FollowTrack : StateMachineBehaviour
    {
        private Paths.NodeTrackPath path;
        private Vector3 previousPosition;
        private Vector3 nextWaypoint;
        private float timer;
        private float bankTarget;
        private float bankAngle;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            path = animator.GetComponent<BossController>().Path;
            previousPosition = animator.transform.localPosition;
            nextWaypoint = path.Advance(1.0f);
            timer = 0.0f;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.transform.rotation = Quaternion.identity;

            bankAngle = 0.0f;
            bankTarget = 0.0f;
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var config = GlobalState.Instance.Config.boss;
            var duration = 1.0f / config.flightSpeed;
            timer += Time.deltaTime;

            var value = timer / duration;
            animator.transform.localPosition = Vector3.Lerp(previousPosition, nextWaypoint, value);
            animator.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(bankAngle, bankTarget, value));

            if (timer >= duration)
            {
                timer -= duration;
                previousPosition = nextWaypoint;
                nextWaypoint = path.Advance(1.0f);

                var yDelta1 = (nextWaypoint.y - previousPosition.y);
                var yDelta2 = path.Peek().y - nextWaypoint.y;
                var yVelocity = yDelta1 * Mathf.Abs(yDelta2);
                yVelocity *= ((Mathf.Sign(yDelta1) == Mathf.Sign(yDelta2)) ? 1.0f : 0.0f);

                bankAngle = bankTarget;
                bankTarget = -Mathf.Sign(nextWaypoint.x - previousPosition.x) * config.bankAngle;
            }
        }
    }
}
