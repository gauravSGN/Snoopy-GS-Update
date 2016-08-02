using Core;
using Event;
using Service;
using Reaction;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using ReactionDict = System.Collections.Generic.Dictionary<
    Reaction.ReactionPriority,
    System.Action<System.Collections.Generic.IEnumerable<Bubble>>>;

namespace Scoring
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField]
        private Level level;

        private readonly ReactionDict handlers = new ReactionDict();

        private readonly List<Tuple<ReactionPriority, Bubble>> pending = new List<Tuple<ReactionPriority, Bubble>>();
        private GameConfig.ScoringConfig config;
        private EventService eventService;

        public void Start()
        {
            config = GlobalState.Instance.Config.scoring;
            eventService = GlobalState.EventService;

            eventService.AddPooledEvent<BubbleScoreEvent>(new BubbleScoreEvent(null, 0));

            handlers[ReactionPriority.Pop] = HandlePoppedBubbles;
            handlers[ReactionPriority.Cull] = HandleCulledBubbles;

            eventService.AddEventHandler<BubbleReactionEvent>(OnBubbleReaction);
            eventService.AddEventHandler<StartReactionsEvent>(OnStartReactions);
        }

        public void OnDestroy()
        {
            eventService.GetPooledEvent<BubbleScoreEvent>();
        }

        private void OnBubbleReaction(BubbleReactionEvent gameEvent)
        {
            pending.Add(new Tuple<ReactionPriority, Bubble>(gameEvent.priority, gameEvent.bubble));
        }

        private void OnStartReactions(StartReactionsEvent gameEvent)
        {
            foreach (var pair in handlers)
            {
                pair.Value.Invoke(pending.Where(b => b.Item1 == pair.Key).Select(b => b.Item2));
            }

            pending.Clear();
        }

        private void HandlePoppedBubbles(IEnumerable<Bubble> bubbles)
        {
            int totalScore = 0;

            foreach (var bubble in bubbles)
            {
                totalScore += bubble.definition.Score;
                ShowBubbleScore(bubble, bubble.definition.Score);
            }

            level.levelState.score += totalScore;
            level.levelState.NotifyListeners();
        }

        private void HandleCulledBubbles(IEnumerable<Bubble> bubbles)
        {
            int totalScore = 0;
            int score;

            foreach (var bubble in bubbles)
            {
                score = bubble.definition.Score;

                if (bubble.definition.category == BubbleCategory.Basic)
                {
                    score = (int)(score * config.dropMultiplier);
                }

                totalScore += score;
                ShowBubbleScore(bubble, score);
            }

            level.levelState.score += totalScore;
            level.levelState.NotifyListeners();
        }

        private void ShowBubbleScore(Bubble bubble, int score)
        {
            var bubbleScore = eventService.GetPooledEvent<BubbleScoreEvent>();

            bubbleScore.bubble = bubble;
            bubbleScore.score = score;

            eventService.DispatchPooled(bubbleScore);
        }
    }
}
