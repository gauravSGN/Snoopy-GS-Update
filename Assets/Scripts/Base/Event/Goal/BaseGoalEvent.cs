using Goal;

public class BaseGoalEvent : GameEvent
{
    public LevelGoal Goal { get; private set; }

    public BaseGoalEvent(LevelGoal goal)
    {
        Goal = goal;
    }
}
