using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyList : MonoBehaviour {

	public List<EnemyData> enemies;
	public bool setListOnAwake = false;

	public virtual void Awake(){
		if (setListOnAwake) {
			SetEnemyList ();
		}
	}

	public virtual void SetEnemyList() {
		EnemyDataManager.control.listOfEnemies.Clear ();
		foreach(EnemyData enemy in enemies){
			EnemyDataManager.control.listOfEnemies.Add (enemy);
		}
		EnemyDataManager.control.ChooseFromAllEnemies (enemies.Count);
	}
}
