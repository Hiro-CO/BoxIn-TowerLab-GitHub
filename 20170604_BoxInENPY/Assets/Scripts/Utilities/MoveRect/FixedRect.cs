using UnityEngine;
using System.Collections;

public class FixedRect : MonoBehaviour {

	public RectTransform rect;
	Vector3 fixedPosition;
	bool fixedTheRect = false;
	float lastCameraZ = 0f;

	void Start () {
		fixedPosition = rect.position;
		fixedTheRect = true;

		Reverse ();
	}

	void Update(){
		if (Camera.main.transform.rotation.eulerAngles.z != lastCameraZ) {
			Reverse ();
		}
	}

	public void Reverse(){
		lastCameraZ = Camera.main.transform.rotation.eulerAngles.z;

		if (lastCameraZ != 0f) {
			fixedPosition.x *= -1;
			fixedPosition.y *= -1;
		}
	}
	
	void LateUpdate () {
		if (rect.position != fixedPosition) {
			rect.position = fixedPosition;
		}

	}
}
