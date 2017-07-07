using UnityEngine;
using System.Collections;

public class StartAnotherCloud : MonoBehaviour {

	public SpawnerCloud spawnerCloud;

	public void StartCloud(){
		spawnerCloud.StartACloud ();
	}

	public void EnableObject(){
		gameObject.SetActive (true);
	}

	public void DisableObject(){
		spawnerCloud.AddCloudToUse (gameObject);
		gameObject.SetActive (false);
	}
}
