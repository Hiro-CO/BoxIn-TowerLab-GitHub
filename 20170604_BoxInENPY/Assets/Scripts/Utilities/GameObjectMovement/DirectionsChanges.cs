using UnityEngine;
using System.Collections;

public class DirectionsChanges : MonoBehaviour {

	static Directions direction;
	static System.Array arrayDirections;

	public static Directions RandomDirection(){
		if (arrayDirections == null) {
			arrayDirections = System.Enum.GetValues (typeof(Directions));
		} else {
			if (arrayDirections.Length > 0) {
				direction = (Directions)arrayDirections.GetValue( Random.Range (0, arrayDirections.Length) );
			}
		}

		return direction;
	}

	public static Vector2 DirectionVector2(Vector2 origin, Vector2 destiny){
		return (destiny - origin).normalized;
	}

	public static Directions GetOpositeDirection(Directions direction){
		switch(direction){
			case Directions.up:
				return Directions.down;
				break;
			case Directions.upRight:
				return Directions.downLeft;
				break;
			case Directions.right:
				return Directions.left;
				break;
			case Directions.downRight:
				return Directions.upLeft;
				break;
			case Directions.down:
				return Directions.up;
				break;
			case Directions.downLeft:
				return Directions.upRight;
				break;
			case Directions.left:
				return Directions.right;
				break;
			case Directions.upLeft:
				return Directions.downRight;
				break;
			default:
				return Directions.up;
				break;
		}
	}
}
