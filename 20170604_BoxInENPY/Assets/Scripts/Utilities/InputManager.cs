using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputManager : MonoBehaviour {

	public float gravityWhileDragging = 1f;
	float gravityOld = 0f;
	float speedOld = 0f;
	public bool useFastFall = false;
	public float fastFallSpeed = 2f;
	public ObjectsPooler glassKnockKnockPooler;
	public AudioNames[] glassKnockSounds;
	GameObject objectBeingDragged;
	EnemyCollisions enemyCollisions;
	BoxMovement boxMovement;
	GameObject jointPosition;
	public GameObject draggedBoxObject;
	public DraggedBox draggedBox;

	void Awake(){
		jointPosition = GameObject.FindGameObjectWithTag (Tags.jointObject);
	}

	void Update () {
		#if UNITY_EDITOR || UNITY_WEBGL
		if (Input.GetMouseButtonDown (0)) {
			StartDrag(Input.mousePosition);
		}
		if (Input.GetMouseButton (0)) {
			if(objectBeingDragged != null && enemyCollisions != null){
				MoveDrag(Input.mousePosition);
			}

			if(objectBeingDragged != null && boxMovement != null){
				MoveDrag(Input.mousePosition);
			}

			if(objectBeingDragged != null && enemyCollisions != null){
				if(!objectBeingDragged.activeInHierarchy){
					EndDrag();
				}
			}
		}
		if(Input.GetMouseButtonUp(0)){
			EndDrag();
		}


		#else
		if(Input.touchCount > 0){
			if(Input.touches[0].phase == TouchPhase.Began){
				StartDrag(Input.touches[0].position);
			}
			if(Input.touches[0].phase == TouchPhase.Moved && objectBeingDragged != null && enemyCollisions != null){
				MoveDrag(Input.touches[0].position);
			}
			if(Input.touches[0].phase == TouchPhase.Moved && objectBeingDragged != null && boxMovement != null){
				MoveDrag(Input.touches[0].position);
			}
			if(Input.touches[0].phase == TouchPhase.Ended){
				EndDrag();
			}
			if(objectBeingDragged != null && enemyCollisions != null){
				if(!objectBeingDragged.activeInHierarchy){
					EndDrag();
				}
			}
			/*
			Touch[] touches = Input.touches;
			for(int i=0; i<touches.Length; i++ ){
				
			}
			*/
		}else{
			//EndDrag();
		}
		#endif
	}

	public void EndDrag(){
		if (enemyCollisions != null) {
			enemyCollisions.joint.enabled = false;
			enemyCollisions.rbody2d.gravityScale = gravityOld;
			enemyCollisions.rbody2d.velocity = Vector2.zero;

			objectBeingDragged.transform.rotation = Quaternion.identity;
			Vector3 newPosition = objectBeingDragged.transform.position;
			newPosition.x = jointPosition.transform.position.x;
			objectBeingDragged.transform.position = newPosition;
			enemyCollisions.rbody2d.angularVelocity = 0f;


			enemyCollisions.movement.speed = speedOld;
		}
		if(boxMovement != null){
			draggedBox.EndDragging ();
			boxMovement.EndDragging ();
		}
		objectBeingDragged = null;
		enemyCollisions = null;
		boxMovement = null;
	}

	public void MoveDrag(Vector3 newPos){
		Vector2 positionToGo = Camera.main.ScreenToWorldPoint (newPos);
		if (enemyCollisions != null) {			
			jointPosition.transform.position = positionToGo;
		}
		if (boxMovement != null) {
			draggedBoxObject.transform.position = positionToGo;
		}
	}

	public void StartDrag(Vector3 point){
		//CODE WITH ONLY ONE RAYCASTHIT
		Ray ray = Camera.main.ScreenPointToRay(point);
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction);


		if (hit != null){
			if (hit.collider != null) {
				//the hit.collider gets the object directly clicked. If you use only the hit, you will get the parent
				switch (hit.collider.transform.gameObject.tag) {

				case Tags.enemy:
					boxMovement = null;

					objectBeingDragged = hit.collider.transform.gameObject;
					enemyCollisions = objectBeingDragged.GetComponent<EnemyCollisions> ();
					enemyCollisions.rbody2d.velocity = Vector2.zero;
					gravityOld = enemyCollisions.rbody2d.gravityScale;
					enemyCollisions.rbody2d.gravityScale = gravityWhileDragging;

					enemyCollisions.spriteSwap.SwapToSprite (1);

					if (useFastFall) {
						enemyCollisions.movement.speed = fastFallSpeed;
					}

					speedOld = enemyCollisions.movement.speed;

					enemyCollisions.movement.speed = 0f;

					Vector2 positionToGo = Camera.main.ScreenToWorldPoint (point);
					jointPosition.transform.position = positionToGo;

					enemyCollisions.joint.enabled = true;

					enemyCollisions.getEnemyData ().ExecuteSecondaryStartDragAction (positionToGo);
					break;									

				case Tags.playerBoxDrag:
					objectBeingDragged = hit.collider.transform.gameObject;
					boxMovement = objectBeingDragged.GetComponent<BoxMovement> ();

					if( boxMovement.boxEnableDisable.getIsEnabled() ){
						enemyCollisions = null;

						Vector2 positionNew = Camera.main.ScreenToWorldPoint(point);
						draggedBoxObject.transform.position = positionNew;

						boxMovement.StartDragging ();
						draggedBox.StartDragging (boxMovement, boxMovement.getSpriteUsed(), boxMovement.getSpriteFrenteUsed(), boxMovement.enemyIndex);
					}else{
						objectBeingDragged = null;
						boxMovement = null;
						enemyCollisions = null;
					}
					break;

				case Tags.playerBox:
					objectBeingDragged = hit.collider.transform.gameObject.GetComponent<FrontBoxMovement> ().objectWithBoxMovement;
					boxMovement = objectBeingDragged.GetComponent<BoxMovement> ();

					if( boxMovement.boxEnableDisable.getIsEnabled() ){
						enemyCollisions = null;

						Vector2 positionNew = Camera.main.ScreenToWorldPoint(point);
						draggedBoxObject.transform.position = positionNew;

						boxMovement.StartDragging ();
						draggedBox.StartDragging (boxMovement, boxMovement.getSpriteUsed(), boxMovement.getSpriteFrenteUsed(), boxMovement.enemyIndex);
					}else{
						objectBeingDragged = null;
						boxMovement = null;
						enemyCollisions = null;
					}
					break;

				case Tags.glass:
					Vector2 knockPosition = Camera.main.ScreenToWorldPoint (point);
					glassKnockKnockPooler.UsePooledObject (knockPosition, Quaternion.identity);

					AudioManager.PlaySound (glassKnockSounds[Random.Range(0, glassKnockSounds.Length)].ToString (), Vector3.zero);

					objectBeingDragged = null;
					boxMovement = null;
					enemyCollisions = null;
					break;

				default:
					objectBeingDragged = null;
					boxMovement = null;
					enemyCollisions = null;
					break;
				}
			}
		}


		/*multiple raycasthit
		// Construct a ray from the current touch coordinates
		Ray ray = Camera.main.ScreenPointToRay(point);

		// Move to hit
		RaycastHit[] allHits = Physics.RaycastAll(ray);
		List<RaycastHit> closestHit = new List<RaycastHit>();

		bool containsPriority = false;

		if(allHits.Length > 0){
			for(int i=0; i<allHits.Length;i++){
				if( i==0 ){
					if(!closestHit.Contains(allHits[i])){
						closestHit.Add(allHits[i]);
						containsPriority = CheckRaycastHitPriority(allHits[i]) || containsPriority;
					}
				}

				if( CheckRaycastHitPriority(allHits[i]) || !containsPriority ){
					if(!closestHit.Contains(allHits[i])){
						closestHit.Add(allHits[i]);
						containsPriority = CheckRaycastHitPriority(allHits[i]) || containsPriority;
					}
				}
			}

			if(!containsPriority && !HasElementWithTag(closestHit, Tags.wall)){
				MoveAllSelected(closestHit[0].point);
			}

			if(containsPriority && HasElementWithTag(closestHit, Tags.enemyClickArea)){
				closestHit = RemoveElementsWithoutTag(closestHit, Tags.enemyClickArea);
				if (closestHit.Count > 0){
					for(int i=0; i<closestHit.Count; i++){
						AttackWithAllSelected(closestHit[i].collider.transform.parent.gameObject);
					}
				}
			}
			*/

			/*
			if(containsPriority && HasElementWithTag(closestHit, Tags.playerClickArea)){
				closestHit = RemoveElementsWithoutTag(closestHit, Tags.playerClickArea);
				if (closestHit.Count > 0){

					closestHit[0].collider.transform.parent.GetComponent<CharacterMovement>().CharacterClickedDeselectAllButtons();
					RemoveAllFromSelected();

					for(int i=0; i<closestHit.Count; i++){
						closestHit[i].collider.transform.parent.GetComponent<CharacterMovement>().CharacterClickedWithoutDeselectingOthers();
					}
				}
			}
			*/
	}


}
