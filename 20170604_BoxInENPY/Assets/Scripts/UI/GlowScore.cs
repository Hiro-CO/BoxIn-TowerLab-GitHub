using UnityEngine;
using System.Collections;

public class GlowScore : MonoBehaviour {
	public ObjectsPooler sparklesPooler;

	public void UseSparkle(){
		sparklesPooler.UsePooledObject (transform.position, Quaternion.identity);
		sparklesPooler.UsePooledObject (transform.position, Quaternion.identity);
		sparklesPooler.UsePooledObject (transform.position, Quaternion.identity);
	}
}
