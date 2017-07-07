using UnityEngine;
using Playtable;
using System.Collections;
using System.Collections.Generic;
using Playmove;

public class PlaytableApiContainer : MonoBehaviour {

	public static PlaytableApiContainer control;
	public PlaytableWin32 playtableWin32Script;
	int actualPlayerIndex = 0;
	const int maxNumberOfPlayers = 2;
	bool[] isPlayerActive = new bool[maxNumberOfPlayers];
	bool[] reversedPlayer = new bool[maxNumberOfPlayers];
	string[] playerName = new string[maxNumberOfPlayers];
	bool[] playerLogged = new bool[maxNumberOfPlayers];
	int[] difficultyLevel = new int[maxNumberOfPlayers];
	int[] turmaIndex = new int[maxNumberOfPlayers];
	int[] score = new int[maxNumberOfPlayers];
	int[] scoreNonStar = new int[maxNumberOfPlayers];
	float[] scorePercentage = new float[maxNumberOfPlayers];
	string[] achievements = new string[maxNumberOfPlayers];
	bool startedScore = false;
	List<ScoreOnQueue> scoresOnQueue = new List<ScoreOnQueue>();
	List<AchievementOnQueue> achievementsOnQueue = new List<AchievementOnQueue>();

	void Awake(){
		if (control == null) {
			DontDestroyOnLoad(this);
			control = this;
		} else {
			Destroy (gameObject);
		}
	}

	void Start(){
		if (control == this) {
			PYScoreData.Initialize ();
		}

		/*
		isPlayerActive [0] = true;
		isPlayerActive [1] = true;
		*/
	}

	void Update(){
		if(!control.startedScore) {
			if (PYScoreData.synchronizedTheScores) {
				control.startedScore = true;
			}
		}
			
		if(scoresOnQueue.Count > 0 && control.startedScore){
			control.UpdateScoresOnQueue ();
		}
		if(achievementsOnQueue.Count > 0 && control.startedScore){
			control.UpdateAchievementsOnQueue ();
		}
	}

	public static void setTurmaIndex(int playerIndex,int turma){
		control.turmaIndex[playerIndex] = turma;
	}
	public static int getTurmaIndex(int playerIndex){
		return control.turmaIndex[playerIndex];
	}

	public static void setScore(int playerIndex,int score){
		control.score[playerIndex] = score;
	}
	public static int getScore(int playerIndex){
		return control.score[playerIndex];
	}

	public static void setScoreNonStar(int playerIndex,int score){
		control.scoreNonStar[playerIndex] = score;
	}
	public static int getScoreNonStar(int playerIndex){
		return control.scoreNonStar[playerIndex];
	}

	public static int[] getAllScores(){
		return control.score;
	}

	public static void setScorePercentage(int playerIndex,float scorePercentage){
		control.scorePercentage[playerIndex] = scorePercentage;
	}

	public static float getScorePercentage(int playerIndex){
		return control.scorePercentage[playerIndex];
	}

	public static bool getStartedScore(){
		return control.startedScore;
	}

	public static string[] getAchievementsFromData(int playerIndex){
		if (PYScoreData.GetStudentByName (control.playerName[playerIndex]) != null) {
			control.achievements[playerIndex] = PYScoreData.GetStudentByName (control.playerName[playerIndex]).Achievements;
		} else {
			control.achievements[playerIndex] = "";
		}
		return control.achievements[playerIndex].Split(';');
	}
	public static string[] getAchievementsActual(int playerIndex){
		return control.achievements[playerIndex].Split(';');
	}

	public static int getNumberOfAchievements(int playerIndex){
		if (control.achievements [playerIndex].Split (';').Length == 1) {
			if (control.achievements [playerIndex].Split (';')[0] == "") {
				return 0;
			}
		}
		return control.achievements[playerIndex].Split(';').Length;
	}

	public static int getNumberOfAchievementsOnDifficulty(int playerIndex, DifficultyTagEnemy difficulty){
		string[] achievementsToCheck = control.achievements [playerIndex].Split (';');

		if (achievementsToCheck.Length == 1) {
			if (achievementsToCheck[0] == "") {
				return 0;
			}
		}

		List<string> achievementsOnDifficulty = new List<string> ();
		for (int i = 0; i < achievementsToCheck.Length; i++) {
			if (StagesCodes.difficulties.ContainsKey(achievementsToCheck [i])) {
				if (StagesCodes.difficulties [achievementsToCheck [i]] == difficulty) {
					achievementsOnDifficulty.Add (achievementsToCheck [i]);
				}
			}
		}

		return achievementsOnDifficulty.Count;
	}

	public static int getMaxNumberOfPlayers(){
		return maxNumberOfPlayers;
	}

	public static void setAchievements(int playerIndex,string achievementName){
		if (achievementName != "") {
			if(!control.achievements[playerIndex].Contains(achievementName + ";")){
				control.achievements[playerIndex] += (achievementName + ";");
				UpdateAchievements(playerIndex);
			}
		}
	}

	public static bool setPlayerName(int playerIndex,string playerName){
		bool repeatedPlayerName = false;
		for (int i = 0; i < playerIndex; i++) {
			if (control.playerName [i] == playerName) {
				repeatedPlayerName = true;
			}
		}
		if (playerName == "" || playerName == "Anônimo") {
			repeatedPlayerName = false;
		}
		if (!repeatedPlayerName) {
			control.playerName [playerIndex] = playerName;
			control.score [playerIndex] = 0;
			control.playerLogged [playerIndex] = true;
			PlaytableApiContainer.getAchievementsFromData (playerIndex);
			return true;
		} else {
			return false;
		}
	}
	public static bool getPlayerLogged(int playerIndex){
		return control.playerLogged[playerIndex];
	}
	public static string getPlayerName(int playerIndex){
		return control.playerName[playerIndex];
	}
	public static void ResetPlayerName(int playerIndex){
		control.playerName [playerIndex] = "";
		control.playerLogged [playerIndex] = false;
	}
	public static void ResetAllPlayersName(){
		for (int i=0; i<control.playerName.Length;i++) {
			control.playerName[i] = "";
			control.playerLogged [i] = false;
		}
	}
	public static void setDifficultyLevel(int playerIndex,int difficultyLevel){
		control.difficultyLevel[playerIndex] = difficultyLevel;
	}
	public static int getDifficultyLevel(int playerIndex){
		return control.difficultyLevel[playerIndex];
	}

	public static void setActualPlayerIndex(int playerIndex){
		control.actualPlayerIndex = playerIndex;
	}
	public static int getActualPlayerIndex(){
		return control.actualPlayerIndex;
	}

	public static int getNumberOfLoggedPlayers(){
		int players = 0;
		for (int i = 0; i < control.isPlayerActive.Length; i++) {
			if(control.playerLogged [i]){
				players++;
			}
		}
		return players;
	}

	public static void setIsPlayerActive(int playerIndex, bool isActive){
		control.isPlayerActive [playerIndex] = isActive;
	}
	public static int getNumberOfActivePlayers(){
		int players = 0;
		for (int i = 0; i < control.isPlayerActive.Length; i++) {
			if(control.isPlayerActive [i]){
				players++;
			}
		}
		return players;
	}
	public static void resetAllIsPlayersActive(){
		for (int i = 0; i < control.isPlayerActive.Length; i++) {
			control.isPlayerActive [i] = false;
		}
	}

	public static int getNextActivePlayer(){
		for (int i = control.actualPlayerIndex+1; i < control.isPlayerActive.Length; i++) {
			if (control.isPlayerActive [i]) {
				return i;
			}
		}
		return 999;
	}

	public static void UpdateScore(int playerIndex){
		if (control.playerName [playerIndex] != "" && control.playerName [playerIndex] != "Anônimo") {
			if (!control.startedScore) {
				control.scoresOnQueue.Add (new ScoreOnQueue (control.playerName [playerIndex], control.score [playerIndex], control.difficultyLevel [playerIndex], control.turmaIndex [playerIndex]));
			} else {
				//PYScoreData.SetScore (control.playerName[playerIndex], control.score[playerIndex], control.difficultyLevel[playerIndex]);
				PYScoreData.RegisterStudentByDifficulty (control.playerName [playerIndex], control.score [playerIndex], control.difficultyLevel [playerIndex], control.turmaIndex [playerIndex]);
				//UpdateDifficultyScores (control.difficultyLevel);
			}
		}
	}
	/*
	public static void UpdateDifficultyScores(int difficulty){
		if (difficulty == 3 || difficulty == 4 || difficulty == 5 || difficulty == 6) {
			int totalScoreDifficulty = 0;
			for (int i = 3; i <= 6; i++) {
				if(PYScoreData.GetStudentByName (i, control.playerName) != null){
					totalScoreDifficulty += PYScoreData.GetStudentByName (i, control.playerName).Score;
				}
			}
			PYScoreData.SetScore (control.playerName, totalScoreDifficulty, 0);
		}
		if (difficulty == 7 || difficulty == 8 || difficulty == 9 || difficulty == 10) {
			int totalScoreDifficulty = 0;
			for (int i = 7; i <= 10; i++) {
				if(PYScoreData.GetStudentByName (i, control.playerName) != null){
					totalScoreDifficulty += PYScoreData.GetStudentByName (i, control.playerName).Score;
				}
			}
			PYScoreData.SetScore (control.playerName, totalScoreDifficulty, 1);
		}
		if (difficulty == 11 || difficulty == 12 || difficulty == 13 || difficulty == 14) {
			int totalScoreDifficulty = 0;
			for (int i = 11; i <= 14; i++) {
				if(PYScoreData.GetStudentByName (i, control.playerName) != null){
					totalScoreDifficulty += PYScoreData.GetStudentByName (i, control.playerName).Score;
				}
			}
			PYScoreData.SetScore (control.playerName, totalScoreDifficulty, 2);
		}
	}
	*/
	public static int GetPointsOnDifficulty(int playerIndex,int difficulty){
		if (PYScoreData.GetStudentByName (control.playerName[playerIndex]) != null) {
			return PYScoreData.GetStudentByName (control.playerName[playerIndex]).Score[difficulty];
		} else {
			return 0;
		}
	}
	public static void UpdateAchievements(int playerIndex){
		if (control.playerName [playerIndex] != "" && control.playerName [playerIndex] != "Anônimo") {
			if (!control.startedScore) {
				control.achievementsOnQueue.Add (new AchievementOnQueue (control.playerName [playerIndex], control.achievements [playerIndex], control.turmaIndex [playerIndex]));
			} else {
				foreach (string achievement in PlaytableApiContainer.getAchievementsActual(playerIndex)) {
					//PYScoreData.SetAchievements (control.playerName[playerIndex], achievement);
					PYScoreData.RegisterStudent (control.playerName [playerIndex], achievement, control.turmaIndex [playerIndex]);
				}
			}
		}
	}
	public void UpdateScoresOnQueue(){
		foreach (ScoreOnQueue score in control.scoresOnQueue) {
			//PYScoreData.SetScore (score.name, score.score, score.difficultyLevel);
			PYScoreData.RegisterStudentByDifficulty (score.name, score.score, score.difficultyLevel, score.turmaIndex);
			//UpdateDifficultyScores (score.difficultyLevel);
		}
		control.scoresOnQueue.Clear ();
	}
	public void UpdateAchievementsOnQueue(){
		foreach (AchievementOnQueue achievement in control.achievementsOnQueue) {
			//PYScoreData.SetAchievements (achievement.name, achievement.achievements);
			PYScoreData.RegisterStudent(achievement.name, achievement.achievements, achievement.turmaIndex);
		}
		control.achievementsOnQueue.Clear ();
	}
	public static void ResetScore(int playerIndex){
		control.score[playerIndex] = 0;
	}
	public static void ResetAllScores(){
		for(int i=0; i<control.score.Length; i++) {
			control.score[i] = 0;
		}
	}
	public static void ResetPlayer(int playerIndex){
		control.playerName[playerIndex] = "";
		control.playerLogged [playerIndex] = false;
		control.score[playerIndex] = 0;
		control.achievements[playerIndex] = "";
		control.difficultyLevel[playerIndex] = 0;
	}
	public static void ResetAllPlayers(){
		for (int i=0; i<control.playerName.Length;i++) {
			control.playerName[i] = "";
			control.playerLogged [i] = false;
		}
		for(int i=0; i<control.score.Length; i++) {
			control.score[i] = 0;
		}
		for(int i=0; i<control.achievements.Length; i++) {
			control.achievements[i] = "";
		}
		for (int i=0; i< control.difficultyLevel.Length; i++) {
			control.difficultyLevel[i] = 0;
		}
		setActualPlayerIndex (0);
	}
	public static void ResetDifficultyLevel(int playerIndex){
		control.difficultyLevel[playerIndex] = 0;
	}
	public static void ResetAllDifficultyLevels(){
		for (int i=0; i< control.difficultyLevel.Length; i++) {
			control.difficultyLevel[i] = 0;
		}
	}

}