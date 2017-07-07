using UnityEngine;
using System.Collections;

public class VolumePersistence : MonoBehaviour {

	const string musicVolumeKeyToPrefs = "MusicVolume";
	private const string effectsVolumeKeyToPrefs = "EffectsVolume";

	public void Save(){
		PlayerPrefs.SetFloat(musicVolumeKeyToPrefs, AudioManager.getMusicVolume());
		PlayerPrefs.SetFloat(effectsVolumeKeyToPrefs, AudioManager.getEffectsVolume());
	}
}
