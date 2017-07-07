using UnityEngine;
using System.Collections;

public class PauseAndUnpause : MonoBehaviour {

	bool paused = false;
	
	public void Pause(){
		Time.timeScale = 0f;
		paused = true;
	}

	public void Unpause(){
		Time.timeScale = 1f;
		paused = false;
	}

	public void togglePause(){
		if(paused){
			Unpause();
		}else{
			Pause();
		}
	}


	public bool getPaused(){
		return paused;
	}
}
