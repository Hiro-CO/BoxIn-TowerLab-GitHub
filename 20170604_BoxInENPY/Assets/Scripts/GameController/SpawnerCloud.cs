using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerCloud : MonoBehaviour {


	public static bool multiSpawning = false;
	public int cloudIndex = 0;
	EnemyData enemyData;
	public float timeToSpawn = 1f;
	float timeToSpawnPassed = 0f;
	bool spawned = false;
	public float timeToDisable = 1.5f;
	public float timeToDisableMultispawn = 4f;
	float timeToDisableThisTurn = 1.5f;
	float timeToDisablePassed = 0f;
	public bool useMultiSpawn = false;
	public int multiSpawnPercentage = 10;
	bool multiSpawnThisTurn = false;
	public int multiSpawnNumber = 3;
	int multiSpawnUsed = 0;
	public float multiSpawnTime = 0.5f;
	public float multiSpawnPositionX = 0.2f;
	public Vector3 multiSpawnScale = new Vector3(0.5f,0.5f,0.5f);
	float multiSpawnTimePassed = 0f;
	public EnemyPooler poolerToUse;
	public List<GameObject> clouds;
	List<GameObject> cloudsToUse = new List<GameObject>();

	void OnLevelWasLoaded(){
		multiSpawning = false;
	}

	void Start(){
		if (EnemyDataManager.control.getChosenEnemies ().Count <= cloudIndex) {
			gameObject.SetActive (false);
		} else {
			if (enemyData == null) {
				enemyData = EnemyDataManager.control.getChosenEnemies () [cloudIndex];

			 	timeToSpawn = enemyData.timeToSpawn;
				timeToDisable = enemyData.timeToDisable;
				timeToDisableMultispawn = enemyData.timeToDisableMultispawn;
				useMultiSpawn = enemyData.useMultiSpawn;
				multiSpawnPercentage = enemyData.multiSpawnPercentage;
				multiSpawnNumber = enemyData.multiSpawnNumber;
				multiSpawnTime = enemyData.multiSpawnTime;
				multiSpawnPositionX = enemyData.multiSpawnPositionX;
				multiSpawnScale = enemyData.multiSpawnScale;
			}
		}
	}

	void OnEnable(){
		timeToSpawnPassed = 0f;
		//timeToDisablePassed = 0f;
		multiSpawnTimePassed = 0f;
		multiSpawnUsed = 0;
		if (useMultiSpawn) {
			multiSpawnThisTurn = Random.Range (0, 100) < multiSpawnPercentage;
		} else {
			multiSpawnThisTurn = false;
		}

		if (multiSpawning) {
			multiSpawnThisTurn = false;
		}

		if (multiSpawnThisTurn) {
			multiSpawning = true;
			timeToDisableThisTurn = timeToDisableMultispawn;
		} else {
			timeToDisableThisTurn = timeToDisable;
		}
		spawned = false;


		cloudsToUse.Clear ();
		foreach (GameObject cloud in clouds) {
			cloud.SetActive (false);
			cloudsToUse.Add (cloud);
		}
		StartACloud ();
	}

	void Update () {
		if (timeToSpawnPassed < timeToSpawn) {
			timeToSpawnPassed += Time.deltaTime;
		} else {
			if (!spawned) {
				SpawnObject ();
			} 
			if (multiSpawnThisTurn && spawned) {
				if (multiSpawnUsed < multiSpawnNumber-1) {
					if (multiSpawnTimePassed < multiSpawnTime) {
						multiSpawnTimePassed += Time.deltaTime;
					} else {
						SpawnObject ();
						multiSpawnTimePassed = 0f;
						multiSpawnUsed++;
					}
				}
			}
		}

		if (timeToDisablePassed < timeToDisableThisTurn) {
			timeToDisablePassed += Time.deltaTime;
		} else {
			Disable ();
		}
	}

	void SpawnObject(){
		if (!spawned && !multiSpawnThisTurn) {
			poolerToUse.UsePooledObject (transform.position, Quaternion.identity);
			spawned = true;
		}

		if (multiSpawnThisTurn) {
			if (spawned) {
				
				Vector3 spawnPosition = transform.position;
				if (multiSpawnUsed % 2 == 0) {
					spawnPosition.x += multiSpawnPositionX;
				} else {
					spawnPosition.x -= multiSpawnPositionX;
				}

				poolerToUse.UsePooledObject (spawnPosition, Quaternion.identity).transform.localScale = multiSpawnScale;

			} else {
				poolerToUse.UsePooledObject (transform.position, Quaternion.identity);
				spawned = true;
			}
		}
	}

	void Disable(){
		timeToDisablePassed = 0f;
		foreach(GameObject cloud in clouds){
			cloud.SetActive (false);
		}

		if (multiSpawnThisTurn) {
			multiSpawning = false;
		}

		gameObject.SetActive (false);
	}

	public void StartACloud(){
		if (cloudsToUse.Count > 0) {
			int rngeezuz = Random.Range (0, cloudsToUse.Count);
			GameObject cloudUsed = cloudsToUse [rngeezuz];
			Quaternion newRotation = Quaternion.Euler (new Vector3 (0f, 0f, Random.Range (0f, 360f)));
			cloudUsed.transform.rotation = newRotation;
			cloudUsed.SetActive (true);
			cloudsToUse.Remove (cloudUsed);
		} else {
			if (multiSpawnThisTurn) {
				foreach (GameObject cloud in clouds) {
					if (!cloud.activeSelf) {
						cloudsToUse.Add (cloud);
					}
				}
				StartACloud ();
			}
		}
	}

	public void AddCloudToUse(GameObject newCloud){
		cloudsToUse.Add (newCloud);
		if (cloudsToUse.Count == 1) {
			StartACloud ();
		}
	}
}