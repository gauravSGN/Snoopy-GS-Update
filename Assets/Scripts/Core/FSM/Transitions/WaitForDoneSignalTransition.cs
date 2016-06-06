using UnityEngine;
using System.Collections.Generic;

namespace FSM {
	public class WaitForDoneSignalTransition : Transition, StateLifeTime {
		private bool done = false;

		public void OnEnter() {
			done = false;
		}
		public void Tick(float deltaT) {}
		public void OnExit() {}

		public override bool IsReady () {
			return done;
		}
		
		public void SetDone(){
			done = true;
		}
	}
}
