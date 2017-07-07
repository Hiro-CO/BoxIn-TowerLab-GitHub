using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddChosenPlayerNameToText : MonoBehaviour {

	public Text text;
	public int playerIndex = 0;

	void Start () {
		string playerName = PlaytableApiContainer.getPlayerName( playerIndex );
		if (playerName != "" && playerName != null) {
			text.text += playerName;
		} else {
			text.text = "PLAYER "+(playerIndex + 1).ToString();
		}
	}
}
