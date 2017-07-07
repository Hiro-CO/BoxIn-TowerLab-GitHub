using UnityEngine;
using System.Collections;

public class RandomMusicPlayChangeOnEnd : MonoBehaviour {

	public MusicNames[] musicToPlay;
	MusicNames nowPlaying;
	public bool playOnStart = true;

	void Start () {
		if(playOnStart){
			PlayTheMusic();
		}
	}

	void Update(){
		if(!AudioManager.getIsPlaying(nowPlaying.ToString())){
			PlayTheMusic();
		}
	}
	
	public void PlayTheMusic () {
		AudioManager.StopAllMusic();
		nowPlaying = musicToPlay[Random.Range(0, musicToPlay.Length)];
		AudioManager.PlayMusic(nowPlaying.ToString(), transform.position);
	}
}
