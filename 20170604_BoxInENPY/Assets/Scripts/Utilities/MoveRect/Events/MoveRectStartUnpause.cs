using UnityEngine;
using System.Collections;

public class MoveRectStartUnpause : MoveRectReachStartEvent {

	public PauseMenuController pause;

	public override void ExecuteEvents(){
		pause.Unpause();
	}
}
