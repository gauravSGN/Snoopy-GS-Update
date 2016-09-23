using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Actions.Meta
{
    sealed public class ParallelActionList : GameAction
    {
        private List<GameAction> actions;

        public bool Done { get { return !actions.Any(a => !a.Done); } }

        public ParallelActionList(List<GameAction> actions)
        {
            this.actions = actions;
        }

        public void Attach(GameObject target)
        {
            foreach (var action in actions)
            {
                action.Attach(target);
            }
        }

        public void Detach(GameObject target)
        {
            foreach (var action in actions)
            {
                action.Detach(target);
            }
        }

        public void Update()
        {
            foreach (var action in actions)
            {
                if (!action.Done)
                {
                    action.Update();
                }
            }
        }
    }
}
