using Goal;
using UnityEngine;

public class LevelGoalMonitor : MonoBehaviour
{
    public GoalType goalType;

    protected void Start()
    {
        EventDispatcher.Instance.AddEventHandler<GoalCreatedEvent>(GoalCreatedHandler);
        gameObject.SetActive(false);
    }

    private void GoalCreatedHandler(GoalCreatedEvent gameEvent)
    {
        if (gameEvent.Goal.Type == goalType)
        {
            var updater = gameObject.GetComponent<TextUpdater>();
            updater.Target = gameEvent.Goal;

            gameObject.SetActive(true);
        }
    }
}
