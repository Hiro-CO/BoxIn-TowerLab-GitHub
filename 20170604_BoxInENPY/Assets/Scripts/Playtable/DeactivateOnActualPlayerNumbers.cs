using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeactivateOnActualPlayerNumbers : MonoBehaviour {

	public int[] actualPlayerNumbersToDeactivate;

	void Awake () {
		foreach (int playerNumber in actualPlayerNumbersToDeactivate) {
			if (PlaytableApiContainer.getActualPlayerIndex () == playerNumber) {
				gameObject.SetActive (false);
			}
		}
	}
}
