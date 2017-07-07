using UnityEngine;
using System.Collections;

public class EnemyCollisions : MonoBehaviour {

	public string enemyType;
	public int enemyIndex = 0;
	public int enemyPoints = 1;
	public SpriteSwap spriteSwap;
	public Rigidbody2D rbody2d;
	public FixedJoint2D joint;
	public static GameObject gameController;
	public static GameController gameControllerScript;
	public static GameControllerScore gameControllerScoreScript;
	public static Rigidbody2D jointRbody2d;
	public static GameObject backgroundObject;
	EnemyData enemyData;
	AudioNames deathAudio;
	public ObjectsPooler sparklesPooler;
	bool useSparkles = false;
	public Collider2D enemyCollider2d;
	public MoveToDirection movement;
	float oldSpeed = 0f;
	Vector3 vectorUnScaled =new Vector3(1f,1f,1f);


	void Awake(){
		if(gameController == null){gameController = GameObject.FindGameObjectWithTag(Tags.gameController);}
		if(gameControllerScript == null){gameControllerScript = gameController.GetComponent<GameController>();}
		if(gameControllerScoreScript == null){gameControllerScoreScript = gameController.GetComponent<GameControllerScore>();}
		oldSpeed = movement.speed;
	}

	void OnEnable(){
		/*
		if(animatorHash.dead != 0){
			animator.SetBool(animatorHash.dead, false);
		}
		*/
		spriteSwap.SwapToSprite (0);

		movement.speed = oldSpeed;

		enemyCollider2d.enabled = true;
	}

	void Start(){
		if (EnemyDataManager.control.getChosenEnemies ().Count <= enemyIndex) {
			gameObject.SetActive (false);
		} else {
			if (enemyData == null) {
				enemyData = EnemyDataManager.control.getChosenEnemies () [enemyIndex];
				enemyType = enemyData.code;
				useSparkles = enemyData.useSparkles;
				//animator.runtimeAnimatorController = enemyData.animator.runtimeAnimatorController;
				spriteSwap.sprites = enemyData.spritesToSwap;
				//gameObject.SetActive (false);

				deathAudio = enemyData.deathAudio;

				enemyData.PrepareSecondaryAction ();

				if (backgroundObject == null) {
					backgroundObject = (GameObject) Instantiate (enemyData.background, Vector3.zero, Quaternion.identity);
				}
			}
		}
		if (jointRbody2d == null) {
			jointRbody2d = GameObject.FindGameObjectWithTag (Tags.jointObject).GetComponent<Rigidbody2D> ();
		}
		joint.connectedBody = jointRbody2d;
		joint.connectedAnchor = Vector2.zero;


		spriteSwap.SwapToSprite (0);
		
	}

	
	void OnTriggerEnter2D (Collider2D other) {
		if(other.tag == Tags.playerBox){
			BoxEnemyChoice weaponEnemyChoice = other.gameObject.GetComponent<BoxEnemyChoice>();
			//string result = RockPaperScissors.WeaponResult(weaponCollision.weaponType, enemyType);

			if (weaponEnemyChoice.weaponType == enemyType) {
				gameControllerScoreScript.addScore(enemyPoints, enemyIndex);
				//animator.SetBool(animatorHash.dead, true);
				weaponEnemyChoice.KilledEnemy();

				if (transform.localScale == vectorUnScaled) {
					if ( !AudioManager.getIsPlaying (deathAudio.ToString ()) ) {
						AudioManager.PlaySound (deathAudio.ToString (), Vector3.zero);
					}
				}
				if (useSparkles) {
					sparklesPooler.UsePooledObject (transform.position, Quaternion.identity);
				}

				gameObject.SetActive(false);
			} else {
				//gameControllerScoreScript.subtractScore(0);
				////Destroy(gameObject);
				AudioManager.PlaySound(AudioNames.PlayerDraw.ToString(), Vector3.zero);
				gameObject.SetActive(false);
			}
			enemyCollider2d.enabled = false;
		}

	}

	void OnDestroy(){
		gameController = null;
		gameControllerScript = null;
		gameControllerScoreScript = null;
	}

	public EnemyData getEnemyData(){
		return enemyData;
	}
}
