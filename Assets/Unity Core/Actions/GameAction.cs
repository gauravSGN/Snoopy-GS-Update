using UnityEngine;

namespace Actions
{
    public interface GameAction
    {
        bool Done { get; }

        void Attach(GameObject target);
        void Detach(GameObject target);

        void Update();
    }
}
