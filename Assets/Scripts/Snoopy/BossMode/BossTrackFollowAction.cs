using Paths;
using UnityEngine;
using Actions.Base;
using System.Collections.Generic;

namespace Snoopy.BossMode
{
    sealed public class BossTrackFollowAction : TargetedAction
    {
        override public bool Done { get { return done; } }
        public NodeTrackPath Path
        {
            get { return path; }
            set
            {
                path = value;
                previousPosition = (target != null) ? target.transform.localPosition : Vector3.zero;
                nextWaypoint = path.Advance(1.0f);
                timer = 0.0f;
            }
        }

        private Animator animator;
        private NodeTrackPath path;
        private Vector3 previousPosition;
        private Vector3 nextWaypoint;
        private float timer;
        private float bankTarget;
        private float bankAngle;
        private bool done;

        public void Start()
        {
            done = false;
        }

        public void Stop()
        {
            done = true;
        }

        public override void Attach(GameObject target)
        {
            base.Attach(target);

            animator = target.GetComponentInChildren<Animator>();
            previousPosition = target.transform.localPosition;
        }

        override public void Update()
        {
            if (Path != null)
            {
                var config = GlobalState.Instance.Config.boss;
                var duration = 1.0f / config.flightSpeed;
                timer += Time.deltaTime;

                var value = timer / duration;
                target.transform.localPosition = Vector3.Lerp(previousPosition, nextWaypoint, value);
                target.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Lerp(bankAngle, bankTarget, value));

                if (timer >= duration)
                {
                    timer -= duration;
                    previousPosition = nextWaypoint;
                    nextWaypoint = path.Advance(1.0f);

                    var yDelta1 = (nextWaypoint.y - previousPosition.y);
                    var yDelta2 = path.Peek().y - nextWaypoint.y;
                    var yVelocity = yDelta1 * Mathf.Abs(yDelta2);
                    yVelocity *= ((Mathf.Sign(yDelta1) == Mathf.Sign(yDelta2)) ? 1.0f : 0.0f);

                    animator.SetFloat("yVelocity", yVelocity);

                    bankAngle = bankTarget;
                    bankTarget = -Mathf.Sign(nextWaypoint.x - previousPosition.x) * config.bankAngle;
                }
            }
        }
    }
}
