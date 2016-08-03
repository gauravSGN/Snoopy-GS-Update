using Model;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Goal;

namespace Scoring
{
    static public class ScoreUtil
    {
        static private Dictionary<int, BubbleData> bubbleMap;

        static public int[] ComputeStarsForLevel(LevelData data, BubbleFactory factory)
        {
            bubbleMap = data.Bubbles.ToDictionary((b) => b.Key);

            var popScore = ComputePopScore(data, factory);
            var dropScore = popScore * GlobalState.Instance.Config.scoring.dropMultiplier;
            var obstacleScore = ComputeObstacleScore(data, factory);
            var goalScore = ComputeGoalScore(data, factory);
            var clusterMultiplier = ComputeClusterMultiplier(data, factory);

            var baseScore = (int)Mathf.Round(popScore * 0.75f + dropScore * 0.25f + obstacleScore + goalScore);
            var clusterBonus = (int)Mathf.Round((popScore * 0.75f) * (clusterMultiplier - 1.0f));

            return new[]
            {
                (int)(popScore + obstacleScore + goalScore),
                (int)Mathf.Round(baseScore + (clusterBonus / 2.0f)),
                (int)Mathf.Round(baseScore + clusterBonus),
            };
        }

        static public List<T> ExtractCluster<T>(List<T> bubbles,
                                                Func<T, IEnumerable<T>> neighbors,
                                                Func<T, T, bool> comparator)
        {
            return ExtractCluster<T>(bubbles, neighbors, comparator, new List<T>(), bubbles[0]);
        }

        static public List<T> ExtractCluster<T>(List<T> bubbles,
                                                Func<T, IEnumerable<T>> neighbors,
                                                Func<T, T, bool> comparator,
                                                List<T> cluster,
                                                T next)
        {
            cluster.Add(next);
            bubbles.Remove(next);

            foreach (var neighbor in neighbors(next))
            {
                if (comparator(next, neighbor) && bubbles.Contains(neighbor) && !cluster.Contains(neighbor))
                {
                    ExtractCluster(bubbles, neighbors, comparator, cluster, neighbor);
                }
            }

            return cluster;
        }

        static public float ComputeClusterMultiplier(int count)
        {
            var config = GlobalState.Instance.Config.scoring;
            var multiplier = 1.0f;

            if (count >= config.minClusterSize)
            {
                multiplier += Mathf.Min(config.maxClusterSize, count) * config.clusterCoefficient;
            }

            return multiplier;
        }

        static private int ComputePopScore(LevelData data, BubbleFactory factory)
        {
            var basicTypes = factory.Bubbles
                .Where(b => b.category == BubbleCategory.Basic)
                .Select(d => d.Type)
                .ToList();

            return data.Bubbles
                .Where(b => basicTypes.Contains(b.Type))
                .Sum(b => factory.GetDefinitionByType(b.Type).Score);
        }

        static private int ComputeObstacleScore(LevelData data, BubbleFactory factory)
        {
            var obstacles = factory.Bubbles
                .Where(b => b.category == BubbleCategory.Obstacle)
                .Select(d => d.Type)
                .ToList();

            return data.Bubbles
                .Where(b => obstacles.Contains(b.Type))
                .Sum(b => factory.GetDefinitionByType(b.Type).Score);
        }

        static private int ComputeGoalScore(LevelData data, BubbleFactory factory)
        {
            var rescueTargets = data.Bubbles
                .Where(b => (b.modifiers != null) && (b.modifiers.Length > 0))
                .SelectMany(b => b.modifiers)
                .Where(m => m.type == BubbleModifierType.RescueTarget)
                .Count();
            
            return rescueTargets * GetGoalScore(GoalType.RescueBabies);
        }

        static private int GetGoalScore(GoalType type)
        {
            foreach (var goal in GlobalState.Instance.Config.scoring.goals)
            {
                if (goal.goalType == type)
                {
                    return goal.score;
                }
            }

            return 0;
        }

        static private float ComputeClusterMultiplier(LevelData data, BubbleFactory factory)
        {
            var basicTypes = factory.Bubbles
                .Where(b => b.category == BubbleCategory.Basic)
                .Select(d => d.Type)
                .ToList();

            var bubbles = data.Bubbles.Where(b => basicTypes.Contains(b.Type)).ToList();

            var randomBubbles = bubbles
                .Where(b => (b.modifiers != null) && (b.modifiers.Length > 0))
                .Where(b => b.modifiers.Any(m => m.type == BubbleModifierType.Random))
                .ToList();

            var nonRandomBubbles = bubbles.Where(b => !randomBubbles.Contains(b)).ToList();

            int count = 0;
            float total = 0.0f;

            while (nonRandomBubbles.Count > 0)
            {
                var cluster = ExtractCluster(
                    nonRandomBubbles,
                    GetNeighbors,
                    (a, b) => a.Type == b.Type
                );

                total += ComputeClusterMultiplier(cluster.Count);
                count++;
            }

            while (randomBubbles.Count > 0)
            {
                var cluster = ExtractCluster(
                    randomBubbles,
                    GetNeighbors,
                    (a, b) => a.modifiers.First(m => m.type == BubbleModifierType.Random).data ==
                              b.modifiers.First(m => m.type == BubbleModifierType.Random).data
                );

                total += ComputeClusterMultiplier(cluster.Count);
                count++;
            }

            return Mathf.Max(1.0f, total / Mathf.Max(1, count));
        }

        static private IEnumerable<BubbleData> GetNeighbors(BubbleData bubble)
        {
            var offset = (bubble.Y & 1) * 2 - 1;
            var neighbors = new int[6];

            neighbors[0] = BubbleData.GetKey(bubble.X + offset, bubble.Y - 1);
            neighbors[1] = BubbleData.GetKey(bubble.X, bubble.Y - 1);
            neighbors[2] = BubbleData.GetKey(bubble.X - 1, bubble.Y);
            neighbors[3] = BubbleData.GetKey(bubble.X + 1, bubble.Y);
            neighbors[4] = BubbleData.GetKey(bubble.X + offset, bubble.Y + 1);
            neighbors[5] = BubbleData.GetKey(bubble.X, bubble.Y + 1);

            foreach (var key in neighbors)
            {
                if (bubbleMap.ContainsKey(key))
                {
                    yield return bubbleMap[key];
                }
            }
        }
    }
}
