using UnityEngine;
using System.Collections;

public class RandomOneMusicPlay : MonoBehaviour {

	public MusicNames[] musicToPlay;
	MusicNames nowPlaying;
	public bool playOnStart = true;

	void Start () {
		if(playOnStart){
			PlayTheMusic();
		}
	}
	
	public void PlayTheMusic () {
		AudioManager.StopAllMusic();
		nowPlaying = musicToPlay[Random.Range(0, musicToPlay.Length)];
		AudioManager.PlayMusic(nowPlaying.ToString(), transform.position);
	}
}
