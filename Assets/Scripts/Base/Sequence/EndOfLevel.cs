using Event;
using UnityEngine;

namespace Sequence
{
    public class EndOfLevel : MonoBehaviour
    {
        [SerializeField]
        private Level level;

        private BaseSequence<LevelState> winOrLoseSequence;

        protected void Start()
        {
            GlobalState.EventService.AddEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
            GlobalState.EventService.AddEventHandler<PreLevelCompleteEvent>(OnPreLevelComplete);
        }

        private void OnReactionsFinished(ReactionsFinishedEvent gameEvent)
        {
            if (level.AllGoalsCompleted)
            {
                winOrLoseSequence = new WinLevel();
            }
            else if (level.levelState.remainingBubbles <= 0)
            {
                winOrLoseSequence = new LoseLevel();
            }

            if (winOrLoseSequence != null)
            {
                GlobalState.EventService.RemoveEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
                winOrLoseSequence.Begin(level.levelState);
            }
            else
            {
                ContinueLevel();
            }
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
            GlobalState.EventService.Dispatch(new InputToggleEvent(true));
        }

        private void DispatchReturnToMap()
        {
            GlobalState.EventService.Dispatch(new TransitionToReturnSceneEvent());
        }
    }
}