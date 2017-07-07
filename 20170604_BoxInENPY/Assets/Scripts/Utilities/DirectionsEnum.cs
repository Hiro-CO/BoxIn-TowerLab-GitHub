using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Directions{
	up,
	upRight,
	right,
	downRight,
	down,
	downLeft,
	left,
	upLeft
}

public static class DirectionsVectors2D{
	public static Dictionary<Directions, Vector2> directionVector = new Dictionary<Directions, Vector2>(){
		{ Directions.up, Vector2.up },
		{ Directions.upRight, Vector2.up+Vector2.right },
		{ Directions.right, Vector2.right },
		{ Directions.downRight, -Vector2.up+Vector2.right },
		{ Directions.down, -Vector2.up },
		{ Directions.downLeft, -Vector2.up-Vector2.right },
		{ Directions.left, -Vector2.right },
		{ Directions.upLeft, Vector2.up-Vector2.right}
	};


}
