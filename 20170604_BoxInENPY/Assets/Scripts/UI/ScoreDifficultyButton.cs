using UnityEngine;
using System.Collections;
using Playmove;

public class ScoreDifficultyButton : MonoBehaviour {

	public ImageUISpriteSwap spriteSwap;
	public ScoreboardWindow scoreboardWindow;
	public DifficultyTagEnemy difficulty;

	public void Clicked(){
		spriteSwap.SwapToSprite ((int)difficulty);
		scoreboardWindow.ChangeDifficulty ((int)difficulty);
	}
}
