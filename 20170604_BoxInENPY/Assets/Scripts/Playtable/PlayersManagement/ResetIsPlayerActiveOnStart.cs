using UnityEngine;
using System.Collections;

public class ResetIsPlayerActiveOnStart : MonoBehaviour {
	void Start () {
		PlaytableApiContainer.resetAllIsPlayersActive ();
	}
}
