using UnityEngine;
using System.Collections;

public class ColorsSplashes : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public float timeToStartFading = 7f;
	float timeToStartFadingPassed = 0f;
	bool fadingStarted = false;
	public float fadingSpeed = 1f;
	float startingAlpha = 0f;
	public AudioNames[] splashesSounds;


	void Awake(){
		startingAlpha = spriteRenderer.color.a;
	}

	void OnEnable(){
		Color newColor = spriteRenderer.color;
		newColor.a = startingAlpha;
		spriteRenderer.color = newColor;
		AudioManager.PlaySound (splashesSounds[Random.Range (0, splashesSounds.Length)].ToString (), Vector3.zero);
	}
	
	void Update () {
		if (!fadingStarted) {
			if (timeToStartFadingPassed < timeToStartFading) {
				timeToStartFadingPassed += Time.deltaTime;
			} else {
				fadingStarted = true;
			}
		} else {
			if (spriteRenderer.color.a > 0f) {
				Color newColor = spriteRenderer.color;
				newColor.a = Mathf.Max(newColor.a - (fadingSpeed * Time.deltaTime), 0f);
				spriteRenderer.color = newColor;
			} else {
				fadingStarted = false;
				timeToStartFadingPassed = 0f;

				gameObject.SetActive (false);
			}
		}
	}


}