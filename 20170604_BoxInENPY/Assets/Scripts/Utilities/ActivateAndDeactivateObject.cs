using UnityEngine;
using System.Collections;

public class ActivateAndDeactivateObject : MonoBehaviour {

	public void ActivateTheObject(){
		gameObject.SetActive(true);
	}
	public void DeactivateTheObject(){
		gameObject.SetActive(false);
	}
}
