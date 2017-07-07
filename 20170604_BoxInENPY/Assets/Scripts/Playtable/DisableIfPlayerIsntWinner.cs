using UnityEngine;
using System.Collections;

public class DisableIfPlayerIsntWinner : MonoBehaviour {

	public int playerIndex = 0;
	public bool showOnDraw = false;

	void Start () {
		int[] scores = PlaytableApiContainer.getAllScores ();
		bool disable = false;

		for(int i=0; i< scores.Length; i++) {
			if (i != playerIndex) {
				if (PlaytableApiContainer.getScore (playerIndex) < PlaytableApiContainer.getScore (i)) {
					disable = true;
				}
				if (PlaytableApiContainer.getScore (playerIndex) == PlaytableApiContainer.getScore (i)) {
					if (showOnDraw) {
						disable = false;
					} else {
						disable = true;
					}
				}
			}
		}

		if (disable) {
			gameObject.SetActive (false);
		}
	}
}
