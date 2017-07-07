using UnityEngine;
using System.Collections;

public class EnableByEnemiesNumber : MonoBehaviour {

	public int numberOfEnemies = 3;
	public GameControllerScore gameControllerScore;
	public BoxEnableDisable[] boxes;

	void Start () {
		if (EnemyDataManager.control.getChosenEnemies ().Count != numberOfEnemies) {
			gameObject.SetActive (false);
		}else{
			if (PlaytableApiContainer.getNumberOfActivePlayers () == 1 && boxes.Length > 0) {
				for (int i = 1; i < boxes.Length; i++) {
					boxes [i].Disable();
				}
			}
			if (boxes.Length > 0) {
				gameControllerScore.setBoxes(boxes);
			}
		}
		
	}
}
