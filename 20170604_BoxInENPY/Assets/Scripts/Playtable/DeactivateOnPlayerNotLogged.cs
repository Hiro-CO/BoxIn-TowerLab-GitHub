using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeactivateOnPlayerNotLogged : MonoBehaviour {

	public int[] playersToBeLogged;

	void Start () {
		foreach (int playerNumber in playersToBeLogged) {
			if (!PlaytableApiContainer.getPlayerLogged (playerNumber)) {
				gameObject.SetActive (false);
			}
		}
	}
}
