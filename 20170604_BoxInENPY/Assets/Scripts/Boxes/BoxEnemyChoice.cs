using UnityEngine;
using System.Collections;

public class BoxEnemyChoice : MonoBehaviour {
	public string weaponType;
	public int enemyIndex;
	public BoxMovement boxMovement;

	void Start(){
		if (EnemyDataManager.control.getChosenEnemies ().Count > enemyIndex) {
			weaponType = EnemyDataManager.control.getChosenEnemies () [enemyIndex].code;
		}
	}

	public void KilledEnemy(){
		boxMovement.KilledEnemy();
	}
}
