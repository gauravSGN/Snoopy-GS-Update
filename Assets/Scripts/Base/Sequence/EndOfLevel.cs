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

        private void ContinueLevel()
        {
            GlobalState.EventService.Dispatch(new ReadyForNextBubbleEvent());
            GlobalState.EventService.Dispatch(new InputToggleEvent(true));
        }
    }
}