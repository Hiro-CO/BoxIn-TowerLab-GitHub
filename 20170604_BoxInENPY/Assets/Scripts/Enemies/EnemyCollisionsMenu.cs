using UnityEngine;
using System.Collections;

public class EnemyCollisionsMenu : MonoBehaviour {

	public string enemyType;
	public int enemyIndex = 0;
	public int enemyPoints = 1;
	public SpriteSwap spriteSwap;
	public Rigidbody2D rbody2d;
	public FixedJoint2D joint;
	public static Rigidbody2D jointRbody2d;
	EnemyData enemyData;
	public Collider2D enemyCollider2d;
	public MoveToDirection movement;
	public bool isStageSelect = false;


	void Start(){
		if (EnemyDataManager.control.getChosenEnemies ().Count <= enemyIndex) {
			gameObject.SetActive (false);
		} else {
			if (enemyData == null) {
				enemyData = EnemyDataManager.control.getChosenEnemies () [enemyIndex];
				enemyType = enemyData.code;
				//animator.runtimeAnimatorController = enemyData.animator.runtimeAnimatorController;
				spriteSwap.sprites = enemyData.spritesToSwap;
				//gameObject.SetActive (false);
				spriteSwap.SwapToSprite(0);
			}
		}
		if (jointRbody2d == null) {
			jointRbody2d = GameObject.FindGameObjectWithTag (Tags.jointObject).GetComponent<Rigidbody2D> ();
		}
		joint.connectedBody = jointRbody2d;
		joint.connectedAnchor = Vector2.zero;
	}

	void OnEnable(){
		/*
		if(animatorHash.dead != 0){
			animator.SetBool(animatorHash.dead, false);
		}
		*/
		if (isStageSelect) {
			if (EnemyDataManager.control.getChosenEnemies ().Count <= enemyIndex) {
				gameObject.SetActive (false);
			} else {
				enemyData = EnemyDataManager.control.getChosenEnemies () [enemyIndex];
				enemyType = enemyData.code;
				//animator.runtimeAnimatorController = enemyData.animator.runtimeAnimatorController;
				spriteSwap.sprites = enemyData.spritesToSwap;
				//gameObject.SetActive (false);
				spriteSwap.SwapToSprite (0);
			}
		}

		spriteSwap.SwapToSprite (0);

		enemyCollider2d.enabled = true;
	}
}