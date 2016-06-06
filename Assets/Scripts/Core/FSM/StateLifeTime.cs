namespace FSM
{
	public interface StateLifeTime 
	{
		void OnEnter();
		void Tick(float deltaT);
		void OnExit();
	}
}