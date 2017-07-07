using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnEnemyList : MonoBehaviour {

	public EnemyList enemyList;
	public GameObject playButton;
	public goToScene goToSceneScriptMultiplayer;
	bool clicked = false;
	public float delayToGoToScene = 0.7f;
	public bool scoreSelect = false;
	float delayToGoToScenePassed = 0;
	public static List<BtnEnemyList> allBtnEnemyLists = new List<BtnEnemyList>();

	void Awake(){
		for (int i = 0; i < allBtnEnemyLists.Count; i++) {
			if(allBtnEnemyLists[i] == null){
				BtnEnemyList enemyListToRemove = allBtnEnemyLists [i];
				allBtnEnemyLists.Remove (enemyListToRemove);
			}
		}
		if (!allBtnEnemyLists.Contains (this)) {
			allBtnEnemyLists.Add (this);
		}
	}

	void Update(){
		if(clicked){
			if (delayToGoToScenePassed < delayToGoToScene) {
				delayToGoToScenePassed += Time.deltaTime;
			}
		}
	}

	public void Clicked(){
		if (scoreSelect) {
			UseEnemyList ();
			goToSceneScriptMultiplayer.selectScene ();
			return;
		}

		if (!clicked) {
			ActivateButton ();
		} 
		if(clicked && delayToGoToScenePassed >= delayToGoToScene) {
			UseEnemyList ();
			if (PlaytableApiContainer.getNumberOfActivePlayers () == 1) {
				goToSceneScriptMultiplayer.selectScene ();
			} else {
				if (goToSceneScriptMultiplayer != null) {
					goToSceneScriptMultiplayer.selectScene ();
				}
			}
		}
	}

	public void UseEnemyList(){
		enemyList.SetEnemyList ();
	}

	public void ActivateButton(){
		if (playButton != null) {
			playButton.SetActive (true);
		}
		clicked = true;

		for (int i = 0; i < allBtnEnemyLists.Count; i++) {
			if (allBtnEnemyLists[i] != this) {
				allBtnEnemyLists [i].DeactivateButton ();
			}
		}
	}

	public void DeactivateButton(){
		if (playButton != null) {
			playButton.SetActive (false);
		}
		clicked = false;
		delayToGoToScenePassed = 0f;
	}
}
