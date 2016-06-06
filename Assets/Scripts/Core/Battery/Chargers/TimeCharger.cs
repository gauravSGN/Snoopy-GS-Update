using UnityEngine;
using System.Collections.Generic;

public class TimeCharger : MonoBehaviour {
	public float timeToCharge;
	public Battery battery;

	void Update(){
		float perc = Time.deltaTime / timeToCharge;
		float diff = battery.totalCapacity * perc;
		battery.Add(diff);
	}
}
