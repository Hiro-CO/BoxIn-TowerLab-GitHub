using UnityEngine;
using System.Collections;

public class BoxEnableDisable : MonoBehaviour {
	
	bool isEnabled = true;
	public Collider2D colliderFrente;
	public SpriteRenderer spriteRenderer;
	public SpriteRenderer spriteRendererFrente;
	Color enabledColor;
	public Color disabledColor;
	public GameObject textObject;

	void Awake(){
		enabledColor = spriteRenderer.color;
	}

	public void Disable(){
		colliderFrente.enabled = false;
		spriteRenderer.color = disabledColor;
		spriteRendererFrente.color = disabledColor;
		textObject.SetActive (false);
		isEnabled = false;
	}

	public void Enable(){
		colliderFrente.enabled = true;
		spriteRenderer.color = enabledColor;
		spriteRendererFrente.color = enabledColor;
		textObject.SetActive (true);
		isEnabled = true;
	}

	public bool getIsEnabled(){
		return isEnabled;
	}
}
