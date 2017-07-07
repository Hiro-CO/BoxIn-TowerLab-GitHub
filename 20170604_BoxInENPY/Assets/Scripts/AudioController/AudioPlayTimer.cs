using UnityEngine;
using System.Collections;

public class AudioPlayTimer : MonoBehaviour {

	public AudioPlay audioPlay;
	public float timeToPlay = 0.5f;
	float timeToPlayPassed = 0f;
	public bool playInLoop = false;
	bool played = false;


	void Update(){
		if (timeToPlayPassed < timeToPlay) {
			timeToPlayPassed += Time.deltaTime;
		} else {
			if (!played || playInLoop) {
				timeToPlayPassed = 0f;
				played = true;
				audioPlay.PlayTheAudio ();
			}
		}
	}
}
