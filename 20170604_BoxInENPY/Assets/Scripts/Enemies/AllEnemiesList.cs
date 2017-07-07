using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllEnemiesList : EnemyList {

	public override void Awake(){
		if (setListOnAwake) {
			SetEnemyList ();
		}
	}

	public override void SetEnemyList() {
		EnemyDataManager.control.listOfEnemies.Clear ();
		for (int i = 0; i < enemies.Count; i++) {
			EnemyDataManager.control.listOfEnemies.Add (enemies[i]);
		}
	}
}
