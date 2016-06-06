using UnityEngine;
using System.Collections;

namespace FSM
{
	public class ImmediateTransition : Transition
	{
		public override bool IsReady()
		{
			return true;
		}
	}
}