using UnityEngine;
using System.Collections.Generic;

public abstract class FancyGizmo : MonoBehaviour {
	public Vector3 positionOffset;
	public Color color = Color.yellow;

	//history
	public bool showHistory;
	public int maxHistory = 10;
	public float historyInterval = 0.1f;
	public Color historyFalloffColor = Color.black;
	private float timer = 0f;

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(showHistory && timer >= historyInterval){
			timer = 0;
			RecordHistory();
		}
	}

	protected abstract void RecordHistory();
}
