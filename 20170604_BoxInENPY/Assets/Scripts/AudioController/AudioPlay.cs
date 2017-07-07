using UnityEngine;
using System.Collections;

public class AudioPlay : MonoBehaviour {

	public AudioNames audioToPlay;

	public void PlayTheAudio () {
		AudioManager.PlaySound(audioToPlay.ToString(), transform.position);
	}
}
