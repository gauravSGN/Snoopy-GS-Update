using UnityEngine;
using System.Collections.Generic;

public class InstantDrainer : MonoBehaviour {
	public Battery battery;
	public float percentDrain;
	public bool startOnEnable;

	void OnEnable() {
		if(startOnEnable) {
			StartDrain();
		}
	}

	public void StartDrain() {
		float final = percentDrain * battery.totalCapacity;
		battery.Sub(final);
	}
}
