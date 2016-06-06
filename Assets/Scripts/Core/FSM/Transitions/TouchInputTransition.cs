using UnityEngine;
using System.Collections;

namespace FSM {
	public class TouchInputTransition : Transition {
		private bool isReady = false;
		
		public override void Tick(float dt) {
			isReady = Input.touchCount > 0;		
		}

		public override bool IsReady(){
			return isReady;
		}
	}
}
