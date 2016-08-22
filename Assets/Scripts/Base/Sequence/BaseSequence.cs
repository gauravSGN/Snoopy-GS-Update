namespace Sequence
{
    abstract public class BaseSequence<T>
    {
        abstract public void Begin(T parameters);

        protected void TransitionToReturnScene()
        {
            GlobalState.EventService.Dispatch(new TransitionToReturnSceneEvent());
        }
    }
}
