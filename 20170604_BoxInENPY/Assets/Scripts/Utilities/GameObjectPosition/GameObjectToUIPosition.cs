using UnityEngine;
using System.Collections;

public class GameObjectToUIPosition : MonoBehaviour {

	public RectTransform uiObject;

	void Start () {
		transform.position = uiObject.position;
	}

	void Update(){
		if (transform.position != uiObject.position) {
			GoToPosition ();
		}
	}

	public void GoToPosition(){
		transform.position = uiObject.position;
	}
}
