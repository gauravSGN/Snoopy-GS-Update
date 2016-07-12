using BubbleContent;
using Model;

namespace Goal
{
    sealed public class RescueBabiesGoal : LevelGoal
    {
        override public GoalType Type { get { return GoalType.RescueBabies; } }

        override public void Initialize(LevelData levelData)
        {
            foreach (var bubble in levelData.Bubbles)
            {
                if (bubble.ContentType == BubbleContentType.BabyPanda)
                {
                    bubble.model.OnPopped += BubbleReactionHandler;
                    bubble.model.OnDisconnected += BubbleReactionHandler;
                    TargetValue++;
                }
            }
        }

        private void BubbleReactionHandler(Bubble bubble)
        {
            bubble.OnPopped -= BubbleReactionHandler;
            bubble.OnDisconnected -= BubbleReactionHandler;

            CurrentValue++;
            NotifyListeners();

            if (CurrentValue == TargetValue)
            {
                CompleteGoal();
            }
        }
    }
}
