using Goal;
using UnityEngine;

public class LevelGoalMonitor : MonoBehaviour
{
    [SerializeField]
    private GoalType goalType;

    [SerializeField]
    UITextUpdater updater;

    protected void Start()
    {
        GlobalState.EventService.AddEventHandler<GoalCreatedEvent>(GoalCreatedHandler);
        gameObject.SetActive(false);
    }

    private void GoalCreatedHandler(GoalCreatedEvent gameEvent)
    {
        if (gameEvent.Goal.Type == goalType)
        {
            updater.Target = gameEvent.Goal;
            gameObject.SetActive(true);
        }
    }
}
