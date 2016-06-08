using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ReactionLogic : MonoBehaviour
{
    private Dictionary<ReactionPriority, List<Action>> currentActions;
    private Dictionary<ReactionPriority, List<Action>> futureActions;

    protected void Start()
    {
        RotateOrReset();
        EventDispatcher.Instance.AddEventHandler<BubbleSettledEvent>(OnBubbleSettled);
        EventDispatcher.Instance.AddEventHandler<BubbleReactionEvent>(OnBubbleReactionEvent);
    }

    private void OnBubbleReactionEvent(BubbleReactionEvent gameEvent)
    {
        if (!futureActions.ContainsKey(gameEvent.priority))
        {
            futureActions[gameEvent.priority] = new List<Action>();
        }

        futureActions[gameEvent.priority].Add(gameEvent.action);

        EventDispatcher.Instance.AddPooledEvent<BubbleReactionEvent>(gameEvent);
    }

    private void OnBubbleSettled(BubbleSettledEvent gameEvent)
    {
        ProcessReactions();
        RotateOrReset();

        var levelState = GetComponent<Level>().LevelState;

        if (levelState.remainingBubbles <= 0)
        {
            EventDispatcher.Instance.Dispatch(new LoseLevelEvent());
        }
        else
        {
            EventDispatcher.Instance.Dispatch(new ReadyForNextBubbleEvent());
        }
    }

    private void ProcessReactions()
    {
        while (true)
        {
            if ((currentActions != null) && futureActions.Count == 0)
            {
                break;
            }

            RotateOrReset(false);

            foreach (var actionList in currentActions)
            {
                for (var index = 0; index < actionList.Value.Count; index++)
                {
                    actionList.Value[index].Invoke();
                }
            }
        }
    }

    private void RotateOrReset(bool reset = true)
    {
        currentActions = reset ? null : futureActions;
        futureActions = new Dictionary<ReactionPriority, List<Action>>();
    }
}
