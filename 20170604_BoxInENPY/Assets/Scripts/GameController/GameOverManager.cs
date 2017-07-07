using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

	public GameObject[] objectsToShowOnGameOver;
	public GameObject[] objectsToHideOnGameOver;
	public GameObject[] objectsToDestroyOnGameOver;
	public ObjectsPooler[] poolersToRemoveObjects;
	public goToScene goToSceneNextPlayer;
	public goToScene goToSceneLastPlayer;
	//public MoveRect moveRectMenu;
	//public MoveRect moveRectScore;
	//public HighScoreToText highScoreToText;
	private GameController gameController;
	private GameControllerScore gameControllerScore;
	private bool gameOverHappened = false;

	void Awake () {
		gameController = gameObject.GetComponent<GameController>();
		gameControllerScore = gameObject.GetComponent<GameControllerScore>();
	}

	void Update () {
		if( !gameController.getIsPlayerAlive() && !gameOverHappened ){
			Persistence();
			//HighScoreTextUpdate();
			HideObjects();
			ShowObjects();
			DestroySelectedObjects ();
			HidePooledEnemies ();

			PlaytableApiContainer.setScorePercentage (PlaytableApiContainer.getActualPlayerIndex (), gameControllerScore.getPercentageDefeated());
			//if (PlaytableApiContainer.getNumberOfActivePlayers () == 1) {


			if (gameControllerScore.getPercentageDefeated () >= gameControllerScore.star1Percentage) {
				PlaytableApiContainer.setAchievements (PlaytableApiContainer.getActualPlayerIndex (), StagesCodes.codes[EnemyDataManager.control.getChosenEnemies () [0].stageTag] + "1");
			}
			if (gameControllerScore.getPercentageDefeated () >= gameControllerScore.star2Percentage) {
				PlaytableApiContainer.setAchievements (PlaytableApiContainer.getActualPlayerIndex (), StagesCodes.codes[EnemyDataManager.control.getChosenEnemies () [0].stageTag] + "2");
			}
			if (gameControllerScore.getPercentageDefeated () >= gameControllerScore.star3Percentage) {
				PlaytableApiContainer.setAchievements (PlaytableApiContainer.getActualPlayerIndex (), StagesCodes.codes[EnemyDataManager.control.getChosenEnemies () [0].stageTag] + "3");
			}
			//}
			PlaytableApiContainer.UpdateAchievements (PlaytableApiContainer.getActualPlayerIndex ());

			PlaytableApiContainer.setScoreNonStar (PlaytableApiContainer.getActualPlayerIndex (), gameControllerScore.getScore ());
			PlaytableApiContainer.setScore (PlaytableApiContainer.getActualPlayerIndex (), PlaytableApiContainer.getNumberOfAchievementsOnDifficulty(PlaytableApiContainer.getActualPlayerIndex (),EnemyDataManager.control.getChosenEnemies()[0].difficulty));
			PlaytableApiContainer.setDifficultyLevel (PlaytableApiContainer.getActualPlayerIndex (), (int)EnemyDataManager.control.getChosenEnemies()[0].difficulty);

			PlaytableApiContainer.UpdateScore (PlaytableApiContainer.getActualPlayerIndex ());

			if (PlaytableApiContainer.getNextActivePlayer () != 999) {
				PlaytableApiContainer.setActualPlayerIndex ( PlaytableApiContainer.getNextActivePlayer ());
				goToSceneNextPlayer.selectScene ();
			} else {
				goToSceneLastPlayer.selectScene ();
			}

			//MoveRectsGoToEnd();
			gameOverHappened = true;
		}	
	}

	void Persistence(){
		/***********
		 ***********
		 
		PersistenceController.control.Load();

		PersistenceController.control.totalCoins += gameControllerScore.getCoins();
		if(PersistenceController.control.maxCoinsHighScore < gameControllerScore.getCoins()){
			PersistenceController.control.maxCoinsHighScore = gameControllerScore.getCoins();
		}

		PersistenceController.control.totalPoints += gameControllerScore.getScore();

		***********
		***********/

		/****/
		//HIGHSCORE START
		/*************
		bool highscoreAltered = false;
		if(!highscoreAltered && PersistenceController.control.maxPointsHighScore < gameControllerScore.getScore()){

			PersistenceController.control.maxPointsHighScore3 = PersistenceController.control.maxPointsHighScore2;
			PersistenceController.control.maxPointsHighScore2 = PersistenceController.control.maxPointsHighScore;
			PersistenceController.control.maxPointsHighScore = gameControllerScore.getScore();

			highscoreAltered = true;
		}

		if(!highscoreAltered && PersistenceController.control.maxPointsHighScore2 < gameControllerScore.getScore()){

			PersistenceController.control.maxPointsHighScore3 = PersistenceController.control.maxPointsHighScore2;
			PersistenceController.control.maxPointsHighScore2 = gameControllerScore.getScore();

			highscoreAltered = true;
		}

		if(!highscoreAltered && PersistenceController.control.maxPointsHighScore3 < gameControllerScore.getScore()){

			PersistenceController.control.maxPointsHighScore3 = gameControllerScore.getScore();
			highscoreAltered = true;
		}
		****/
		//HIGHSCORE END
		//*************


		//PersistenceController.control.Save();
	}
	/*
	void TextsUpdate(){
		//highScoreText.text += PersistenceController.control.maxPointsHighScore.ToString();
		//yourScoreText.text += gameControllerScore.getScore().ToString();
		//totalScoreText.text += PersistenceController.control.totalPoints.ToString();

	}
*/
	void HideObjects(){
		foreach(GameObject obj in objectsToHideOnGameOver){
			obj.SetActive(false);
		}
	}

	void ShowObjects(){
		foreach(GameObject obj in objectsToShowOnGameOver){
			obj.SetActive(true);
		}
	}

	void DestroySelectedObjects(){
		foreach(GameObject obj in objectsToDestroyOnGameOver){
			Destroy (obj);
		}
	}

	void HidePooledEnemies(){
		foreach (ObjectsPooler pool in poolersToRemoveObjects) {
			pool.DeactivateAllPooledObjects ();
		}
	}

	/*
	void MoveRectsGoToEnd(){
		//moveRectMenu.GoToEnd();
		//moveRectScore.GoToEnd();
	}
*//*
	void HighScoreTextUpdate(){
		//highScoreToText.RefreshTopPlayers();
	}
	*/
	public void goToScene(string sceneName){
		SceneManager.LoadScene(sceneName);
	}
}
