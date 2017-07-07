using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

	static AudioManager audioManager;
	public static Dictionary<string, AudioSource> audioSounds;
	Dictionary<string, float> backgroundMusicStartingVolume;
	static float musicVolume = 1f;
	static float effectsVolume = 1f;
	static bool useVoice = true;
	static bool useSound = true;
	static bool useMusic = true;

	void Awake () {
		if(audioManager == null){
			DontDestroyOnLoad(this);
			audioManager = this;
		}else{
			Destroy(gameObject);
		}
		if (audioSounds == null)
		{
			audioSounds = new Dictionary<string, AudioSource>();
			backgroundMusicStartingVolume = new Dictionary<string, float>();

			//\\string asterere = "";

			AudioSource[] sounds = gameObject.GetComponentsInChildren<AudioSource>();
			for (int i = 0; i < sounds.Length; i++)
			{
				if(sounds[i].tag == Tags.backgroundMusicObject){
					//sounds[i].ignoreListenerVolume = true;
					backgroundMusicStartingVolume[sounds[i].name] = sounds[i].volume;
				}
				audioSounds[sounds[i].gameObject.name] = sounds[i];


				//\\asterere += "\n"+sounds [i].gameObject.name;
				//DontDestroyOnLoad(sounds[i]);
			}
			//\\Debug.Log (asterere);
		}
	}

	public void SetEffectsVolume(float volume){
		volume = Mathf.Clamp(volume, 0.0f, 1.0f);
		AudioListener.volume = volume;
		effectsVolume = volume;
	}

	public void SetMusicVolume(float volume){
		volume = Mathf.Clamp(volume, 0.0f, 1.0f);
		foreach(AudioSource soundToCheck in audioSounds.Values){
			if(soundToCheck.tag == Tags.backgroundMusicObject){
				soundToCheck.volume = backgroundMusicStartingVolume[soundToCheck.name] * volume;
			}
		}
		musicVolume = volume;
	}

	public static void StopAllSounds(){
		foreach(AudioSource soundToCheck in audioSounds.Values){
			if(soundToCheck.tag != Tags.backgroundMusicObject){
				soundToCheck.Stop();
			}
		}
	}

	public static void StopAllMusic(){
		foreach(AudioSource soundToCheck in audioSounds.Values){
			if(soundToCheck.tag == Tags.backgroundMusicObject){
				soundToCheck.Stop();
			}
		}
	}

	public static void setUseVoice(bool useVoiceNew){
		useVoice = useVoiceNew;
		if(!useVoice){
			foreach(AudioSource soundToCheck in audioSounds.Values){
				if(soundToCheck.tag == Tags.voiceSoundObject){
					soundToCheck.Stop();
				}
			}
		}
	}

	public static void setUseSound(bool useSoundNew){
		useSound = useSoundNew;
		if(!useSound){
			foreach(AudioSource soundToCheck in audioSounds.Values){
				if(soundToCheck.tag != Tags.backgroundMusicObject && soundToCheck.tag != Tags.voiceSoundObject){
					soundToCheck.Stop();
				}
			}
		}
	}

	public static void setUseMusic(bool useMusicNew){
		useMusic = useMusicNew;
		if(!useMusic){
			foreach(AudioSource soundToCheck in audioSounds.Values){
				if(soundToCheck.tag == Tags.backgroundMusicObject){
					soundToCheck.Stop();
				}
			}
		}
	}

	public static bool getUseMusic(){
		return useMusic;
	}
	public static bool getUseSound(){
		return useSound;
	}

	public static bool getUseVoice(){
		return useVoice;
	}

	public static void PlaySound(string soundName, Vector3 positionToPlay){
		if (audioSounds [soundName].tag == Tags.voiceSoundObject && useVoice) {
			audioSounds [soundName].transform.position = positionToPlay;
			audioSounds [soundName].PlayOneShot (audioSounds [soundName].clip);
		}
		if (audioSounds [soundName].tag != Tags.voiceSoundObject && useSound) {
			audioSounds [soundName].transform.position = positionToPlay;
			audioSounds [soundName].PlayOneShot (audioSounds [soundName].clip);
		}

	}

	public static void PlayLoopSound(string soundName, Vector3 positionToPlay){
		if (audioSounds [soundName].tag != Tags.voiceSoundObject || useVoice) {
			audioSounds [soundName].transform.position = positionToPlay;
			audioSounds [soundName].Play ();
		}
	}

	public static void PlayMusic(string musicName, Vector3 positionToPlay){
		if (useMusic) {
			audioSounds [musicName].transform.position = positionToPlay;
			audioSounds [musicName].Play ();
		}
	}

	public static GameObject CopySound(string soundName, Transform parentTransform){
		GameObject soundCopiedObject = (GameObject) Instantiate(audioSounds[soundName].gameObject);
		soundCopiedObject.transform.parent = parentTransform;
		soundCopiedObject.transform.localPosition = Vector3.zero;

		return soundCopiedObject;
	}

	public static float getMusicVolume(){
		return musicVolume;
	}

	public static float getEffectsVolume(){
		return effectsVolume;
	}

	public static bool getIsPlaying(string soundName){
		return audioSounds[soundName].isPlaying;
	}

}
