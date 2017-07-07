using UnityEngine;
using System.Collections;
using Playmove;

public class MenuPlayerLogInit : MonoBehaviour {

	public NamesRegisterButton nameRegisterButtonPlayer1;
	public GameObject[] objectsToHideSinglePlayer;

	void Start () {
		if (PlaytableApiContainer.getNumberOfActivePlayers () == 1) {
			nameRegisterButtonPlayer1.ClickSimulation ();
			foreach (GameObject objectToHide in objectsToHideSinglePlayer) {
				objectToHide.SetActive (false);
			}
		}
	}
}
