using UnityEngine;
using System.Collections;

public class EnemyPooler : ObjectsPooler {

	public GameControllerScore scoreController;
	public int enemyIndex = 0;

	public override void Start (){
		if (EnemyDataManager.control.getChosenEnemies ().Count > enemyIndex) {
			base.Start ();
		} else {
			gameObject.SetActive (false);
		}
	}

	public override GameObject UsePooledObject(Vector3 positionToUse, Quaternion rotationToUse){
		scoreController.addOneSpawnedEnemy (enemyIndex);
		GameObject unScaledObj = base.UsePooledObject (positionToUse, rotationToUse);
		unScaledObj.transform.localScale = new Vector3(1f,1f,1f);
		return unScaledObj;
	}
}
