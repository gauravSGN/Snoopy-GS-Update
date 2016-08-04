using Goal;

public class GoalIncrementEvent : BaseGoalEvent
{
    public Bubble bubble;
    public int score;

    public GoalIncrementEvent(LevelGoal goal, Bubble bubble, int score) : base(goal)
    {
        this.bubble = bubble;
        this.score = score;
    }
}
