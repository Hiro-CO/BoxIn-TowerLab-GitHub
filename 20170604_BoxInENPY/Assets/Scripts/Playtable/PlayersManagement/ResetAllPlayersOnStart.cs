using UnityEngine;
using System.Collections;

public class ResetAllPlayersOnStart : MonoBehaviour {

	void Start(){
		PlaytableApiContainer.ResetAllPlayers ();
	}
}
