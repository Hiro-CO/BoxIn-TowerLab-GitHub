using UnityEngine;
using System.Collections;

public class MoveRectEndSlideOtherAndSetObjects : MoveRectReachEndEvent {

	public MoveRect[] moveToStart;
	public MoveRect[] moveToEnd;
	public GameObject[] objectsToShow;
	public GameObject[] objectsToHide;

	public override void ExecuteEvents(){
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
	}
}
