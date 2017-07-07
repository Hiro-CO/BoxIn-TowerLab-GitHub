using UnityEngine;
using System.Collections;

public class ColorsSecondaryAction : EnemySecondaryAction {

	public Color splashColor;
	public GameObject splashesPoolerPrefab;
	public static ObjectsPooler splashesPooler;

	public override void PrepareAction (){
		base.PrepareAction ();
		if (splashesPooler == null) {
			GameObject splashesObject = (GameObject) Instantiate (splashesPoolerPrefab, Vector3.zero, Quaternion.identity);
			splashesPooler = splashesObject.GetComponent<ObjectsPooler> ();
		}
	}

	public override void ExecuteAction (Vector3 dragPosition){
		Vector3 vectorRotation = new Vector3(0,0,Random.Range(0f,360f));
		Quaternion newRotation = Quaternion.Euler (vectorRotation);
		GameObject splashObj = splashesPooler.UsePooledObject (dragPosition, newRotation);
		splashObj.GetComponent<SpriteRenderer> ().color = splashColor;
	}
}
