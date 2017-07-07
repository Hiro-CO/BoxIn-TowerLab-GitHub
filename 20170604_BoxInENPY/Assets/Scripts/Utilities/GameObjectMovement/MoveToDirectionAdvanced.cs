using UnityEngine;
using System.Collections;

public class MoveToDirectionAdvanced : MonoBehaviour {

	public float speed = 1f;
	float speedActual = 1f;
	public float distanceToMove = 1f;
	float distanceToMoveActual = 1f;
	float distanceMoved = 0f;
	public bool useSmoothMovement = false;
	bool useSmoothMovementActual = false;
	public float smoothValue = 1f;
	float smoothValueActual = 1f;
	public bool moveOnAwake = true;
	public bool randomDirection = false;
	public Directions direction;
	Directions directionActual;
	Vector3 directionToMoveVector;
	Vector3 targetPosActual;
	bool isMoving = false;

	public virtual void Awake(){
		isMoving = false;
		distanceMoved = 0f;
		setSpeedActual (speed);
		setUseSmoothMovementActual (useSmoothMovement);
		setSmoothValueActual (smoothValue);
		setDistanceToMoveActual(distanceToMove);
		if (randomDirection) {
			direction = DirectionsChanges.RandomDirection ();
		}
		ChangeDirection(direction);
		isMoving = moveOnAwake;
	}

	public virtual void OnEnable(){
		Awake();
	}

	public virtual void Update () {
		if(isMoving){
			if (distanceMoved + 0.00001f < distanceToMoveActual) {

				if(useSmoothMovementActual){					
					transform.position = Vector2.Lerp (transform.position, targetPosActual, smoothValueActual * Time.deltaTime * speedActual);
					distanceMoved = distanceToMoveActual - Vector2.Distance(transform.position, targetPosActual);
				}else{
					transform.Translate (directionToMoveVector * Time.deltaTime * speedActual);
					distanceMoved += (Time.deltaTime * speedActual);
				}

			} else {
				StopMoving ();
			}
		}
	}

	public virtual void StopMoving(){
		isMoving = false;
	}

	public virtual void StartMoving(){
		ChangeDirection (direction);
		distanceMoved = 0f;

		Vector3 target = distanceToMoveActual * directionToMoveVector;
		targetPosActual = transform.position + target;

		isMoving = true;
	}

	public virtual void StartMovingVectorDirection(){
		distanceMoved = 0f;

		Vector3 target = distanceToMoveActual * directionToMoveVector;
		targetPosActual = transform.position + target;

		isMoving = true;
	}

	public virtual void StartMovingOpositeDirection(){
		ChangeDirection (DirectionsChanges.GetOpositeDirection(direction));
		distanceMoved = 0f;

		Vector3 target = distanceToMoveActual * directionToMoveVector;
		targetPosActual = transform.position + target;

		isMoving = true;
	}

	public void setDistanceToMoveActual(float distanceToMoveActual){
		this.distanceToMoveActual = distanceToMoveActual;
	}

	public void setSpeedActual(float speedActual){
		this.speedActual = speedActual;
	}

	public void setSmoothValueActual(float smoothValueActual){
		this.smoothValueActual = smoothValueActual;
	}

	public void setUseSmoothMovementActual(bool useSmoothMovementActual){
		this.useSmoothMovementActual = useSmoothMovementActual;
	}

	public void setDistanceMoved(float distanceMoved){
		this.distanceMoved = distanceMoved;
	}

	public void ChangeDirection(Directions direction){
		directionActual = direction;
		directionToMoveVector = DirectionsVectors2D.directionVector[directionActual];

		Vector3 target = distanceToMoveActual * directionToMoveVector;
		targetPosActual = transform.position + target;
	}

	public void ChangeDirection(Vector2 direction){
		directionToMoveVector = direction;

		Vector3 target = distanceToMoveActual * directionToMoveVector;
		targetPosActual = transform.position + target;
	}
}
