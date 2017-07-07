using UnityEngine;
using System.Collections;

public class RandomSprite : MonoBehaviour {

	public Sprite[] spritesToRandomize;
	SpriteRenderer spriteRenderer;
	
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = spritesToRandomize[Random.Range(0, spritesToRandomize.Length)];
	}

	void OnEnable () {
		if (spriteRenderer == null) {
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}
		spriteRenderer.sprite = spritesToRandomize[Random.Range(0, spritesToRandomize.Length)];
	}
}
