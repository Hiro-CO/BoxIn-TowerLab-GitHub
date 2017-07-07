using UnityEngine;
using System.Collections;

public class MusicPlay : MonoBehaviour {

	public MusicNames musicToPlay;
	public bool playOnStart = true;

	void Start () {
		if(playOnStart){
			PlayTheMusic();
		}
	}
	
	public void PlayTheMusic () {
		AudioManager.StopAllMusic();
		AudioManager.PlayMusic(musicToPlay.ToString(), transform.position);
	}
}
