using UnityEngine;
using System.Collections;

public class MoveRectStartSlideOther : MoveRectReachStartEvent {

	public MoveRect[] moveToStart;
	public MoveRect[] moveToEnd;

	public override void ExecuteEvents(){
		foreach(MoveRect moveRect in moveToStart){
			moveRect.GoToStart();
		}
		foreach(MoveRect moveRect in moveToEnd){
			moveRect.GoToEnd();
		}
	}
}
