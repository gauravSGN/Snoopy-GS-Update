using UnityEngine;

namespace Actions.Base
{
    abstract public class UntargettedAction : GameAction
    {
        abstract public bool Done { get; }

        abstract public void Update();

        virtual public void Attach(GameObject target)
        {
            // Nothing to do
        }

        virtual public void Detach(GameObject target)
        {
            // Nothing to do
        }
    }
}
