using UnityEngine;

namespace FSM 
{
	public abstract class Transition : MonoBehaviour, StateLifeTime 
	{
		public State nextState;
		public Color color = Color.white;
		public int priority = 100;

		public abstract bool IsReady();

		public virtual State Fire()
		{
			return nextState;
		}

		protected void Awake() 
		{
			if (nextState == null) 
			{
				Debug.LogError ("Transition to nowhere on state: " + gameObject.name);
			}
		}

		public virtual void OnEnter(){}

		public virtual void Tick(float deltaT){}

		public virtual void OnExit() {}

		public virtual void HandleMessage(string message) {}

		void OnDrawGizmos()
		{
			Gizmos.color = color;
			if (nextState != null) 
			{	
				Gizmos.DrawLine (transform.position, nextState.transform.position);
			}
		}
	}
}