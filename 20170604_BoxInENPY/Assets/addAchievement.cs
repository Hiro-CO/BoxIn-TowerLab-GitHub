using UnityEngine;
using System.Collections;

public class addAchievement : MonoBehaviour {

	public string achievementCode = "";
	public int playerIndex = 0;

	void Awake(){
		PlaytableApiContainer.setAchievements (playerIndex,achievementCode);
	}
}
