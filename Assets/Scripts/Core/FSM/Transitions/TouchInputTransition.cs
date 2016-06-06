using UnityEngine;
using System.Collections;

namespace FSM {
	public class TouchInputTransition : Transition {
		private bool isReady = false;
		
		void Update () {
			isReady = Input.touchCount > 0;		
		}

		public override bool IsReady(){
			return isReady;
		}
	}
}
