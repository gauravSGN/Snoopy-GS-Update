using UnityEngine;

namespace Actions.Base
{
    abstract public class TargettedAction : UntargettedAction
    {
        protected GameObject target;

        override public void Attach(GameObject target)
        {
            this.target = target;
        }
    }
}
