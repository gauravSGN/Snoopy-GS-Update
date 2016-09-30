using UnityEngine;

namespace Actions.Base
{
    abstract public class TargetedAction : UntargetedAction
    {
        protected GameObject target;

        override public void Attach(GameObject target)
        {
            this.target = target;
        }
    }
}
