using Model;
using Service;

namespace Goal
{
    abstract public class LevelGoal : Observable
    {
        abstract public GoalType Type { get; }
        public int CurrentValue { get; protected set; }
        public int TargetValue { get; protected set; }
        public int Score { get; protected set; }
        public bool Complete { get; private set; }

        abstract public void Initialize(LevelData levelData);

        protected void CompleteGoal()
        {
            Complete = true;
            GlobalState.Instance.Services.Get<EventService>().Dispatch(new GoalCompleteEvent(this));
        }

        protected int GetScoreForGoalType(GoalType goalType)
        {
            var config = GlobalState.Instance.Config.scoring;
            int score = 0;

            foreach (var goalScore in config.goals)
            {
                if (goalScore.goalType == goalType)
                {
                    score = goalScore.score;
                    break;
                }
            }

            return score;
        }
    }
}
