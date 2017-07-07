using UnityEngine;
using System.Collections;

public class UnpauseEventSlide : UnpauseEvents {

	public MoveRect[] moveRects;

	public override void ExecuteEvents () {
		foreach(MoveRect moveRect in moveRects){
			moveRect.GoToStart();
		}
	}
}
