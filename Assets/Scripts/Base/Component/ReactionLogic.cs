using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ReactionLogic : MonoBehaviour
{
    private Dictionary<int, List<Action>> currentActions = null;
    private Dictionary<int, List<Action>> futureActions = new Dictionary<int, List<Action>>();

    protected void Start()
    {
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
    }

    private void OnBubbleSettled(BubbleSettledEvent gameEvent)
    {
        ProcessReactions();
        Reset();

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

            currentActions = futureActions;
            futureActions = new Dictionary<int, List<Action>>();

            foreach (var actionList in currentActions)
            {
                for (var index = 0; index < actionList.Value.Count; index++)
                {
                    actionList.Value[index].Invoke();
                }
            }
        }
    }

    private void Reset()
    {
        currentActions = null;
        futureActions = new Dictionary<int, List<Action>>();
    }
}
