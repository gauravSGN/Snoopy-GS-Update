using UnityEngine;

namespace Actions.Basic
{
    sealed public class DelayAction : Actions.Base.UntargetedAction
    {
        private float duration;
        private float timer;

        override public bool Done { get { return timer >= duration; } }

        public DelayAction(float duration)
        {
            this.duration = duration;
        }

        override public void Attach(GameObject target)
        {
            timer = 0.0f;
        }

        override public void Update()
        {
            timer += Time.deltaTime;
        }
    }
}
