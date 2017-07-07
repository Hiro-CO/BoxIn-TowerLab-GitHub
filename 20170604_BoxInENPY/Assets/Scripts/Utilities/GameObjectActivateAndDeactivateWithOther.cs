using UnityEngine;
using System.Collections;

public class GameObjectActivateAndDeactivateWithOther : MonoBehaviour {

	public GameObject activationFollower;
	public GameObject activationFollowed;

	void Update () {
		if(activationFollower.activeInHierarchy != activationFollowed.activeInHierarchy){
			activationFollower.SetActive (activationFollowed.activeInHierarchy);
		}
	}
}
