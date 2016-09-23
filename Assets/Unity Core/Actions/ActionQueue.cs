using UnityEngine;
using System.Collections.Generic;

namespace Actions
{
    sealed public class ActionQueue : MonoBehaviour, UpdateReceiver
    {
        private readonly Queue<GameAction> actions = new Queue<GameAction>();

        public void AddGameAction(GameAction action)
        {
            actions.Enqueue(action);
            action.Attach(gameObject);

            if (actions.Count == 1)
            {
                GlobalState.UpdateService.Updates.Add(this);
            }
        }

        public void OnUpdate()
        {
            var action = actions.Peek();

            action.Update();

            if (action.Done)
            {
                actions.Dequeue();
                action.Detach(gameObject);

                if (actions.Count == 0)
                {
                    GlobalState.UpdateService.Updates.Remove(this);
                }
            }
        }
    }
}
