using UnityEngine;
using System.Collections;
using Playmove;

public class ReverseOnActualPlayerNumbers : MonoBehaviour {

	public int[] actualPlayerNumbers;

	void Start () {
		foreach (int player in actualPlayerNumbers) {
			if (player == PlaytableApiContainer.getActualPlayerIndex ()) {
				InvertScreen.Instance.Rotate (false);
			}
		}
	}

	public void Reverse(){
		foreach (int player in actualPlayerNumbers) {
			if (player == PlaytableApiContainer.getActualPlayerIndex ()) {
				InvertScreen.Instance.Rotate (false);
			}
		}
	}
}
