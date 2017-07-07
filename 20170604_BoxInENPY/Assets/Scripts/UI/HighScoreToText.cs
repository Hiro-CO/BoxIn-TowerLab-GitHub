using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScoreToText : MonoBehaviour {

	public Text top1;
	public Text top2;
	public Text top3;

	// Use this for initialization
	void Start () {
		RefreshTopPlayers();
	}
	
	// Update is called once per frame
	public void RefreshTopPlayers () {
		PersistenceController.control.Load();

		top1.text = PersistenceController.control.maxPointsHighScore.ToString();
		top2.text = PersistenceController.control.maxPointsHighScore2.ToString();
		top3.text = PersistenceController.control.maxPointsHighScore3.ToString();
	}
}
