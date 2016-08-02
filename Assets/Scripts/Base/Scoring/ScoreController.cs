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
            var bubbleList = bubbles.ToList();
            int totalScore = 0;

            while (bubbleList.Count > 0)
            {
                var cluster = ExtractCluster(bubbleList);
                var multiplier = ComputeClusterMultiplier(cluster.Count);

                foreach (var bubble in cluster)
                {
                    ShowBubbleScore(bubble, bubble.definition.Score);
                }

                totalScore += (int)(cluster[0].definition.Score * cluster.Count * multiplier);
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

        private List<Bubble> ExtractCluster(List<Bubble> bubbles)
        {
            return ExtractCluster(bubbles, new List<Bubble>(), bubbles[0]);
        }

        private List<Bubble> ExtractCluster(List<Bubble> bubbles, List<Bubble> cluster, Bubble next)
        {
            cluster.Add(next);
            bubbles.Remove(next);

            foreach (Bubble neighbor in next.Neighbors)
            {
                if (next.IsMatching(neighbor) && bubbles.Contains(neighbor) && !cluster.Contains(neighbor))
                {
                    ExtractCluster(bubbles, cluster, neighbor);
                }
            }

            return cluster;
        }

        private float ComputeClusterMultiplier(int count)
        {
            var multiplier = 1.0f;

            if (count >= config.minClusterSize)
            {
                multiplier += Mathf.Min(config.maxClusterSize, count) * config.clusterCoefficient;
            }

            return multiplier;
        }
    }
}
