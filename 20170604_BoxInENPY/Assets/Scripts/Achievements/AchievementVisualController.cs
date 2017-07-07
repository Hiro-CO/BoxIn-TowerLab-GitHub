using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AchievementVisualController : MonoBehaviour {

	public MoveRect moveRect;
	public Text text;
	List<string> achievementsQueued = new List<string>();
	bool showingAchievement = false;


	public void QueueAchievement(string textToQueue){
		achievementsQueued.Add (textToQueue);

		if (!showingAchievement && achievementsQueued.Count > 0) {
			ShowAchievement (achievementsQueued[0]);
		}
	}

	public void ShowAchievement(string textNew){
		showingAchievement = true;

		text.text = textNew;
		moveRect.GoToEnd ();
		//AudioManager.PlaySound (AudioNames.achievement.ToString(), Vector3.zero);

		achievementsQueued.Remove (textNew);
	}

	public void ShowNextAchievement(){
		if (achievementsQueued.Count > 0) {
			string textNew = achievementsQueued [0];

			text.text = textNew;
			moveRect.GoToEnd ();
			//AudioManager.PlaySound (AudioNames.achievement.ToString(), Vector3.zero);

			achievementsQueued.Remove (textNew);
		} else {
			showingAchievement = false;
		}
	}

}
