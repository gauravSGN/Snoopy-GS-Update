using Event;
using UnityEngine;

namespace Sequence
{
    public class EndOfLevel : MonoBehaviour
    {
        [SerializeField]
        private Level level;

        [SerializeField]
        private WinLevel winLevelSequence;

        [SerializeField]
        private LoseLevel loseLevelSequence;

        private bool readyToContinue = false;

        protected void Start()
        {
            GlobalState.EventService.AddEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
            GlobalState.EventService.AddEventHandler<FiringAnimationCompleteEvent>(OnFiringAnimationComplete);
        }

        private void OnReactionsFinished(ReactionsFinishedEvent gameEvent)
        {
            if (level.AllGoalsCompleted)
            {
                GlobalState.EventService.RemoveEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
                winLevelSequence.Begin(level.levelState);
            }
            else if (level.levelState.remainingBubbles <= 0)
            {
                GlobalState.EventService.RemoveEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
                loseLevelSequence.Begin(level.levelState);
            }
            else
            {
                ContinueLevel();
            }
        }

        // Only continue with the level if both the reaction queue and the launcher
        // character's animations are complete.
        private void ContinueLevel()
        {
            if (readyToContinue)
            {
                GlobalState.EventService.Dispatch(new ReadyForNextBubbleEvent());
                GlobalState.EventService.Dispatch(new InputToggleEvent(true));
                readyToContinue = false;
            }
            else
            {
                readyToContinue = true;
            }
        }

        private void OnFiringAnimationComplete(FiringAnimationCompleteEvent gameEvent)
        {
            if (readyToContinue)
            {
                ContinueLevel();
            }
            else
            {
                readyToContinue = true;
            }
        }
    }
}