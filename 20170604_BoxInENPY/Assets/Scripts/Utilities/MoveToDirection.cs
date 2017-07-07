using UnityEngine;
using System.Collections;

public class MoveToDirection : MonoBehaviour {

	public float speed = 1f;
	public bool moveOnAwake = true;
	public Directions direction;
	Vector2 directionToMoveVector;
	bool isMoving = false;

	void Awake(){
		isMoving = moveOnAwake;
		ChangeDirection(direction);
	}

	void Update () {
		if(isMoving){
			transform.Translate(directionToMoveVector * Time.deltaTime * speed);
		}
	}

	public void StopMoving(){
		isMoving = false;
	}

	public void StartMoving(){
		isMoving = true;
	}

	public void ChangeDirection(Directions direction){
		directionToMoveVector = DirectionsVectors2D.directionVector[direction];
	}
}
