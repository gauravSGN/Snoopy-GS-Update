using BubbleContent;
using Model;

namespace Goal
{
    public sealed class RescueBabiesGoal : LevelGoal
    {
        override public GoalType Type { get { return GoalType.RescueBabies; } }

        override public void Initialize(LevelData levelData)
        {
            foreach (var bubble in levelData.Bubbles)
            {
                if (bubble.ContentType == BubbleContentType.BabyPanda)
                {
                    bubble.model.OnPopped += BubblePoppedHandler;
                    TargetValue++;
                }
            }
        }

        private void BubblePoppedHandler(Bubble bubble)
        {
            bubble.OnPopped -= BubblePoppedHandler;

            CurrentValue++;
            NotifyListeners();

            if (CurrentValue == TargetValue)
            {
                CompleteGoal();
            }
        }
    }
}
