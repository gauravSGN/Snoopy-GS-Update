using BubbleContent;

namespace Goal
{
    public sealed class RescueBabiesGoal : LevelGoal
    {
        public override GoalType Type { get { return GoalType.RescueBabies; } }

        public override void Initialize(LevelData levelData)
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
