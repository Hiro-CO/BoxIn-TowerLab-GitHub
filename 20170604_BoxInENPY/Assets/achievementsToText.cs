using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class achievementsToText : MonoBehaviour {

	public Text text;
	public int playerIndex = 0;

	void Start () {
		string[] achievements = PlaytableApiContainer.getAchievementsActual (playerIndex);
		foreach (string achievement in achievements) {
			text.text += achievement + "\n";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
