using UnityEngine;
using System.Collections;

public class MusicMuteChange : MonoBehaviour {

	public ImageUISpriteSwap imageUISpriteSwap;

	void Start(){
		Changed ();
	}

	public void Changed(){
		if(AudioManager.getMusicVolume() > 0f){
			imageUISpriteSwap.SwapToSprite(1);
		}else{
			imageUISpriteSwap.SwapToSprite(0);
		}
	}
}
