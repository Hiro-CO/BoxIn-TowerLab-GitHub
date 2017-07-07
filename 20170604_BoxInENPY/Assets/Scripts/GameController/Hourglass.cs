using UnityEngine;

using System.Collections;

public class Hourglass : MonoBehaviour {
	//public MoveRect moveRectSandDown;
	//public MoveRect moveRectSandUp;
	public MoveRect moveRectSandLine;
	//MoveRects Speeds
	//time: 95
	//Line: 0.5 
	//Up: 0.0057 
	//Down: 0.011


	void Awake(){
		//moveRectSandDown.GoToEnd ();
		//moveRectSandUp.GoToEnd ();
		moveRectSandLine.GoToEnd ();
	}

}
