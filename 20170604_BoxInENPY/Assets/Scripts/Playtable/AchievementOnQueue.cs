using UnityEngine;
using System.Collections;

public class AchievementOnQueue{
	public string name;
	public string achievements;
	public int turmaIndex;

	public AchievementOnQueue(string name, string achievements, int turmaIndex){
		this.name = name;
		this.achievements = achievements;
		this.turmaIndex = turmaIndex;
	}
}
