﻿using UnityEngine;
using System.Collections;

public class MoveRectEndSlideOtherAndSetObjectsWithTimer : MoveRectReachEndEvent {

	public MoveRect[] moveToStart;
	public MoveRect[] moveToEnd;
	public GameObject[] objectsToShow;
	public GameObject[] objectsToHide;
	public float timeToExecute = 1f;
	float timeToExecutePassed = 0f;
	bool countingTime = false;

	void Update(){
		if (countingTime) {
			if (timeToExecutePassed < timeToExecute) {
				timeToExecutePassed += Time.deltaTime;
			} else {
				foreach(MoveRect moveRect in moveToStart){
					moveRect.GoToStart();
				}
				foreach(MoveRect moveRect in moveToEnd){
					moveRect.GoToEnd();
				}
				foreach(GameObject objectToShow in objectsToShow){
					objectToShow.SetActive(true);
				}
				foreach(GameObject objectToHide in objectsToHide){
					objectToHide.SetActive(false);
				}

				countingTime = false;
			}
		}
	}

	public override void ExecuteEvents(){
		countingTime = true;
		timeToExecutePassed = 0f;
	}
}
