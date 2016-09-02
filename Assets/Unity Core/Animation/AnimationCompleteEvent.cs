using UnityEngine;

namespace Animation
{
    sealed public class AnimationCompleteEvent : GameEvent
    {
        public GameObject gameObject;

        static public void Dispatch(GameObject gameObject)
        {
            var gameEvent = GlobalState.EventService.GetPooledEvent<AnimationCompleteEvent>();

            gameEvent.gameObject = gameObject;

            GlobalState.EventService.DispatchPooled(gameEvent);
        }
    }
}
