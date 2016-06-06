using UnityEngine;
using System.Collections;

namespace FSM
{
	public class TimedTransition : Transition
	{
		public float waitTime;
		protected float timer;

		public override void OnEnter()
		{
			ResetTimer();
		}

		public override void Tick(float delta)
		{
			timer -= delta;
		}

		public override bool IsReady()
		{
			return timer <= 0f;
		}

		protected void ResetTimer()
		{
			timer = waitTime;
		}
	}
}