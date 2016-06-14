using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class ReactionLogic : MonoBehaviour
{
    [SerializeField]
    private string mapSceneName;

    [SerializeField]
    private float percentageOfFrameTime = 0.1f;

    private long maximumProcessingTimeInTicks;
    private readonly Stopwatch stopwatch = new Stopwatch();
    private SortedDictionary<ReactionPriority, List<Action>> currentActions;
    private SortedDictionary<ReactionPriority, List<Action>> futureActions;

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
    }

    private void OnBubbleSettled(BubbleSettledEvent gameEvent)
    {
        StartCoroutine(ProcessReactions());
        RotateOrReset();

        var levelState = GetComponent<Level>().levelState;

        if (levelState.remainingBubbles <= 0)
        {
            EventDispatcher.Instance.Dispatch(new LoseLevelEvent());
            SceneManager.LoadScene(mapSceneName);
        }
        else
        {
            EventDispatcher.Instance.Dispatch(new ReadyForNextBubbleEvent());
        }
    }

    private IEnumerator ProcessReactions()
    {
        while (true)
        {
            if ((currentActions != null) && (futureActions.Count == 0))
            {
                break;
            }

            RotateOrReset(false);

            RestartTimer();

            foreach (var actionList in currentActions)
            {
                for (int index = 0, maxIndex = actionList.Value.Count; index < maxIndex; index++)
                {
                    actionList.Value[index].Invoke();

                    if (stopwatch.ElapsedTicks >= maximumProcessingTimeInTicks)
                    {
                        yield return null;
                        RestartTimer();
                    }
                }
            }
        }
    }

    private void RotateOrReset(bool reset = true)
    {
        currentActions = reset ? null : futureActions;
        futureActions = new SortedDictionary<ReactionPriority, List<Action>>();
    }

    private void RestartTimer()
    {
        maximumProcessingTimeInTicks = (long)(Time.smoothDeltaTime * percentageOfFrameTime * Stopwatch.Frequency);
        stopwatch.Reset();
        stopwatch.Start();
    }
}
