using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public int spawnerAppearsOnLevel = 0;
	//public ObjectsPooler cloudPooler;
	public ObjectsPooler[] poolersArray;
	public float minimumSpawnTimeStart = 6f;
	public float maximumSpawnTimeStart = 7f;
	public float minimumSpawnTime = 1f;
	public float maximumSpawnTime = 2f;
	public float[] minimumX;
	public float[] maximumX;
	public static int maximumIndex = 0;
	public float delayToSpawnFirstObject = 0f;
	private float delayPassed = 0f;
	bool firstSpawn = true;
	public bool useMaximumIndex = true;
	bool isSpawning = true;
	float nextObjectTime = 1f;
	float nextObjectTimePassed = 0f;

	void Awake(){
		maximumIndex = 0;
	}

	void Update(){
		if (firstSpawn) {
			if (delayPassed >= delayToSpawnFirstObject) {
				Spawn ();
				firstSpawn = false;
			} else {
				delayPassed += Time.deltaTime;
			}
		} else {
			if (nextObjectTimePassed < nextObjectTime) {
				nextObjectTimePassed += Time.deltaTime;
			} else {
				Spawn ();
			}
		}
	}

	
	void Spawn () {
		if (gameObject.activeSelf && isSpawning) {			
			//The max value in Random.Range is EXCLUSIVE, so you have to put maximumNumber+1 in max.
			//(here its objectsArray.Lenght instead of objectsArray.Length-1)
			//(and maximumIndex+1 instead of maximumIndex)
			//The min value is INCLUSIVE, so its the exact minimum number
			//Instantiate(objectsArray[Random.Range(0, objectsArray.Length)], transform.position, Quaternion.identity);
			int rngeezuz = Random.Range (0, minimumX.Length);
			Vector3 newPos = transform.position;
			newPos.x = Random.Range (minimumX [rngeezuz], maximumX [rngeezuz]);
			if (maximumIndex < poolersArray.Length && PlaytableApiContainer.getNumberOfActivePlayers () == 1 && useMaximumIndex) {
				poolersArray [maximumIndex].UsePooledObject (newPos, Quaternion.identity);
			} else {
				poolersArray [Random.Range (0, poolersArray.Length)].UsePooledObject (newPos, Quaternion.identity);
			}

			if (maximumIndex < poolersArray.Length && PlaytableApiContainer.getNumberOfActivePlayers () == 1 && useMaximumIndex) {
				nextObjectTimePassed = 0f;
				nextObjectTime = Random.Range (minimumSpawnTimeStart, maximumSpawnTimeStart);
			} else {
				nextObjectTimePassed = 0f;
				nextObjectTime = Random.Range (minimumSpawnTime, maximumSpawnTime);
			}
		}
	}

	public void setIsSpawning(bool isSpawning){
		this.isSpawning = isSpawning;
	}
}
