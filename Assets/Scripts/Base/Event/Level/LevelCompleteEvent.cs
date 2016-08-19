public class LevelCompleteEvent : GameEvent
{
    public bool Won { get; private set; }

    public LevelCompleteEvent(bool won)
    {
        Won = won;
        GlobalState.EventService.Dispatch(new PreLevelCompleteEvent());
    }
}
