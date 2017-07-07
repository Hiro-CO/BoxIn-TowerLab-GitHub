using UnityEngine;
using System.Collections;

public class GameControllerDifficulty : MonoBehaviour {

	public Transform spawnersContainer;
	Transform[] spawners;
	public Spawner[] spawnersScripts;
	public int difficultyLevel = 0;

	void Awake(){
		if(spawnersContainer != null){
			spawners = new Transform[spawnersContainer.childCount];
			for(int i=0; i<spawners.Length; i++){
				spawners[i] = spawnersContainer.GetChild(i);
			}

			spawnersScripts = new Spawner[spawners.Length];
			for(int i=0; i<spawners.Length; i++){
				spawnersScripts[i] = spawners[i].GetComponent<Spawner>();
			}
		}
	}

	void changeToLevel(int level){
		if(level <= 20){
			changeSpawnersLevel();
		}
	}

	void changeSpawnersLevel(){
		foreach(Spawner spawnerScript in spawnersScripts){
			if(spawnerScript.spawnerAppearsOnLevel < difficultyLevel){
				spawnerScript.gameObject.SetActive(false);
			}
			if(spawnerScript.spawnerAppearsOnLevel == difficultyLevel){
				spawnerScript.gameObject.SetActive(true);
			}
		}
	}

	public void setIsSpawning(bool isSpawning){
		foreach(Spawner spawnerScript in spawnersScripts){
			spawnerScript.setIsSpawning (isSpawning);
		}
	}

	public int getDifficultyLevel(){
		return difficultyLevel;
	}

	public void setDifficultyLevel(int difficultyLevel){
		this.difficultyLevel = difficultyLevel;
		changeToLevel(this.difficultyLevel);
	}
}
