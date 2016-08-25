using Event;
using Service;
using Registry;
using UnityEngine;

namespace Sequence
{
    public class EndOfLevel : MonoBehaviour, Blockade
    {
        [SerializeField]
        private Level level;

        [SerializeField]
        private WinLevel winLevelSequence;

        [SerializeField]
        private LoseLevel loseLevelSequence;

        private BlockadeService blockade;
        private bool readyToContinue = false;

        public BlockadeType BlockadeType { get { return BlockadeType.All; } }

        protected void Start()
        {
            blockade = GlobalState.Instance.Services.Get<BlockadeService>();

            var eventService = GlobalState.EventService;
            eventService.AddEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
            eventService.AddEventHandler<FiringAnimationCompleteEvent>(OnFiringAnimationComplete);
            eventService.AddEventHandler<PurchasedExtraMovesEvent>(OnPurchasedExtraMoves);
            eventService.AddEventHandler<BubbleFiringEvent>(OnBubbleFiring);
        }

        private void OnReactionsFinished(ReactionsFinishedEvent gameEvent)
        {
            if (level.AllGoalsCompleted)
            {
                BeginNextSequence(winLevelSequence);
            }
            else if (level.levelState.remainingBubbles <= 0)
            {
                BeginNextSequence(loseLevelSequence);
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
                blockade.Remove(this);
            }
            else
            {
                readyToContinue = true;
            }
        }

        private void BeginNextSequence(BaseSequence<LevelState> sequence)
        {
            blockade.Remove(this);
            GlobalState.EventService.RemoveEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
            sequence.Begin(level.levelState);
        }

        private void OnFiringAnimationComplete(FiringAnimationCompleteEvent gameEvent)
        {
            ContinueLevel();
        }

        private void OnPurchasedExtraMoves(PurchasedExtraMovesEvent gameEvent)
        {
            GlobalState.EventService.AddEventHandler<ReactionsFinishedEvent>(OnReactionsFinished);
        }

        private void OnBubbleFiring(BubbleFiringEvent gameEvent)
        {
            blockade.Add(this);
        }
    }
}
