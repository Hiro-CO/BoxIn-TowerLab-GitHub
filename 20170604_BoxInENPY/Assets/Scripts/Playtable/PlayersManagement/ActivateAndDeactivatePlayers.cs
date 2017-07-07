using UnityEngine;
using System.Collections;

public class ActivateAndDeactivatePlayers : MonoBehaviour {

	public int[] playerIndexes;

	public void ActivatePlayer(){
		for (int i = 0; i < playerIndexes.Length; i++) {
			if (PlaytableApiContainer.getMaxNumberOfPlayers () > playerIndexes[i]) {
				PlaytableApiContainer.setIsPlayerActive (playerIndexes[i], true);
			}
		}
	}
	public void DeactivatePlayer(){
		for (int i = 0; i < playerIndexes.Length; i++) {
			if (PlaytableApiContainer.getMaxNumberOfPlayers () > playerIndexes [i]) {
				PlaytableApiContainer.setIsPlayerActive (playerIndexes [i], false);
			}
		}
	}
}
