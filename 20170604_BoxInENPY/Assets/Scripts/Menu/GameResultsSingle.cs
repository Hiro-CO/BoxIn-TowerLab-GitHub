using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameResultsSingle : MonoBehaviour {
	public Image backgroundImage;
	public Sprite[] bgSprites;

	void Start(){
		if(EnemyDataManager.control.getChosenEnemies().Count > 0){
			if(bgSprites.Length > (int)EnemyDataManager.control.getChosenEnemies () [0].mainTag){
				backgroundImage.sprite = bgSprites[(int)EnemyDataManager.control.getChosenEnemies () [0].mainTag];
			}
		}

	}
}
