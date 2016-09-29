using Paths;
using UnityEngine;
using Actions.Base;
using System.Collections.Generic;

namespace Snoopy.BossMode
{
    sealed public class BossTrackFollowAction : TargettedAction
    {
        override public bool Done { get { return done; } }
        public NodeTrackPath Path
        {
            get { return path; }
            set
            {
                path = value;
                previousPosition = (target != null) ? target.transform.position : Vector3.zero;
                nextWaypoint = path.Advance(1.0f);
                timer = 0.0f;
            }
        }

        private Animator animator;
        private NodeTrackPath path;
        private Vector3 previousPosition;
        private Vector3 nextWaypoint;
        private float timer;
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
            previousPosition = target.transform.position;
        }

        override public void Update()
        {
            if (Path != null)
            {
                var duration = 0.25f;
                timer += Time.deltaTime;

                target.transform.position = Vector3.Lerp(previousPosition, nextWaypoint, timer / duration);
                animator.SetFloat("yVelocity", nextWaypoint.y - previousPosition.y);

                if (timer >= duration)
                {
                    timer -= duration;
                    previousPosition = nextWaypoint;
                    nextWaypoint = path.Advance(1.0f);
                }
            }
        }
    }
}
