using Event;
using UnityEngine;

namespace Sequence
{
    public class EndOfLevel : MonoBehaviour
    {
        [SerializeField]
        private Level level;

        private BaseSequence<LevelState> winOrLoseSequence;

        // Note: Reenabling input happens within the Launcher Character's state machine to
        // account for animation and transition times.
        protected void Start()
        {
            GlobalState.EventService.AddEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
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