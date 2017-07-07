using UnityEngine;
using System.Collections;

public class EnemyData : MonoBehaviour {

	public string code;
	public string nameEn;
	public string namePt;
	public string nameEs;
	public MainTagEnemy mainTag;
	public StageTagEnemy stageTag;
	public DifficultyTagEnemy difficulty;
	public ExtraTagsEnemy[] extraTags;
	public Sprite[] spritesToSwap;
	public GameObject background;
	public AudioNames deathAudio;
	public EnemySecondaryAction secondaryAction;
	public bool useSparkles = false;

	public float timeToSpawn = 1f;
	public float timeToDisable = 1.5f;
	public float timeToDisableMultispawn = 4f;
	public bool useMultiSpawn = false;
	public int multiSpawnPercentage = 10;
	public int multiSpawnNumber = 3;
	public float multiSpawnTime = 0.5f;
	public float multiSpawnPositionX = 0.2f;
	public Vector3 multiSpawnScale = new Vector3(0.5f,0.5f,0.5f);

	public void PrepareSecondaryAction(){
		secondaryAction.PrepareAction ();
	}

	public void ExecuteSecondaryStartDragAction(Vector3 dragPosition){
		secondaryAction.ExecuteAction (dragPosition);
	}
}
