using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDataManager : MonoBehaviour {

	public static EnemyDataManager control;
	public List<EnemyData> listOfEnemies;
	List<EnemyData> chosenEnemies = new List<EnemyData> ();
	List<EnemyData> availableEnemies = new List<EnemyData> ();
	List<EnemyData> filteredEnemies = new List<EnemyData> ();

	void Awake () {
		if(control == null){
			DontDestroyOnLoad(this);
			control = this;
			filteredEnemies = listOfEnemies;
			//ChooseFromAllEnemies (5);
		}else{
			Destroy(gameObject);
		}
	}

	public bool ChooseEnemies (int numberOfEnemies, List<EnemyData> enemiesList) {
		if (enemiesList.Count >= numberOfEnemies) {
			chosenEnemies.Clear ();
			availableEnemies = enemiesList;

			for (int i = 0; i < numberOfEnemies; i++) {
				EnemyData enemyToAdd = availableEnemies [Random.Range (0, availableEnemies.Count)];
				chosenEnemies.Add (enemyToAdd);
				availableEnemies.Remove (enemyToAdd);
			}

			return true;
		}

		return false;
	}
	public bool ChooseFromAllEnemies (int numberOfEnemies) {
		return ChooseEnemies (numberOfEnemies, listOfEnemies);
	}
	public bool ChooseFromFilteredEnemies(int numberOfEnemies){
		return ChooseEnemies (numberOfEnemies, filteredEnemies);
	}
	public void AddChosenEnemy(EnemyData enemyChosen){
		chosenEnemies.Add (enemyChosen);
	}

	public void FilterEnemies(List<MainTagEnemy> mainTags, List<DifficultyTagEnemy> difficulties, List<ExtraTagsEnemy> extraTags){
		filteredEnemies = listOfEnemies;

		foreach (MainTagEnemy tag in mainTags) {
			filteredEnemies = getEnemiesWithMainTag (tag, filteredEnemies);
		}
		foreach (DifficultyTagEnemy tag in difficulties) {
			filteredEnemies = getEnemiesWithDifficulty (tag, filteredEnemies);
		}
		foreach (ExtraTagsEnemy tag in extraTags) {
			filteredEnemies = getEnemiesWithExtraTag (tag, filteredEnemies);
		}

		if (mainTags.Count == 0 || difficulties.Count == 0 || extraTags.Count == 0) {
			filteredEnemies.Clear ();
		}
	}
	public void FilterEnemies(List<MainTagEnemy> mainTags, List<DifficultyTagEnemy> difficulties){
		filteredEnemies = listOfEnemies;

		foreach (MainTagEnemy tag in mainTags) {
			filteredEnemies = getEnemiesWithMainTag (tag, filteredEnemies);
		}
		foreach (DifficultyTagEnemy tag in difficulties) {
			filteredEnemies = getEnemiesWithDifficulty (tag, filteredEnemies);
		}

		if (mainTags.Count == 0 || difficulties.Count == 0) {
			filteredEnemies.Clear ();
		}
	}
	public void FilterEnemies(MainTagEnemy mainTag){
		List<MainTagEnemy> mainTags = new List<MainTagEnemy>();
		mainTags.Add (mainTag);
		FilterEnemies (mainTags);
	}
	public void FilterEnemies(List<MainTagEnemy> mainTags){
		filteredEnemies = listOfEnemies;
		foreach (MainTagEnemy tag in mainTags) {
			filteredEnemies = getEnemiesWithMainTag (tag, filteredEnemies);
		}
		if (mainTags.Count == 0) {
			filteredEnemies.Clear ();
		}
	}
	public void FilterEnemies(List<StageTagEnemy> stageTags){
		filteredEnemies = listOfEnemies;

		foreach (StageTagEnemy tag in stageTags) {
			filteredEnemies = getEnemiesWithStageTag (tag, filteredEnemies);
		}

		if (stageTags.Count == 0) {
			filteredEnemies.Clear ();
		}
	}
	public void FilterEnemies(List<DifficultyTagEnemy> difficulties){
		filteredEnemies = listOfEnemies;

		foreach (DifficultyTagEnemy tag in difficulties) {
			filteredEnemies = getEnemiesWithDifficulty (tag, filteredEnemies);
		}

		if (difficulties.Count == 0) {
			filteredEnemies.Clear ();
		}
	}
	public void FilterEnemies(List<ExtraTagsEnemy> extraTags){
		filteredEnemies = listOfEnemies;

		foreach (ExtraTagsEnemy tag in extraTags) {
			filteredEnemies = getEnemiesWithExtraTag (tag, filteredEnemies);
		}

		if (extraTags.Count == 0) {
			filteredEnemies.Clear ();
		}
	}

	public List<EnemyData> getFilteredEnemies(){
		return filteredEnemies;
	}

	public List<EnemyData> getChosenEnemies(){
		return chosenEnemies;
	}


	public List<EnemyData> getEnemiesWithMainTag(MainTagEnemy chosenTag, List<EnemyData> enemiesList){
		List<EnemyData> enemiesWithTag = new List<EnemyData>();

		foreach(EnemyData enemy in enemiesList){
			if (enemy.mainTag == chosenTag) {
				enemiesWithTag.Add (enemy);
			}
		}

		return enemiesWithTag;
	}

	public List<EnemyData> getEnemiesWithStageTag(StageTagEnemy chosenTag, List<EnemyData> enemiesList){
		List<EnemyData> enemiesWithTag = new List<EnemyData>();

		foreach(EnemyData enemy in enemiesList){
			if (enemy.stageTag == chosenTag) {
				enemiesWithTag.Add (enemy);
			}
		}

		return enemiesWithTag;
	}

	public List<EnemyData> getEnemiesWithDifficulty(DifficultyTagEnemy chosenTag, List<EnemyData> enemiesList){
		List<EnemyData> enemiesWithTag = new List<EnemyData>();

		foreach(EnemyData enemy in enemiesList){
			if (enemy.difficulty == chosenTag) {
				enemiesWithTag.Add (enemy);
			}
		}

		return enemiesWithTag;
	}


	public List<EnemyData> getEnemiesWithExtraTag(ExtraTagsEnemy chosenTag, List<EnemyData> enemiesList){
		List<EnemyData> enemiesWithTag = new List<EnemyData>();

		foreach(EnemyData enemy in enemiesList){
			foreach (ExtraTagsEnemy extraTag in enemy.extraTags) {
				if (extraTag == chosenTag) {
					enemiesWithTag.Add (enemy);
				}
			}
		}

		return enemiesWithTag;
	}

}