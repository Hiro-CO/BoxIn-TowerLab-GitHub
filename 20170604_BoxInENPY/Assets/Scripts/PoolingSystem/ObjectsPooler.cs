using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsPooler : MonoBehaviour {

	public GameObject objectPrefab;
	public int numberOfObjects = 5;
	public bool poolOnAwake = true;
	List<GameObject> pooledObjects = new List<GameObject>();

	public virtual void Awake () {
		if (poolOnAwake) {
			for (int i = 0; i < numberOfObjects; i++) {
				GameObject pooledObject = (GameObject)Instantiate (objectPrefab);
				pooledObject.SetActive (false);
				pooledObjects.Add (pooledObject);
			}
		}
	}

	public virtual void Start(){
		if (!poolOnAwake) {
			for (int i = 0; i < numberOfObjects; i++) {
				GameObject pooledObject = (GameObject)Instantiate (objectPrefab);
				pooledObject.SetActive (false);
				pooledObjects.Add (pooledObject);
			}
		}
	}
	
	public virtual GameObject UsePooledObject(Vector3 positionToUse, Quaternion rotationToUse){
		for(int i=0; i < pooledObjects.Count; i++){
			if (!pooledObjects [i].activeInHierarchy) {
				pooledObjects [i].transform.position = positionToUse;
				pooledObjects [i].transform.rotation = rotationToUse;
				pooledObjects [i].SetActive (true);

				return pooledObjects [i];
			}
		}

		GameObject pooledObject = (GameObject)Instantiate(objectPrefab);
		pooledObjects.Add (pooledObject);
		pooledObject.transform.position = positionToUse;
		pooledObject.transform.rotation = rotationToUse;
		pooledObject.SetActive (true);

		return pooledObject;
	}

	public void DeactivateAllPooledObjects(){
		if (pooledObjects.Count > 0) {
			for (int i = 0; i < pooledObjects.Count; i++) {
				pooledObjects [i].SetActive (false);
			}
		}
	}
}
