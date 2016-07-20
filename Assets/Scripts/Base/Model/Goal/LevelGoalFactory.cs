using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Goal
{
    public static class LevelGoalFactory
    {
        private static Dictionary<Func<LevelData, bool>, Type> goalDeterminers = new Dictionary<Func<LevelData, bool>, Type>
        {
            { RescueTargetDeterminer, typeof(RescueTargetGoal) },
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
            return levelData.Bubbles.Where(b => b.modifiers != null).SelectMany(b => b.modifiers).Any(m => m.type == BubbleModifierType.RescueTarget);
        }
    }
}
