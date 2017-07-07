using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeSlider : MonoBehaviour {

	Slider slider;
	public enum EffectsOrMusic{effects, music};
	public EffectsOrMusic effectsOrMusic;

	void OnEnable() {
		slider = GetComponent<Slider>();

		RefreshVolume();
	}

	public void RefreshVolume(){
		if(effectsOrMusic == EffectsOrMusic.effects){
			slider.value = AudioManager.getEffectsVolume() * 1;
		}
		
		if(effectsOrMusic == EffectsOrMusic.music){
			slider.value = AudioManager.getMusicVolume() * 1;
		}
	}


}
