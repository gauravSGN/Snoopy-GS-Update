using System;
using System.Collections.Generic;
using System.Linq;
using BubbleContent;

namespace Goal
{
    public static class LevelGoalFactory
    {
        private static Dictionary<Func<LevelData, bool>, Type> goalDeterminers = new Dictionary<Func<LevelData, bool>, Type>
        {
            { RescueBabiesDeterminer, typeof(RescueBabiesGoal) },
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

        private static bool RescueBabiesDeterminer(LevelData levelData)
        {
            return levelData.bubbles.Any(b => (BubbleContentType)b.contentType == BubbleContentType.BabyPanda);
        }
    }
}
