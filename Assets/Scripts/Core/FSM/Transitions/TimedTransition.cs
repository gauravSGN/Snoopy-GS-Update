using UnityEngine;
using System.Collections;

namespace FSM {
	public class TimedTransition : Transition {
		public float waitTime;
		protected float timer;

		void OnEnable() {
			ResetTimer();
		}
		
		void Update() {
			float delta = Time.deltaTime;
			timer -= delta;
		}

		public override bool IsReady(){
			return timer <= 0f;
		}

		protected void ResetTimer() {
			timer = waitTime;
		}
	}
}