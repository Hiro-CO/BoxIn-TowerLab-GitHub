using UnityEngine;
using System.Collections;

public class ScreenPointToPosition2D : MonoBehaviour {

	public static Vector3 GetWorldPosition2D(Vector3 screenPoint, float zPosition){
		Ray ray = Camera.main.ScreenPointToRay(screenPoint);
		Vector3 point = ray.origin + (ray.direction * Mathf.Abs(Camera.main.transform.position.z - zPosition) );
		point.z = zPosition;
		return point;
	}

	public static float GetZRotation2D(Vector3 initial, Vector3 lookAtPosition){
		if(lookAtPosition.y > initial.y){
			if(lookAtPosition.x < initial.x){
				float tangent = (lookAtPosition.y - initial.y)/(initial.x - lookAtPosition.x);
				return - RadiansToDegrees(Mathf.Atan(tangent)) + 90;
			}
			if(lookAtPosition.x > initial.x){
				float tangent = (lookAtPosition.y - initial.y)/(lookAtPosition.x - initial.x);
				return RadiansToDegrees(Mathf.Atan(tangent)) - 90f;
			}

			//lookAtPosition.x == initial.x
			return 0f;
		}

		if(lookAtPosition.y < initial.y){
			if(lookAtPosition.x < initial.x){
				float tangent = (initial.y - lookAtPosition.y)/(initial.x - lookAtPosition.x);
				return RadiansToDegrees(Mathf.Atan(tangent)) + 90f;
			}
			if(lookAtPosition.x > initial.x){
				float tangent = (initial.y - lookAtPosition.y)/(lookAtPosition.x - initial.x);
				return - RadiansToDegrees(Mathf.Atan(tangent)) - 90f;
			}

			//lookAtPosition.x == initial.x
			return 180f;
		}

		//lookAtPosition.y == initial.y
		if(lookAtPosition.x < initial.x){
			return 90f;
		}
		if(lookAtPosition.x > initial.x){
			return -90f;
		}

		//initial == lookAtPosition
		return 0f;
	}

	public static float RadiansToDegrees(float radianAngle){
		return (radianAngle*180)/Mathf.PI;
	}
}
