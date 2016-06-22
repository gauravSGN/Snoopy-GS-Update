using BubbleContent;

namespace Goal
{
    public sealed class RescueBabiesGoal : LevelGoal
    {
        override public GoalType Type { get { return GoalType.RescueBabies; } }

        override public void Initialize(LevelData levelData)
        {
            foreach (var bubble in levelData.bubbles)
            {
                if ((BubbleContentType)bubble.contentType == BubbleContentType.BabyPanda)
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
