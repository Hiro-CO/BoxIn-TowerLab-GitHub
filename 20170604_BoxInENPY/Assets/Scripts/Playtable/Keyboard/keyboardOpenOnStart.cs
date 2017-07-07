using UnityEngine;
using System.Collections;
using Playmove;

public class keyboardOpenOnStart : MonoBehaviour {

	public PlayTableKeyboard keyboard;
	public string sceneToOpenKeyboard;
	bool firstStartOpened = false;

	void OnLevelWasLoaded(int level) {
		firstStartOpened = false;
	}


	void Update () {
		if (!firstStartOpened) {
			if (BannedWordsManager.BannedWords.Words.Count > 0) {
				keyboard.ClearText ();
				//keyboard.Display.Text = "";
				keyboard.Open ();
				firstStartOpened = true;
			}
		}
	}
}
