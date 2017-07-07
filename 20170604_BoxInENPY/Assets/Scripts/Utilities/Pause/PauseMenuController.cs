using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour {

	public PauseAndUnpause pauseAndUnpause;
	public PauseEvents pauseEvents;
	public UnpauseEvents unpauseEvents;

	public void Pause () {
		pauseAndUnpause.Pause();

		if(pauseEvents != null){pauseEvents.ExecuteEvents();}
	}

	public void Unpause(){
		pauseAndUnpause.Unpause();

		if(unpauseEvents != null){unpauseEvents.ExecuteEvents();}
	}
}
