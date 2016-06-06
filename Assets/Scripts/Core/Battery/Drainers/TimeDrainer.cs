using UnityEngine;
using System.Collections.Generic;

public class TimeDrainer : MonoBehaviour {
	public float timeToDrain;
	public Battery battery;
	public bool startOnEnable;
	[SerializeField]
	private bool continuous;
	private bool reachedTarget;
	private float targetDrainLevel;

	void OnEnable() {
		reachedTarget = true;

		if(startOnEnable) {
			StartDrain();
		}
	}

	public void StartDrain(){
		battery.ChargeBlockerEnabled();
		reachedTarget = false;
		targetDrainLevel = battery.NextTargetEnergyLevel();
	}

	public void StopDrain(){
		battery.ChargeBlockerDisabled();
	}

	public void InterruptDrain(){
		reachedTarget = true;
		battery.SetCapacity(targetDrainLevel);
	}
	
	//will always drain until target value reached
	void Update(){
		if(!reachedTarget){
			float perc = Time.deltaTime / timeToDrain;
			float diff = battery.totalCapacity * perc;
			battery.Sub(diff);
			if(battery.CompareCapacity(targetDrainLevel) < 1){
				if(continuous){
					targetDrainLevel = battery.NextTargetEnergyLevel();
				}
				else{
					reachedTarget = true;
					battery.SetCapacity(targetDrainLevel);
					StopDrain();
				}
			}
		}
	}
}
