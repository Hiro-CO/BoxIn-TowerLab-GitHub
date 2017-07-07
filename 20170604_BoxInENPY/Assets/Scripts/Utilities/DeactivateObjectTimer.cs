using UnityEngine;
using System.Collections;

public class DeactivateObjectTimer : MonoBehaviour {

	public ActivateAndDeactivateObject activateAndDeactivateObject;
	public float timeToDeactivate = 1f;
	float timeToDeactivatePassed = 0f;

	void OnEnable(){
		timeToDeactivatePassed = 0f;
	}

	void Update(){
		if (timeToDeactivatePassed < timeToDeactivate) {
			timeToDeactivatePassed += Time.deltaTime;
		} else {
			timeToDeactivatePassed = 0f;
			activateAndDeactivateObject.DeactivateTheObject ();
		}
	}
}
