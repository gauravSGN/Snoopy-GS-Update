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
        static private List<float[]> randomWeights;

        static public int[] ComputeStarsForLevel(LevelData data, BubbleFactory factory)
        {
            ComputeRandomWeights(data);

            var popScore = ComputePopScore(data, factory);
            var dropScore = popScore * GlobalState.Instance.Config.scoring.dropMultiplier;
            var obstacleScore = ComputeObstacleScore(data, factory);
            var goalScore = ComputeGoalScore(data, factory);
            var clusterMultiplier = ComputeMeanClusterMultiplier(data, factory);

            var baseScore = (int)Mathf.Round(popScore * 0.75f + dropScore * 0.25f + obstacleScore + goalScore);
            var clusterBonus = (int)Mathf.Round((popScore * 0.75f) * (clusterMultiplier - 1.0f));

            return new[]
            {
                (int)(popScore + obstacleScore + goalScore),
                (int)Mathf.Round(baseScore + (clusterBonus / 2.0f)),
                (int)Mathf.Round(baseScore + clusterBonus),
            };
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
            var basicTypes = GetTypeGroup(factory, BubbleCategory.Basic);
            var shotScore = (data.ShotCount * factory.GetDefinitionByType(BubbleType.Blue).Score);

            return shotScore +
                data.Bubbles
                .Where(b => basicTypes.Contains(b.Type))
                .Sum(b => factory.GetDefinitionByType(b.Type).Score);
        }

        static private int ComputeObstacleScore(LevelData data, BubbleFactory factory)
        {
            var obstacles = GetTypeGroup(factory, BubbleCategory.Obstacle);

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

        static private float ComputeMeanClusterMultiplier(LevelData data, BubbleFactory factory)
        {
            var basicTypes = GetTypeGroup(factory, BubbleCategory.Basic);
            var bubbleMap = new Dictionary<int, float>();
            float total = 0.0f;
            int count = 0;

            foreach (var type in basicTypes)
            {
                PopulateBubbleMap(bubbleMap, data, type);

                while (bubbleMap.Count > 0)
                {
                    total += ComputeClusterMultiplier(1 + (int)ExtractCluster(bubbleMap));
                    count++;
                }
            }

            return total / Mathf.Max(1, count);
        }

        static private float ExtractCluster(Dictionary<int, float> bubbleMap)
        {
            var max = 0.0f;
            int key = 0;

            foreach (var pair in bubbleMap)
            {
                if (pair.Value > max)
                {
                    max = pair.Value;
                    key = pair.Key;
                }
            }

            return ExtractCluster(bubbleMap, key, max);
        }

        static private float ExtractCluster(Dictionary<int, float> bubbleMap, int key, float current)
        {
            float value = 0.0f;

            if (bubbleMap.ContainsKey(key) && (bubbleMap[key] <= current))
            {
                value = bubbleMap[key];
                bubbleMap.Remove(key);

                var x = key & ((1 << 4) - 1);
                var y = key >> 4;
                var offset = (y & 1) * 2 - 1;
                var next = Mathf.Min(current, value);

                value += ExtractCluster(bubbleMap, BubbleData.GetKey(x + offset, y - 1), next);
                value += ExtractCluster(bubbleMap, BubbleData.GetKey(x, y - 1), next);
                value += ExtractCluster(bubbleMap, BubbleData.GetKey(x - 1, y), next);
                value += ExtractCluster(bubbleMap, BubbleData.GetKey(x + 1, y), next);
                value += ExtractCluster(bubbleMap, BubbleData.GetKey(x + offset, y + 1), next);
                value += ExtractCluster(bubbleMap, BubbleData.GetKey(x, y + 1), next);
            }

            return value;
        }

        static private void PopulateBubbleMap(Dictionary<int, float> bubbleMap, LevelData data, BubbleType type)
        {
            bubbleMap.Clear();

            foreach (var bubble in data.Bubbles)
            {
                if ((bubble.modifiers != null) && (bubble.modifiers.Length > 0))
                {
                    var modifier = bubble.modifiers.FirstOrDefault(m => m.type == BubbleModifierType.Random);

                    if (modifier != null)
                    {
                        var weight = randomWeights[int.Parse(modifier.data)][(int)type];

                        if (weight > 0.0f)
                        {
                            bubbleMap.Add(bubble.Key, weight);
                        }

                        continue;
                    }
                }

                if (bubble.Type == type)
                {
                    bubbleMap.Add(bubble.Key, 1.0f);
                }
            }
        }

        static private void ComputeRandomWeights(LevelData data)
        {
            var count = data.Randoms.Length;
            randomWeights = new List<float[]>(data.Randoms.Length);

            for (var index = 0; index < count; index++)
            {
                randomWeights.Add(null);
            }

            for (var index = 0; index < count; index++)
            {
                ComputeRandomWeights(data, index);
            }
        }

        static private void ComputeRandomWeights(LevelData data, int groupIndex)
        {
            var group = data.Randoms[groupIndex];
            var count = group.weights.Length;

            if (randomWeights[groupIndex] == null)
            {
                float sum = Mathf.Max(1.0f, group.weights.Sum());
                randomWeights[groupIndex] = group.weights.Select(w => w / sum).ToArray();

                foreach (var exclusion in group.exclusions)
                {
                    ComputeRandomWeights(data, exclusion);

                    for (var index = 0; index < count; index++)
                    {
                        randomWeights[groupIndex][index] *= (1.0f - randomWeights[exclusion][index]);
                    }
                }

                sum = randomWeights[groupIndex].Sum();
                randomWeights[groupIndex] = randomWeights[groupIndex].Select(w => w / sum).ToArray();
            }
        }

        static private List<BubbleType> GetTypeGroup(BubbleFactory factory, BubbleCategory category)
        {
            return factory.Bubbles
                .Where(b => b.category == category)
                .Select(d => d.Type)
                .ToList();
        }
    }
}
