using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginText : MonoBehaviour {
	public Text text;
	public bool startWithName = false;

	void Awake(){
		if (startWithName) {
			GetNameFromApiContainer ();
		}
	}

	public void GetNameFromApiContainer(){
		text.text = PlaytableApiContainer.getPlayerName(0);
	}
}
