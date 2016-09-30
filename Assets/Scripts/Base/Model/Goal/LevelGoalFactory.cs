using Model;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Goal
{
    public static class LevelGoalFactory
    {
        private static Dictionary<Func<LevelData, bool>, Type> goalDeterminers = new Dictionary<Func<LevelData, bool>, Type>
        {
            { RescueTargetDeterminer, typeof(RescueTargetGoal) },
            { BossModeDeterminer, typeof(Snoopy.Model.Goal.BossModeGoal) },
        };

        public static List<LevelGoal> GetGoalsForLevel(LevelData levelData)
        {
            var goals = new List<LevelGoal>();

            foreach (var pair in goalDeterminers)
            {
                if (pair.Key(levelData))
                {
                    var goal = (LevelGoal)Activator.CreateInstance(pair.Value);
                    goal.Initialize(levelData);
                    goals.Add(goal);
                }
            }

            return goals;
        }

        private static bool RescueTargetDeterminer(LevelData levelData)
        {
            return levelData.Bubbles.Where(b => b.modifiers != null)
                                    .SelectMany(b => b.modifiers)
                                    .Any(m => m.type == BubbleModifierType.RescueTarget);
        }

        private static bool BossModeDeterminer(LevelData levelData)
        {
            return levelData.Bubbles.Any(b => b.Type == BubbleType.BossTrack);
        }
    }
}
