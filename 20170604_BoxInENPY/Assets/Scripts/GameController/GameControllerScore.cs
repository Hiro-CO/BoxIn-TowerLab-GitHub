using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameControllerScore : MonoBehaviour {

	public int pointsToLevelUp = 20;
	//public MoveToDirection glassMovement;
	public float star1Percentage = 70f;
	public float star2Percentage = 80f;
	public float star3Percentage = 90f;
	private int score = 0;
	private int coins = 0;
	//private Text scoreNumber;
	private int[] enemiesSpawned = new int[6];
	private int[] enemiesDefeated = new int[6];
	private int totalSpawned = 0;
	private int totalDefeated = 0;
	private BoxEnableDisable[] boxes;
	//private GameControllerDifficulty difficultyController;

	/*
	void Awake () {
		//scoreNumber = GameObject.FindGameObjectWithTag(Tags.scoreNumber).GetComponent<Text>();
		//difficultyController = GetComponent<GameControllerDifficulty>();
	}
	*/

	public void addOneSpawnedEnemy(int index){
		enemiesSpawned [index]++;
		totalSpawned++;
	}

	public void addScore(int scoreToAdd, int index){
		enemiesDefeated [index]++;
		totalDefeated++;
		score += scoreToAdd;
		//updateScoreNumber();
		updateDifficulty(index);
	}

	public void subtractScore(int scoreToSubtract){
		score -= scoreToSubtract;
		AudioManager.PlaySound(AudioNames.PlayerDraw.ToString(), Vector3.zero);
		if(score < 0){
			score = 0;
		}
		//updateScoreNumber();
	}

	/*
	void updateScoreNumber(){
		//scoreNumber.text = score.ToString();
	}
	*/

	public void addCoins(int coinsToAdd){
		coins += coinsToAdd;
	}

	void updateDifficulty(int index){
		if(score != 0){
			if (index == Spawner.maximumIndex) {
				Spawner.maximumIndex++;
				if (boxes.Length > Spawner.maximumIndex) {
					boxes [Spawner.maximumIndex].Enable();
				}
				/*
				if (boxes.Length == Spawner.maximumIndex && glassMovement.gameObject.activeSelf) {
					glassMovement.StartMoving ();
				}
				*/
			}
			
			/*
			 if(difficultyController.getDifficultyLevel() < Mathf.RoundToInt(score/pointsToLevelUp)){
				difficultyController.setDifficultyLevel(Mathf.RoundToInt(score/pointsToLevelUp));
			}
			*/
		}
	}

	public float getPercentageDefeated(){
		return (100 * totalDefeated) / totalSpawned;
	}

	public int getCoins(){
		return coins;
	}

	public int getScore(){
		return score;
	}

	public void setBoxes(BoxEnableDisable[] boxes){
		this.boxes = boxes;
	}
}
