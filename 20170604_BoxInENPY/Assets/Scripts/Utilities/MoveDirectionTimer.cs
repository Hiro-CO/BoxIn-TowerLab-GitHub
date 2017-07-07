using UnityEngine;
using System.Collections;

public class MoveDirectionTimer : MonoBehaviour {

	public float timeToStop = 40f;
	float timeToStopPassed = 0f;
	bool stopped = false;
	public MoveToDirection[] moveToDirectionScripts;

	void Update () {
		if (!stopped) {
			if (timeToStopPassed < timeToStop) {
				timeToStopPassed += Time.deltaTime;
			} else {
				foreach (MoveToDirection move in moveToDirectionScripts) {
					move.StopMoving ();
				}
				stopped = true;
			}
		}
	}
}
