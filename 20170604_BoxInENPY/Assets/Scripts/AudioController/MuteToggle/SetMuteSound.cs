using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetMuteSound : MonoBehaviour {

	public enum EffectsOrMusicOrAll{effects, music, all};
	public EffectsOrMusicOrAll effectsMusicOrAll;
	AudioManager audioManager;
	Toggle toggle;
	
	void Awake () {
		audioManager = GameObject.FindGameObjectWithTag(Tags.audioController).GetComponent<AudioManager>();
		toggle = GetComponent<Toggle>();
	}

	void Start(){
		RefreshVolume();
	}

	public void RefreshVolume(){
		if(effectsMusicOrAll == EffectsOrMusicOrAll.effects){
			toggle.isOn = AudioManager.getEffectsVolume() > 0f;
		}
		if(effectsMusicOrAll == EffectsOrMusicOrAll.music){
			toggle.isOn = AudioManager.getMusicVolume() > 0f;
		}
		if(effectsMusicOrAll == EffectsOrMusicOrAll.all){
			toggle.isOn = AudioManager.getEffectsVolume() > 0f || AudioManager.getMusicVolume() > 0f;
		}
	}


	void MuteTheEffects(){
		if(audioManager != null){
			audioManager.SetEffectsVolume(0f);
		}
	}	
	void UnMuteTheEffects(){
		if(audioManager != null){
			audioManager.SetEffectsVolume(1f);
		}
	}
	public void ChangedToggleEffects(){
		if(toggle.isOn){
			UnMuteTheEffects();
		}else{
			MuteTheEffects();
		}
	}


	void MuteTheMusic(){
		if(audioManager != null){
			audioManager.SetMusicVolume(0f);
		}
	}	
	void UnMuteTheMusic(){
		if(audioManager != null){
			audioManager.SetMusicVolume(1f);
		}
	}
	public void ChangedToggleMusic(){
		if(toggle.isOn){
			UnMuteTheMusic();
		}else{
			MuteTheMusic();
		}
	}


	void MuteAllSounds(){
		MuteTheMusic();
		MuteTheEffects();
	}
	void UnMuteAllSounds(){
		UnMuteTheMusic();
		UnMuteTheEffects();
	}
	public void ChangedToggleAllSounds(){
		if(toggle.isOn){
			UnMuteAllSounds();
		}else{
			MuteAllSounds();
		}
	}
}
