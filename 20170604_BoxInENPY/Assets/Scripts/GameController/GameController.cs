using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	private bool isPlayerAlive = true;
	public int playerStartingLife = 3;
	public int playerMaxLife = 5;
	public float timeToPlay = 120f;
	float timeToPlayPassed = 0f;
	float timerToShow;
	public float timeBeforeGameOver = 9f;
	public float timeBeforeMask = 8f;
	float timeBeforeGameOverPassed = 0f;
	public GameControllerDifficulty gameControllerDifficulty;
	bool didBeforeGameOver = false;
	bool didMask = false;
	//public Text textTimer;


	void Awake(){
		timerToShow = timeToPlay;
		timeBeforeGameOverPassed = 0f;
	}

	void Update(){
		if (timeToPlayPassed < timeToPlay) {
			timeToPlayPassed += Time.deltaTime;

			timerToShow = timeToPlay - timeToPlayPassed;
		} else {
			if (!didBeforeGameOver) {
				gameControllerDifficulty.setIsSpawning (false);
				didBeforeGameOver = true;
			}
			if (timeBeforeGameOverPassed > timeBeforeMask && !didMask) {
				didMask = true;
				PrefabsInAllScenes.control.ScreenTransition ();
			}
			if (timeBeforeGameOverPassed < timeBeforeGameOver) {
				timeBeforeGameOverPassed += Time.deltaTime;
			} else {
				setIsPlayerAlive (false);
			}
		}
	}

	public void setIsPlayerAlive(bool isPlayerAlive){
		this.isPlayerAlive = isPlayerAlive;
	}

	public bool getIsPlayerAlive(){
		return isPlayerAlive;
	}

}
