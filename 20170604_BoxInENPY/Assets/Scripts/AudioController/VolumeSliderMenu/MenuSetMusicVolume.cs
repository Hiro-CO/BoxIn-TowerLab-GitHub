using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuSetMusicVolume : MonoBehaviour {

	AudioManager audioManager;
	Slider slider;
	
	// Use this for initialization
	void Awake () {
		audioManager = GameObject.FindGameObjectWithTag(Tags.audioController).GetComponent<AudioManager>();
		slider = GetComponent<Slider>();
		slider.value = AudioManager.getMusicVolume() * slider.maxValue;
	}
	
	public void SetMusicVolume(){
		if(slider != null && audioManager != null){
			audioManager.SetMusicVolume(slider.value/slider.maxValue);
		}
	}
}
