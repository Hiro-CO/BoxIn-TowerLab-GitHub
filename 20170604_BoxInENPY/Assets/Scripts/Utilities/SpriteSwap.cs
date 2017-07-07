using UnityEngine;
using System.Collections;

public class SpriteSwap : MonoBehaviour {

	public Sprite[] sprites;
	SpriteRenderer spriteRenderer;
	
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SwapToSprite (int spriteIndex) {
		if (spriteRenderer == null) {
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		if(spriteIndex < sprites.Length){
			spriteRenderer.sprite = sprites[spriteIndex];
		}
	}
}
