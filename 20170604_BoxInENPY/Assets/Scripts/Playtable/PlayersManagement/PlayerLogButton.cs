using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerLogButton : MonoBehaviour {

	public int playerIndex = 0;
	public GameObject checkObject;
	public goToScene sceneToGo;

	public void Clicked(string playerName, int turmaIndex, GameObject objectToHide){
		if (!PlaytableApiContainer.getPlayerLogged (playerIndex)) {
			PlaytableApiContainer.setPlayerName (playerIndex, playerName);
			PlaytableApiContainer.setTurmaIndex (playerIndex, turmaIndex);
			checkObject.SetActive (true);
			objectToHide.SetActive (false);
		}
		if (PlaytableApiContainer.getNumberOfLoggedPlayers () >= PlaytableApiContainer.getNumberOfActivePlayers()) {
			PlaytableApiContainer.setActualPlayerIndex(0);
			sceneToGo.selectScene ();
		}
	}
}
