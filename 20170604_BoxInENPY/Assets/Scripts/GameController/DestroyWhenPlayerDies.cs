using UnityEngine;
using System.Collections;

public class DestroyWhenPlayerDies : MonoBehaviour {

	private GameObject gameController;
	private GameController gameControllerScript;

	void Awake(){
		gameController = GameObject.FindGameObjectWithTag(Tags.gameController);
		gameControllerScript = gameController.GetComponent<GameController>();
	}

	void Update () {
		if(!gameControllerScript.getIsPlayerAlive()){
			Destroy(gameObject);
		}
	}
}
