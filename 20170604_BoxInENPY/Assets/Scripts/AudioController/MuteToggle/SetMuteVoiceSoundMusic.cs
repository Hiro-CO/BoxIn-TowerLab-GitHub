using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetMuteVoiceSoundMusic : MonoBehaviour {

	public Toggle toggle;
	public SoundType soundType;

	void Start(){
		RefreshVolume();
	}

	public void RefreshVolume(){
		if (soundType == SoundType.music) {
			toggle.isOn = AudioManager.getUseMusic ();
		}
		if (soundType == SoundType.sound) {
			toggle.isOn = AudioManager.getUseSound ();
		}
		if (soundType == SoundType.voice) {
			toggle.isOn = AudioManager.getUseVoice ();
		}
	}

	void ChangeUseVoice(bool useVoice){
		AudioManager.setUseVoice(useVoice);
	}
	public void ChangedToggleVoice(){
		ChangeUseVoice (toggle.isOn);
	}

	void ChangeUseSound(bool useSound){
		AudioManager.setUseSound(useSound);
	}
	public void ChangedToggleSound(){
		ChangeUseSound (toggle.isOn);
	}

	void ChangeUseMusic(bool useMusic){
		AudioManager.setUseMusic(useMusic);
	}
	public void ChangedToggleMusic(){
		ChangeUseMusic (toggle.isOn);
	}
}

public enum SoundType{
	music,
	sound,
	voice
}