using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddActualPlayerNameToText : MonoBehaviour {

	public Text text;

	void Start () {
		string playerName = PlaytableApiContainer.getPlayerName( PlaytableApiContainer.getActualPlayerIndex () );
		if (playerName != "" && playerName != null && playerName != "Anônimo") {
			text.text += playerName;
		} else {
			text.text += "PLAYER " + (PlaytableApiContainer.getActualPlayerIndex () + 1).ToString();
		}
	}
}
