using Event;
using System;
using UI.Popup;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sequence
{
    public class EndOfLevel : MonoBehaviour
    {
        [SerializeField]
        private Level level;

        // Note: Reenabling input happens within the Launcher Character's state machine to
        // account for animation and transition times.
        protected void Start()
        {
            GlobalState.EventService.AddEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
            GlobalState.EventService.AddEventHandler<PreLevelCompleteEvent>(OnPreLevelComplete);
        }

        private void OnReactionsFinished(ReactionsFinishedEvent gameEvent)
        {
            if (level.AllGoalsCompleted)
            {
                GlobalState.EventService.RemoveEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
                WinLevel();
            }
            else if (level.levelState.remainingBubbles <= 0)
            {
                GlobalState.EventService.RemoveEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
                LoseLevel();
            }
            else
            {
                ContinueLevel();
            }
        }

        private void OnPreLevelComplete(PreLevelCompleteEvent gameEvent)
        {
            // kick off bubble shower and other sequences
        }

        private void WinLevel()
        {
            GlobalState.EventService.Dispatch(new LevelCompleteEvent(true));
            level.UpdateUser();

            GlobalState.EventService.AddEventHandler<ReactionsFinishedEvent>(OnCullAllBubblesComplete);

            GlobalState.EventService.Dispatch(new CullAllBubblesEvent());
            GlobalState.EventService.Dispatch(new BubbleSettledEvent());
        }

        private void OnCullAllBubblesComplete(ReactionsFinishedEvent gameEvent)
        {
            GlobalState.EventService.RemoveEventHandler<ReactionsFinishedEvent>(OnCullAllBubblesComplete);

            StartCoroutine(ShowPopupAfterDelay(1.0f, new GenericPopupConfig
            {
                title = "Level Won",
                mainText = ("Score: " + level.levelState.score.ToString() + "\n" +
                            "Stars: " + GlobalState.User.levels[level.levelState.levelNumber].stars.ToString()),
                closeActions = new List<Action> { DispatchReturnToMap },
                affirmativeActions = new List<Action> { DispatchReturnToMap }
            }));
        }

        private void LoseLevel()
        {
            GlobalState.EventService.Dispatch(new LevelCompleteEvent(false));

            GlobalState.User.purchasables.hearts.quantity--;
            StartCoroutine(ShowPopupAfterDelay(1.0f, new GenericPopupConfig
            {
                title = "Level Lost",
                mainText = "Hearts Left: " + GlobalState.User.purchasables.hearts.quantity.ToString(),
                closeActions = new List<Action> { DispatchReturnToMap },
                affirmativeActions = new List<Action> { DispatchReturnToMap }
            }));
        }

        private void ContinueLevel()
        {
            GlobalState.EventService.Dispatch(new ReadyForNextBubbleEvent());
        }

        private void DispatchReturnToMap()
        {
            GlobalState.EventService.Dispatch(new TransitionToReturnSceneEvent());
        }

        private IEnumerator ShowPopupAfterDelay(float delay, PopupConfig config)
        {
            yield return new WaitForSeconds(delay);
            GlobalState.PopupService.Enqueue(config);
        }
    }
}