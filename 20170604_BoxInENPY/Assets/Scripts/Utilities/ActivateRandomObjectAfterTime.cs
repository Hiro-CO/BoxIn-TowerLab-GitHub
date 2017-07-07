using UnityEngine;
using System.Collections;

public class ActivateRandomObjectAfterTime : MonoBehaviour {

	public GameObject[] objectsToRandom;
	public float timeToShow = 1f;
	public bool startTimerOnAwake = true;
	float timePassed = 0f;
	bool countTime = false;

	void Awake () {
		StartTimer();
	}
	
	void Update () {
		if(countTime){
			if(timePassed < timeToShow){
				timePassed += Time.deltaTime;
			}else{
				objectsToRandom[Random.Range(0, objectsToRandom.Length)].SetActive(true);
				countTime = false;
				timePassed = 0f;
			}
		}
	}

	public void StartTimer(){
		countTime = true;
	}
}
