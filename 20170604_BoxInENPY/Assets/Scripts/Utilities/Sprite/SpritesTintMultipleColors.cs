using UnityEngine;
using System.Collections;

public class SpritesTintMultipleColors : MonoBehaviour {

	public Color[] colorsToTint;
	public SpriteRenderer[] spritesRenderers;
	public bool tintOnStart = false;
	public int firstTintIndex = 0;
	public bool randomTint = false;
	Color[] startColor;

	void Awake () {
		startColor = new Color[spritesRenderers.Length];
		for(int i=0; i<spritesRenderers.Length; i++){
			startColor[i] = spritesRenderers[i].color;
		}
		if (tintOnStart) {
			if (randomTint) {
				StartTintRandom ();
			} else {
				StartTintSelectedColor (firstTintIndex);
			}
		}
	}

	void OnEnable(){
		if (tintOnStart) {
			if (randomTint) {
				StartTintRandom ();
			} else {
				StartTintSelectedColor (firstTintIndex);
			}
		} else {
			EndTint ();
		}
	}
	
	public void StartTintRandom () {
		for(int i=0; i<spritesRenderers.Length; i++){
			spritesRenderers[i].color = colorsToTint[Random.Range(0, colorsToTint.Length)];
		}
	}
	public void StartTintSelectedColor (int index) {
		for(int i=0; i<spritesRenderers.Length; i++){
			if (index > colorsToTint.Length) {
				spritesRenderers [i].color = colorsToTint [index];
			}
		}
	}
	public void EndTint () {
		for(int i=0; i<spritesRenderers.Length; i++){
			spritesRenderers[i].color = startColor[i];
		}
	}

}
