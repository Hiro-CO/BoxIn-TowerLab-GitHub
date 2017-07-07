using UnityEngine;
using System.Collections;

public class RandomRotationZ : MonoBehaviour {

	public float minRotation = 0f;
	public float maxRotation = 360f;

	void Awake(){
		RandomRotation ();
	}

	void OnEnable () {
		RandomRotation ();
	}

	public void RandomRotation(){
		Vector3 newRotation = transform.rotation.eulerAngles;
		newRotation.z = Random.Range (minRotation, maxRotation);
		transform.rotation = Quaternion.Euler (newRotation);
	}
}
