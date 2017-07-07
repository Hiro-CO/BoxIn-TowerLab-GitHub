using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnChooseEnemies : MonoBehaviour {

	public List<MainTagEnemy> mainTags;
	public List<StageTagEnemy> stageTags;
	public List<DifficultyTagEnemy> difficulties;
	public List<ExtraTagsEnemy> extraTags;
	public goToScene goToScene;
	public int numberOfEnemies = 3;

	public void FilterByMainAndDifficulty(){
		EnemyDataManager.control.FilterEnemies (mainTags, difficulties);
	}

	public void FilterByStage(){
		EnemyDataManager.control.FilterEnemies (stageTags);
	}

	public void FilterByAll(){
		EnemyDataManager.control.FilterEnemies (mainTags, difficulties, extraTags);
	}

	public void ChooseFiltered(){
		if(EnemyDataManager.control.ChooseFromFilteredEnemies (numberOfEnemies)){
			goToScene.selectScene ();
		}
	}
}
