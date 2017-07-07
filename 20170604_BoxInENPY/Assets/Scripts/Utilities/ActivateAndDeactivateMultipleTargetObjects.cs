using UnityEngine;
using System.Collections;

public class ActivateAndDeactivateMultipleTargetObjects : MonoBehaviour {

	public GameObject[] targetObjects;

	public void ActivateTheObjects(){
		foreach(GameObject targetObject in targetObjects){
			targetObject.SetActive(true);
		}
	}
	public void DeactivateTheObjects(){
		foreach(GameObject targetObject in targetObjects){
			targetObject.SetActive(false);
		}
	}
}
