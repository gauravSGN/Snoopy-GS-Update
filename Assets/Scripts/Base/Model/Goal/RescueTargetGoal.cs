using Model;
using System.Linq;

namespace Goal
{
    sealed public class RescueTargetGoal : LevelGoal
    {
        override public GoalType Type { get { return GoalType.RescueBabies; } }

        override public void Initialize(LevelData levelData)
        {
            foreach (var bubble in levelData.Bubbles.Where(HasRescueTarget))
            {
                bubble.model.OnPopped += BubbleReactionHandler;
                bubble.model.OnDisconnected += BubbleReactionHandler;
                TargetValue++;
            }

            Score = GetScoreForGoalType(Type);
        }

        private void BubbleReactionHandler(Bubble bubble)
        {
            bubble.OnPopped -= BubbleReactionHandler;
            bubble.OnDisconnected -= BubbleReactionHandler;

            GlobalState.EventService.Dispatch(new GoalIncrementEvent(this, bubble, Score));

            CurrentValue++;
            NotifyListeners();

            if (CurrentValue == TargetValue)
            {
                CompleteGoal();
            }
        }

        private static bool HasRescueTarget(BubbleData bubble)
        {
            return (bubble.modifiers != null) &&
                   bubble.modifiers.Any(m => m.type == BubbleModifierType.RescueTarget);
        }
    }
}
