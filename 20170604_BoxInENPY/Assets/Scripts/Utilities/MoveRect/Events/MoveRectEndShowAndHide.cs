using UnityEngine;
using System.Collections;

public class MoveRectEndShowAndHide : MoveRectReachEndEvent {

	public GameObject[] objectsToShow;
	public GameObject[] objectsToHide;

	public override void ExecuteEvents () {
		ShowObjects(objectsToShow);
		HideObjects(objectsToHide);
	}

	public void ShowObjects(GameObject[] objects){
		foreach(GameObject objectToShow in objects){
			objectToShow.SetActive(true);
		}
	}
	
	public void HideObjects(GameObject[] objects){
		foreach(GameObject objectToHide in objects){
			objectToHide.SetActive(false);
		}
	}
}
