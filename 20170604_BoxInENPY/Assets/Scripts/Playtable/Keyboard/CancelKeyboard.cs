using UnityEngine;
using System.Collections;
using Playmove;

public class CancelKeyboard : MonoBehaviour {
	public PlayTableKeyboard keyboard;
	public goToScene goToSceneNextPlayerLogin;
	public goToScene goToSceneLastPlayerLogged;
	public goToScene goToSceneSinglePlayer;
	public float timeToGoToScene = 1.5f;
	float timeToGoToScenePassed = 0f;
	bool closing = false;

	LoginText loginText;

	void LateUpdate(){
		if (closing) {
			if (timeToGoToScenePassed < timeToGoToScene) {
				timeToGoToScenePassed += Time.deltaTime;
			} else {
				CloseFunction ();
				closing = false;
				timeToGoToScenePassed = 0f;
			}
		}
	}

	public void CancelTheKeyboard () {

		GameObject loginTextObject = GameObject.FindGameObjectWithTag (Tags.loginText);

		if(loginTextObject != null){
			loginText = loginTextObject.GetComponent<LoginText>();
		}

		if(PlaytableApiContainer.getStartedScore()){
			//AudioManager.setUseVoice (true);
			PlaytableApiContainer.setPlayerName (PlaytableApiContainer.getActualPlayerIndex(), "");
			if (loginText != null) {
				loginText.GetNameFromApiContainer ();
			}
			keyboard.Close ();
			closing = true;
				
		}

	}

	void CloseFunction(){
		if(PlaytableApiContainer.getStartedScore()){
			
			if (PlaytableApiContainer.getNumberOfActivePlayers() > PlaytableApiContainer.getNumberOfLoggedPlayers()) {
				goToSceneNextPlayerLogin.selectScene ();
				if (1 == PlaytableApiContainer.getActualPlayerIndex ()) {
					InvertScreen.Instance.Rotate (false);
				}
			} else {				
				if (PlaytableApiContainer.getNumberOfActivePlayers () > 1) {
					goToSceneLastPlayerLogged.selectScene ();
					if (1 == PlaytableApiContainer.getActualPlayerIndex ()) {
						InvertScreen.Instance.Rotate (false);
					}
					PlaytableApiContainer.setActualPlayerIndex(0);
				} else {
					PlaytableApiContainer.setActualPlayerIndex(0);
					goToSceneSinglePlayer.selectScene ();
				}
			}	

		}
	}

}
