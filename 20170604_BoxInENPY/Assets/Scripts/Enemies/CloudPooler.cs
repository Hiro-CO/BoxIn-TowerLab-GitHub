using UnityEngine;
using System.Collections;

public class CloudPooler : ObjectsPooler {

	public int cloudIndex = 0;

	public override void Start (){
		if (EnemyDataManager.control.getChosenEnemies ().Count > cloudIndex) {
			base.Start ();
		} else {
			gameObject.SetActive (false);
		}
	}
}
