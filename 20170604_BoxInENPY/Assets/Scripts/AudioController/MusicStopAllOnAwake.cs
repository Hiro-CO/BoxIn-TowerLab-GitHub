using UnityEngine;
using System.Collections;

public class MusicStopAllOnAwake : MonoBehaviour {

	void Awake () {
		AudioManager.StopAllMusic();
	}
}
