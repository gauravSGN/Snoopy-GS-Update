using UnityEngine;
using System.Collections.Generic;

namespace Actions.Meta
{
    sealed public class LoopingActionList : Actions.Base.TargettedAction
    {
        override public bool Done { get { return true; } }

        private List<GameAction> actions;

        public LoopingActionList(List<GameAction> actions)
        {
            this.actions = actions;
        }

        override public void Update()
        {
            var actionQueue = target.GetComponent<ActionQueue>();

            foreach (var action in actions)
            {
                actionQueue.AddGameAction(action);
            }

            actionQueue.AddGameAction(this);
        }
    }
}
