namespace FSM
{
    public interface StateLifeCycle
    {
        void OnEnter();
        void Tick(float deltaT);
        void OnExit();
    }
}