using UnityEngine;
using System.Collections;

public class RotateRectToCameraZ : MonoBehaviour {

	//public Camera cameraToUse;
	public RectTransform[] rectsToUse;
	//public InputManager inputManagerScript;
	float lastCameraZ = 0f;

	void OnLevelWasLoaded(int level) {
		Reverse ();
	}

	void Start () {
		Reverse ();
	}

	void Update(){
		if (Camera.main.transform.rotation.eulerAngles.z != lastCameraZ) {
			Reverse ();
		}
	}

	public void Reverse(){
		lastCameraZ = Camera.main.transform.rotation.eulerAngles.z;
		foreach(RectTransform rectToUse in rectsToUse){
			rectToUse.localRotation = Quaternion.Euler(new Vector3(rectToUse.localRotation.eulerAngles.x,rectToUse.localRotation.eulerAngles.y,lastCameraZ)); 
		}

		/*
		if (inputManagerScript != null) {
			inputManagerScript.RecalculateAllButtons (lastCameraZ);
		}
		*/
	}
}
