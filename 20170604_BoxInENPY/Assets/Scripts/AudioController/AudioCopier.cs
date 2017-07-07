using UnityEngine;
using System.Collections;

public class AudioCopier : MonoBehaviour {

	public AudioNames audioToCopy;
	public bool playOnStart = true;
	GameObject audioCopiedObject;
	AudioSource audioSourceCopied;

	void Start(){
		if(playOnStart){
			PlaySound();
		}
	}

	public void MakeSoundCopy(){
		audioCopiedObject = AudioManager.CopySound(audioToCopy.ToString(), transform);
		audioSourceCopied = audioCopiedObject.GetComponent<AudioSource>();
	}

	public void PlaySound(){
		if(audioCopiedObject != null && audioSourceCopied != null){
			if(!audioSourceCopied.isPlaying){
				audioSourceCopied.Play();
			}
		}else{
			MakeSoundCopy();
			PlaySound();
		}
	}

	public void StopSound(){
		if(audioCopiedObject != null && audioSourceCopied != null){
			audioSourceCopied.Stop();
		}else{
			MakeSoundCopy();
		}
	}

	public GameObject getAudioCopiedObject(){
		return audioCopiedObject;
	}
}
