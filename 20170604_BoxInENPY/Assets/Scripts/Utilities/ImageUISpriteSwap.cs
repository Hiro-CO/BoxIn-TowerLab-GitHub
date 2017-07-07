using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageUISpriteSwap : MonoBehaviour {

	public Sprite[] sprites;
	Image image;
	
	void Awake () {
		image = GetComponent<Image>();
	}

	public void SwapToSprite (int spriteIndex) {
		if(image == null){
			image = GetComponent<Image>();
		}

		if(spriteIndex < sprites.Length){
			image.sprite = sprites[spriteIndex];
		}
	}
}
