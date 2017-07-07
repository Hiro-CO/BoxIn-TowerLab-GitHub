using UnityEngine;
using System.Collections;
using Playmove;

public class CloseKeyboard : MonoBehaviour {
	public PlayTableKeyboard keyboard;

	public void CloseTheKeyboard(){
		keyboard.Close ();
	}
}
