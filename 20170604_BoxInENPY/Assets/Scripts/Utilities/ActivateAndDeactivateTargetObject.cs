using UnityEngine;
using System.Collections;

public class ActivateAndDeactivateTargetObject : MonoBehaviour {

	public GameObject targetObject;

	public void ActivateTheObject(){
		targetObject.SetActive(true);
	}
	public void DeactivateTheObject(){
		targetObject.SetActive(false);
	}
}
