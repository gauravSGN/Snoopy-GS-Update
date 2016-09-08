namespace Sequence
{
    sealed public class StartBubblePartyEvent : GameEvent
    {
        public static void Dispatch()
        {
            var gameEvent = GlobalState.EventService.GetPooledEvent<StartBubblePartyEvent>();
            GlobalState.EventService.DispatchPooled(gameEvent);
        }
    }
}
