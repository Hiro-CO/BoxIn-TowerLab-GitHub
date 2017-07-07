using UnityEngine;
using System.Collections;

public class NotesMovements : MoveToDirectionAdvanced {

	public float SecondMoveSpeed;
	public Transform SecondMoveTarget;

	public override void StopMoving(){
		base.StopMoving();
		setDistanceMoved (0f);
		setSpeedActual(SecondMoveSpeed);
		ChangeDirection( DirectionsChanges.DirectionVector2(transform.position, SecondMoveTarget.position) );
		setDistanceToMoveActual( Vector2.Distance(transform.position, SecondMoveTarget.position) );
		StartMovingVectorDirection ();
	}
}
