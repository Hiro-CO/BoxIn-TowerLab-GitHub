using UnityEngine;
using System.Collections;
using Playmove;

public class ReverseOnActivePlayersTotal : MonoBehaviour {

	public int[] activelPlayersTotal;

	void Start () {
		foreach (int player in activelPlayersTotal) {
			if (player == PlaytableApiContainer.getNumberOfActivePlayers ()) {
				InvertScreen.Instance.Rotate (false);
			}
		}
	}
}
