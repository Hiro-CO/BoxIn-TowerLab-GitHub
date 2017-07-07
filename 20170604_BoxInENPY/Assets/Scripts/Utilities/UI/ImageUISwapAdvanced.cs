using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageUISwapAdvanced : MonoBehaviour {

	public Image imageUI;
	public Sprite[] spritesToSwap;
	public float timeToSwap;
	float timeToSwapPassed = 0f;
	public bool repeatSpritesOnARow = false;
	int lastIndex = 999;
	int actualIndex = 999;
	int[] numbersToChoose;
	bool isSwapping = false;

	void Awake(){
		numbersToChoose = new int[spritesToSwap.Length - 1];
	}

	void Update () {
		if (isSwapping) {
			if (timeToSwap > timeToSwapPassed) {
				timeToSwapPassed += Time.deltaTime;
			} else {
				DoTheSwap ();
			}
		}
	}

	public void StartSwapping(){
		isSwapping = true;
	}

	public void EndSwapping(){
		isSwapping = false;
		timeToSwapPassed = 0f;
	}

	void DoTheSwap(){
		if (repeatSpritesOnARow) {
			actualIndex = Random.Range (0, spritesToSwap.Length);
		} else {
			for(int i=0; i<numbersToChoose.Length; i++){
				if (i != lastIndex) {
					numbersToChoose [i] = i;
				} else {
					numbersToChoose [i] = spritesToSwap.Length - 1;
				}

				actualIndex = numbersToChoose [Random.Range (0, numbersToChoose.Length)];
			}
		}

		lastIndex = actualIndex;
		imageUI.sprite = spritesToSwap [actualIndex];

		timeToSwapPassed = 0f;
	}
}

