using UnityEngine;
using System.Collections;

public class MoveRectStartNextAchievement : MoveRectReachStartEvent {

	public AchievementVisualController achievementController;

	public override void ExecuteEvents(){
		achievementController.ShowNextAchievement ();
	}
}
