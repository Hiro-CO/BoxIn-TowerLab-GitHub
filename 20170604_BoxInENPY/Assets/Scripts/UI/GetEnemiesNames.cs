using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetEnemiesNames : MonoBehaviour {

	public int enemyIndex;
	public Text text;

	void Start () {
		if (EnemyDataManager.control.getChosenEnemies ().Count > enemyIndex) {
			text.text = EnemyDataManager.control.getChosenEnemies () [enemyIndex].nameEn.ToUpper();
		}
	}
}
