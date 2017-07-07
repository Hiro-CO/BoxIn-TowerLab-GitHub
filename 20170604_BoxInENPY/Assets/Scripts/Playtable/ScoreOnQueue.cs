using UnityEngine;
using System.Collections;

public class ScoreOnQueue{
	public string name;
	public int score;
	public int difficultyLevel;
	public int turmaIndex;

	public ScoreOnQueue(string name, int score, int difficultyLevel, int turmaIndex){
		this.name = name;
		this.score = score;
		this.difficultyLevel = difficultyLevel;
		this.turmaIndex = turmaIndex;
	}
}
